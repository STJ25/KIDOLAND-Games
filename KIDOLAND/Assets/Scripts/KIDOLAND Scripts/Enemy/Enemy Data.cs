using UnityEngine;
/// <summary>
/// Basic Scriptable Object for Enemy Types. Can configure multiple types of enemies with different Data and Attributes
/// Added in the KIDOLAND folder check project view, Right click to find enemy data.
/// </summary>
[CreateAssetMenu(fileName = "EnemyData", menuName = "KIDOLAND/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Min(0f)] public float moveSpeed = 2f;
    [Min(1f)] public float chaseMultiplier = 2f;
}
