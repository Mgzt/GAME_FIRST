using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotbarSlotUI : InventorySlotUI
{
    public Image highlight;

    public void SetSlot(InventorySlot slot, bool selected)
    {
        highlight.enabled = selected;
        base.SetSlot(slot);
    }
}
