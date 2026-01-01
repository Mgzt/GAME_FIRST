using System;
using UnityEngine;

[Serializable]
public class SaveFarmTile
{
    public Vector3Int cell;
    public int seedID;
    public int stage;
    public int growDay;
}
