using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUIRenderer : MonoBehaviour
{
    public GameObject itemIconPrefab;
    public Transform inventoryPanel;

    private List<GameObject> currentIcons = new List<GameObject>();

    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        foreach (GameObject icon in currentIcons)
        {
            Destroy(icon);
        }
        currentIcons.Clear();

        var items = InventoryManager.Instance.GetItems();
        foreach (var item in items)
        {
            GameObject iconGO = Instantiate(itemIconPrefab, inventoryPanel);
            iconGO.GetComponent<Image>().sprite = item.icon;
            currentIcons.Add(iconGO);
        }
    }
}
