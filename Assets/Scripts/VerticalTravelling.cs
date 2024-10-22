using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalTravelling : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float minY = -4f;
    [SerializeField] private float maxY = 4f;

    // Update is called once per frame
    void Update()
    {
        // Calculate the new Y position using Mathf.PingPong
        float newY = Mathf.PingPong(Time.time * speed, maxY - minY) + minY;

        // Update the object's position
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
