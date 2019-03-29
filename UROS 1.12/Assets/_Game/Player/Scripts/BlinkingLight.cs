using UnityEngine;

// linking light class
public class BlinkingLight : MonoBehaviour
{
    ///////////////////////////Variables////////////////////////////

    // Min/max values
    public float minimum;
    public float maximum;

    // Starting value for the Lerp
    static float timer = 0.0f;
    public Light blinkingLight;

    ///////////////////////End of Variables//////////////////////////



    ///////////////////////////Functions////////////////////////////

    // Use this for initialization
    void Start()
    {
        blinkingLight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        // Set the intensity 
        blinkingLight.intensity = Mathf.Lerp(minimum, maximum, timer);

        // Increase the t interpolater
        timer += Time.deltaTime;

        // Check if the interpolator has reached 1.0
        if (timer > 1.0f)
        {
            // Swap values
            float temp = maximum;
            maximum = minimum;
            minimum = temp;
            timer = 0.0f;
        }
    }

    ///////////////////////End of Functions/////////////////////////
}