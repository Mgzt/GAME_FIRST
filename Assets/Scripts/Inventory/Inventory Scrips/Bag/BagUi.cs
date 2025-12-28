using UnityEngine;

public class BagUI : MonoBehaviour
{
    public InventorySlotUI slotPrefab;
    public Transform slotParent;

    InventorySlotUI[] slots;

    void Start()
    {
        CreateSlots();
        gameObject.SetActive(false); // mặc định ẩn
    }

    void Update()
    {
        if (!gameObject.activeSelf) return;

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetSlot(Inventory.Instance.slots[i]);
        }
    }

    void CreateSlots()
    {
        int count = Inventory.Instance.slots.Count;
        slots = new InventorySlotUI[count];

        for (int i = 0; i < count; i++)
        {
            var ui = Instantiate(slotPrefab, slotParent);
            ui.name = $"BagSlot_{i}";
            slots[i] = ui;
        }
    }
}
