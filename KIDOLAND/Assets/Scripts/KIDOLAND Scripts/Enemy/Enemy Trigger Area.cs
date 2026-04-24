using UnityEngine;
using System;
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