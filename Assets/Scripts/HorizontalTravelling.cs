using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalTravelling : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float minX = -4f;
    [SerializeField] private float maxX = 4f;

   
    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the new X position using Mathf.PingPong
        float newX = Mathf.PingPong(Time.time * speed, maxX - minX) + minX;

        // Update the object's position
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}
