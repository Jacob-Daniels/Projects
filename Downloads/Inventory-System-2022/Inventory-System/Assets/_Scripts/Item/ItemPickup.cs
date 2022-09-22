using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;

    public void PickupItem()
    {
        // Add item to inventory
        if (Inventory.instance.AddItemPickup(item))
        {
            Destroy(gameObject);
        }
    }
}
