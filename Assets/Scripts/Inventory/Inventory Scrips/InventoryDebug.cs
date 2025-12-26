using UnityEngine;

public class InventoryDebug : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            var slot = Inventory.Instance.GetSelectedSlot();
            if (slot == null || slot.IsEmpty)
            {
                Debug.Log("Selected slot is empty");
                return;
            }

            Debug.Log($"Selected: {slot.itemData.itemName} x{slot.quantity}");
        }
    }
}
