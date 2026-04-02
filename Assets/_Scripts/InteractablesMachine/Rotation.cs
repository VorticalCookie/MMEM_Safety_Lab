using UnityEngine;

public class Rotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 100f;
    private bool isRotating = false;

    void Update()
    {
        if (isRotating)
        {
            transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
        }
    }

    public void ToggleRotation()
    {
        isRotating = !isRotating;
    }

    public void StopRotation()
    {
        isRotating = false;
    }
}