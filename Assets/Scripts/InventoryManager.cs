using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [SerializeField] private List<GameItem> items = new List<GameItem>();
    [SerializeField] private List<GameItem> equippedItems = new List<GameItem>();
    [SerializeField] private List<GameItem> keyItems = new List<GameItem>();
    [SerializeField] private List<GameItem> evidence = new List<GameItem>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Métodos para manejar objetos

    public void AddItem(GameItem item)
    {
        if (!items.Contains(item))
        {
            items.Add(item);

            if (item.isKeyItem)
                keyItems.Add(item);
            if (item.isEvidence)
                evidence.Add(item);
        }
    }
    public void AddItem(GameItemData data)
    {
        GameItem item = new GameItem(data.itemName, data.type, data.description, data.icon)
        {
            isKeyItem = data.isKeyItem,
            isEvidence = data.isEvidence,
            compatibleStyles = new List<string>(data.compatibleStyles)
        };

        AddItem(item); // llamada al método anterior
    }


    public void RemoveItem(GameItem item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            if (item.isKeyItem) keyItems.Remove(item);
            if (item.isEvidence) evidence.Remove(item);
            if (item.isEquipped) UnequipItem(item);
        }
    }

    public void EquipItem(GameItem item)
    {
        if (items.Contains(item) && !item.isEquipped)
        {
            item.isEquipped = true;
            equippedItems.Add(item);
        }
    }

    public void UnequipItem(GameItem item)
    {
        if (equippedItems.Contains(item))
        {
            item.isEquipped = false;
            equippedItems.Remove(item);
        }
    }

    // Métodos para verificar objetos
    public bool HasItem(string itemName)
    {
        return items.Exists(i => i.itemName == itemName);
    }

    public bool HasEquippedItem(GameItem item)
    {
        return equippedItems.Contains(item);
    }

    public bool HasKeyItem(string itemName)
    {
        return keyItems.Exists(i => i.itemName == itemName);
    }

    public bool HasEvidence(string itemName)
    {
        return evidence.Exists(i => i.itemName == itemName);
    }

    // Métodos para obtener listas de objetos
    public List<GameItem> GetItemsByType(GameItem.ItemType type)
    {
        return items.FindAll(i => i.type == type);
    }

    public List<GameItem> GetItemsByStyle(string style)
    {
        return items.FindAll(i => i.compatibleStyles.Contains(style));
    }

    // Método para reiniciar el inventario
    public void ResetInventory()
    {
        items.Clear();
        equippedItems.Clear();
        keyItems.Clear();
        evidence.Clear();
    }

    // Método para actualizar la UI del inventario
    public void UpdateInventoryUI()
    {
        // Aquí iría la lógica para actualizar la UI del inventario
        // Por ejemplo, si tienes un Canvas con diferentes secciones:
        GameObject inventoryUI = GameObject.Find("InventoryUI");
        if (inventoryUI != null)
        {
            // Actualizar la UI de los items equipados
            UpdateEquippedItemsUI();

            // Actualizar la UI de los key items
            UpdateKeyItemsUI();

            // Actualizar la UI de las evidencias
            UpdateEvidenceUI();
        }
    }

    private void UpdateEquippedItemsUI()
    {
        // Aquí iría la lógica para actualizar la UI de los items equipados
    }

    private void UpdateKeyItemsUI()
    {
        // Aquí iría la lógica para actualizar la UI de los key items
    }

    private void UpdateEvidenceUI()
    {
        // Aquí iría la lógica para actualizar la UI de las evidencias
    }

    public GameItem FindItemByName(string name)
    {
        return items.Find(i => i.itemName == name);
    }

    public List<GameItem> GetEquippedItems()
    {
        return new List<GameItem>(equippedItems);
    }

}
