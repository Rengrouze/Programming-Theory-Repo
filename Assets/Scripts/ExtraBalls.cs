using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraBalls : Bonus
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    protected override void ApplyBonus()
    {

        int currentBalls = gameManager.GetBallsLeft(); // Get current balls left
        gameManager.SetBallsLeft(currentBalls + 5); // Add 5 extra balls to the player's reserve

        Debug.Log("5 balls gained");
    }
}
