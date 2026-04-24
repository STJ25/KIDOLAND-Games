using UnityEngine;
using System;
/// <summary>
/// Trigger area script for the Player. Uses a Transform Event action. 
/// Uses tag to identify and store player collider info.
/// Fires Events to trigger enemy states in Enemy Behaviour
/// </summary>
[RequireComponent(typeof(BoxCollider2D))] // isTrigger will be on 
public class EnemyTriggerArea : MonoBehaviour
{
    public static event Action<Transform> OnPlayerEnter;
    public static event Action OnPlayerExit;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerEnter?.Invoke(other.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerExit?.Invoke();
        }
    }
}