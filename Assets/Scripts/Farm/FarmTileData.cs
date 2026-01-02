[System.Serializable]
public class FarmTileData
{
    public bool tilled;     // đã cuốc chưa
    public bool watered;    // đã tưới chưa
    public int seedID = -1; // chưa trồng gì
    public int growDay;     // số ngày đã lớn

    public int stage;
    public int emptyDays;

    public bool IsReadyToHarvest(CropData crop)
    {
        return seedID != -1 &&
               stage >= crop.growthTiles.Length - 1;
    }

}
