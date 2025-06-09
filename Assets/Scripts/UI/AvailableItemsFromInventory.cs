using UnityEngine;
using System.Collections.Generic;

public class AvailableItemsFromInventory : MonoBehaviour
{
    public Transform itemGridContainer;          // El Content del ScrollView
    public PrefabDictionary prefabDictionary;    // Tu ScriptableObject que vincula items a prefabs

    [Header("Configuración visual")]
    public float spacing = 2f;
    public Vector2 startOffset = Vector2.zero;

    void Start()
    {
    List<GameItem> available = InventoryManager.Instance.GetItems();

        if (available == null || available.Count == 0)
        {
            Debug.Log("No hay ítems equipados actualmente");
            return;
        }

        int index = 0;
        foreach (GameItem item in available)
        {
            GameItemData data = prefabDictionary.entries.Find(e => e.itemData.itemName == item.itemName)?.itemData;

            if (data == null)
            {
                Debug.LogWarning($"No se encontró GameItemData para {item.itemName}");
                continue;
            }

            // Cambié esta línea para pasar GameItemData en vez de string
            GameObject prefab = prefabDictionary.GetPrefab(data);

            if (prefab == null)
            {
                Debug.LogWarning($"No hay prefab vinculado a {data.itemName} en el diccionario");
                continue;
            }

            GameObject clone = Instantiate(prefab, itemGridContainer);
            clone.name = data.itemName;

            Vector3 localPos = new Vector3(
                startOffset.x + index * spacing,
                0f, 0f
            );
            clone.transform.localPosition = localPos;

            index++;
        }

        // Expandir el ancho del Content para que se active el scroll horizontal
        RectTransform rt = itemGridContainer.GetComponent<RectTransform>();
        float totalWidth = index * spacing + 2f;
        rt.sizeDelta = new Vector2(totalWidth, rt.sizeDelta.y);
    }
}
