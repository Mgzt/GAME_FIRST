using UnityEngine;
using System;

public class DayManager : MonoBehaviour
{
    public static DayManager Instance;

    public int day = 1;

    // 🌞 SỰ KIỆN SANG NGÀY
    public static event Action OnNewDay;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Update()
    {
        // test nhanh
        if (Input.GetKeyDown(KeyCode.N))
        {
            NextDay();
        }
    }

    public void NextDay()
    {
        day++;
        Debug.Log("🌞 DAY " + day);

        OnNewDay?.Invoke(); // PHÁT SỰ KIỆN
    }
}
