using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewGameItem", menuName = "Game/Item")]
public class GameItemData : ScriptableObject
{
    public string itemName;
    public GameItem.ItemType type;
    public string description;
    public Sprite icon;
    public Sprite galleryImage;
    public bool isKeyItem;
    public bool isEvidence;
    public bool isRevelationClue;

    public bool isEquippedInitially;
    public List<string> compatibleStyles;
}
