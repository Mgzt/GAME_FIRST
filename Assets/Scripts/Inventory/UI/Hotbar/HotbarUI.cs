using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarUI : MonoBehaviour
{
    [Header("UI")]
    public HotbarSlotUI slotPrefab;
    public Transform slotParent;
    HotbarSlotUI[] slots;
    IEnumerator Start()
    {
        while (Inventory.Instance == null)
            yield return null;

        CreateSlots();
    }
    void Update()
    {
        if (Inventory.Instance == null || slots == null) return;

        int count = Mathf.Min(slots.Length, Inventory.Instance.HotbarSize);

        for (int i = 0; i < count; i++)
        {
            var dataSlot = Inventory.Instance.GetHotbarSlot(i);
            bool selected = i == Inventory.Instance.SelectedHotbarIndex;
            slots[i].SetSlot(dataSlot, selected);
        }
    }
    void CreateSlots()
    {
        int hotbarSize = Inventory.Instance.HotbarSize;

        slots = new HotbarSlotUI[hotbarSize];

        for (int i = 0; i < hotbarSize; i++)
        {
            HotbarSlotUI uiSlot =
                Instantiate(slotPrefab, slotParent);

            // ✅ Đặt tên rõ ràng
            uiSlot.gameObject.name = $"HotbarSlot_{i}";

            slots[i] = uiSlot;
        }
    }
}
