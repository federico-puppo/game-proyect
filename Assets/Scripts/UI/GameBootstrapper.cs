using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Este script debe estar en el Menú Principal. Limpia el juego antes de empezar.
/// </summary>
public class GameBootstrapper : MonoBehaviour
{
    [Header("Nombre de la escena de juego")]
    public string nombreEscenaInicial = "EscenaInicio";

    void Start()
    {
        LimpiarObjetosPersistentes();
        Time.timeScale = 1f; // Por si alguna escena anterior pausó el tiempo
    }

    public void NuevaPartida()
    {
        Debug.Log("[Bootstrapper] Iniciando nueva partida...");

        // Resetear sistemas principales
        GameManager.Instance?.ResetGame();
        InventoryManager.Instance?.ResetInventory();

        // Cargar escena inicial de juego
        SceneManager.LoadScene(nombreEscenaInicial);
    }

    private void LimpiarObjetosPersistentes()
    {
        // Lista de nombres exactos de objetos que deberían destruirse al volver al menú
        string[] objetosADestruir = {
            "InventoryUIRenderer",
            "InventoryGalleryUI",
            "MentalSummaryUIManager",
            "InventoryInputController"
        };

        foreach (string nombre in objetosADestruir)
        {
            GameObject obj = GameObject.Find(nombre);
            if (obj != null)
            {
                Destroy(obj);
                Debug.Log($"[Bootstrapper] Se destruyó {nombre}");
            }
        }

        // Elimina duplicados de GameManager o InventoryManager si los hay
        DestruirDuplicados<GameManager>();
        DestruirDuplicados<InventoryManager>();
    }

    private void DestruirDuplicados<T>() where T : MonoBehaviour
    {
        var managers = FindObjectsOfType<T>();
        if (managers.Length > 1)
        {
            for (int i = 1; i < managers.Length; i++)
            {
                Destroy(managers[i].gameObject);
                Debug.LogWarning($"[Bootstrapper] Duplicado de {typeof(T).Name} destruido.");
            }
        }
    }
}
