using UnityEngine;
//Script to add test items to the inventory for debugging purposes.
public class InventoryTestAdder : MonoBehaviour
{
    public GameItemData[] testItems;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (testItems == null || testItems.Length == 0)
            {
                Debug.LogWarning("No hay objetos asignados para testear.");
                return;
            }

            foreach (var itemData in testItems)
            {
                InventoryManager.Instance.AddItem(itemData);

                if (itemData.isEvidence)
                {
                    GameManager.Instance.AddClue(itemData.itemName); // Esto suma pistas
                    Debug.Log($"[TEST] Pista añadida: {itemData.itemName}");
                }
                else
                {
                    GameManager.Instance.AddItem(itemData.itemName); // para otros ítems normales
                    Debug.Log($"[TEST] Ítem añadido (no pista): {itemData.itemName}");
                }
            }
        }
    }
}
