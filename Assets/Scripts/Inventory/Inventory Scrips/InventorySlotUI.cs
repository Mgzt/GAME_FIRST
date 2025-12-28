using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    public Image icon;
    public Text countText;

    InventorySlot slotData;

    public virtual void SetSlot(InventorySlot slot)
    {
        slotData = slot;

        if (slot == null || slot.IsEmpty)
        {
            icon.enabled = false;
            countText.text = "";
            return;
        }

        icon.enabled = true;
        icon.sprite = slot.itemData.icon;
        countText.text =
            slot.quantity > 1 ? slot.quantity.ToString() : "";
    }
}
