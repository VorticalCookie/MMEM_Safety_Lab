using UnityEngine;

public class SelfDeactivate : MonoBehaviour
{
    public float deactivateAfterSeconds = 3f;

    void OnEnable()
    {
        // Start the deactivation coroutine only if the GameObject is active
        if (gameObject.activeInHierarchy)
        {
            Invoke(nameof(Deactivate), deactivateAfterSeconds);
        }
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
