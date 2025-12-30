using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedAction : ToolAction
{
    public override ToolType ToolType => ToolType.Seed;
    public override void Use()
    {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cell = FarmManager.Instance.groundMap.WorldToCell(worldPos);

        if (FarmManager.Instance.PlantSeed(cell, seed))
        {
            Inventory.Instance.ConsumeSelectedItem(1);
        }
    }

}
