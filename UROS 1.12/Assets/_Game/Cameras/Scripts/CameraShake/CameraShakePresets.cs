using UnityEngine;

// Modified version of the "EZ Camera Shake v1.0.2"
// Created By Kevin Somers

// Camera shake presets
public static class CameraShakePresets
{
    // One shot shake for rock damage
    public static CameraShakeInstance RockDamaged
    {
        get
        {
            CameraShakeInstance instance = new CameraShakeInstance(1.0f, 1.5f, 0.1f, 0.5f);
            instance.positionInfluence = Vector3.one * 0.1f;
            instance.rotationInfluence = Vector3.one;
            return instance;
        }
    }

    // One shot shake for crystal damage
    public static CameraShakeInstance CrystalDamaged
    {
        get
        {
            CameraShakeInstance instance = new CameraShakeInstance(2.0f, 3.0f, 0.1f, 1.0f);
            instance.positionInfluence = Vector3.one * 0.1f;
            instance.rotationInfluence = Vector3.one;
            return instance;
        }
    }
}

//public static class CameraShakePresets
//{
//    // [One-Shot] A high magnitude, short, yet smooth shake.
//    public static CameraShakeInstance Bump
//    {
//        get
//        {
//            CameraShakeInstance c = new CameraShakeInstance(2.5f, 4, 0.1f, 0.75f);
//            c.positionInfluence = Vector3.one * 0.15f;
//            c.rotationInfluence = Vector3.one;
//            return c;
//        }
//    }
//
//    // [One-Shot] An intense and rough shake.
//    public static CameraShakeInstance Explosion
//    {
//        get
//        {
//            CameraShakeInstance c = new CameraShakeInstance(5f, 10, 0, 1.5f);
//            c.positionInfluence = Vector3.one * 0.25f;
//            c.rotationInfluence = new Vector3(4, 1, 1);
//            return c;
//        }
//    }
//
//    // [Sustained] A continuous, rough shake.
//    public static CameraShakeInstance Earthquake
//    {
//        get
//        {
//            CameraShakeInstance c = new CameraShakeInstance(0.6f, 3.5f, 2f, 10f);
//            c.positionInfluence = Vector3.one * 0.25f;
//            c.rotationInfluence = new Vector3(1, 1, 4);
//            return c;
//        }
//    }
//
//    // [Sustained] A bizarre shake with a very high magnitude and low roughness.
//    public static CameraShakeInstance BadTrip
//    {
//        get
//        {
//            CameraShakeInstance c = new CameraShakeInstance(10f, 0.15f, 5f, 10f);
//            c.positionInfluence = new Vector3(0, 0, 0.15f);
//            c.rotationInfluence = new Vector3(2, 1, 4);
//            return c;
//        }
//    }
//
//    // [Sustained] A subtle, slow shake. 
//    public static CameraShakeInstance HandheldCamera
//    {
//        get
//        {
//            CameraShakeInstance c = new CameraShakeInstance(1f, 0.25f, 5f, 10f);
//            c.positionInfluence = Vector3.zero;
//            c.rotationInfluence = new Vector3(1, 0.5f, 0.5f);
//            return c;
//        }
//    }
//
//    // [Sustained] A very rough, yet low magnitude shake.
//    public static CameraShakeInstance Vibration
//    {
//        get
//        {
//            CameraShakeInstance c = new CameraShakeInstance(0.4f, 20f, 2f, 2f);
//            c.positionInfluence = new Vector3(0, 0.15f, 0);
//            c.rotationInfluence = new Vector3(1.25f, 0, 4);
//            return c;
//        }
//    }
//
//    // [Sustained] A slightly rough, medium magnitude shake.
//    public static CameraShakeInstance RoughDriving
//    {
//        get
//        {
//            CameraShakeInstance c = new CameraShakeInstance(1, 2f, 1f, 1f);
//            c.positionInfluence = Vector3.zero;
//            c.rotationInfluence = Vector3.one;
//            return c;
//        }
//    }
//}