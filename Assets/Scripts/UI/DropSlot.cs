using UnityEngine;
using UnityEngine.UI;

public class DropSlot : MonoBehaviour
{
    private Image slotImage;
    private Color originalColor;
    private UIManager uiManager;

    [HideInInspector] public bool ocupado = false;

    private void Start()
    {
        slotImage = GetComponent<Image>();
        if (slotImage != null)
        {
            originalColor = slotImage.color;
        }
        
        // Buscar el UIManager en la escena
        uiManager = FindObjectOfType<UIManager>();
    }

    public void PaintSlot(Color color)
    {
        if (slotImage != null)
        {
            slotImage.color = color;
            ocupado = true;
            
            // Notificar al UIManager que actualice el color del botón
            if (uiManager != null)
            {
                uiManager.UpdateButtonColor();
            }
        }
    }

    public void ResetColor()
    {
        if (slotImage != null)
        {
            slotImage.color = originalColor;
            ocupado = false;
            
            // Notificar al UIManager que actualice el color del botón
            if (uiManager != null)
            {
                uiManager.UpdateButtonColor();
            }
        }
    }
}