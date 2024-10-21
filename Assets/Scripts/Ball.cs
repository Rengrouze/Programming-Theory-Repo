using UnityEngine;

public class Ball : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField] private AudioClip hitSound; // Sound to play when the ball hits something
    private AudioSource audioSource; // AudioSource to play the sound
    [Range(0f, 1f)] // This will create a slider in the inspector for volume control
    public float volume = 1f; // Volume range from 0 to 1

    private void Start()
    {
        // Get the GameManager reference
        gameManager = FindObjectOfType<GameManager>();
        audioSource = gameObject.AddComponent<AudioSource>(); // Add an AudioSource component to the ball
        audioSource.clip = hitSound; // Set the hit sound clip
    }

    private void OnDestroy()
    {
        // Call the method in GameManager when this ball is destroyed
        gameManager.BallDestroyed();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Play the hit sound when the ball collides with something
        if (audioSource != null && hitSound != null && audioSource.enabled)
        {
            audioSource.PlayOneShot(hitSound);
        }
    }
}
