using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class InventoryInteract : MonoBehaviour
{
    #region Singleton
    public static InventoryInteract instance;
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
    [Header("Hand data:")]
    public SlotData handSlot;

    [Header("Previous Slot Data:")]
    public SlotData previousSlot;

    [Header("Swap Hand Item Data:")]
    public SlotData swapSlot;
    public void OnMouseSlide(GameObject clickedSlot)
    {
        foreach(SlotData inventorySlot in Inventory.instance.inventorySlots)
        {
            if (inventorySlot.Slot == clickedSlot)
            {
                if (handSlot.item != null)   // Does hand have an item
                {
                    SingleItemStack(inventorySlot);
                }
            }
        }
    }
    public void OnSlotCLick(GameObject clickedSlot, bool rightClick)
    {
        foreach (SlotData inventorySlot in Inventory.instance.inventorySlots)
        {
            if (inventorySlot.Slot == clickedSlot)  // Check if slot clicked is in inventory array
            {
                if (handSlot.item == null)   // Does cursor/hand contain an item
                {
                    if (inventorySlot.item) // Does inventory slot contain an item
                    {
                        if (Input.GetKey(Controls.instance.inventoryShortcuts[0]))
                        {
                            // Quick move items around inventory
                            InventoryShortcuts.instance.QuickItemSort(inventorySlot);
                            // Update UI information for item in hovered slot
                            InventoryUI.instance.UpdateInventoryDisplay(null);
                        } else
                        {
                            if (rightClick)
                            {
                                // Grab half of existing slot stack
                                PickupHalfStack(inventorySlot);
                                StorePreviousSlot(previousSlot, inventorySlot);
                                // Update UI information for item in hovered slot
                                if (inventorySlot.stackSize > 0)
                                {
                                    InventoryUI.instance.UpdateInventoryDisplay(inventorySlot);
                                } else
                                {
                                    InventoryUI.instance.UpdateInventoryDisplay(null);
                                }
                            } else
                            {
                                // Put full item stack from slot into hand
                                AssignDataToHandSlot(handSlot, inventorySlot);
                                StorePreviousSlot(previousSlot, inventorySlot);
                                RemoveDataFromSlot(inventorySlot);
                                // Update UI information for item in hovered slot
                                InventoryUI.instance.UpdateInventoryDisplay(null);
                            }
                        }
                    }
                } else // Hand has item
                {
                    if (inventorySlot.item) // Does inventory contain an item
                    {
                        if (handSlot.item == inventorySlot.item)    // Stack item
                        {
                            if (rightClick)
                            {
                                // place single item into slot
                                SingleItemStack(inventorySlot);
                            } else
                            {
                                // Stack max items from hand to slot
                                PlaceWholeStack(handSlot, inventorySlot);
                            }
                        } else
                        {
                            if (!rightClick)
                            {
                                // Swap items from hand to slot
                                AssignDataToHandSlot(swapSlot, handSlot);
                                AssignDataToHandSlot(handSlot, inventorySlot);
                                AssignDataToSlot(inventorySlot, swapSlot);
                                RemoveDataFromSlot(swapSlot);
                                // Update UI information for item in hovered slot
                                InventoryUI.instance.UpdateInventoryDisplay(inventorySlot);
                            }
                        }
                    } else
                    {
                        if (rightClick)
                        {
                            // Place single item into slot
                            SingleItemStack(inventorySlot);
                            // Update UI information for item in hovered slot
                            InventoryUI.instance.UpdateInventoryDisplay(inventorySlot);
                        } else
                        {
                            // Put item in slot from hand
                            AssignDataToSlot(inventorySlot, handSlot);
                            RemoveDataFromSlot(handSlot);
                            // Update UI information for item in hovered slot
                            InventoryUI.instance.UpdateInventoryDisplay(inventorySlot);
                        }
                    }
                }
            }
        }
    }

    // Methods dealing with stacking/spliting and single stacking of items
    public void PickupHalfStack(SlotData inventorySlot)
    {
        //AssignDataToHand(handSlot, inventorySlot);
        AssignDataToHandSlot(handSlot, inventorySlot);
        if (inventorySlot.stackSize % 2 == 0)   // Can half the stack
        {
            handSlot.stackSize = inventorySlot.stackSize / 2;
            inventorySlot.stackSize /= 2;
        }
        else
        {
            if (inventorySlot.stackSize > 1)    // Check stack is large enough to half
            {
                handSlot.stackSize = (inventorySlot.stackSize - 1) / 2;
                inventorySlot.stackSize = ((inventorySlot.stackSize - 1) / 2) + 1;
            }
            else
            {
                RemoveDataFromSlot(inventorySlot);
            }
        }
    }
    public void SingleItemStack(SlotData inventorySlot)  // Place single item from hand stack
    {
        if (inventorySlot.item)
        {
            if (inventorySlot.item == handSlot.item)    // Check items are the same to stack
            {
                if (inventorySlot.stackSize + 1 <= inventorySlot.item.MaxStack)   // Can stack onto slot
                {
                    inventorySlot.stackSize++;
                    CheckHandStack();
                }
            }
        } else
        {
            // Place single item into empty slot
            AssignDataToSlot(inventorySlot, handSlot);
            inventorySlot.stackSize = 1;
            CheckHandStack();
        }

        // Check handStack size to remove item
        void CheckHandStack()
        {
            if (handSlot.stackSize > 1)
            {
                handSlot.stackSize--;
            }
            else
            {
                RemoveDataFromSlot(handSlot);
            }
        }
    }
    public void PlaceWholeStack(SlotData firstSlot, SlotData secondSlot)
    {
        if (secondSlot.item) // Item in slot
        {
            if (secondSlot.item == firstSlot.item)    // Same item in slot as hand
            {
                if (secondSlot.stackSize + firstSlot.stackSize <= secondSlot.item.MaxStack)    // Can stack full hand into slot
                {
                    secondSlot.stackSize += firstSlot.stackSize;
                    RemoveDataFromSlot(firstSlot);
                }
                else
                {
                    // Calculate the difference to stack the max amount possible
                    int stackDifference = secondSlot.item.MaxStack - secondSlot.stackSize;
                    firstSlot.stackSize -= stackDifference;
                    secondSlot.stackSize += stackDifference;
                }
            }
        }
    }

    // Methods that assign data to InventorySlots/HandSlot/PreviousSlot
    public void StorePreviousSlot(SlotData slotHolder, SlotData attachSD) // HandSlot / PreviousSlot
    {
        slotHolder.Slot = attachSD.Slot;
        slotHolder.item = attachSD.item;
        slotHolder.slotIcon = attachSD.slotIcon;
        slotHolder.slotText = attachSD.slotText;
        slotHolder.stackSize = attachSD.stackSize;
    }
    public void AssignDataToSlot(SlotData inventorySlot, SlotData attachSD)  // InventorySlot
    {
        inventorySlot.slotIcon.GetComponent<Image>().sprite = attachSD.slotIcon.GetComponent<Image>().sprite;
        inventorySlot.slotText.GetComponent<TextMeshProUGUI>().text = attachSD.slotText.GetComponent<TextMeshProUGUI>().text;
        inventorySlot.item = attachSD.item;
        inventorySlot.stackSize = attachSD.stackSize;
        inventorySlot.slotIcon.SetActive(true);
        inventorySlot.slotText.SetActive(true);
    }

    public void AssignDataToHandSlot(SlotData swap, SlotData hand)
    {
        swap.item = hand.item;
        swap.stackSize = hand.stackSize;
        swap.slotIcon.GetComponent<Image>().sprite = hand.slotIcon.GetComponent<Image>().sprite;
        swap.slotText.GetComponent<TextMeshProUGUI>().text = hand.slotText.GetComponent<TextMeshProUGUI>().text;
    }

    // Methods that remove data from InventorySlot/HandSlot
    public void RemoveDataFromSlot(SlotData removeSlot)
    {
        removeSlot.slotText.GetComponent<TextMeshProUGUI>().text = "";
        removeSlot.item = null;
        removeSlot.stackSize = 0;
        removeSlot.slotText.SetActive(false);
        removeSlot.slotIcon.SetActive(false);
    }

    // Clear items from hand back to slot on inventory close
    public void CheckInventoryClose(SlotData[] inventorySlots)
    {
        SlotData prevSlot = Array.Find(inventorySlots, inventSlot => inventSlot.Slot.name == previousSlot.Slot.name);
        if (prevSlot != null)
        {
            if (prevSlot.item == null)  // Add item to previous slot if it is empty
            {
                AssignDataToSlot(prevSlot, handSlot);
                RemoveDataFromSlot(handSlot);
                return;
            }
            else
            {
                // Place item into next free slot
                InventoryShortcuts.instance.LoopInventorySlots(handSlot, 0, 30);
                // If no free slot then delete item
                if (handSlot.item != null)
                {
                    Inventory.instance.RemoveItem();
                }
                return;
            }
        }
    }
}
