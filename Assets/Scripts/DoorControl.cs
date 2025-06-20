// === GameManagerDoorControl.cs ===
using UnityEngine;
using System.Collections.Generic;

public class GameManagerDoorControl : MonoBehaviour
{
    [System.Serializable]
    public class DoorInfo
    {
        public GameObject doorObject;
        public string requiredItem; // si está vacío, la puerta está siempre activa
    }

    public List<DoorInfo> doors;
    public string colorLayerName = "ColorOnly"; // capa a la que se cambian las puertas desbloqueadas

    void Start()
    {
        UpdateDoors();
    }

    public void UpdateDoors()
    {
        foreach (DoorInfo door in doors)
        {
            bool unlock = string.IsNullOrEmpty(door.requiredItem) || InventoryManager.Instance.HasItem(door.requiredItem);

            door.doorObject.SetActive(unlock);

            if (unlock)
            {
                door.doorObject.layer = LayerMask.NameToLayer(colorLayerName);
            }
        }
    }

    // Podés llamar a esto después de recoger un ítem:
    // FindObjectOfType<GameManagerDoorControl>().UpdateDoors();
}
