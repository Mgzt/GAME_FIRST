using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public int day;
    public Vector3 playerPosition;

    public List<SaveFarmTile> farmTiles = new();
    public List<SaveInventorySlot> inventory = new();
}
