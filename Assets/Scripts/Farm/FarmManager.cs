using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FarmManager : MonoBehaviour
{
    public static FarmManager Instance;

    [Header("Tilemaps")]
    public Tilemap groundMap;
    public Tilemap cropMap;
    public Tilemap wateredMap;

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
        var tile = GetTile(data.cell);
        tile.seedID = data.seedID;
        tile.stage = data.stage;
        tile.growDay = data.growDay;

        CropData crop = cropDB.Get(tile.seedID);
        cropMap.SetTile(data.cell, crop.growthTiles[tile.stage]);
    }




    // ================= DAY SYSTEM =================

    void GrowCrops()
    {
        foreach (var pair in tiles)
        {
            Vector3Int cell = pair.Key;
            FarmTileData tile = pair.Value;

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
    void OnNewDay()
    {
        GrowCrops();
        ResetWater();
    }
}
