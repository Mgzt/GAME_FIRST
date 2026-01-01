using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Farm/Crop Database")]
public class CropDatabase : ScriptableObject
{
    public CropData[] crops;
    Dictionary<int, CropData> map;

    void OnEnable()
    {
        map = new Dictionary<int, CropData>();
        foreach (var c in crops)
            map[c.id] = c;
    }

    public CropData Get(int id)
    {
        return map.TryGetValue(id, out var data) ? data : null;
    }
}
