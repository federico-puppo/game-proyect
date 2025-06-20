using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]
public class ScenePortalWithMentalScene : MonoBehaviour
{
    [Tooltip("Nombre exacto de la escena a cargar luego del plano mental")]
    public string targetScene;

    [Tooltip("Tag del objeto que puede activar este portal (por defecto: Player)")]
    public string activatorTag = "Player";

    [Tooltip("Tecla para activar el portal")]
    public KeyCode interactionKey = KeyCode.E;

    [Tooltip("Layout mental que se muestra antes de cambiar de escena")]
    public GameObject mentalSceneUI;

    [Tooltip("¿Omitir la UI mental en este portal?")]
    public bool skipMentalScene = false;

    private bool isActivatorNearby = false;
    private bool mentalSceneOpen = false;

    void Update()
    {
        if (isActivatorNearby && Input.GetKeyDown(interactionKey))
        {
            if (skipMentalScene)
            {
                LoadTargetScene();
            }
            else if (!mentalSceneOpen)
            {
                OpenMentalScene();
            }
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

    private void OpenMentalScene()
    {
        if (mentalSceneUI != null)
        {
            mentalSceneUI.SetActive(true);
            mentalSceneOpen = true;
            Time.timeScale = 0f; // Pausar el juego

            // Mostrar inventario y estilos
            GameManager.Instance.RefreshInventoryUI();
            GameManager.Instance.UpdateStyleOptions();
        }
    }

    public void ConfirmTransition()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(targetScene);
    }

    public void CancelTransition()
    {
        Time.timeScale = 1f;
        if (mentalSceneUI != null)
        {
            mentalSceneUI.SetActive(false);
            mentalSceneOpen = false;
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