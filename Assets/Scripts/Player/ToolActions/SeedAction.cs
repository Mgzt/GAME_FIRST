using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;

public class SeedAction : ToolAction
{
    public override ToolType ToolType => ToolType.Seed;

    public Tilemap tilledMap;     // đất đã cuốc
    public Tilemap cropMap;       // layer cây trồng

    [SerializeField] float useRange = 1.5f;
    [SerializeField] Transform player;

   
    
    public override void Use()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cell = tilledMap.WorldToCell(mouseWorldPos);

        // Check khoảng cách
        Vector3 center = tilledMap.GetCellCenterWorld(cell);
        if (Vector2.Distance(player.position, center) > useRange)
            return;

        // Phải là đất đã cuốc
        if (tilledMap.GetTile(cell) == null)
            return;

        FarmTileData data = FarmManager.Instance.GetTile(cell);

        // Chưa cuốc thì không trồng
        if (!data.tilled)
            return;

        // Đã trồng rồi thì không trồng nữa
        // ❌ ĐÃ CÓ CÂY → KHÔNG ĐƯỢC GIEO
        if (data.seedID != -1)
        {
            Debug.Log("❌ Tile already has a crop");
            return;
        }
        ItemData item = Inventory.Instance.GetSelectedItem();
        if (item == null) return;

        // 🌱 GIEO HẠT
        data.seedID = item.seedID;
        data.stage = 0;
        data.growDay = 0;
        data.watered = false;
        data.emptyDays = 0;
        CropData crop = FarmManager.Instance.cropDB.Get(item.seedID);
        if (crop == null)
        {
            Debug.LogError("❌ Crop not found ID: " + item.seedID);
            return;
        }

        // vẽ mầm cây (stage 0)
        cropMap.SetTile(cell, crop.growthTiles[0]);
    }
}
