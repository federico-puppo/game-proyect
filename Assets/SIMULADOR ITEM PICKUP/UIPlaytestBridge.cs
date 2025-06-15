using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIPlaytestBridge : MonoBehaviour
{
    public Button goToUISceneButton;
    public PrefabDictionary prefabDictionary;

    [Header("Prefabs de ítems a simular")]
    public GameObject[] simulatedItemPrefabs;

    void Start()
    {
        foreach (GameObject itemPrefab in simulatedItemPrefabs)
        {
            var holder = itemPrefab.GetComponent<GameItemHolder>();
            if (holder == null || holder.data == null)
            {
                Debug.LogWarning($"[Bridge] El prefab {itemPrefab.name} no tiene GameItemHolder con data");
                continue;
            }

            var data = holder.data;
            if (!InventoryManager.Instance.HasItem(data.itemName))
                InventoryManager.Instance.AddItem(data);
            //InventoryManager.Instance.EquipItem(InventoryManager.Instance.FindItemByName(data.itemName));

            Debug.Log($"[Bridge] Equipado (por referencia): {data.itemName}");
        }

        if (goToUISceneButton != null)
        {
            goToUISceneButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("MentalUI");
            });
        }
    }
}
