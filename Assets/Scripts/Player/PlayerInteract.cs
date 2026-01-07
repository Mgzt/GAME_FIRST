using UnityEngine;
using System.Collections.Generic;

public class PlayerUseItem : MonoBehaviour
{
    Dictionary<ToolType, ToolAction> actionMap;

    void Awake()
    {
        actionMap = new Dictionary<ToolType, ToolAction>();

        foreach (ToolAction action in GetComponents<ToolAction>())
        {
            actionMap[action.ToolType] = action;
        }
    }

    void Update()
    {
        if (!GameState.CanControlPlayer)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            UseSelectedItem();
        }

    }

    void UseSelectedItem()
    {
        if (Inventory.Instance == null)
            return;

        ItemData item = Inventory.Instance.GetSelectedItem();

        // ✋ TAY KHÔNG → HARVEST
        ToolType typeToUse = item == null
            ? ToolType.None
            : item.toolType;

        if (actionMap.TryGetValue(typeToUse, out ToolAction action))
        {
            action.TryUse();   // ⭐ DÙNG CHUNG
        }
        else
        {
            Debug.LogWarning("❌ No ToolAction for: " + typeToUse);
        }
    }
}
