using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarInput : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) Inventory.Instance.SelectHotbar(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) Inventory.Instance.SelectHotbar(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) Inventory.Instance.SelectHotbar(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) Inventory.Instance.SelectHotbar(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) Inventory.Instance.SelectHotbar(4);
        if (Input.GetKeyDown(KeyCode.Alpha6)) Inventory.Instance.SelectHotbar(5);
        if (Input.GetKeyDown(KeyCode.Alpha7)) Inventory.Instance.SelectHotbar(6);
        if (Input.GetKeyDown(KeyCode.Alpha8)) Inventory.Instance.SelectHotbar(7);
        if (Input.GetKeyDown(KeyCode.Alpha9)) Inventory.Instance.SelectHotbar(8);
        if (Input.GetKeyDown(KeyCode.Alpha0)) Inventory.Instance.SelectHotbar(9);
    }
}