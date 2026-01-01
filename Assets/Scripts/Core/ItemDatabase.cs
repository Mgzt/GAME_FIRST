using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item Database")]
public class ItemDatabase : ScriptableObject
{
    public List<ItemData> items;

    public ItemData Get(int id)
    {
        return items.Find(i => i.itemID == id);
    }
}
