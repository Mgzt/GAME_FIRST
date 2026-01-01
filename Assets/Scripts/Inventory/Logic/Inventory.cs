using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    public int SelectedHotbarIndex { get; private set; }

    public InventorySlot heldSlot = new InventorySlot(); // stack đang cầm
    public ItemDatabase itemDB;

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
    public bool AddItem(ItemData data, int amount=1 )
    {
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
    //================= MOVE ITEM ======================
    public void Move(InventorySlot from, InventorySlot to)
    {
        if (from.IsEmpty) return;

        // STACK
        if (to.CanStack(from.itemData))
        {
            int space = to.itemData.maxStack - to.quantity;
            int moveAmount = Mathf.Min(space, from.quantity);

            to.quantity += moveAmount;
            from.quantity -= moveAmount;

            if (from.quantity <= 0)
                from.Clear();

            return;
        }

        // SWAP
        (from.itemData, to.itemData) = (to.itemData, from.itemData);
        (from.quantity, to.quantity) = (to.quantity, from.quantity);
    }

    // 👉 CHUỘT PHẢI CHIA STACK
    public void SplitStack(InventorySlotUI fromUI)
    {
        Debug.Log("SplitStack CALLED");

        // 1️⃣ Check fromUI
        if (fromUI == null)
        {
            Debug.LogError("fromUI NULL");
            return;
        }

        // 2️⃣ Check SlotData
        InventorySlot from = fromUI.SlotData;
        if (from == null)
        {
            Debug.LogError("SlotData NULL");
            return;
        }

        // 3️⃣ Check empty
        if (from.IsEmpty)
        {
            Debug.LogError("SlotData EMPTY");
            return;
        }

        // 4️⃣ Check quantity
        if (from.quantity <= 1)
        {
            Debug.Log("NOT ENOUGH QUANTITY TO SPLIT");
            return;
        }

        // 5️⃣ Check heldSlot (logic)
        if (heldSlot == null)
        {
            Debug.LogError("heldSlot NULL (should never happen)");
            return;
        }

        // 6️⃣ Check HeldItemUI
        if (HeldItemUI.Instance == null)
        {
            Debug.LogError("HeldItemUI.Instance NULL");
            return;
        }

        // ================= SPLIT =================
        int splitAmount = from.quantity / 2;

        heldSlot.itemData = from.itemData;
        heldSlot.quantity = splitAmount;

        from.quantity -= splitAmount;

        // ================= UI =================
        fromUI.Refresh();
        HeldItemUI.Instance.Show(heldSlot);
    }


    public void PlaceHeldStack(InventorySlotUI toUI)
    {
        if (heldSlot.IsEmpty)
            return;

        InventorySlot to = toUI.SlotData;

        // SLOT TRỐNG
        if (to.IsEmpty)
        {
            to.itemData = heldSlot.itemData;
            to.quantity = heldSlot.quantity;

            heldSlot.Clear();
            toUI.Refresh();
            HeldItemUI.Instance.Hide();
            return;
        }

        // STACK CÙNG LOẠI
        if (to.CanStack(heldSlot.itemData))
        {
            int space = to.itemData.maxStack - to.quantity;
            int move = Mathf.Min(space, heldSlot.quantity);

            to.quantity += move;
            heldSlot.quantity -= move;

            if (heldSlot.quantity <= 0)
            {
                heldSlot.Clear();
                HeldItemUI.Instance.Hide();
            }

            toUI.Refresh();
        }
    }
    public void Clear()
    {
        // clear tất cả slot
        foreach (var slot in slots)
            slot.Clear();

        // clear item đang cầm chuột
        heldSlot.Clear();

        // reset hotbar chọn
        SelectedHotbarIndex = 0;
    }
    // ================= SAVE SUPPORT =================
    public List<InventorySlot> GetAllSlots()
    {
        return slots;
    }

}
