using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RestorePhysics : MonoBehaviour
{
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void RestorePhysicsGravity()
    {
        rb.isKinematic = false;
        rb.useGravity = true;
    }
}
