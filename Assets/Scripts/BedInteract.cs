using UnityEngine;

public class BedInteract : MonoBehaviour
{
    bool playerInRange;

    void Update()
    {
        if (!playerInRange) { return; }
        if (!GameState.CanControlPlayer) return;
        if (Input.GetKeyDown(KeyCode.E))
        {
            Sleep();
        }
    }

    void Sleep()
    {
        if (GameState.IsSleeping)
            return;

        GameState.IsSleeping = true;

        FadeUI.Instance.FadeOut(() =>
        {
            DayManager.Instance.NextDay();
            SaveManager.Instance.Save();

            FadeUI.Instance.FadeIn(() =>
            {
                GameState.IsSleeping = false;
            });
        });
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}
