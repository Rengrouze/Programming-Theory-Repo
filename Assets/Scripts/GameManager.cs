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

    public bool isGameOn = false;

    [SerializeField] GameObject ball;
    [SerializeField] private float spawnRate = 0.25f;  // Time between spawns (4 per second)
    private float nextSpawnTime = 0f;  // Keeps track of when the next ball can be spawned
    [SerializeField] GameObject titleScreen;  // Reference to the title screen object
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
            ballsLeftText.text = "Peebles left : " + ballsLeft;
            ballsLeftText.gameObject.SetActive(true);
            dropBall();

            // Check if the game should end
            if (ballsLeft <= 0 && ballOnScreen <= 0)
            {
                endGame();
            }
        }
    }


    void dropBall()
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

    public void updateScore(int points)
    {
        score += points;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        scoreText.text = score.ToString();
    }

    // Function called by the Start Button
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
