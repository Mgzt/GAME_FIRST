using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler
{
    public Image icon;
    public Text countText;

    public InventorySlot SlotData { get; private set; }

    public virtual void SetSlot(InventorySlot slot)
    {
        SlotData = slot;
        Refresh();
    }
    public void Refresh()
    {
        if (SlotData == null || SlotData.IsEmpty)
        {
            icon.enabled = false;
            countText.text = "";
            return;
        }

        icon.enabled = true;
        icon.sprite = SlotData.itemData.icon;
        countText.text =
            SlotData.quantity > 1 ? SlotData.quantity.ToString() : "";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Inventory.Instance.SplitStack(this);
        }

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Inventory.Instance.PlaceHeldStack(this);
        }
    }

}
