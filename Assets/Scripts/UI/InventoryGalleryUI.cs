using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class InventoryGalleryUI : MonoBehaviour
{
    public Image iconDisplay;
    public TMP_Text itemNameText;
    public TMP_Text descriptionText;
    public Button nextButton;
    public Button prevButton;

    public GameObject panelRoot;

    private List<GameItem> clues = new List<GameItem>();
    private int currentIndex = 0;

    void Awake()
    {
        DontDestroyOnLoad(gameObject); // mantiene la UI entre escenas
        gameObject.SetActive(false);   // lo ocultamos al iniciar
    }

    public void Show()
    {
        clues = InventoryManager.Instance.GetEquippedItems()
            .FindAll(item => item.isEvidence); // solo las pruebas

        gameObject.SetActive(true);

        if (clues.Count == 0)
        {
            iconDisplay.sprite = null;
            itemNameText.text = "No hay pistas recogidas.";
            return;
        }

        currentIndex = 0;
        UpdateUI();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Next()
    {
        if (clues.Count == 0) return;
        currentIndex = (currentIndex + 1) % clues.Count;
        UpdateUI();
    }

    public void Previous()
    {
        if (clues.Count == 0) return;
        currentIndex = (currentIndex - 1 + clues.Count) % clues.Count;
        UpdateUI();
    }

    private void UpdateUI()
    {
        var item = clues[currentIndex];
        iconDisplay.sprite = item.galleryImage != null ? item.galleryImage : item.icon;
        itemNameText.text = item.itemName;
        descriptionText.text = item.description;
    }
}
