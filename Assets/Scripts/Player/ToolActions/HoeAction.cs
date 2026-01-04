using UnityEngine;
using UnityEngine.Tilemaps;

public class HoeAction : ToolAction
{
    public override ToolType ToolType => ToolType.Hoe;

    public Tilemap groundMap;       // Layer Ground (đất gốc)
    public Tilemap tilledMap;       // Layer TileGround (đất đã cuốc)
    public Tilemap decorationMap;   // Layer Decoration (cây, hoa, đá trang trí...)
    public Tilemap solidMap;        // Optional: nếu có layer Solid (tường, đá lớn...)
    public Tilemap cropMap;        // Optional: nếu có layer Solid (tường, đá lớn...)


    public TileBase tilledTile;

    [SerializeField] float hoeRange = 1.5f;
    [SerializeField] Transform player;

    public override void Use()
    {
        if (groundMap == null || tilledMap == null || tilledTile == null || decorationMap == null) return;

        // Lấy vị trí chuột trong world
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Raycast 2D để tìm collider nào bị trúng đầu tiên
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, Mathf.Infinity);

        Vector3Int cell;

        // Nếu raycast trúng một collider nào đó
        if (hit.collider != null)
        {
            Tilemap hitTilemap = hit.collider.GetComponent<Tilemap>();

            if (hitTilemap != null)
            {
                // Lấy cell từ điểm trúng (chính xác hơn)
                cell = hitTilemap.WorldToCell(hit.point);

                // Ưu tiên: Nếu trúng Decoration hoặc Solid → KHÔNG cho cuốc
                if (hitTilemap == decorationMap && decorationMap.GetTile(cell) != null)
                {
                    Debug.Log("❌ Không cuốc được: Có Decoration ở đây");
                    return;
                }

                if (solidMap != null && hitTilemap == solidMap && solidMap.GetTile(cell) != null)
                {
                    Debug.Log("❌ Không cuốc được: Có Solid ở đây");
                    return;
                }

                // Nếu trúng Ground → cho phép cuốc (sẽ check tiếp bên dưới)
                if (hitTilemap != groundMap)
                {
                    // Nếu trúng layer khác không phải ground/decoration/solid → có thể bỏ qua hoặc reject
                    // Tùy bạn: ở đây mình cho phép nếu không phải decoration/solid
                }
            }
            else
            {
                // Hit vào object không phải tilemap (ví dụ player, NPC...) → reject
                return;
            }
        }
        else
        {
            // Không hit gì → fallback: dùng vị trí chuột để lấy cell từ groundMap
            cell = groundMap.WorldToCell(mouseWorldPos);
        }

        // Check khoảng cách từ player
        Vector3 cellCenter = groundMap.GetCellCenterWorld(cell);
        float distance = Vector2.Distance(new Vector2(player.position.x, player.position.y), cellCenter);
        FarmTileData data = FarmManager.Instance.GetTile(cell);
        // nếu là cây chết → dọn
        if (data.dead)
        {
            data.dead = false;
            data.seedID = -1;
            data.stage = 0;
            data.growDay = 0;
            data.dryDays = 0;

            data.tilled = true;
            data.watered = false; // cuốc xong là đất khô
            cropMap.SetTile(cell, null);
          //  Debug.Log("✅ DON ");
            return;
        }

        if (distance > hoeRange)
        {
            Debug.Log("❌ Quá xa để cuốc");
            return;
        }

        // Check lại Decoration (phòng trường hợp fallback)
        if (decorationMap.GetTile(cell) != null)
        {
            Debug.Log("❌ Không cuốc được: Có Decoration ở đây");
            return;
        }

        // Check Solid nếu có
        if (solidMap != null && solidMap.GetTile(cell) != null)
        {
            Debug.Log("❌ Không cuốc được: Có Solid");
            return;
        }

        // Check có phải đất và chưa cuốc chưa
        if (groundMap.GetTile(cell) == null)
        {
            Debug.Log("❌ Không phải đất để cuốc");
            return;
        }

        if (tilledMap.GetTile(cell) != null)
        {
            Debug.Log("❌ Đã cuốc rồi");
            return;
        }


        // Thành công → cuốc đất
        tilledMap.SetTile(cell, tilledTile);
        Debug.Log("✅ HOE SUCCESS at " + cell);
        data.tilled = true;
        data.watered = false; // cuốc xong là đất khô
    }
}