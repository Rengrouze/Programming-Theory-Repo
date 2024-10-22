using UnityEngine;

public class Ball : MonoBehaviour
{
    private GameManager gameManager;

  
    private float lowerBound = -10f; // Lower bound for the ball's position


    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        
    }

    private void OnDestroy()
    {
        gameManager.BallDestroyed(); // Call the method in GameManager when this ball is destroyed
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
