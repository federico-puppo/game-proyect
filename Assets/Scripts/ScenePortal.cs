using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]
public class ScenePortal : MonoBehaviour
{
    [Tooltip("Nombre exacto de la escena a cargar")]
    public string targetScene;

    [Tooltip("Tag del objeto que puede activar este portal (por defecto: Player)")]
    public string activatorTag = "Player";

    [Tooltip("Tecla para activar el portal")]
    public KeyCode interactionKey = KeyCode.E;

    private bool isActivatorNearby = false;

    void Update()
    {
        if (isActivatorNearby && Input.GetKeyDown(interactionKey))
        {
            LoadTargetScene();
        }
    }

    private void LoadTargetScene()
    {
        if (!string.IsNullOrEmpty(targetScene))
        {
            SceneManager.LoadScene(targetScene);
        }
        else
        {
            Debug.LogWarning($"[ScenePortal] No se ha especificado una escena en el portal '{gameObject.name}'.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(activatorTag))
        {
            isActivatorNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(activatorTag))
        {
            isActivatorNearby = false;
        }
    }
}
