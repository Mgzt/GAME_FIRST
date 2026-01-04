public static class GameState
{
    public static bool IsSleeping = false;
    public static bool IsUIBlocking = false;

    //== chua dung===
    //IsDialogue

    //IsCutscene

    //IsPaused
    public static bool CanControlPlayer =>
    !IsSleeping && !IsUIBlocking;
}
