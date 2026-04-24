using UnityEngine;
/// <summary>
/// Rigidbody based 2D Player controller. 8 Directional Movement with animations.
/// Flips sprites on the X axis for opposite Directions and Animations
/// Uses Old Input system.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
#region Inspector
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;

    private Vector2 input;
    private Vector2 movement;
    private Vector2 lastDirection = Vector2.down;
#endregion

#region Unity Cycle and Events
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        movement = input.normalized;

        if (movement != Vector2.zero)
            lastDirection = GetSnappedDirection(movement);

        UpdateAnimation();
        HandleFlip();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = movement * moveSpeed;
    }
#endregion

#region Directional and Animation logic
    private Vector2 GetSnappedDirection(Vector2 dir)
    {
        dir.Normalize();

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        if (angle < 0) angle += 360f;

        if (angle >= 337.5f || angle < 22.5f) return Vector2.right;
        if (angle < 67.5f) return new Vector2(1, 1).normalized;
        if (angle < 112.5f) return Vector2.up;
        if (angle < 157.5f) return new Vector2(-1, 1).normalized;
        if (angle < 202.5f) return Vector2.left;
        if (angle < 247.5f) return new Vector2(-1, -1).normalized;
        if (angle < 292.5f) return Vector2.down;
        return new Vector2(1, -1).normalized;
    }

    private void UpdateAnimation()
    {
        animator.SetFloat("moveX", lastDirection.x);
        animator.SetFloat("moveY", lastDirection.y);
        animator.SetBool("isMoving", movement != Vector2.zero);
    }

    private void HandleFlip()
    {
        if (lastDirection.x != 0)
            spriteRenderer.flipX = lastDirection.x > 0;
    }

#endregion

}