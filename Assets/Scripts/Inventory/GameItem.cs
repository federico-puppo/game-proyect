using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GameItem
{
    public enum ItemType
    {
        Weapon,
        Tool,
        KeyItem,
        Evidence,
        Consumable,
        Other
    }

    public string itemName;
    public ItemType type;
    public string description;
    public Sprite icon;
    public bool isEquipped = false;
    public bool isKeyItem = false; // Indica si es un item importante para el gameplay
    public bool isEvidence = false; // Indica si es una pista o evidencia
    public List<string> compatibleStyles; // Estilos que pueden usar este item

    public GameItem(string name, ItemType itemType, string desc, Sprite sprite)
    {
        itemName = name;
        type = itemType;
        description = desc;
        icon = sprite;
        compatibleStyles = new List<string>();
    }

    public void AddCompatibleStyle(string style)
    {
        if (!compatibleStyles.Contains(style))
        {
            compatibleStyles.Add(style);
        }
    }
}
