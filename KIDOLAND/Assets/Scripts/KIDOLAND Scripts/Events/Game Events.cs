using System;
/// <summary>
/// Game Events Static Class. All Major Events reside here. 
/// Can be subscribed to and SUed in different functions
/// </summary>
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