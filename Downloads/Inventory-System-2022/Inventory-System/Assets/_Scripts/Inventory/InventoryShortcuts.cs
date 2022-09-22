using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InventoryShortcuts : MonoBehaviour
{
    #region Singleton
    public static InventoryShortcuts instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }
    #endregion
    public int index;
    private int hotbarIndex = 9;
    private int inventoryIndex = 30;
    public void DoubleClick()
    {
        Debug.Log("double click");
        // Loop through all slots to check whether they can be stacked into the handSlot
        SlotData handST = InventoryInteract.instance.handSlot;
        foreach (SlotData inventorySlot in Inventory.instance.inventorySlots)
        {
            if (inventorySlot.item) // Skip iteration if stack if max stack size
            {
                if (inventorySlot.stackSize == inventorySlot.item.MaxStack)
                {
                    continue;
                }
            }
            InventoryInteract.instance.PlaceWholeStack(inventorySlot, handST);
        }
    }

    public void QuickItemSort(SlotData inventorySlot)
    {
        // Get index of clicked slot
        index = System.Array.IndexOf(Inventory.instance.inventorySlots, inventorySlot);

        // Find next available slot
        if (index <= hotbarIndex)   // Move from hotbar to inventory
        {
            LoopInventorySlots(inventorySlot, hotbarIndex + 1, inventoryIndex);
        } else if (index <= inventoryIndex) // Move from inventory to hotbar
        {
            LoopInventorySlots(inventorySlot, 0, hotbarIndex);
        }
    }

    public void LoopInventorySlots(SlotData inventorySlot, int startValue, int maxValue)
    {
        // Loop array for same item
        for (int i = startValue; i <= maxValue; i++)
        {
            if (Inventory.instance.inventorySlots[i].item == inventorySlot.item)
            {
                // Item stack
                if (Inventory.instance.inventorySlots[i].stackSize < inventorySlot.item.MaxStack)
                {
                    if (Inventory.instance.inventorySlots[i].stackSize + inventorySlot.stackSize <= Inventory.instance.inventorySlots[i].item.MaxStack)
                    {
                        Inventory.instance.inventorySlots[i].stackSize += inventorySlot.stackSize;
                        InventoryInteract.instance.RemoveDataFromSlot(inventorySlot);
                        return;
                    }
                    else
                    {
                        // Calculate the difference to stack the max amount possible
                        int stackDifference = inventorySlot.item.MaxStack - Inventory.instance.inventorySlots[i].stackSize;
                        inventorySlot.stackSize -= stackDifference;
                        Inventory.instance.inventorySlots[i].stackSize += stackDifference;
                    }
                }
            }
        }
        // Loop array for empty slots
        for (int i = startValue; i <= maxValue; i++)
        {
            if (Inventory.instance.inventorySlots[i].item == null)
            {
                InventoryInteract.instance.AssignDataToSlot(Inventory.instance.inventorySlots[i], inventorySlot);
                InventoryInteract.instance.RemoveDataFromSlot(inventorySlot);
                return;
            }
        }
    }

}
