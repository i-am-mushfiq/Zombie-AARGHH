using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public float jumpForce = 100f;
    public float coyoteTime = 0.2f;
    public float jumpBufferTime = 0.1f;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    private bool isGrounded = false;
    private bool controlsEnabled = true;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            if (jumpBufferCounter > 0f)
            {
                Jump();
            }
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        jumpBufferCounter -= Time.deltaTime;
    }

    public void OnJump()
    {
        if (controlsEnabled && (isGrounded || coyoteTimeCounter > 0))
        {
            Jump();
        }
        else
        {
            jumpBufferCounter = jumpBufferTime;
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        coyoteTimeCounter = 0;
        jumpBufferCounter = 0;

        if (isGrounded)
        {
            isGrounded = false;
        }
    }

    public void SetControlsEnabled(bool enabled)
    {
        controlsEnabled = enabled;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
