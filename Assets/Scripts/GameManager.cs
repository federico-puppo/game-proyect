using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [System.Serializable]
    public class InventoryItem
    {
        public string itemName;
        public bool isEquipped;
    }

    public List<InventoryItem> items = new List<InventoryItem>();
    private int totalCluesFound = 0;
    private bool hasRevelationClue = false;
    private const int totalCluesRequired = 6;

    private HashSet<string> locationsVisited = new HashSet<string>();
    private bool readyToShowSummary = false;

    private HashSet<string> objetosRecolectados = new HashSet<string>(); // NUEVO

    [System.Serializable]
    public class OrderedDoor
    {
        public GameObject doorObject;
        public string doorID;
    }

    public List<OrderedDoor> orderedDoors = new List<OrderedDoor>();
    public string colorLayerName = "ColorOnly";
    private int currentIndex = 0;

    [Header("UI de recordatorio")]
    public GameObject summaryReminderUI;

    [Header("Finales")]
    public string escenaFinalBueno = "FinalBueno";
    public string escenaFinalMalo = "FinalMalo";
    public string escenaConfrontacion = "Confrontacion";

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

    private void Start()
    {
        UpdateDoorStates();
    }

    public void AddItem(string itemName, bool equip = false)
    {
        InventoryItem item = items.Find(i => i.itemName == itemName);
        if (item == null)
        {
            items.Add(new InventoryItem { itemName = itemName, isEquipped = equip });
        }
        else if (equip)
        {
            item.isEquipped = true;
        }
    }

    public void EquipItem(string itemName)
    {
        InventoryItem item = items.Find(i => i.itemName == itemName);
        if (item != null)
        {
            item.isEquipped = true;
        }
    }

    public void AddClue(string clueName, bool isRevelation = false)
    {
        if (!HasItem(clueName))
        {
            AddItem(clueName);
            totalCluesFound++;
            if (isRevelation) hasRevelationClue = true;
        }
    }

    public bool HasAllClues() => totalCluesFound >= totalCluesRequired;
    public bool HasRevelation() => hasRevelationClue;
    public int GetClueCount() => totalCluesFound;

    public void MarkLocationVisited(string locationName)
    {
        locationsVisited.Add(locationName);

        if (HasVisitedAllKeyLocations() && summaryReminderUI != null)
        {
            summaryReminderUI.SetActive(true);
        }
    }

    public bool HasVisitedAllKeyLocations()
    {
        return locationsVisited.Contains("CrimeScene") &&
               locationsVisited.Contains("NewspapperOffice") &&
               locationsVisited.Contains("Bar");
    }

    public void CheckSummaryTrigger(string enteringScene)
    {
        readyToShowSummary = (enteringScene == "Oficina" && HasVisitedAllKeyLocations());

        if (readyToShowSummary && summaryReminderUI != null)
        {
            summaryReminderUI.SetActive(false);
        }
    }

    public bool ShouldShowSummary() => readyToShowSummary;

    public bool HasItem(string itemName)
    {
        return items.Exists(i => i.itemName == itemName);
    }

    public bool IsEquipped(string itemName)
    {
        InventoryItem item = items.Find(i => i.itemName == itemName);
        return item != null && item.isEquipped;
    }

    public void UnlockNextDoor()
    {
        if (currentIndex < orderedDoors.Count)
        {
            GameObject door = orderedDoors[currentIndex].doorObject;
            door.layer = LayerMask.NameToLayer(colorLayerName);
            door.SetActive(true);
            currentIndex++;
        }
    }

    public void NotifyDoorUsed(string doorID)
    {
        int usedIndex = orderedDoors.FindIndex(d => d.doorID == doorID);
        if (usedIndex >= 0 && usedIndex + 1 < orderedDoors.Count)
        {
            GameObject nextDoor = orderedDoors[usedIndex + 1].doorObject;
            nextDoor.layer = LayerMask.NameToLayer(colorLayerName);
            nextDoor.SetActive(true);
            currentIndex = Mathf.Max(currentIndex, usedIndex + 2);
        }
    }

    public void UpdateDoorStates()
    {
        for (int i = 0; i < orderedDoors.Count; i++)
        {
            GameObject door = orderedDoors[i].doorObject;
            bool isUnlocked = i < currentIndex;
            door.SetActive(isUnlocked);
            if (isUnlocked)
            {
                door.layer = LayerMask.NameToLayer(colorLayerName);
            }
        }
    }

    public void ResetGame()
    {
        currentIndex = 0;
        items.Clear();
        totalCluesFound = 0;
        hasRevelationClue = false;
        locationsVisited.Clear();
        readyToShowSummary = false;
        objetosRecolectados.Clear(); // RESETEO de objetos recolectados
        UpdateDoorStates();
    }

    // === Lógica de objetos recolectables ===

    public void MarcarRecolectado(string id)
    {
        if (!objetosRecolectados.Contains(id))
        {
            objetosRecolectados.Add(id);
            Debug.Log($"Objeto recolectado: {id}");
        }
    }

    public bool YaFueRecolectado(string id)
    {
        return objetosRecolectados.Contains(id);
    }

    // === Lógica de finales ===

    public bool PuedeConfrontar()
    {
        return HasItem("Pistola") && HasRevelation();
    }

    public void IrAJuicio()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(HasAllClues() ? escenaFinalBueno : escenaFinalMalo);
    }

    public void IrAConfrontacion()
    {
        if (PuedeConfrontar())
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(escenaConfrontacion);
        }
    }
}
