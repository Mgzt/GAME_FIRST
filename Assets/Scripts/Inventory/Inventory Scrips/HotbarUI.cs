using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarUI : MonoBehaviour
{
    public HotbarSlotUI[] slots;

    void Update()
    {
    //    Debug.Log(
    //$"[HotbarUI] SelectedIndex = {Inventory.Instance.SelectedHotbarIndex}");
        for (int i = 0; i < slots.Length; i++)
        {
            var slot = Inventory.Instance.GetHotbarSlot(i);
            bool selected = (i == Inventory.Instance.SelectedHotbarIndex);
            slots[i].SetSlot(slot, selected);
        }
    }
}
