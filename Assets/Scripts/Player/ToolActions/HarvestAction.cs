using UnityEngine;
using UnityEngine.Tilemaps;

public class HarvestAction : ToolAction
{
    public override ToolType ToolType => ToolType.None; // click tay không

    public Tilemap cropMap;
    public Tilemap tilledMap;
    public Transform player;

    [SerializeField] float range = 1.5f;

    public override void Use()
    {
        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cell = cropMap.WorldToCell(mouseWorld);

        if (Vector2.Distance(player.position, cropMap.GetCellCenterWorld(cell)) > range)
            return;

        FarmTileData tile = FarmManager.Instance.GetTile(cell);
        if (tile.dead)
            return; // không thu cây chết

        if (tile.seedID == -1)
            return;

        CropData crop = FarmManager.Instance.cropDB.Get(tile.seedID);
        if (crop == null)
            return;

        if (!tile.IsReadyToHarvest(crop))
            return;




        // 🎁 THƯỞNG ITEM
        ItemData item =
            Inventory.Instance.itemDB.Get(crop.harvestItemID);

        if (item == null)
        {
            Debug.LogError("❌ Item not found ID: " + crop.harvestItemID);
            return;
        }

        Inventory.Instance.AddItem(item, 1);

        // 🌾 THU HOẠCH
        if (crop.regrow)
        {
            // 🌱 CHUYỂN SANG REGROW MODE
            tile.waitingRegrow = true;
            tile.regrowCounter = 0;

            // ❗ KHÔNG ĐƯỢC ĐỤNG growDay
            // ❗ KHÔNG reset stage về 0

            int regrowStage = crop.growthTiles.Length - 2; // cây sau khi hái
            tile.stage = regrowStage;

            cropMap.SetTile(cell, crop.growthTiles[regrowStage]);
        }
        else
        {
            tile.seedID = -1;
            tile.stage = 0;
            tile.growDay = 0;
            tile.emptyDays = 0;
            cropMap.SetTile(cell, null);
            // đất vẫn còn → giữ tilled
        }

        Debug.Log("🌾 Harvested!");
    }
}
