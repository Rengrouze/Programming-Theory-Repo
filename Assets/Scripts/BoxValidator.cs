using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxValidator : MonoBehaviour
{
    [SerializeField] private int points;  // Points to add when the ball enters the box
    private GameManager gameManager;  // Reference to the GameManager script

    [SerializeField] private AudioClip pointSound; // Reference to the sound clip for scoring
    private AudioSource audioSource; // AudioSource to play sounds

    [Range(0f, 1f)] // This will create a slider in the inspector for volume control
    public float volume = 1f; // Volume range from 0 to 1

    // Start is called before the first frame update
    void Start()
    {
        // Find the GameManager object and get the GameManager script
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        // Add an AudioSource component to this GameObject if it doesn't already exist
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    // This is called when another object enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger has the tag "Ball"
        if (other.CompareTag("Ball"))
        {
            // Call the updateScore function from the GameManager script
            gameManager.updateScore(points);

            // Set the audio source volume
            audioSource.volume = volume; // Set the audio source volume

            // Play the point sound
            audioSource.PlayOneShot(pointSound);

            // Destroy the ball object
            Destroy(other.gameObject);
        }
    }
}
