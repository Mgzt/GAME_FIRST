using UnityEngine;

public class InventoryDebug : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
            SaveManager.Instance.Save();

        if (Input.GetKeyDown(KeyCode.F9))
            SaveManager.Instance.Load();

        if (Input.GetKeyDown(KeyCode.T))
        {
            PlayerStats.Instance.UseStamina(10);
            Debug.Log("Stamina: " + PlayerStats.Instance.stamina);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerStats.Instance.RestoreStamina();
            Debug.Log("Restore stamina");
        }
    }
}
