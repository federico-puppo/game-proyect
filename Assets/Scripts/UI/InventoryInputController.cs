using UnityEngine;
using System.Linq;

public class InventoryInputController : MonoBehaviour
{
    private InventoryGalleryUI gallery;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        gallery = Resources.FindObjectsOfTypeAll<InventoryGalleryUI>()
                           .FirstOrDefault(g => g.name == "InventoryGalleryUI");

        if (gallery == null)
            Debug.LogError("No se encontró InventoryGalleryUI");
        else
            Debug.Log("Gallery encontrada y lista.");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (gallery == null)
            {
                Debug.LogWarning("Gallery es null en Update.");
                return;
            }

            if (!gallery.gameObject.activeSelf)
                gallery.Show();
            else
                gallery.Hide();
        }
    }
}


