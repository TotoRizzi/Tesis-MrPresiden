using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenLight : MonoBehaviour
{
    public float minTime = 0.1f; // Minimum time interval for the object to turn on/off
    public float maxTime = 0.5f; // Maximum time interval for the object to turn on/off
    public GameObject objectToToggle; // Reference to the object to toggle

    private bool isOn = true; // Flag to keep track of the current state of the object
    private float timer = 0f; // Timer to control the intervals between turning the object on/off
    private float timeToToggle = 0f; // Time to toggle the object

    void Start()
    {
        // Calculate the first random interval to toggle the object
        timeToToggle = Random.Range(minTime, maxTime);
    }

    void Update()
    {
        // Update the timer
        timer += Time.deltaTime;

        // Check if it's time to toggle the object
        if (timer >= timeToToggle)
        {
            // Toggle the visibility of the object
            isOn = !isOn;
            objectToToggle.SetActive(isOn);

            // Reset the timer and calculate a new random interval to toggle the object
            timer = 0f;
            timeToToggle = Random.Range(minTime, maxTime);
        }
    }
}