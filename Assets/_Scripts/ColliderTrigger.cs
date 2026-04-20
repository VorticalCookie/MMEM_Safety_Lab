using UnityEngine;

public class ColliderTrigger : MonoBehaviour
{
    [Header("Mesh Renderer to Enable")]
    public MeshRenderer meshRendererToEnable;
    [Header("Pop Up Panel to Enable")]
    public GameObject popUpPanelToEnable;
    public GameObject optionalpPopUpPanelToEnable;

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
        }
    }
}
