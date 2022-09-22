using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Hotbar : MonoBehaviour
{
    #region Singleton
    public static Hotbar instance;
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
    [Header("UI Components:")]
    public GameObject hotbarSelection;
    public GameObject hotbarSelectionText;

    [Header("Scroll Variables:")]
    public int slotNumber = 0;
    private const int minScroll = 0;
    private const int maxScroll = 9;

    private void Update()
    {
        // Check input for hotbar scroll
        HotbarInput();
        // Display selection on hotbar
        HotbarDisplay();
    }

    public void HotbarDisplay()
    {
        // Assign selection image to slot location
        if (Inventory.instance.inventoryActive.activeSelf)
        {
            hotbarSelection.SetActive(false);
        } else
        {
            hotbarSelection.SetActive(true);
        }
        hotbarSelection.transform.position = Inventory.instance.inventorySlots[slotNumber].Slot.transform.position;
        // Assign current selection text in hotbar
        if (Inventory.instance.inventorySlots[slotNumber].item == null)
        {
            hotbarSelectionText.GetComponent<TextMeshProUGUI>().text = "";
        } else
        {
            hotbarSelectionText.GetComponent<TextMeshProUGUI>().text = Inventory.instance.inventorySlots[slotNumber].item.name;
        }
    }

    private void HotbarInput()
    {
        if(!UserInterfaceManager.instance.isOpen)  // Check game isn't paused
        {
            // ScrollWheel input
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                if (slotNumber <= minScroll)
                {
                    slotNumber = maxScroll;
                } else
                {
                    slotNumber--;
                }
            } else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                if (slotNumber >= maxScroll)
                {
                    slotNumber = minScroll;
                } else
                {
                    slotNumber++;
                }
            }
            // Number input
            for (int i = 0; i < Controls.instance.hotBarKeys.Length; i++)
            {
                if (Input.GetKey(Controls.instance.hotBarKeys[i]))
                {
                    if (i == minScroll)
                    {
                        slotNumber = maxScroll;
                    } else
                    {
                        slotNumber = i - 1;
                    }
                }
            }
        }
    }
}
