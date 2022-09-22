using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private bool slideDrop = true;
    public float clickTime;

    public void OnPointerEnter(PointerEventData slotData)   // Mouse Hover Enter
    {
        InventoryUI.instance.SlotHoverEnter(gameObject);

        if (Input.GetMouseButton(1) && slideDrop)   // Drag items across multiple slots in inventory
        {
            InventoryInteract.instance.OnMouseSlide(gameObject);
            slideDrop = false;
            return;
        }
    }

    public void OnPointerExit(PointerEventData slotData)    // Mouse Hover Exit
    {
        InventoryUI.instance.SlotHoverExit();

        if (Input.GetMouseButton(1) && slideDrop)   // Drag items across multiple slots in inventory
        {
            InventoryInteract.instance.OnMouseSlide(gameObject);
            slideDrop = false;
        } else if (!slideDrop)
        {
            slideDrop = true;
        }
    }

    public void OnPointerClick(PointerEventData slotData)
    {
        if (Input.GetMouseButtonUp(0))  // Left click
        {
            // Check for double click
            if (Time.time - clickTime < 0.2f)
            {
                InventoryShortcuts.instance.DoubleClick();
            } else
            {
                // Click on slot
                clickTime = Time.time;
                InventoryInteract.instance.OnSlotCLick(gameObject, false);
            }
        }
        else if (Input.GetMouseButtonUp(1)) // Right click
        {
            InventoryInteract.instance.OnSlotCLick(gameObject, true);
        }
    }
}
