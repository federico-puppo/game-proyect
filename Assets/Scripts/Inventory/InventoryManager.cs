using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private List<GameItem> items = new List<GameItem>();
    private List<GameItem> equippedItems = new List<GameItem>();
    private List<GameItem> keyItems = new List<GameItem>();
    private List<GameItem> evidence = new List<GameItem>();

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

    public void AddItem(GameItemData data)
    {
        GameItem item = new GameItem(
            data.itemName,
            data.type,
            data.description,
            data.icon,
            data.galleryImage
        )
        {
            isKeyItem = data.isKeyItem,
            isEvidence = data.isEvidence,
            compatibleStyles = new List<string>(data.compatibleStyles)
        };

        items.Add(item);
        if (item.isKeyItem) keyItems.Add(item);
        if (item.isEvidence) evidence.Add(item);
        if (!equippedItems.Contains(item)) equippedItems.Add(item);

        FindObjectOfType<InventoryUIRenderer>()?.UpdateUI();
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

    public bool HasItem(string itemName)
    {
        return items.Exists(i => i.itemName == itemName);
    }

    public GameItem FindItemByName(string name)
    {
        return items.Find(i => i.itemName == name);
    }

    public List<GameItem> GetEquippedItems() => new List<GameItem>(equippedItems);
    public List<GameItem> GetItems() => new List<GameItem>(items);

    public void ResetInventory()
    {
        items.Clear();
        equippedItems.Clear();
        keyItems.Clear();
        evidence.Clear();
    }
}
