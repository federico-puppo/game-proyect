using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]
public class SimpleScenePortal : MonoBehaviour
{
    public string targetScene;
    public string activatorTag = "Player";
    public KeyCode interactionKey = KeyCode.E;

    private bool isPlayerNearby = false;

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(interactionKey))
        {
            if (!string.IsNullOrEmpty(targetScene))
            {
                SceneManager.LoadScene(targetScene);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(activatorTag))
        {
            isPlayerNearby = true;

            DoorLabel label = GetComponent<DoorLabel>();
            if (label != null) label.ShowLabel();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(activatorTag))
        {
            isPlayerNearby = false;

            DoorLabel label = GetComponent<DoorLabel>();
            if (label != null) label.HideLabel();
        }
    }
}
