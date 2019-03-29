using System.Collections;
using UnityEngine;

// Particle System Controller class
public class ParticleSystemController : MonoBehaviour
{
    // Static particle system controller instance
    public static ParticleSystemController instance;

    // Use this for initialization
    void Start()
    {
        instance = this;
    }

    // Instaniate a particle system
    public static void InstaniateParticleSystem(ParticleSystem particles, Vector3 position, Quaternion rotation)
    {
        ParticleSystem ps = Instantiate(particles, position + Vector3.up, rotation, instance.transform) as ParticleSystem;
        instance.StartCoroutine(DestroyParticleSystem(ps));
    }

    // Instaniate a particle system
    public static void InstaniateCubeParticleSystem(ParticleSystem particles, Transform cubeTransform)
    {
        ParticleSystem ps = Instantiate(particles, cubeTransform.position + Vector3.up, Quaternion.identity, instance.transform) as ParticleSystem;
        particles.shape.rotation.Set(cubeTransform.rotation.eulerAngles.x, cubeTransform.rotation.eulerAngles.y, cubeTransform.rotation.eulerAngles.z);
        instance.StartCoroutine(DestroyParticleSystem(ps));
    }

    // Destroy particle systems if needed
    static IEnumerator DestroyParticleSystem(ParticleSystem ps)
    {
        // While the particles are alive return null
        while (ps.IsAlive())
            yield return null;

        // Destroy the particle system
        Destroy(ps.gameObject);
    }
}