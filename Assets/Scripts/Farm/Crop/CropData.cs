using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Farm/Crop")]
public class CropData : ScriptableObject
{
    [Header("ID")]
    public int id;                     // ID cây (PHẢI TRÙNG seedID)

    [Header("Visual")]
    public TileBase[] growthTiles;     // sprite từng giai đoạn

    [Header("Growth")]
    public int[] daysPerStage;         // số ngày mỗi stage

    [Header("Harvest")]
    public bool regrow;                // có mọc lại không
    public int regrowDays;
    public int harvestItemID;          // item nhận khi thu
    public int regrowStage = 1;
}
