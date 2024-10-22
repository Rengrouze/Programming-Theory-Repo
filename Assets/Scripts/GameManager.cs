using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int score = 0;
    public int highScore = 0;
    public int startingBalls;
    public int ballsLeft;
    public int ballOnScreen = 0; // Keep track of how many balls are currently on the screen
    private float timer = 0f; // Timer to count the time after the last ball is destroyed
    private float timeToEndGame = 2f; // Time before ending the game


    public bool isGameOn = false;
    public int scoreMultiplier = 1;
    [SerializeField] private float bonusSpawnRate = 2f; // Time between bonus spawns
    [SerializeField] GameObject[] bonusPrefabs; // Array to hold bonus prefabs
    private float nextBonusSpawnTime = 0f; // Keeps track of when the next bonus can be spawned
    


    [SerializeField] GameObject ball;
    [SerializeField] private float spawnRate = 0.16f;  // Time between spawns (4 per second)
    private float nextSpawnTime = 0f;  // Keeps track of when the next ball can be spawned
    [SerializeField] GameObject titleScreen;  // Reference to the title screen object
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI highScoreText;
    [SerializeField] TextMeshProUGUI finalScoreText;
    [SerializeField] TextMeshProUGUI ballsLeftText;

    // Variables for bonus spawn limits
    [SerializeField] private float bonusSpawnMinX = -6f; // Minimum X spawn position
    [SerializeField] private float bonusSpawnMaxX = 6f;  // Maximum X spawn position
    [SerializeField] private float bonusSpawnMinY = -1.8f;  // Minimum Y spawn position
    [SerializeField] private float bonusSpawnMaxY = 5.30f;  // Maximum Y spawn position

    [SerializeField] private AudioClip gameOverSound;
    [SerializeField] private AudioClip popSound;
    private AudioSource audioSource;
    [Range(0f, 1f)] // This will create a slider in the inspector for volume control
    public float volume = 1f; // Volume range from 0 to 1

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        ballsLeft = startingBalls;
        UpdateScoreUI();
        
    }

    void Update()
    {
        if (isGameOn)
        {
            ballsLeftText.text = "Peebles left : " + ballsLeft;
            ballsLeftText.gameObject.SetActive(true);
            dropBall();
            SpawnBonuses();

            // Check if the game should end
            if (ballsLeft <= 0 && ballOnScreen <= 0)
            {
                timer += Time.deltaTime; // Increment the timer
                if (timer >= timeToEndGame) // Check if 5 seconds have passed
                {
                    endGame();
                }
            }
            else
            {
                timer = 0f; // Reset the timer if there are still balls left
            }
        }
    }


    void dropBall()
    {
        // Check if space is pressed or the left mouse button is clicked, and enough time has passed since the last spawn
        if ((Input.GetKey(KeyCode.Space) || Input.GetMouseButtonDown(0)) && Time.time >= nextSpawnTime && ballsLeft > 0)
        {
            // Get the mouse position in screen coordinates
            Vector3 mousePosition = Input.mousePosition;

            // Create a Ray from the camera through the mouse position
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red); // Draw the ray for 10 units

            // Perform a raycast to find the point where the ray intersects with the plane
            Plane plane = new Plane(Vector3.up, new Vector3(0, 9.19f, 0)); // Creating a horizontal plane at Y = 9.19f
            float enter;


            if (plane.Raycast(ray, out enter))
            {
                // Get the world position where the ray intersects the plane
                Vector3 worldPosition = ray.GetPoint(enter);

                // Clamp the X position between -6.45 and 6.45
                float clampedX = Mathf.Clamp(worldPosition.x, -6.45f, 6.45f);

                // Create the spawn position using the clamped X value and fixed Y (9.19) and Z (0)
                Vector3 spawnPosition = new Vector3(clampedX, 9.19f, 0f);

                // Spawn the ball
                Instantiate(ball, spawnPosition, Quaternion.identity);
                audioSource.volume = volume; // Set the audio source volume
                audioSource.PlayOneShot(popSound);
                ballOnScreen++; // Increment the ball counter
                ballsLeft--; // Decrease the number of balls left
                nextSpawnTime = Time.time + spawnRate; // Set the next spawn time
            }
        }
    }





    public void updateScore(int points)
    {
        score += points;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        scoreText.text = score.ToString();
    }

    public void startGame()
    {
        Debug.Log("startGame() was clicked");
        if (!isGameOn)
        {
            isGameOn = true;
            titleScreen.SetActive(false);  // Hide the title screen
            scoreText.gameObject.SetActive(true); // Show the score text
            highScoreText.gameObject.SetActive(false);

         
        }
    }

    // New method to spawn bonuses
    private void SpawnBonuses()
    {
        // Check if enough time has passed for the next bonus spawn
        if (Time.time >= nextBonusSpawnTime)
        {
            // Choose a random bonus from the array
            GameObject randomBonus = bonusPrefabs[Random.Range(0, bonusPrefabs.Length)];
            string bonusType = randomBonus.name; // Assuming each bonus prefab has a unique name

            // Check if this bonus type is already active
            
                // Choose a random position within the play area
                Vector3 spawnPosition = new Vector3(
                    Random.Range(bonusSpawnMinX, bonusSpawnMaxX),
                    Random.Range(bonusSpawnMinY, bonusSpawnMaxY),
                    0f
                );

                // Instantiate the bonus
                Instantiate(randomBonus, spawnPosition, Quaternion.identity);

              

                // Set the next time when a bonus can be spawned
                nextBonusSpawnTime = Time.time + bonusSpawnRate; // Update the next spawn time
            
        }
    }



    public void endGame()
    {
        audioSource.volume = volume; // Set the audio source volume
        audioSource.PlayOneShot(gameOverSound);

        // Wait for 5 seconds before resetting the game
        StartCoroutine(ResetGameAfterDelay(5f));
        // Hide the score text
        scoreText.gameObject.SetActive(false);
        ballsLeftText.gameObject.SetActive(false);
        // Show the final score text
        finalScoreText.gameObject.SetActive(true);
        finalScoreText.text = "Score: " + score.ToString();

        // Set the game state to not running
        isGameOn = false;

        // Update the high score if the current score is greater
        if (score > highScore)
        {
            highScore = score;  // Update high score
            highScoreText.text = "High Score: " + highScore.ToString();  // Update UI
        }

        // Wait for 5 seconds before resetting the game
        StartCoroutine(ResetGameAfterDelay(5f));
    }

    // Coroutine to reset the game state after a delay
    private IEnumerator ResetGameAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);  // Wait for the specified delay
                                                 //remove all bonus on screen
                                                 // Remove all bonuses on screen
        GameObject[] bonuses = GameObject.FindGameObjectsWithTag("Bonus");
        foreach (GameObject bonus in bonuses)
        {
            Destroy(bonus);
        }


        // Reset score and balls left
        score = 0;
        ballsLeft = startingBalls;

        // Hide the final score text
        finalScoreText.gameObject.SetActive(false);

        // Show the title screen
        titleScreen.SetActive(true);

        // Ensure the score text and high score text are reset
        scoreText.gameObject.SetActive(false);
        
        highScoreText.gameObject.SetActive(true);

        // Optionally, update score display if the game is restarted
        UpdateScoreUI();
        Debug.Log("game ready to start again");
    }


    // Call this method when a ball is destroyed to decrement ballOnScreen
    public void BallDestroyed()
    {
        ballOnScreen--; // Decrease the count of balls on screen
    }
}
