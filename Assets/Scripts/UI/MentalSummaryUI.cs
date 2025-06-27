using UnityEngine;
using TMPro;

public class MentalSummaryUI : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject visualPanel; // El panel visual que contiene la UI (debe estar activo/inactivo, no este script)
    public TMP_Text tituloResumen;
    public TMP_Text juicioInfoText;
    public GameObject btnJuicio;
    public GameObject btnConfrontar;

    public void MostrarResumen()
    {
        if (visualPanel == null)
        {
            Debug.LogError("[MentalSummaryUI] El panel visual no está asignado.");
            return;
        }

        visualPanel.SetActive(true);


        bool tieneRevelacion = GameManager.Instance.HasRevelation();
        bool todasLasPistas = GameManager.Instance.HasAllClues();
        int pistas = GameManager.Instance.GetClueCount();

        tituloResumen.text = "¿Cómo deseas proceder?";
        juicioInfoText.text = todasLasPistas
            ? "Reuniste todas las pruebas para llevar el caso a juicio."
            : "Tienes algunas pruebas. Puedes intentar llevarlas a juicio.";

        btnJuicio.SetActive(pistas > 0);
        btnConfrontar.SetActive(GameManager.Instance.PuedeConfrontar());

        Debug.Log("[MentalSummaryUI] Resumen mostrado correctamente.");
    }

    public void IrAJuicio()
    {
        GameManager.Instance.IrAJuicio();
    }

    public void IrAConfrontacion()
    {
        GameManager.Instance.IrAConfrontacion();
    }

    public void VolverAlMundo()
    {
        Time.timeScale = 1f;

        if (visualPanel != null)
            visualPanel.SetActive(false);

        Debug.Log("[MentalSummaryUI] Se volvió al mundo.");
    }
}
