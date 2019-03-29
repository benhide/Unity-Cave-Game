using UnityEngine;

// Firebase push data helper class
public class FirebasePushData : MonoBehaviour
{
    // Firebase settings reference
    public FirebaseSettings settings;

    // Push data from object
    public void PushData(object obj)
    {
        FirebaseManager.Push(settings, obj, null);
    }
}
