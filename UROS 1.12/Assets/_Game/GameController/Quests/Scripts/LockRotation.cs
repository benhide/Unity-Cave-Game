using UnityEngine;

public class LockRotation : MonoBehaviour
{
    Quaternion startRotation;
    Vector3 startPosition;

    // Use this for initialization
    void Start()
    {
        startRotation = transform.rotation;
        startPosition = transform.position;
    }

    // Update is called once per frame
    void /*Late*/Update()
    {
        transform.rotation = startRotation;
        transform.position = startPosition;
    }
}
