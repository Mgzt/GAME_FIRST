using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItem : MonoBehaviour
{
    [SerializeField] ItemData itemData;
    [SerializeField] int quantity = 1;

    public void Pickup()
    {
        bool added = Inventory.Instance.AddItem(itemData, quantity);
        if (added)
            Destroy(gameObject);
    }
}