using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public ItemData itemData;
    public int quantity;

    public bool IsEmpty => itemData == null;

    public void Clear()
    {
        itemData = null;
        quantity = 0;
    }

    public bool CanStack(ItemData data)
    {
        return !IsEmpty &&
               itemData == data &&
               itemData.stackable &&
               quantity < itemData.maxStack;
    }
}

