using UnityEngine;
using TMPro;

public class DoorLabel : MonoBehaviour
{
    [Tooltip("Texto que se mostrará sobre la puerta (nombre destino)")]
    public string destino;

    private GameObject labelCanvas;
    private TMP_Text labelText;

    private void Awake()
    {
        labelCanvas = transform.Find("LabelCanvas")?.gameObject;
        labelText = labelCanvas?.GetComponentInChildren<TMP_Text>();

        if (labelCanvas != null)
        {
            labelCanvas.SetActive(false);
            UpdateTexto();
        }
    }

    public void UpdateTexto()
    {
        if (labelText != null)
            labelText.text = $"{destino}\n[E] para entrar";
    }

    public void ShowLabel()
    {
        if (labelCanvas != null)
        {
            labelCanvas.SetActive(true);
            UpdateTexto();
        }
    }

    public void HideLabel()
    {
        if (labelCanvas != null)
            labelCanvas.SetActive(false);
    }
}
