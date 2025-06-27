using UnityEngine;

public class GameItemHolder : MonoBehaviour
{
    public GameItemData data;
    public string uniqueID;

    private void Start()
    {
        // Verificamos si ya fue recolectado
        if (GameManager.Instance != null && GameManager.Instance.YaFueRecolectado(uniqueID))
        {
            gameObject.SetActive(false); // Oculta el objeto en la escena
        }
    }

    public void Recolectar()
    {
        if (data == null) return;

        InventoryManager.Instance.AddItem(data);
        GameManager.Instance.MarcarRecolectado(uniqueID);
        gameObject.SetActive(false);
    }
}
