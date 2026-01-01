using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragUI : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Reference")]
    public InventorySlotUI fromSlotUI;

    RectTransform rect;
    Canvas canvas;
    CanvasGroup canvasGroup;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // 1️⃣ BẮT ĐẦU KÉO
    public void OnBeginDrag(PointerEventData eventData)
    {
        // Slot trống → không cho kéo
        if (fromSlotUI.SlotData == null || fromSlotUI.SlotData.IsEmpty)
            return;

        // Cho phép raycast xuyên qua icon
        canvasGroup.blocksRaycasts = false;

        // Đưa icon lên Canvas (tránh bị che)
        transform.SetParent(canvas.transform);
    }

    // 2️⃣ ĐANG KÉO
    public void OnDrag(PointerEventData eventData)
    {
        rect.position = eventData.position;
    }

    // 3️⃣ THẢ
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // Slot được thả vào
        InventorySlotUI toSlotUI =
            eventData.pointerEnter?.GetComponentInParent<InventorySlotUI>();

        // ❌ Thả ra ngoài hoặc thả vào chính nó
        if (toSlotUI == null || toSlotUI == fromSlotUI)
        {
            ReturnToSlot();
            return;
        }

        // 🔁 DI CHUYỂN DATA
        Inventory.Instance.Move(
            fromSlotUI.SlotData,
            toSlotUI.SlotData
        );

        // 🔄 CẬP NHẬT UI
        fromSlotUI.Refresh();
        toSlotUI.Refresh();

        ReturnToSlot();
    }

    void ReturnToSlot()
    {
        transform.SetParent(fromSlotUI.transform);
        rect.localPosition = Vector3.zero;
    }
}
