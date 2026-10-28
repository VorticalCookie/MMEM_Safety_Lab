using UnityEngine;
using UnityEngine.Events;

public class ColliderTrigger : MonoBehaviour
{
    [Header("Mesh Renderer to Enable")]
    public MeshRenderer meshRendererToEnable;
    [Header("Pop Up Panel to Enable")]
    public GameObject popUpPanelToEnable;
    public GameObject optionalpPopUpPanelToEnable;

    //To run when player enters the collider
    [Header("Events On Triggered")]
    public UnityEvent onTriggered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger!");
            // Enable the assigned MeshRenderer
            if (meshRendererToEnable != null)
            {
                meshRendererToEnable.enabled = true;
            }
            if (popUpPanelToEnable != null)
            {
                popUpPanelToEnable.SetActive(true);
            }
            if (optionalpPopUpPanelToEnable != null)
            {
                optionalpPopUpPanelToEnable.SetActive(true);
            }

            onTriggered?.Invoke();
        }
    }
}
