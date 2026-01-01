using System.IO;
using UnityEngine;
using static UnityEditor.Progress;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    string path;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        path = Application.persistentDataPath + "/save.json";
    }

    public void Save()
    {
        SaveData data = new SaveData();

        // 🌞 DAY
        data.day = DayManager.Instance.day;

        // 👤 PLAYER - Sửa phần này trong hàm Save()
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            data.playerPosition = player.transform.position;
            Debug.Log("✅ Đã lưu vị trí Player: " + data.playerPosition);
        }
        else
        {
            Debug.LogWarning("⚠️ Không tìm thấy Player trong scene khi Save! Vị trí sẽ được lưu là Vector3.zero.");
            data.playerPosition = Vector3.zero; // hoặc vị trí spawn mặc định của bạn
        }

        // 🌱 FARM
        foreach (var pair in FarmManager.Instance.GetAllTiles())
        {
            var t = pair.Value;
            if (t.seedID == -1) continue;

            data.farmTiles.Add(new SaveFarmTile
            {
                cell = pair.Key,
                seedID = t.seedID,
                stage = t.stage,
                growDay = t.growDay
            });
        }

        // 🎒 INVENTORY
        Debug.Log("🔍 Đang lưu Inventory - Các item sẽ được save:");
        int index = 0;
        foreach (var slot in Inventory.Instance.GetAllSlots())
        {
            if (slot.IsEmpty)
            {
                continue;
            }

            int itemID = slot.itemData.itemID;
            int qty = slot.quantity;
            string itemName = slot.itemData.itemName; // giả sử ItemData có field itemName (nếu không có thì bỏ qua dòng này)

            Debug.Log($"   [{index}] ItemID: {itemID} | Quantity: {qty} | Name: {itemName}");

            data.inventory.Add(new SaveInventorySlot
            {
                itemID = itemID,
                quantity = qty
            });

            index++;
        }

        if (index == 0)
        {
            Debug.Log("   → Inventory trống, không lưu item nào.");
        }
        else
        {
            Debug.Log($"✅ Đã lưu {index} item(s) vào save file.");
        }

        File.WriteAllText(path, JsonUtility.ToJson(data, true));
        Debug.Log("💾 GAME SAVED");
    }

    public void Load()
    {
        if (!File.Exists(path))
        {
            Debug.Log("❌ No save file");
            return;
        }

        SaveData data = JsonUtility.FromJson<SaveData>(File.ReadAllText(path));

        // 🌞 DAY
        DayManager.Instance.day = data.day;

        // 👤 PLAYER
        // Trong hàm Load()
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            player.transform.position = data.playerPosition;
            Debug.Log("✅ Đã load vị trí Player: " + data.playerPosition);
        }
        else
        {
            Debug.LogWarning("⚠️ Không tìm thấy Player trong scene khi Load! Không thể khôi phục vị trí.");
        }

        // 🌱 FARM
        FarmManager.Instance.ClearFarm();

        foreach (var t in data.farmTiles)
        {
            FarmManager.Instance.RestoreTile(t);
        }

        // 🎒 INVENTORY
        Inventory.Instance.Clear();
        foreach (var s in data.inventory)
        {
            ItemData item = Inventory.Instance.itemDB.Get(s.itemID);

            if (item == null)
            {
                Debug.LogWarning($"⚠️ Item với ID {s.itemID} không tồn tại trong database! Bỏ qua item này khi load.");
                continue; // Bỏ qua item không hợp lệ
            }

            Inventory.Instance.AddItem(item, s.quantity);
        }

        Debug.Log("📂 GAME LOADED");
    }
}
