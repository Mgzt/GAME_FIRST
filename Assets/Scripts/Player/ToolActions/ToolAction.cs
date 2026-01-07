using UnityEngine;

public abstract class ToolAction : MonoBehaviour
{
    public abstract ToolType ToolType { get; }

    [Header("Stamina")]
    public int staminaCost = 1;

    // 👉 HÀM DUY NHẤT ĐƯỢC GỌI TỪ BÊN NGOÀI
    public void TryUse()
    {
        if (PlayerStats.Instance == null)
            return;

        // ❌ Không đủ stamina → không làm gì
        if (!PlayerStats.Instance.UseStamina(staminaCost))
        {
            Debug.Log("❌ Not enough stamina");
            return;
        }

        // ✅ Đủ stamina → cho phép dùng tool
        Use();
    }

    // 🔒 Tool con chỉ override hàm này
    protected abstract void Use();
}
