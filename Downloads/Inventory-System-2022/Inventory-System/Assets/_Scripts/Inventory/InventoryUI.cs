using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class InventoryUI : MonoBehaviour
{
    #region Singleton
    public static InventoryUI instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }
    #endregion
    [Header("Hovered Item Detail Componenets:")]
    public GameObject InventoryItemInfo;
    public Image detailIcon;
    public TextMeshProUGUI detailName;
    public TextMeshProUGUI detailDescription;

    [Header("UI Objects:")]
    public GameObject mouseSelection;


    [Header("Grabbed Item Components:")]
    public GameObject grabbedObject;
    public TextMeshProUGUI grabbedText;

    private void Start()
    {
        grabbedObject.GetComponent<Image>().color = new Color(255, 255, 255, 1);
    }

    private void Update()
    {
        // Update hand item UI
        ItemGrabbed(InventoryInteract.instance.handSlot);

        // Update slot text
        foreach (SlotData inventorySlot in Inventory.instance.inventorySlots)
        {
            inventorySlot.slotText.GetComponent<TextMeshProUGUI>().text = inventorySlot.stackSize.ToString();
        }
    }

    private void ItemGrabbed(SlotData handSlot)
    {
        // Assign UI components to handSlot components
        if (handSlot.item != null)
        {
            grabbedObject.transform.position = Input.mousePosition;
            grabbedObject.GetComponent<Image>().sprite = handSlot.slotIcon.GetComponent<Image>().sprite;
            grabbedText.text = handSlot.stackSize.ToString();
            grabbedObject.SetActive(true);
        }
        else
        {
            grabbedText.text = null;
            grabbedObject.SetActive(false);
        }
    }

    public void SlotHoverEnter(GameObject hoveredSlot)
    {
        // Set position & size of mouseSelection to hovered slot
        RectTransform hoveredPos = hoveredSlot.GetComponent<RectTransform>();
        mouseSelection.transform.position = hoveredPos.position;
        mouseSelection.GetComponent<RectTransform>().sizeDelta = new Vector2(hoveredPos.sizeDelta.x + 5, hoveredPos.sizeDelta.y + 5);
        // Assign data to UI objects
        UpdateInventoryDisplay(Array.Find(Inventory.instance.inventorySlots, inventSlot => inventSlot.Slot.name == hoveredSlot.name));
    }

    public void SlotHoverExit()
    {
        // Reset hover display on mouse exit
        mouseSelection.gameObject.SetActive(false);
        UpdateInventoryDisplay(null);
    }

    public void UpdateInventoryDisplay(SlotData inventorySlot)
    {
        if (inventorySlot != null)
        {
            mouseSelection.gameObject.SetActive(true);
            if (inventorySlot.item != null)
            {
                // Assign data
                detailIcon.sprite = inventorySlot.slotIcon.GetComponent<Image>().sprite;
                detailName.text = inventorySlot.item.name;
                detailDescription.text = inventorySlot.item.details;
                // Display object
                InventoryItemInfo.SetActive(true);
            }
        } else
        {
            // Remove data / hide UI objects
            InventoryItemInfo.SetActive(false);
            detailName.text = null;
            detailDescription.text = null;
        }
    }
}
