using UnityEngine;
using TMPro;

public class PickUpItem : MonoBehaviour
{
    private GameItemData data;
    private bool isPlayerNearby = false;
    private GameObject labelCanvas;
    private TMP_Text labelText;

    private void Awake()
    {
        if (InventoryManager.Instance == null)
        {
            Debug.LogWarning("Buscando InventoryManager manualmente...");
            InventoryManager fallback = FindObjectOfType<InventoryManager>();
            if (fallback != null)
                Debug.Log("InventoryManager encontrado manualmente.");
            else
                Debug.LogError("NO se encontró InventoryManager en escena.");
        }

        GameItemHolder holder = GetComponent<GameItemHolder>();
        if (holder != null && holder.data != null)
        {
            data = holder.data;
        }
        else
        {
            Debug.LogWarning($"[PickUpItem] No se encontró GameItemData en {gameObject.name}");
        }

        labelCanvas = transform.Find("LabelCanvas")?.gameObject;
        labelText = labelCanvas?.GetComponentInChildren<TMP_Text>();

        if (labelCanvas != null)
        {
            labelCanvas.SetActive(false);
            if (labelText != null && data != null)
            {
                labelText.text = $"{data.itemName}\n<E> Recoger";
            }
        }
    }

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) && data != null)
        {
            if (InventoryManager.Instance == null)
            {
                Debug.LogError("[PickUpItem] InventoryManager no está inicializado.");
                return;
            }

            // Añadir al inventario interno
            InventoryManager.Instance.AddItem(data);

            // Añadir al GameManager como pista o ítem normal
            if (GameManager.Instance != null)
            {
                if (data.isEvidence)
                {
                    GameManager.Instance.AddClue(data.itemName, data.isRevelationClue);
                }
                else
                {
                    GameManager.Instance.AddItem(data.itemName, equip: true);
                }
            }

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            if (labelCanvas != null)
                labelCanvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            if (labelCanvas != null)
                labelCanvas.SetActive(false);
        }
    }
}
