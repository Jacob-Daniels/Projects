using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory instance;
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
    [Header("Inventory State:")]
    public GameObject inventoryActive;

    [Header("Inventory Storage:")]
    public List<Item> items = new List<Item>();
    public SlotData[] inventorySlots;

    private void Update()
    {
        // Check to open / close inventory
        if (Input.GetKeyDown(Controls.instance.inventory))
        {
            if (UserInterfaceManager.instance.UIArray[0].isActive)
            {
                CloseInventory();
            } else
            {
                OpenInventory();
            }
        }
    }

    public void OpenInventory()
    {
        // Check to open inventory
        if (UserInterfaceManager.instance.OpenUI("Inventory"))
        {
            PlayerMovement.instance.canMove = false;
            inventoryActive.SetActive(true);
            PauseMenu.instance.MouseState(false);
            Hotbar.instance.hotbarSelectionText.SetActive(false);
        }
    }

    public void CloseInventory()
    {
        // Check to remove item from hand on inventory close
        if (InventoryInteract.instance.handSlot.item != null)
        {
            InventoryInteract.instance.CheckInventoryClose(inventorySlots);
        }
        PlayerMovement.instance.canMove = true;
        inventoryActive.SetActive(false);
        PauseMenu.instance.MouseState(true);
        Hotbar.instance.hotbarSelectionText.SetActive(true);
        UserInterfaceManager.instance.CloseUI("Inventory");
    }

    public bool AddItemPickup(Item item)
    {
        // Stack item in slot with same item
        foreach(SlotData inventorySlot in inventorySlots)
        {
            if (inventorySlot.item == item)
            {
                if (inventorySlot.stackSize < inventorySlot.item.MaxStack)
                {
                    inventorySlot.stackSize++;
                    inventorySlot.slotText.GetComponent<TextMeshProUGUI>().text = inventorySlot.stackSize.ToString();
                    items.Add(item);
                    return true;
                }
            }
        }
        // Stack item into empty slot
        foreach (SlotData inventorySlot in inventorySlots)
        {
            if (!inventorySlot.item)
            {
                inventorySlot.item = item;
                inventorySlot.stackSize++;
                inventorySlot.slotIcon.GetComponent<Image>().sprite = item.icon;
                inventorySlot.slotText.GetComponent<TextMeshProUGUI>().text = inventorySlot.stackSize.ToString();
                inventorySlot.slotIcon.SetActive(true);
                inventorySlot.slotText.SetActive(true);
                items.Add(item);
                return true;
            }
        }
        Debug.Log("Inventory is full!");
        return false;
    }

    public void RemoveItem()    // Remove item from list
    {
        SlotData deleteItem = InventoryInteract.instance.handSlot;
        for (int i = 0; i < deleteItem.stackSize; i++)
        {
            items.Remove(deleteItem.item);
        }
        InventoryInteract.instance.RemoveDataFromSlot(deleteItem);
    }
}
