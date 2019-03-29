using UnityEngine;

// Modified version of the "EZ Camera Shake v1.0.2"
// Created By Kevin Somers

// Camera utilities class
public class CameraUtilities
{
    // Smoothes a Vector3 that represents euler angles.
    public static Vector3 SmoothDampEuler(Vector3 current, Vector3 target, ref Vector3 velocity, float smoothTime)
    {
        Vector3 vec;

        vec.x = Mathf.SmoothDampAngle(current.x, target.x, ref velocity.x, smoothTime);
        vec.y = Mathf.SmoothDampAngle(current.y, target.y, ref velocity.y, smoothTime);
        vec.z = Mathf.SmoothDampAngle(current.z, target.z, ref velocity.z, smoothTime);

        return vec;
    }

    // Multiplies each element in Vector3 vec by the corresponding element of Vector3 vec2
    public static Vector3 MultiplyVectors(Vector3 vec, Vector3 vec2)
    {
        vec.x *= vec2.x;
        vec.y *= vec2.y;
        vec.z *= vec2.z;

        return vec;
    }
}