using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInteract : MonoBehaviour
{
    [Header("Mouse:")]
    public float lookDistance = 3f;

    [Header("Raycast:")]
    RaycastHit raycastHit;
    Ray ray;
    RaycastHit raycastCheck;

    void Update()
    {
        // Grab raycasts to limit interact range
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out raycastHit, lookDistance);
        Physics.Raycast(ray, out raycastCheck, Mathf.Infinity);

        if (!UserInterfaceManager.instance.isOpen)   // Check if game is paused
        {
            if (Input.GetKeyDown(Controls.instance.interactKeys[0]) || Input.GetKeyDown(Controls.instance.interactKeys[1]))
            {
                if (raycastHit.collider != null)
                {
                    Interact(raycastHit);
                }
            }
            else
            {
                // Check hovered on object
                if (raycastHit.collider != null)
                {
                    MouseHover(raycastHit);
                }
                else if (raycastCheck.collider != null)   // Remove object selection after max range
                {
                    MouseHover(raycastCheck);
                }
            }
        }
    }

    public void MouseHover(RaycastHit hit)  // Hover over object
    {
        // Add script to gameobject
        MouseHover hoverObject = hit.transform.gameObject.GetComponent<MouseHover>();
        if (hit.collider.CompareTag("Interactable") && hoverObject == null)
        {
            hoverObject = hit.transform.gameObject.AddComponent<MouseHover>();
        }

        // Check object has script and change colour
        if (hoverObject)
        {
            if (hit.distance < lookDistance)
            {
                hoverObject.hovered = true;
            } else
            {
                hoverObject.hovered = false;
            }
        }
    }

    public void Interact(RaycastHit hit)  // Interact with object
    {
        // Get object on click
        GameObject selectedObject = hit.transform.gameObject;
        if (hit.collider.CompareTag("Interactable"))
        {
            hit.transform.gameObject.GetComponent<ItemPickup>().PickupItem();
        }
    }
}
