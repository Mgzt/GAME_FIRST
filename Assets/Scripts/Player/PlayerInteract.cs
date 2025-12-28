using UnityEngine;
using System.Collections.Generic;

public class PlayerUseItem : MonoBehaviour
{
    Dictionary<ToolType, ToolAction> actionMap;

    void Awake()
    {
        actionMap = new Dictionary<ToolType, ToolAction>();

        // Tự động lấy tất cả ToolAction gắn trên Player
        foreach (ToolAction action in GetComponents<ToolAction>())
        {
            actionMap[action.ToolType] = action;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            UseSelectedItem();
        }
    }

    void UseSelectedItem()
    {
        if (Inventory.Instance == null) return;

        ItemData item = Inventory.Instance.GetSelectedItem();
        if (item == null) return;

        if (item.itemType != ItemType.Tool) return;

        // 🔑 GỌI ACTION ĐÚNG THEO TOOL
        if (actionMap.TryGetValue(item.toolType, out ToolAction action))
        {
            action.Use();
        }
        else
        {
            Debug.LogWarning("No ToolAction for: " + item.toolType);
        }
    }
}
