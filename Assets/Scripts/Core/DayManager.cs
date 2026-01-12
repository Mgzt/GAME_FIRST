using UnityEngine;
using System;

public enum TimeOfDay
{
    Morning,
    Afternoon,
    Evening,
    Night
}

public class DayManager : MonoBehaviour
{
    public static DayManager Instance;
    public static event Action OnNewDay;
    public static event Action<TimeOfDay> OnTimeChanged;

    // 🌞 SỰ KIỆN SANG NGÀY
    [Header("Day Info")]
    public int day = 1;
    public TimeOfDay currentTime = TimeOfDay.Morning;

    [Header("Timer Info")]
    public float dayDuration = 300f; // 1 ngày = 300s (5 phút)
    float timer;
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
        UpdateTime();
    }

    void UpdateTime()
    {
        timer += Time.deltaTime;

        float percent = timer / dayDuration;

        if (percent < 0.25f)
            SetTime(TimeOfDay.Morning);
        else if (percent < 0.5f)
            SetTime(TimeOfDay.Afternoon);
        else if (percent < 0.75f)
            SetTime(TimeOfDay.Evening);
        else
            SetTime(TimeOfDay.Night);

        // Hết ngày → tự sang ngày mới (OPTIONAL)
        if (timer >= dayDuration)
        {
            timer = dayDuration; // khóa lại
        }
    }

    void SetTime(TimeOfDay time)
    {
        if (currentTime == time) return;

        currentTime = time;
        Debug.Log("⏰ Time: " + currentTime);

        OnTimeChanged?.Invoke(currentTime);
    }
    public void NextDay()
    {
        day++;
        // Debug.Log("🌞 DAY " + day);
        timer = 0f;
        currentTime = TimeOfDay.Morning;
        OnNewDay?.Invoke(); // PHÁT SỰ KIỆN
        OnTimeChanged?.Invoke(currentTime);
    }
}
