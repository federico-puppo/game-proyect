using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class PlayerInventory
{
    [System.Serializable]
    public class InventoryItem
    {
        public string itemName;
        public bool isEquipped;
    }

    public int maxItems = 5;
    public List<InventoryItem> items = new List<InventoryItem>();

    public void AddItem(string itemName)
    {
        if (items.Count >= maxItems)
        {
            InventoryPopup popup = GameObject.FindObjectOfType<InventoryPopup>();
            if (popup != null)
            {
                popup.ShowMessage("Inventario lleno. No se puede agregar el item.");
            }
            return;
        }

        InventoryItem item = items.Find(i => i.itemName == itemName);
        if (item != null)
        {
            item.isEquipped = true;
        }
        else
        {
            items.Add(new InventoryItem { itemName = itemName, isEquipped = true });
        }
    }

    public bool HasItem(string itemName)
    {
        return items.Exists(i => i.itemName == itemName && i.isEquipped);
    }
}

[System.Serializable]
public class PlayerStyle
{
    [System.Serializable]
    public class StyleRequirement
    {
        public string styleName;
        public List<string> requiredItems = new List<string>();
    }

    public List<StyleRequirement> styles = new List<StyleRequirement>();
    public string currentStyle = "";

    public void AddStyle(string styleName, List<string> requiredItems)
    {
        styles.Add(new StyleRequirement
        {
            styleName = styleName,
            requiredItems = requiredItems
        });
    }

    public bool CanUseStyle(string styleName)
    {
        var style = styles.Find(s => s.styleName == styleName);
        if (style != null)
        {
            foreach (var item in style.requiredItems)
            {
                if (!GameManager.Instance.playerInventory.HasItem(item))
                {
                    return false;
                }
            }
            return true;
        }
        return false;
    }

    public void ChooseStyle(string styleName)
    {
        if (CanUseStyle(styleName))
        {
            currentStyle = styleName;
        }
    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public PlayerInventory playerInventory = new PlayerInventory();

    [SerializeField] private bool hasMadeStyleChoice = false;
    private string currentStyle = "";
    private string nextScene = "";

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

    public void ChooseAggressive()
    {
        if (CanUseStyle("Aggressive") && !hasMadeStyleChoice)
        {
            currentStyle = "Aggressive";
            hasMadeStyleChoice = true;
            RefreshInventoryUI();
        }
    }

    public void ChooseInvestigative()
    {
        if (CanUseStyle("Investigative") && !hasMadeStyleChoice)
        {
            currentStyle = "Investigative";
            hasMadeStyleChoice = true;
            RefreshInventoryUI();
        }
    }

    public void ChooseStealth()
    {
        if (CanUseStyle("Stealth") && !hasMadeStyleChoice)
        {
            currentStyle = "Stealth";
            hasMadeStyleChoice = true;
            RefreshInventoryUI();
        }
    }

    public string GetCurrentStyle() => currentStyle;
    public bool HasMadeChoice() => hasMadeStyleChoice;

    public bool CanUseStyle(string style)
    {
        List<GameItem> requiredItems = InventoryManager.Instance.GetItemsByStyle(style);
        foreach (GameItem item in requiredItems)
        {
            if (!InventoryManager.Instance.HasEquippedItem(item))
            {
                return false;
            }
        }
        return true;
    }

    public void UpdateStyleOptions()
    {
        GameObject uiCanvas = GameObject.Find("StyleSelectionUI");
        if (uiCanvas != null)
        {
            Transform buttonsPanel = uiCanvas.transform.Find("Pannel General/Panel Buttons");
            TMP_Text feedbackText = uiCanvas.transform.Find("FeedbackText")?.GetComponent<TMP_Text>();

            if (buttonsPanel != null)
            {
                void SetButtonState(string style, Transform button)
                {
                    bool canUse = CanUseStyle(style);
                    button.gameObject.SetActive(true);
                    button.GetComponent<Button>().interactable = canUse;

                    if (!canUse && feedbackText != null)
                    {
                        var missing = GetMissingItemsForStyle(style);
                        feedbackText.text = $"Para {style} necesitas: {string.Join(", ", missing)}";
                    }
                }

                SetButtonState("Aggressive", buttonsPanel.Find("AggressiveButton"));
                SetButtonState("Investigative", buttonsPanel.Find("InvestigativeButton"));
                SetButtonState("Stealth", buttonsPanel.Find("StealthButton"));
            }
        }
    }

    private List<string> GetMissingItemsForStyle(string style)
    {
        List<string> missing = new List<string>();
        List<GameItem> requiredItems = InventoryManager.Instance.GetItemsByStyle(style);
        foreach (var item in requiredItems)
        {
            if (!InventoryManager.Instance.HasEquippedItem(item))
            {
                missing.Add(item.itemName);
            }
        }
        return missing;
    }

    public void SaveGame()
    {
        PlayerPrefs.SetString("SavedStyle", currentStyle);
        List<GameItem> equipped = InventoryManager.Instance.GetEquippedItems();
        List<string> equippedItemNames = equipped.Select(i => i.itemName).ToList();
        PlayerPrefs.SetString("EquippedItems", string.Join(",", equippedItemNames));
        PlayerPrefs.Save();
    }

    public void LoadGame()
    {
        currentStyle = PlayerPrefs.GetString("SavedStyle", "");
        string equipped = PlayerPrefs.GetString("EquippedItems", "");
        if (!string.IsNullOrEmpty(equipped))
        {
            string[] itemNames = equipped.Split(',');
            foreach (string name in itemNames)
            {
                GameItem item = InventoryManager.Instance.FindItemByName(name);
                if (item != null)
                {
                    InventoryManager.Instance.EquipItem(item);
                }
            }
        }

        RefreshInventoryUI();
    }

    public void ResetGame()
    {
        hasMadeStyleChoice = false;
        currentStyle = "";
        InventoryManager.Instance.ResetInventory();
        RefreshInventoryUI();
    }

    public void SetNextScene(string sceneName)
    {
        nextScene = sceneName;
    }

    public string GetNextScene()
    {
        return nextScene;
    }

    public void RefreshInventoryUI()
    {
        InventoryUIRenderer ui = FindObjectOfType<InventoryUIRenderer>();
        if (ui != null)
            ui.UpdateUI();
    }
}