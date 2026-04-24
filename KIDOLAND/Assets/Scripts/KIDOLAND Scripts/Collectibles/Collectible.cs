using UnityEngine;

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