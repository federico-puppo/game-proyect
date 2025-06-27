using UnityEngine;

public class OficinaStartCheck : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Entrando a Oficina...");
        GameManager.Instance.CheckSummaryTrigger("Oficina");
        Debug.Log("Should show summary: " + GameManager.Instance.ShouldShowSummary());

        if (GameManager.Instance.ShouldShowSummary())
        {
            var summaryUI = FindObjectOfType<MentalSummaryUI>();
            if (summaryUI != null)
            {
                Debug.Log("Mostrando MentalSummaryUI");
                summaryUI.MostrarResumen();
            }
            else
            {
                Debug.LogWarning("MentalSummaryUI no se encontró en la escena.");
            }
        }
    }
}
