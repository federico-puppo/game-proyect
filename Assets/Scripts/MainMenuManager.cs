using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Oficina"); // Cambia por el nombre de tu primera escena real
    }

    public void OpenSettings()
    {
        // Puedes cargar una página de configuración o activar un panel
        Debug.Log("Abrir configuración");
    }

    public void ShowCredits()
    {
        // Puedes cargar una escena de créditos o activar un panel
        Debug.Log("Mostrar créditos");
    }

    public void QuitGame()
    {
        Debug.Log("Salir del juego");
        Application.Quit();
    }
}
