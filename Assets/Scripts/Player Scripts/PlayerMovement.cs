using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 moveInput;
    private Rigidbody2D rb;
    private bool isFacingRight = true;
    private bool controlsEnabled = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (controlsEnabled)
        {
            Move();
        }
    }

    public void OnMove(Vector2 input)
    {
        moveInput = input;
    }

    private void Move()
    {
        Vector2 moveVelocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);
        rb.velocity = moveVelocity;

        if (moveInput.x > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveInput.x < 0 && isFacingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void SetControlsEnabled(bool enabled)
    {
        controlsEnabled = enabled;
    }
}
