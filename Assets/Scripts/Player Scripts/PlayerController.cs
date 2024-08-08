using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 100f;
    public GameObject bulletPrefab;
    public Transform shootPoint;
    public PlayerInputActions playerInputActions;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    public bool isFacingRight = true;
    private bool controlsEnabled = true;
    public BulletController bulletController;
    public PlayerHealth playerHealth;
    private bool isGrounded = true;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    void Awake()
    {
        playerInputActions = new PlayerInputActions();
        rb = GetComponent<Rigidbody2D>();

        playerInputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerInputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        playerInputActions.Player.Jump.performed += ctx => Jump();
        playerInputActions.Player.Shoot.performed += ctx => Shoot();
        playerInputActions.Player.Reload.performed += ctx => Reload();
    }

    void OnEnable()
    {
        playerInputActions.Enable();
    }

    void OnDisable()
    {
        playerInputActions.Disable();
    }

    void Update()
    {
        if (controlsEnabled)
        {
            Move();
        }

        // Check if the player is grounded
        //isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void Move()
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

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void Jump()
    {
        //if (controlsEnabled && isGrounded)
        if (controlsEnabled)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            //isGrounded = false;
        }
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
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.CompareTag("Ground"))
        {
            isGrounded = false; // Reset isGrounded when leaving the ground
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