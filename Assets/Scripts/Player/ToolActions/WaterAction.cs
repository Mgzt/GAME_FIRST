using UnityEngine;
using UnityEngine.Tilemaps;

public class WaterAction : ToolAction
{
    public override ToolType ToolType => ToolType.WateringCan;

    public Tilemap tilledMap;     // đất đã cuốc (khô)
    public Tilemap wateredMap;    // đất đã tưới (ướt)
    public TileBase wateredTile; // sprite đất ướt
    [SerializeField] float useRange = 1.5f;
    [SerializeField] Transform player;

    public override void Use()
    {
        if (tilledMap == null || wateredMap == null || player == null) return;

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

        // Phải có cây
        if (data.seedID == -1)
            return;

        // Đã tưới rồi thì thôi
        if (data.watered)
            return;

        // 💧 TƯỚI NƯỚC
        data.watered = true;

        // Hiển thị đất ướt
        wateredMap.SetTile(cell, wateredTile);

       // Debug.Log("💧 Watered at " + cell);  
    }
}
