using UnityEngine;

public class BagUI : MonoBehaviour
{
    public InventorySlotUI slotPrefab;
    public Transform slotParent;

    InventorySlotUI[] slots;

    void Start()
    {
        CreateSlots();
        gameObject.SetActive(false); // ẩn ban đầu
    }

    void Update()
    {
        if (Inventory.Instance == null) return;

        for (int i = 0; i < slots.Length; i++)
        {
            InventorySlot slot = Inventory.Instance.GetBagSlot(i);
            slots[i].SetSlot(slot);
        }
    }

    void CreateSlots()
    {
        int count = Inventory.Instance.BagSize;
        slots = new InventorySlotUI[count];

        for (int i = 0; i < count; i++)
        {
            var ui = Instantiate(slotPrefab, slotParent);
            ui.name = $"BagSlot_{i}";
            slots[i] = ui;
        }
    }
}
