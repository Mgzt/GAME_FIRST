using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    public int SelectedHotbarIndex { get; private set; }

    [Header("Size")]
    [SerializeField] int totalSlots = 24;
    [SerializeField] int hotbarSlots = 12;
    public int HotbarSize => hotbarSlots;
    public int BagSize => totalSlots-hotbarSlots;
    public List<InventorySlot> slots = new();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        InitSlots();
    }

    void InitSlots()
    {
        slots.Clear();
        for (int i = 0; i < totalSlots; i++)
            slots.Add(new InventorySlot());
    }

    // ================= ADD ITEM =================
    public bool AddItem(ItemData data, int amount = 1)
    {
        //// 1️⃣ Stack trước
        //if (data.stackable)
        //{
        //    foreach (var slot in slots)
        //    {
        //        if (slot.CanStack(data))
        //        {
        //            slot.quantity += amount;
        //            return true;
        //        }
        //    }
        //}

        //// 2️⃣ Slot trống
        //foreach (var slot in slots)
        //{
        //    if (slot.IsEmpty)
        //    {
        //        slot.itemData = data;
        //        slot.quantity = amount;
        //        return true;
        //    }
        //}

        // 1️⃣ STACK TRƯỚC (ưu tiên hotbar)
        if (data.stackable)
        {
            // stack trong hotbar
            for (int i = 0; i < hotbarSlots; i++)
            {
                if (slots[i].CanStack(data))
                {
                    slots[i].quantity += amount;
                    return true;
                }
            }

            // stack trong bag
            for (int i = hotbarSlots; i < slots.Count; i++)
            {
                if (slots[i].CanStack(data))
                {
                    slots[i].quantity += amount;
                    return true;
                }
            }
        }

        // 2️⃣ SLOT TRỐNG TRONG HOTBAR
        for (int i = 0; i < hotbarSlots; i++)
        {
            if (slots[i].IsEmpty)
            {
                slots[i].itemData = data;
                slots[i].quantity = amount;
                return true;
            }
        }

        // 3️⃣ SLOT TRỐNG TRONG BAG
        for (int i = hotbarSlots; i < slots.Count; i++)
        {
            if (slots[i].IsEmpty)
            {
                slots[i].itemData = data;
                slots[i].quantity = amount;
                return true;
            }
        }



        Debug.Log("Inventory Full!");
        return false;
    }
    public InventorySlot GetBagSlot(int index)
    {
        int bagIndex = hotbarSlots + index;

        if (bagIndex < 0 || bagIndex >= slots.Count)
            return null;

        return slots[bagIndex];
    }

    // ================= HOTBAR =================
    public InventorySlot GetHotbarSlot(int index)
    {
        if (index < 0 || index >= hotbarSlots) return null;
        return slots[index];
    }


    public void SelectHotbar(int index)
    {
        if (index < 0 || index >= hotbarSlots) return;
        SelectedHotbarIndex = index;
    }

    public InventorySlot GetSelectedSlot()
    {
        return GetHotbarSlot(SelectedHotbarIndex);
    }

    public ItemData GetSelectedItem()
    {
        InventorySlot slot = GetSelectedSlot();
        if (slot == null || slot.IsEmpty)
            return null;

        return slot.itemData;
    }
}
