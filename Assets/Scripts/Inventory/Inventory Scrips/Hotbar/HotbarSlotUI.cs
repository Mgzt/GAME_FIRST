using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotbarSlotUI : MonoBehaviour
{
    [Header("UI")]
    public Image icon;
    public Text countText;
    public Image highlight;

    public void SetSlot(InventorySlot slot, bool selected)
    {
        highlight.enabled = selected;

        if (slot == null || slot.IsEmpty)
        {
            icon.enabled = false;
            countText.text = "";
            return;
        }

        icon.enabled = true;
        icon.sprite = slot.itemData.icon;
        countText.text = slot.quantity > 1 ? slot.quantity.ToString() : "";
    }
}