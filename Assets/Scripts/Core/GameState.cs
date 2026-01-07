public static class GameState
{
    public static bool IsSleeping = false;
    public static bool IsUIBlocking = false;

    //== chua dung===
    //IsDialogue

    //IsCutscene

    //IsPaused
    public static bool CanControlPlayer
    {
        get
        {
            if (IsSleeping) return false;
            if (IsUIBlocking) return false;

            // 🔋 CHECK STAMINA
            if (PlayerStats.Instance != null &&
                PlayerStats.Instance.stamina <= 0)
                return false;

            return true;
        }
    }
}
