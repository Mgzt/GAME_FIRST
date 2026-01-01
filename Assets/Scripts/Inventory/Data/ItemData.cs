using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType
{
    Tool,
    Seed,
    Crop,
    Resource
}
public enum ToolType
{
    None,
    Hoe,
    Axe,
    WateringCan,
    Seed
}
[CreateAssetMenu(menuName = "Inventory/Item Data")]
public class ItemData : ScriptableObject
{
    [Header("Basic Info")]
    public string itemId;      // ví dụ: hoe_basic
    public string itemName;    // Cuốc
    public ItemType itemType;
    public ToolType toolType;

    public int seedID;
    public int itemID;

    [Header("Visual")]
    public Sprite icon;

    [Header("Stack")]
    public bool stackable = true;
    public int maxStack = 99;
}

