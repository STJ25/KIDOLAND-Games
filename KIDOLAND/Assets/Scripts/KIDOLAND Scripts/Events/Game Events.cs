using System;

public static class GameEvents
{
    public static Action OnGameOver;

    public static Action<int> OnCollectiblePicked;

    public static void TriggerGameOver()
    {
        OnGameOver?.Invoke();
    }

    public static void TriggerCollectiblePicked(int value)
    {
        OnCollectiblePicked?.Invoke(value);
    }
}