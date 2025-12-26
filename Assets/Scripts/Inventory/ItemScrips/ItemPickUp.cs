using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[RequireComponent(typeof(Collider2D))]
public class ItemPickUp : MonoBehaviour
{
    public float PickUpRadius = 1f;
    public InventoryItemData ItemData;
   // private Collider2D myCollider;

    //private void Awake()
    //{
    //    myCollider= GetComponent<SphereCollider>(); 
    //    myCollider.isTrigger=true;
    //    myCollider.radius= PickUpRadius;    

    //}
    private void OnTriggerEnter2D(Collider2D other)
    {
        var inventory =other.transform.GetComponent<InventoryHolder>();
        if (!inventory)
        {
            print("not inventory !");
            return;
        }

        if (inventory.InventorySystem.AddToInventory(ItemData,1))
        {
            print(" inventory ok!");
            Destroy(this.gameObject);
        }
    }
}
