using UnityEngine;

public class PlayerJumpController : MonoBehaviour
{
    public float jumpForce = 100f;
    public float jumpApexModifier = 1.2f;

    public float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    public float jumpBufferTime = 0.1f;
    private float jumpBufferCounter;

    public float fallSpeedMultiplier = 2.0f;
    public float upwardSpeedMultiplier = 1.2f; // New multiplier to increase upward velocity

    private Rigidbody2D rb;
    private bool isGrounded = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleCoyoteTime();
        HandleJumpBuffer();
        ApplyFallSpeedMultiplier();
    }

    void HandleCoyoteTime()
    {
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    void HandleJumpBuffer()
    {
        jumpBufferCounter -= Time.deltaTime;
    }

    void ApplyFallSpeedMultiplier()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallSpeedMultiplier - 1) * Time.deltaTime;
        }
    }

    public void Jump()
    {
        if (isGrounded || coyoteTimeCounter > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            coyoteTimeCounter = 0;
            jumpBufferCounter = 0;

            if (!isGrounded && rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpApexModifier * upwardSpeedMultiplier); // Apply upward speed multiplier
            }

            isGrounded = false;
        }
        else
        {
            jumpBufferCounter = jumpBufferTime;
        }
    }

    public void SetGroundedState(bool grounded)
    {
        isGrounded = grounded;
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    public float GetCoyoteTimeCounter()
    {
        return coyoteTimeCounter;
    }
}
