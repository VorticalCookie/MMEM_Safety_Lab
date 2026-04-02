using UnityEngine;

public class ShakeInteractable : MonoBehaviour
{

    [SerializeField] private float shakeAngle = 5f;
    [SerializeField] private float shakeSpeed = 5f;

    private bool isShaking = false;
    private Quaternion originalRotation;

    void Start()
    {
        originalRotation = transform.rotation;
    }

    void Update()
    {
        if (isShaking)
        {
            float zRotation = Mathf.Sin(Time.time * shakeSpeed) * shakeAngle;
            transform.rotation = originalRotation * Quaternion.Euler(0f, 0f, zRotation);
        }
    }

    // Flip-flop shake on/off
    public void ToggleShake()
    {
        isShaking = !isShaking;

        if (!isShaking)
        {
            transform.rotation = originalRotation;
        }
    }

    // Failsafe: force stop and reset
    public void StopShake()
    {
        isShaking = false;
        transform.rotation = originalRotation;
    }
}
