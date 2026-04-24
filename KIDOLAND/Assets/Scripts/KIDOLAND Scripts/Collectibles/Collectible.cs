using UnityEngine;

/// <summary>
/// Basic Collectible script has the base detection logic for player using Tags.
/// sets the Object false in the pool after use and fires events to increase score UI value
/// </summary>
public class Collectible : MonoBehaviour
{
    [SerializeField] private int value = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        gameObject.SetActive(false);
        GameEvents.TriggerCollectiblePicked(value); // fire event at last or it bugs out
    }
}