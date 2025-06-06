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

    public List<InventoryItem> items = new List<InventoryItem>();



    public void AddItem(string itemName)
    {
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

    // Métodos para gestionar los estilos
    public void ChooseAggressive()
    {
        if (CanUseStyle("Aggressive") && !hasMadeStyleChoice)
        {
            currentStyle = "Aggressive";
            hasMadeStyleChoice = true;
        }
    }

    public void ChooseInvestigative()
    {
        if (CanUseStyle("Investigative") && !hasMadeStyleChoice)
        {
            currentStyle = "Investigative";
            hasMadeStyleChoice = true;
        }
    }

    public void ChooseStealth()
    {
        if (CanUseStyle("Stealth") && !hasMadeStyleChoice)
        {
            currentStyle = "Stealth";
            hasMadeStyleChoice = true;
        }
    }

    public string GetCurrentStyle() => currentStyle;
    public bool HasMadeChoice() => hasMadeStyleChoice;

    // Verificar si se puede usar un estilo basado en los items equipados
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

    // Método para actualizar las opciones disponibles en la UI
    public void UpdateStyleOptions()
    {
        GameObject uiCanvas = GameObject.Find("StyleSelectionUI");
        if (uiCanvas != null)
        {
            Transform buttonsPanel = uiCanvas.transform.Find("Pannel General/Panel Buttons");
            TMP_Text feedbackText = uiCanvas.transform.Find("FeedbackText")?.GetComponent<TMP_Text>(); // Agrega un objeto Text

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
        // Guardar estilo
        PlayerPrefs.SetString("SavedStyle", currentStyle);

        // Obtener los ítems equipados desde InventoryManager
        List<GameItem> equipped = InventoryManager.Instance.GetEquippedItems();

        // Guardar solo los nombres
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
    }


    // Método para reiniciar el juego
    public void ResetGame()
    {
        hasMadeStyleChoice = false;
        currentStyle = "";
        InventoryManager.Instance.ResetInventory();
    }
}
