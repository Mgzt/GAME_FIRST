using UnityEngine;

public abstract class ToolAction : MonoBehaviour
{
    public abstract ToolType ToolType { get; }

    [Header("Cooldown")]
    public float cooldown = 0.3f;
    float lastUseTime;
    [Header("Animation")]
    protected Animator animator;
    [SerializeField] protected string animationStateName;
    protected int animationHash;

    [Header("Stamina")]
    public int staminaCost = 1;

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        animationHash = Animator.StringToHash(animationStateName);
        if (animator == null)
            Debug.LogError("❌ Animator NOT FOUND in parent");
    }


    // 👉 HÀM DUY NHẤT ĐƯỢC GỌI TỪ BÊN NGOÀI
    public void TryUse()
    {
        if (PlayerStats.Instance == null)
            return;

        // ✅ Đủ stamina → cho phép dùng tool
        // ❌ Không đủ stamina → không làm gì
        if (!PlayerStats.Instance.UseStamina(staminaCost))
        {
            Debug.Log("❌ Not enough stamina");
            return;
        }
        //======= ANIMATION =============
        if (Time.time - lastUseTime < cooldown)
            return;

        lastUseTime = Time.time;
        PlayAnimation();
        //=========== use tool =========
        Use();
    }
    void PlayAnimation()
    {
        if (animator == null)
        {
            Debug.LogError("Animator NULL");
            return;
        }

        if (string.IsNullOrEmpty(animationStateName))
        {
            Debug.LogError("animationStateName empty");
            return;
        }

        animator.CrossFade(animationHash, 0);
    }
    // 🔒 Tool con chỉ override hàm này
    protected abstract void Use();
}
