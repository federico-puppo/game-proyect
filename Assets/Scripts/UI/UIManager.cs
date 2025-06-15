using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Referencias UI")]
    public Button sceneEnterButton;
    public DropSlot[] allSlots;

    [Header("Prefabs válidos (los que pueden determinar el rol)")]
    public GameObject[] includedCubePrefabs;

    [Header("Prefabs excluidos (neutrales o claves)")]
    public GameObject[] excludedCubePrefabs;

    [Header("Mínimo de ítems iguales para activar color")]
    public int minimumPrefabCount = 2;

    [Header("Color por defecto del botón")]
    public Color defaultButtonColor = Color.white;

    private Image buttonImage;

    private void Start()
    {
        buttonImage = sceneEnterButton?.GetComponent<Image>();

        if (allSlots == null || allSlots.Length == 0)
        {
            allSlots = FindObjectsOfType<DropSlot>();
        }
        if (sceneEnterButton == null)
        {
            sceneEnterButton = GameObject.Find("SceneEnterButton")?.GetComponent<Button>();
        }

        UpdateButtonColor();
    }

    public void UpdateButtonColor()
    {
        if (buttonImage == null) return;

        Dictionary<GameObject, int> prefabCount = GetValidPrefabCounts();

        if (prefabCount.Count == 0)
        {
            buttonImage.color = defaultButtonColor;
            return;
        }

        GameObject predominantPrefab = GetPredominantPrefab(prefabCount);

        if (predominantPrefab != null)
        {
            buttonImage.color = GetPrefabColor(predominantPrefab);
        }
        else
        {
            buttonImage.color = defaultButtonColor;
        }
    }

    private Dictionary<GameObject, int> GetValidPrefabCounts()
    {
        Dictionary<GameObject, int> prefabCount = new Dictionary<GameObject, int>();

        foreach (DropSlot slot in allSlots)
        {
            if (!slot.ocupado) continue;

            Color slotColor = slot.GetComponent<Image>().color;

            GameObject included = FindPrefabByColor(slotColor, includedCubePrefabs);
            GameObject excluded = FindPrefabByColor(slotColor, excludedCubePrefabs);

            if (included != null && excluded == null)
            {
                if (prefabCount.ContainsKey(included))
                    prefabCount[included]++;
                else
                    prefabCount[included] = 1;
            }
        }

        return prefabCount;
    }

    private GameObject GetPredominantPrefab(Dictionary<GameObject, int> prefabCount)
    {
        var validPrefabs = prefabCount
            .Where(x => x.Value >= minimumPrefabCount)
            .OrderByDescending(x => x.Value);

        if (validPrefabs.Any())
        {
            return validPrefabs.First().Key;
        }

        return null;
    }

    private GameObject FindPrefabByColor(Color color, GameObject[] prefabs)
    {
        foreach (GameObject prefab in prefabs)
        {
            if (prefab == null) continue;

            Outline outline = prefab.GetComponent<Outline>();
            if (outline != null && ColorsAreEqual(outline.OutlineColor, color))
            {
                return prefab;
            }
        }

        return null;
    }

    private Color GetPrefabColor(GameObject prefab)
    {
        Outline outline = prefab.GetComponent<Outline>();
        return outline != null ? outline.OutlineColor : defaultButtonColor;
    }

    private bool ColorsAreEqual(Color a, Color b, float tolerance = 0.01f)
    {
        return Mathf.Abs(a.r - b.r) <= tolerance &&
               Mathf.Abs(a.g - b.g) <= tolerance &&
               Mathf.Abs(a.b - b.b) <= tolerance;
    }

    public void RequestColorUpdate()
    {
        UpdateButtonColor();
    }

    public void OnClickSceneEnter()
    {
        string next = PlayerPrefs.GetString("NextSceneAfterUI", "SampleScene");
        SceneManager.LoadScene(GameManager.Instance.GetNextScene());
    }

    public void GuardarItemsSeleccionados()
    {
        if (InventoryManager.Instance == null)
        {
            Debug.LogWarning("InventoryManager no encontrado.");
            return;
        }

        // 1) Limpiar solo los equipados (no todo el inventario)
        foreach (var it in InventoryManager.Instance.GetEquippedItems().ToList())
            InventoryManager.Instance.UnequipItem(it);

        // 2) Equipar solo los 4 elegidos; NO llamamos a AddItem
        foreach (DropSlot slot in allSlots)
        {
            if (!slot.ocupado || slot.transform.childCount == 0) continue;

            var holder = slot.transform.GetChild(0).GetComponent<GameItemHolder>();
            if (holder?.data != null)
            {
                var data = holder.data;
                var item = InventoryManager.Instance.FindItemByName(data.itemName);
                if (item != null)
                    InventoryManager.Instance.EquipItem(item);
            }
        }
    }
}
