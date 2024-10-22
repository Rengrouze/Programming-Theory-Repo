using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bonus : MonoBehaviour
{
    // Handle common properties like how long the bonus lasts (if applicable)
    [SerializeField] protected float bonusDuration = 4f;

    // This method will be called when a bonus is collected
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {

            ApplyBonus();
            Destroy(gameObject); // Destroy the bonus object after it has been collected
        }
    }
    public void Update()
    {
        transform.Rotate(0f, 0f, 360f * Time.deltaTime);

    }


    // Each specific bonus will implement this method
    protected abstract void ApplyBonus();
}
