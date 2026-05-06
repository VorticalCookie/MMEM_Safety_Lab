using UnityEngine;
using UnityEngine.XR;

public class PlayerRespawn : MonoBehaviour
{
    [Header("Teleport Destination")]
    public Transform teleportDestination;

    [Header("Player Tag")]
    public string playerTag = "Player";

    [Header("XR Camera")]
    public Transform xrCamera; // Assign the XR camera (usually the "Main Camera" under the XR Rig)

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag) && teleportDestination != null)
        {
            // If using XR, move the rig so the camera aligns with the destination
            if (xrCamera != null && other.transform == xrCamera.root)
            {
                Vector3 cameraOffset = xrCamera.position - other.transform.position;
                Vector3 newRigPosition = teleportDestination.position - cameraOffset;
                other.transform.position = newRigPosition;
            }
            else
            {
                // Fallback: just move the object
                other.transform.position = teleportDestination.position;
            }

            // Optionally reset velocity if using Rigidbody
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }
}
