using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "KIDOLAND/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Min(0f)] public float moveSpeed = 2f;
    [Min(1f)] public float chaseMultiplier = 2f;
}
