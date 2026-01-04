using UnityEngine;

public class InventoryInput : MonoBehaviour
{
    public GameObject bagUI;

    void Update()
    {
        if (GameState.IsSleeping)
            return;
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleBag();
        }
    }

    void ToggleBag()
    {
        bool open = !bagUI.activeSelf;
        bagUI.SetActive(open);
        GameState.IsUIBlocking = open;
    }


}
