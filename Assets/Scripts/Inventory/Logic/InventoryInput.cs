using UnityEngine;

public class InventoryInput : MonoBehaviour
{
    public GameObject bagUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            bagUI.SetActive(!bagUI.activeSelf);
        }
    }
}
