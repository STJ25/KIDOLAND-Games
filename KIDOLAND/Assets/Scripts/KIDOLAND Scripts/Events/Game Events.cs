using System;

public static class GameEvents
{
    public static Action OnGameOver;

    public static void TriggerGameOver()
    {
        OnGameOver?.Invoke();
    }
}