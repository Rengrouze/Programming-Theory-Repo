using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : Bonus
{
    private GameManager gameManager;
    [SerializeField] private GameObject ballPrefab; // The ball prefab to spawn
    [SerializeField] private int ballCount = 3; // Number of balls to spawn

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    protected override void ApplyBonus()
    {
        for (int i = 0; i < ballCount; i++)
        {
            Instantiate(ballPrefab, transform.position, Quaternion.identity);
            gameManager.SetBallOnScreen(gameManager.GetBallOnScreen() + 1); // Use the setter to update ballOnScreen
        }
        Debug.Log($"{ballCount} balls have spawned");
    }
}
