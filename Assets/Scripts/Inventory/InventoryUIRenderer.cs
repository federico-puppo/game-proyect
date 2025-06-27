using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class InventoryUIRenderer : MonoBehaviour
{
    public GameObject itemIconPrefab;
    public Transform inventoryPanel;

    public TMP_Text evidenceCounterText;

    private List<GameObject> currentIcons = new List<GameObject>();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        // Limpia los íconos actuales
        foreach (GameObject icon in currentIcons)
        {
            Destroy(icon);
        }
        currentIcons.Clear();

        // Trae todos los ítems equipados desde InventoryManager
        var items = InventoryManager.Instance.GetEquippedItems();

        foreach (var item in items)
        {
            // Mostrar solo si NO es evidencia o SI es la pista de revelación
            if (!item.isEvidence || item.itemName.ToLower().Contains("revelacion") || item.isKeyItem)
            {
                GameObject iconGO = Instantiate(itemIconPrefab, inventoryPanel);
                iconGO.GetComponent<Image>().sprite = item.icon;
                currentIcons.Add(iconGO);
            }
        }

        // Mostrar contador numérico de evidencias
        if (evidenceCounterText != null)
        {
            int count = InventoryManager.Instance.GetItems().FindAll(i => i.isEvidence).Count;
            evidenceCounterText.text = $"{count}";
        }
    }
}

