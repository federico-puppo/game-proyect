using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewGameItem", menuName = "Game/Item")]
public class GameItemData : ScriptableObject
{
    public string itemName;
    public GameItem.ItemType type;
    public string description;
    public Sprite icon;
    public bool isKeyItem;
    public bool isEvidence;
    public List<string> compatibleStyles;
}
