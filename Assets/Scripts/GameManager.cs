using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int score { get; private set; } = 0; // Score is publicly readable but only writable within this class
    public int highScore { get; private set; } = 0; // Same for highScore
    public int startingBalls;
    public int ballsLeft { get; private set; } // Make ballsLeft read-only from outside
    private int ballOnScreen = 0; // Keep track of how many balls are currently on the screen
    private float timer = 0f; // Timer to count the time after the last ball is destroyed
    private float timeToEndGame = 2f; // Time before ending the game

    public bool isGameOn { get; private set; } = false; // Make isGameOn read-only from outside
    public int scoreMultiplier { get; private set; } = 1; // Same for scoreMultiplier

    [SerializeField] private float bonusSpawnRate = 2f; // Time between bonus spawns
    [SerializeField] GameObject[] bonusPrefabs; // Array to hold bonus prefabs
    private float nextBonusSpawnTime = 0f; // Keeps track of when the next bonus can be spawned

    [SerializeField] GameObject ball;
    [SerializeField] private float spawnRate = 0.25f; // Time between spawns (4 per second)
    private float nextSpawnTime = 0f; // Keeps track of when the next ball can be spawned

    [SerializeField] GameObject titleScreen; // Reference to the title screen object
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI highScoreText;
    [SerializeField] TextMeshProUGUI finalScoreText;
    [SerializeField] TextMeshProUGUI ballsLeftText;

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
            ballsLeftText.text = "Peebles left: " + ballsLeft;
            ballsLeftText.gameObject.SetActive(true);

            DropBall();
            SpawnBonuses();

            // Check if the game should end
            if (ballsLeft <= 0 && ballOnScreen <= 0)
            {
                timer += Time.deltaTime; // Increment the timer
                if (timer >= timeToEndGame) // Check if time to end the game
                {
                    EndGame();
                }
            }
            else
            {
                timer = 0f; // Reset the timer if there are still balls left
            }
        }
    }

    public void DropBall()
    {
        // Check if space is pressed and enough time has passed since the last spawn
        if ((Input.GetKey(KeyCode.Space) || Input.GetMouseButtonDown(0)) && Time.time >= nextSpawnTime && ballsLeft > 0)
        {
            // Spawn the ball
            Instantiate(ball, new Vector3(0, 9.19f, 0), Quaternion.identity);
            audioSource.volume = volume; // Set the audio source volume
            audioSource.PlayOneShot(popSound);
            ballOnScreen++; // Increment the ball counter

            // Remove a ball from reserve
            ballsLeft--;

            // Set the next time when a ball can be spawned
            nextSpawnTime = Time.time + spawnRate;
            Debug.Log("You have " + ballsLeft + " balls left");
        }
        else if (Input.GetKey(KeyCode.Space) && ballsLeft <= 0)
        {
            Debug.Log("No balls left");
        }
    }

    // Public method to update the score
    public void updateScore(int pointsToAdd)
    {
        score += pointsToAdd * scoreMultiplier; // Update score with multiplier
        UpdateScoreUI(); // Call a method to update the UI if needed
    }

    void UpdateScoreUI()
    {
        scoreText.text = score.ToString();
    }

    // Function called by the Start Button
    public void StartGame()
    {
        Debug.Log("startGame() was clicked");
        if (!isGameOn)
        {
            isGameOn = true;
            titleScreen.SetActive(false); // Hide the title screen
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

            // Choose a random position within the play area
            Vector3 spawnPosition = new Vector3(
                Random.Range(-6.45f, 6.45f),
                Random.Range(-1.8f, 5.30f),
                0f
            );

            // Instantiate the bonus
            Instantiate(randomBonus, spawnPosition, Quaternion.identity);

            // Set the next time when a bonus can be spawned
            nextBonusSpawnTime = Time.time + bonusSpawnRate; // Update the next spawn time
        }
    }

    public void EndGame()
    {
        audioSource.volume = volume; // Set the audio source volume
        audioSource.PlayOneShot(gameOverSound);

        // Wait for 5 seconds before resetting the game
        StartCoroutine(ResetGameAfterDelay(5f));
        scoreText.gameObject.SetActive(false);
        ballsLeftText.gameObject.SetActive(false);
        finalScoreText.gameObject.SetActive(true);
        finalScoreText.text = "Score: " + score.ToString();

        isGameOn = false;

        if (score > highScore)
        {
            highScore = score; // Update high score
            highScoreText.text = "High Score: " + highScore.ToString(); // Update UI
        }
    }

    private IEnumerator ResetGameAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay

        // Remove all bonuses on screen
        GameObject[] bonuses = GameObject.FindGameObjectsWithTag("Bonus");
        foreach (GameObject bonus in bonuses)
        {
            Destroy(bonus);
        }

        // Reset score and balls left
        score = 0;
        ballsLeft = startingBalls;

        finalScoreText.gameObject.SetActive(false);
        titleScreen.SetActive(true);
        scoreText.gameObject.SetActive(false);
        highScoreText.gameObject.SetActive(true);

        UpdateScoreUI();
        Debug.Log("Game ready to start again");
    }

    // Call this method when a ball is destroyed to decrement ballOnScreen
    public void BallDestroyed()
    {
        ballOnScreen--; // Decrease the count of balls on screen
    }

    // Getter for ballsLeft
    public int GetBallsLeft()
    {
        return ballsLeft;
    }

    // Setter for ballsLeft
    public void SetBallsLeft(int value)
    {
        ballsLeft = value;
    }

    // Getter and setter for scoreMultiplier
    public int ScoreMultiplier
    {
        get => scoreMultiplier;
        set => scoreMultiplier = value;
    }

    // Getter for ballOnScreen
    public int GetBallOnScreen()
    {
        return ballOnScreen;
    }

    // Setter for ballOnScreen
    public void SetBallOnScreen(int count)
    {
        ballOnScreen = count;
    }
}
