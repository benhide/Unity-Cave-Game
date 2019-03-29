using UnityEngine;
using System.Collections.Generic;

// Modified version of the "EZ Camera Shake v1.0.2"
// Created By Kevin Somers

// Camera shake class
public class CameraShaker : MonoBehaviour
{
    // Static CameraShaker instance
    public static CameraShaker Instance;

    // The default position influcence of all shakes created by this shaker
    public Vector3 DefaultPosInfluence = new Vector3(0.15f, 0.15f, 0.15f);

    // The default rotation influcence of all shakes created by this shaker
    public Vector3 DefaultRotInfluence = new Vector3(1, 1, 1);

    // Position and rotation vectors
    Vector3 posAddShake;
    Vector3 rotAddShake;

    // List of camera shake instances
    List<CameraShakeInstance> cameraShakeInstances = new List<CameraShakeInstance>();

    // Use this for initialization
    void Start()
    {
        // Set static instance to this
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        // Initialise the shake values
        posAddShake = Vector3.zero;
        rotAddShake = Vector3.zero;

        // Loop through shake instances
        for (int i = 0; i < cameraShakeInstances.Count; i++)
        {
            // Break from loop if exceeded instances count
            if (i >= cameraShakeInstances.Count)
                break;

            // Get the current camera shake instance
            CameraShakeInstance camShakeInstance = cameraShakeInstances[i];

            // If the current instance is inactive
            if (camShakeInstance.CurrentState == CAMERA_SHAKE_STATE.INACTIVE && camShakeInstance.deleteOnInactive)
            {
                // Remove from list from the list
                cameraShakeInstances.RemoveAt(i);
                i--;
            }

            // Else if current camera shake instance is not inactive
            else if (camShakeInstance.CurrentState != CAMERA_SHAKE_STATE.INACTIVE)
            {
                // Update camera position and rotation
                posAddShake += CameraUtilities.MultiplyVectors(camShakeInstance.UpdateShake(), camShakeInstance.positionInfluence);
                rotAddShake += CameraUtilities.MultiplyVectors(camShakeInstance.UpdateShake(), camShakeInstance.rotationInfluence);
            }
        }

        // Set camera position and rotation
        transform.localPosition = posAddShake;
        transform.localEulerAngles = rotAddShake;
    }

    // Starts a shake using the given preset
    public CameraShakeInstance Shake(CameraShakeInstance shake)
    {
        // Add preset to the list of camera shake instaces
        cameraShakeInstances.Add(shake);
        return shake;
    }
}