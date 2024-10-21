using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateY : MonoBehaviour
{
    [SerializeField] private float speed = 2f;  // Speed of rotation
    [SerializeField] private bool reverseRotation = false;

    // Update is called once per frame
    void Update()
    {
        if (reverseRotation)
        {
            transform.Rotate(0f, -speed * Time.deltaTime, 0f);
        }
        else
        {
            transform.Rotate(0f, speed * Time.deltaTime, 0f);
        }

        // Rotate around the Y-axis
        
    }
}
