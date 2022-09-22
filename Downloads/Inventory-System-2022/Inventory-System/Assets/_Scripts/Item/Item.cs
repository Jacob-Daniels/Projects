using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    [Header("Item Elements:")]
    new public string name = "New Item";
    public Sprite icon = null;
    public int MaxStack;
    public string details = "";
    public GameObject itemPrefab = null;

    [Header("Item Type:")]
    public bool Duplicate;
    public bool Tool;
    public bool Armour;
    public bool Food;
    public bool Researchable;
    public bool Craftable;
}
