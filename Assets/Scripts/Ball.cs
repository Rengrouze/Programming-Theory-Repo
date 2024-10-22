using UnityEngine;

public class Ball : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField] private AudioClip hitSound; // Sound to play when the ball hits something
    private AudioSource audioSource; // AudioSource to play the sound

    [Range(0f, 1f)] public float volume = 0.5f; // Volume range from 0 to 1
    private float lowerBound = -10f; // Lower bound for the ball's position


    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = hitSound; // Set the hit sound clip
    }

    private void OnDestroy()
    {
        gameManager.BallDestroyed(); // Call the method in GameManager when this ball is destroyed
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (audioSource != null && hitSound != null && audioSource.enabled)
        {
            audioSource.PlayOneShot(hitSound);
        }
    }


    private void Update()
    {
        // Check if the ball has fallen below the lower bound
        if (transform.position.y < lowerBound)
        {
            gameManager.SetBallOnScreen(gameManager.GetBallOnScreen() - 1); // Decrease the count of balls on screen
            Destroy(gameObject); // Destroy the ball when out of bounds

        }
    }
}
