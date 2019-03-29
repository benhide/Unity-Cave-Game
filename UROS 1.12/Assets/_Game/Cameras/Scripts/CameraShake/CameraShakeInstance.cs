using UnityEngine;

// Modified version of the "EZ Camera Shake v1.0.2"
// Created By Kevin Somers

// Global camera shake state enum
public enum CAMERA_SHAKE_STATE { FADING_IN, FADING_OUT, SUSTAINED, INACTIVE }

// Camera shake instance class
public class CameraShakeInstance
{
    // The intensity of the shake. It is recommended that you use ScaleMagnitude to alter the magnitude of a shake.
    public float magnitude;

    // Roughness of the shake. It is recommended that you use ScaleRoughness to alter the roughness of a shake.
    public float roughness;

    // How much influence this shake has over the local position axes of the camera.
    public Vector3 positionInfluence;

    // How much influence this shake has over the local rotation axes of the camera.
    public Vector3 rotationInfluence;

    // Should this shake be removed from the CameraShakeInstance list when not active?
    public bool deleteOnInactive = true;

    // Camera shake variables
    float roughMod = 1;
    float magnMod = 1;
    float fadeOutDuration;
    float fadeInDuration;
    bool sustain;
    float currentFadeTime;
    float tick = 0;
    Vector3 amt;

    // Create a new instance that will shake once and fade over the given number of seconds.
    public CameraShakeInstance(float magnitude, float roughness, float fadeInTime, float fadeOutTime)
    {
        this.magnitude = magnitude;
        fadeOutDuration = fadeOutTime;
        fadeInDuration = fadeInTime;
        this.roughness = roughness;

        if (fadeInTime > 0)
        {
            sustain = true;
            currentFadeTime = 0;
        }

        else
        {
            sustain = false;
            currentFadeTime = 1;
        }

        tick = Random.Range(-100, 100);
    }

    // Create a new instance that will start a sustained shake
    public CameraShakeInstance(float magnitude, float roughness)
    {
        this.magnitude = magnitude;
        this.roughness = roughness;
        sustain = true;

        tick = Random.Range(-100, 100);
    }

    // Update the camera shake
    public Vector3 UpdateShake()
    {
        amt.x = Mathf.PerlinNoise(tick, 0) - 0.5f;
        amt.y = Mathf.PerlinNoise(0, tick) - 0.5f;
        amt.z = Mathf.PerlinNoise(tick, tick) - 0.5f;

        if (fadeInDuration > 0 && sustain)
        {
            if (currentFadeTime < 1) currentFadeTime += Time.deltaTime / fadeInDuration;
            else if (fadeOutDuration > 0) sustain = false;
        }

        if (!sustain) currentFadeTime -= Time.deltaTime / fadeOutDuration;

        if (sustain) tick += Time.deltaTime * roughness * roughMod;
        else tick += Time.deltaTime * roughness * roughMod * currentFadeTime;

        return amt * magnitude * magnMod * currentFadeTime;
    }

    // Starts a fade out over the given number of seconds
    public void StartFadeOut(float fadeOutTime)
    {
        if (fadeOutTime == 0)
            currentFadeTime = 0;

        fadeOutDuration = fadeOutTime;
        fadeInDuration = 0;
        sustain = false;
    }

    // Starts a fade in over the given number of seconds
    public void StartFadeIn(float fadeInTime)
    {
        if (fadeInTime == 0)
            currentFadeTime = 1;

        fadeInDuration = fadeInTime;
        fadeOutDuration = 0;
        sustain = true;
    }

    // Scales this shake's roughness while preserving the initial Roughness
    public float ScaleRoughness
    {
        get { return roughMod; }
        set { roughMod = value; }
    }

    // Scales this shake's magnitude while preserving the initial Magnitude
    public float ScaleMagnitude
    {
        get { return magnMod; }
        set { magnMod = value; }
    }

    // A normalized value (about 0 to about 1) that represents the current level of intensity
    public float NormalizedFadeTime
    {
        get { return currentFadeTime; }
    }

    // Is the camera shaking
    bool IsShaking
    {
        get { return currentFadeTime > 0 || sustain; }
    }

    // Is the shake fading out
    bool IsFadingOut
    {
        get { return !sustain && currentFadeTime > 0; }
    }

    // Is the shake fading in
    bool IsFadingIn
    {
        get { return currentFadeTime < 1 && sustain && fadeInDuration > 0; }
    }

    // Gets the current state of the shake
    public CAMERA_SHAKE_STATE CurrentState
    {
        get
        {
            if (IsFadingIn) return CAMERA_SHAKE_STATE.FADING_IN;
            else if (IsFadingOut) return CAMERA_SHAKE_STATE.FADING_OUT;
            else if (IsShaking) return CAMERA_SHAKE_STATE.SUSTAINED;
            else return CAMERA_SHAKE_STATE.INACTIVE;
        }
    }
}