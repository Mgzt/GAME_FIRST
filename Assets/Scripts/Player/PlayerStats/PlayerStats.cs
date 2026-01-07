using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;


    [Header("Stamina")]
    public int maxStamina = 100;
    public int stamina;

    void Awake()
    {
        // Singleton cơ bản
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Khi game bắt đầu → stamina đầy
        stamina = maxStamina;
    }



    // 🔋 DÙNG STAMINA
    public bool UseStamina(int amount)
    {
        if (stamina < amount)
            return false;

        stamina -= amount;
        return true;
    }

    // ❤️ HỒI STAMINA
    public void RestoreStamina()
    {
        stamina = maxStamina;
    }
}
