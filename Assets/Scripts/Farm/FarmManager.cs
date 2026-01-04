using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;

public class FarmManager : MonoBehaviour
{
    public static FarmManager Instance;

    [Header("Tilemaps")]
    public Tilemap groundMap;
    public Tilemap cropMap;
    public Tilemap tilledMap;       // Layer TileGround (đất đã cuốc)
    public Tilemap wateredMap;
    public TileBase tilledTile;
    public TileBase baseGroundTile; // đất chưa cuốc (kéo sprite ground gốc vào)
    public TileBase deadCropTile; // cay chet

    [Header("Database")]
    public CropDatabase cropDB;

    Dictionary<Vector3Int, FarmTileData> tiles = new();

    /// SYSTEM CYCLE
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void OnEnable()
    {
        DayManager.OnNewDay += OnNewDay;
    }

    void OnDisable()
    {
        DayManager.OnNewDay -= OnNewDay;
    }

    //   function 
    public FarmTileData GetTile(Vector3Int cell)
    {
        if (!tiles.ContainsKey(cell))
            tiles[cell] = new FarmTileData();

        return tiles[cell];
    }
    //=============== SAVE ===================
    public Dictionary<Vector3Int, FarmTileData> GetAllTiles()
    {
        return tiles;
    }

    public void ClearFarm()
    {
        tiles.Clear();
        cropMap.ClearAllTiles();
        wateredMap.ClearAllTiles();
    }

    public void RestoreTile(SaveFarmTile data)
    {
        FarmTileData tile = GetTile(data.cell);

        tile.tilled = data.tilled;
        tile.seedID = data.seedID;
        tile.stage = data.stage;
        tile.growDay = data.growDay;

        // 🌱 1. VẼ ĐẤT ĐÃ CUỐC (NẾU CÓ)
        if (tile.tilled)
        {
            groundMap.SetTile(data.cell, tilledTile);
        }

        // 🌾 2. VẼ CÂY (NẾU CÓ)
        if (tile.seedID != -1)
        {
            CropData crop = cropDB.Get(tile.seedID);
            if (crop != null)
            {
                int stage = Mathf.Clamp(tile.stage, 0, crop.growthTiles.Length - 1);
                cropMap.SetTile(data.cell, crop.growthTiles[stage]);
            }
        }
    }


    // ================= DAY SYSTEM =================

    void GrowCrops()
    {

        foreach (var pair in tiles)
        {
            Vector3Int cell = pair.Key;
            FarmTileData tile = pair.Value;

            if (tile.waitingRegrow || tile.dead)
                continue;

            if (tile.dead)
                continue;

            if (tile.seedID == -1)
                continue;

            if (!tile.watered)
                continue;

            if (!cropMap.HasTile(cell))
                continue;

            CropData crop = cropDB.Get(tile.seedID);
            if (crop == null)
                continue;

            tile.growDay++;

            int nextStage = tile.stage;
            int dayCount = 0;

            for (int i = 0; i < crop.daysPerStage.Length; i++)
            {
                dayCount += crop.daysPerStage[i];
                if (tile.growDay >= dayCount)
                    nextStage = i + 1;
            }

            nextStage = Mathf.Clamp(
                nextStage,
                0,
                crop.growthTiles.Length - 1
            );

            if (nextStage != tile.stage)
            {
                tile.stage = nextStage;
                cropMap.SetTile(cell, crop.growthTiles[tile.stage]);
            }
        }
    }

    void ResetWater()
    {
        foreach (var tile in tiles.Values)
            tile.watered = false;

        if (wateredMap != null)
            wateredMap.ClearAllTiles();
    }

    void LandReset()
    {
        foreach (var pair in tiles)
        {
            Vector3Int cell = pair.Key;
            FarmTileData tile = pair.Value;

            // ✔ chỉ áp dụng cho đất đã cuốc & KHÔNG có cây
            if (tile.tilled && tile.seedID == -1)
            {
                tile.emptyDays++;

                if (tile.emptyDays >= 3)
                {
                    Debug.Log("🌍 Land reset at " + cell);

                    // reset data
                    tile.tilled = false;
                    tile.seedID = -1;
                    tile.stage = 0;
                    tile.growDay = 0;
                    tile.watered = false;
                    tile.emptyDays = 0;

                    // reset visual
                    tilledMap.SetTile(cell, null);              // XÓA ĐẤT CUỐC
                    groundMap.SetTile(cell, baseGroundTile);    // TRẢ ĐẤT GỐC
                    wateredMap.SetTile(cell, null);
                    cropMap.SetTile(cell, null);

                }
            }
            else
            {
                // có cây hoặc chưa cuốc → reset đếm ngày
                tile.emptyDays = 0;
            }
        }
    }
    //=== cay chet  ===
    void HandleDryCrops()
    {
        foreach (var pair in tiles)
        {
            Vector3Int cell = pair.Key;
            FarmTileData tile = pair.Value;

            // chỉ xử lý khi có cây và chưa chết
            if (tile.seedID == -1 || tile.dead)
                continue;

            if (!tile.watered)
            {
                tile.dryDays++;

                if (tile.dryDays >= 2)
                {
                    // ☠️ CÂY CHẾT
                    tile.dead = true;

                    cropMap.SetTile(cell, deadCropTile);

                    Debug.Log("☠️ Crop died at " + cell);
                }
            }
            else
            {
                // được tưới → reset đếm khô
                tile.dryDays = 0;
            }
        }
    }
    // =========== regrow ============
    void HandleRegrow()
    {
        print("HandleRegrow ");
        foreach (var pair in tiles)
        {
            Vector3Int cell = pair.Key;
            FarmTileData tile = pair.Value;

            if (!tile.waitingRegrow)
                continue;

            if (!tile.watered)
                continue; // regrow cũng cần tưới

            tile.regrowCounter++;

            CropData crop = cropDB.Get(tile.seedID);
            if (crop == null)
                continue;

            if (tile.regrowCounter >= crop.regrowDays)
            {
                print("LONLAI ");
                tile.waitingRegrow = false;
                tile.regrowCounter = 0;

                // 🌿 LỚN LẠI
                tile.stage = crop.growthTiles.Length - 1;
                cropMap.SetTile(cell, crop.growthTiles[tile.stage]);

                Debug.Log("🌱 Regrow completed at " + cell);
            }
        }
    }
    void OnNewDay()
    {
        
        GrowCrops();
        HandleRegrow();
        HandleDryCrops();
        ResetWater();
        LandReset();

    }
}
