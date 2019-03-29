using UnityEngine;
using System.Collections.Generic;

// Written by Steve Streeting 2017
// License: CC0 Public Domain http://creativecommons.org/publicdomain/zero/1.0/

public class LightFlicker : MonoBehaviour
{
    ///////////////////////////Variables////////////////////////////

    [Header("Light Flicker settings")]
    public new Light light;
    [Range(0, 20)]
    public float minIntensity;
    [Range(0, 20)]
    public float maxIntensity;
    [Range(0, 5)]
    public float minRange;
    [Range(0, 5)]
    public float maxRange;
    [Range(1, 50)]
    public int intensitySmoothing;
    [Range(1, 50)]
    public int rangeSmoothing;

    // Continuous average calculation via FIFO queue
    Queue<float> smoothIntensityQueue;
    Queue<float> smoothRangeQueue;
    float lastIntensitySum = 0;
    float lastRangeSum = 0;

    ///////////////////////End of Variables//////////////////////////



    ///////////////////////////Functions////////////////////////////

    // Use this for initialization
    void Start()
    {
        // Assign the queue and light
        smoothIntensityQueue = new Queue<float>(intensitySmoothing);
        smoothRangeQueue = new Queue<float>(rangeSmoothing);
        light = GetComponent<Light>();

        // Clamp the min max values
        ClampMinMaxValues();
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate new smoothed average
        lastIntensitySum = LightChanger(minIntensity, maxIntensity, lastIntensitySum, intensitySmoothing, smoothIntensityQueue);
        light.intensity = lastIntensitySum / smoothIntensityQueue.Count;

        lastRangeSum = LightChanger(minRange, maxRange, lastRangeSum, rangeSmoothing, smoothRangeQueue);
        light.range = lastRangeSum / smoothRangeQueue.Count;
    }

    // Clamp the min/max cave generation settings
    void ClampMinMaxValues()
    {
        minIntensity = Mathf.Clamp(minIntensity, 0, maxIntensity);
        maxIntensity = Mathf.Clamp(maxIntensity, minIntensity, 20);
        minRange = Mathf.Clamp(minRange, 0, maxRange);
        maxRange = Mathf.Clamp(maxRange, minRange, 5);
    }

    // Reset the light flicker
    public void Reset()
    {
        smoothIntensityQueue.Clear();
        lastIntensitySum = 0;
    }

    // Change the lights intensity and range
    float LightChanger(float min, float max, float sum, int smoothing, Queue<float> queue)
    {
        // Pop off an item if too big
        while (queue.Count >= smoothing)
            sum -= queue.Dequeue();

        // Generate random new value, calculate new average
        float newValue = Random.Range(min, max);
        queue.Enqueue(newValue);
        return sum += newValue;
    }

    ///////////////////////End of Functions/////////////////////////
}