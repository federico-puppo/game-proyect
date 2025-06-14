using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    private GameItemData data;

    private void Awake()
    {
        GameItemHolder holder = GetComponent<GameItemHolder>();
        if (holder != null && holder.data != null)
        {
            data = holder.data;
        }
        else
        {
            Debug.LogWarning($"[PickUpItem] No se encontró GameItemData en {gameObject.name}");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && data != null)
        {
            InventoryManager.Instance.AddItem(data);
            Destroy(gameObject);
            Debug.Log($"[PickUpItem] Se recogió: {data.itemName}");
        }
    }
}
