using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 100f;
    public float jumpApexModifier = 1.2f; // Modifier for the jump apex

    public float dashSpeed = 10f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 2f;
    public GameObject dashAvailableCanvas;

    public GameObject bulletPrefab;
    public Transform shootPoint;

    public PlayerInputActions playerInputActions;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    public bool isFacingRight = true;
    private bool controlsEnabled = true;
    public BulletController bulletController;
    public PlayerHealth playerHealth;
    private bool isGrounded = false;

    public float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    public float jumpBufferTime = 0.1f;
    private float jumpBufferCounter;

    private float lastDashTime = -Mathf.Infinity; // Ensure this is declared and initialized
    private bool isDashing = false; // Ensure this is declared

    void Awake()
    {
        controlsEnabled = true;
        playerInputActions = new PlayerInputActions();
        rb = GetComponent<Rigidbody2D>();

        playerInputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerInputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        playerInputActions.Player.Jump.performed += ctx => Jump();
        playerInputActions.Player.Shoot.performed += ctx => Shoot();
        playerInputActions.Player.Reload.performed += ctx => Reload();
        playerInputActions.Player.Dash.performed += ctx => Dash();
    }

    void OnEnable()
    {
        playerInputActions.Enable();
        controlsEnabled = true;
    }

    void OnDisable()
    {
        playerInputActions.Disable();
    }

    void Update()
    {
        if (controlsEnabled && isGrounded)
        {
            Move();
        }

        // Handle coyote time and jump buffering logic
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime; // Reset coyote time counter when grounded
            if (jumpBufferCounter > 0f)
            {
                Jump(); // Execute jump if within the buffer time
            }
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime; // Decrease coyote time counter
        }

        jumpBufferCounter -= Time.deltaTime; // Decrease jump buffer counter

        // Reactivate the dashAvailableCanvas if the cooldown has ended
        if (Time.time >= lastDashTime + dashCooldown)
        {
            dashAvailableCanvas.gameObject.SetActive(true);
        }
    }

    void Move()
    {
        if (isDashing || !isGrounded) return; // Prevent movement if dashing or in the air

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

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void Jump()
    {
        if (controlsEnabled && (isGrounded || coyoteTimeCounter > 0))
        {
            rb.velocity = new Vector2(rb.velocity.x, 0); // Reset the vertical velocity
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            coyoteTimeCounter = 0; // Reset coyote time after jumping
            jumpBufferCounter = 0; // Reset jump buffer after jumping

            // Apply the jump apex modifier if the player is in the air and moving upwards
            if (!isGrounded && rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpApexModifier);
            }

            // If the player is grounded, jumping will make them not grounded
            if (isGrounded)
            {
                isGrounded = false;
            }
        }
        else
        {
            jumpBufferCounter = jumpBufferTime; // Set jump buffer if jump can't be executed
        }
    }

    void Dash()
    {
        if (controlsEnabled && Time.time >= lastDashTime + dashCooldown)
        {
            StartCoroutine(DashRoutine());
            lastDashTime = Time.time;
            dashAvailableCanvas.gameObject.SetActive(false); // Deactivate the dash canvas on dash
        }
    }

    IEnumerator DashRoutine()
    {
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.velocity = new Vector2((isFacingRight ? 1 : -1) * dashSpeed, 0);
        yield return new WaitForSeconds(dashDuration);
        rb.gravityScale = originalGravity;
        isDashing = false;
    }

    void Shoot()
    {
        if (controlsEnabled && bulletController != null)
        {
            bulletController.SpawnBullet();
        }
        else
        {
            Debug.LogWarning("BulletController not assigned or controls disabled!");
        }
    }

    void Reload()
    {
        if (controlsEnabled && bulletController != null)
        {
            bulletController.ammoManager.Reload();
        }
        else
        {
            Debug.LogWarning("AmmoManager not assigned or controls disabled!");
        }
    }

    public void TakeDamage()
    {
        if (controlsEnabled && playerHealth != null)
        {
            playerHealth.TakeDamage();
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (controlsEnabled && other.collider.CompareTag("Enemy"))
        {
            other.gameObject.SetActive(false);
            // Needs to be returned to the pool
            TakeDamage();
        }

        if (other.collider.CompareTag("Ground"))
        {
            isGrounded = true; // Player is grounded when colliding with "Ground"
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.CompareTag("Ground"))
        {
            isGrounded = false; // Player is no longer grounded when leaving "Ground"
        }
    }

    void Death()
    {
        if (controlsEnabled)
        {
            playerHealth.Death();
        }
    }

    void OnPauseStateChanged(bool isPaused)
    {
        controlsEnabled = !isPaused;
    }

    void OnDestroy()
    {
        if (PauseManager.Instance != null)
        {
            PauseManager.Instance.onPauseStateChanged -= OnPauseStateChanged;
        }
    }

    public void DisableControls()
    {
        controlsEnabled = false;
    }

    public void EnableControls()
    {
        controlsEnabled = true;
    }
}
