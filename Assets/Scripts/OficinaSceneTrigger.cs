using UnityEngine;
using UnityEngine.SceneManagement;

public class OficinaSceneTrigger : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("Escena actual: " + SceneManager.GetActiveScene().name);
        GameManager.Instance.CheckSummaryTrigger("Oficina");

        Debug.Log("¿Mostrar resumen? " + GameManager.Instance.ShouldShowSummary());

        if (GameManager.Instance.ShouldShowSummary())
        {
            MentalSummaryUI resumen = FindObjectOfType<MentalSummaryUI>();
            if (resumen != null)
            {
                Debug.Log("MentalSummaryUI encontrado, llamando a MostrarResumen()");
                resumen.MostrarResumen();
            }
            else
            {
                Debug.LogWarning("No se encontró MentalSummaryUI en la escena.");
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F9))
        {
            Debug.Log("[CHEAT] Forzando resumen mental");

            GameManager.Instance.MarkLocationVisited("CrimeScene");
            GameManager.Instance.MarkLocationVisited("NewspapperOffice");
            GameManager.Instance.MarkLocationVisited("Bar");

            GameManager.Instance.CheckSummaryTrigger("Oficina");

            if (GameManager.Instance.ShouldShowSummary())
            {
                var resumen = FindObjectOfType<MentalSummaryUI>();
                if (resumen != null)
                {
                    resumen.MostrarResumen();
                }
            }
        }
    }

}
