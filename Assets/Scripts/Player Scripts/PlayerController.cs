using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dashSpeed = 10f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 2f;
    public GameObject dashAvailableCanvas;

    public Transform shootPoint;

    public PlayerInputActions playerInputActions;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    public bool isFacingRight = true;
    private bool controlsEnabled = true;
    public BulletController bulletController;
    public PlayerHealth playerHealth;

    private float lastDashTime = -Mathf.Infinity;
    private bool isDashing = false;

    private PlayerJumpController playerJumpController; // Reference to PlayerJumpController

    void Awake()
    {
        controlsEnabled = true;
        playerInputActions = new PlayerInputActions();
        rb = GetComponent<Rigidbody2D>();
        playerJumpController = GetComponent<PlayerJumpController>(); // Get reference to PlayerJumpController

        playerInputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerInputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        playerInputActions.Player.Jump.performed += ctx => playerJumpController.Jump(); // Delegate jump to PlayerJumpController
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
        if (controlsEnabled && playerJumpController.IsGrounded())
        {
            Move();
        }

        // Reactivate the dashAvailableCanvas if the cooldown has ended
        if (Time.time >= lastDashTime + dashCooldown)
        {
            dashAvailableCanvas.gameObject.SetActive(true);
        }
    }

    void Move()
    {
        if (isDashing || !playerJumpController.IsGrounded()) return; // Prevent movement if dashing or in the air

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

    public bool IsGrounded()
    {
        return playerJumpController.IsGrounded();
    }

    public bool IsFacingRight()
    {
        return isFacingRight;
    }
    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void Dash()
    {
        if (controlsEnabled && Time.time >= lastDashTime + dashCooldown)
        {
            StartCoroutine(DashRoutine());
            lastDashTime = Time.time;
            dashAvailableCanvas.gameObject.SetActive(false);
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
            TakeDamage();
        }

        if (other.collider.CompareTag("Ground"))
        {
            playerJumpController.SetGroundedState(true); 
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.CompareTag("Ground"))
        {
            playerJumpController.SetGroundedState(false); 
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
