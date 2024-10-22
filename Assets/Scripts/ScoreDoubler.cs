using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreDoubler : Bonus
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    protected override void ApplyBonus()
    {
        StartCoroutine(DoubleScore());
    }

    private IEnumerator DoubleScore()
    {
        Debug.Log("double score bonus !");
        gameManager.scoreMultiplier = 2; // Apply the score multiplier
        yield return new WaitForSeconds(bonusDuration); // Wait for the bonus duration
        gameManager.scoreMultiplier = 1; // Reset the multiplier after the bonus ends
    }
}