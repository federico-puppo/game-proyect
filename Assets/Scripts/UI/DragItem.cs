using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Camera cam;
    
    private Vector3 originalLocalPosition;   // Posición local inicial dentro del padre original
    private Transform originalParent;        // Padre original (container del scroll)

    private DropSlot currentSlot;            // Slot donde está alojado actualmente

    private void Start()
    {
        cam = Camera.main;

        originalParent = transform.parent;           // Guardar padre original
        originalLocalPosition = transform.localPosition;  // Guardar posición local original
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Al comenzar drag, si está en slot ocupado, resetear color y limpiar slot
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("Slot"))
            {
                DropSlot slot = result.gameObject.GetComponent<DropSlot>();
                if (slot != null && slot.ocupado)
                {
                    slot.ResetColor();
                    currentSlot = null;
                    break;
                }
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Ray ray = cam.ScreenPointToRay(eventData.position);
        Plane plane = new Plane(Vector3.back, transform.position);

        if (plane.Raycast(ray, out float distance))
        {
            transform.position = ray.GetPoint(distance);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        bool droppedOnValidSlot = false;

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("Slot"))
            {
                DropSlot slot = result.gameObject.GetComponent<DropSlot>();

                if (slot != null && !slot.ocupado)
                {
                    // Soltar dentro del slot
                    transform.position = result.gameObject.transform.position;
                    transform.SetParent(slot.transform, true);  // El cubo es hijo del slot

                    Outline outline = GetComponent<Outline>();
                    if (outline != null)
                    {
                        slot.PaintSlot(outline.OutlineColor);
                    }

                    currentSlot = slot;
                    droppedOnValidSlot = true;
                    return;
                }
            }
        }

        // Si no se soltó en un slot válido, volver al padre y posición original dentro del scroll
        if (!droppedOnValidSlot)
        {
            transform.SetParent(originalParent, false);           // Padre original y conservar posición local
            transform.localPosition = originalLocalPosition;      // Volver a la posición original dentro del scroll
            currentSlot = null;
        }
    }

    public void ResetToOriginalPosition()
    {
        if (currentSlot != null)
        {
            currentSlot.ResetColor();
            currentSlot = null;
        }
        transform.SetParent(originalParent, false);
        transform.localPosition = originalLocalPosition;
    }
}
