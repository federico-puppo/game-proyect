using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalUIController : MonoBehaviour
{
    public string escenaMenuPrincipal = "MenuPrincipal";
    public string escenaInicioJuego = "Oficina"; // Escena que actúa como "nuevo juego"

    public void ReiniciarJuego()
    {
        Time.timeScale = 1f; // Por si estaba pausado
        GameManager.Instance.ResetGame(); // Reinicia el estado del juego
        SceneManager.LoadScene(escenaInicioJuego);
    }

    public void IrAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(escenaMenuPrincipal);
    }
}
