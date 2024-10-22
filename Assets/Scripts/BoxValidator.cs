using UnityEngine;

public class BoxValidator : MonoBehaviour
{
    [SerializeField] private int points;  // Points to add when the ball enters the box
    [SerializeField] private AudioClip pointSound; // Sound clip for scoring
    [Range(0f, 1f)] public float volume = 1f; // Volume range from 0 to 1

    private GameManager gameManager;  // Reference to the GameManager script
    private AudioSource audioSource; // AudioSource to play sounds

    private void Start()
    {
        InitializeComponents();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsBall(other))
        {
            HandleBallEntry(other);
        }
    }

    private void InitializeComponents()
    {
        gameManager = FindObjectOfType<GameManager>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = volume; // Set the audio source volume
    }

    private bool IsBall(Collider other)
    {

        return other.CompareTag("Ball");
    }


    private void HandleBallEntry(Collider ball)
    {
        UpdateScore(); // Update the score
        PlayPointSound(); // Play the sound effect
        Destroy(ball.gameObject); // Destroy the ball
    }

    private void UpdateScore()
    {
        int scoreToAdd = points;
        gameManager.updateScore(scoreToAdd); // Call the updateScore function from the GameManager script
    }

    private void PlayPointSound()
    {
        if (audioSource != null && pointSound != null)
        {
            audioSource.PlayOneShot(pointSound);
        }
    }
}
