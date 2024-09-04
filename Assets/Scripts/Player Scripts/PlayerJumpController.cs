using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using TMPro;

public class PlayerJumpController : MonoBehaviour
{
    public float jumpForce = 100f;
    public float jumpApexModifier = 1.2f;
    public float coyoteTime = 0.2f;
    private float coyoteTimeCounter;
    public float jumpBufferTime = 0.1f;
    private float jumpBufferCounter;
    public float fallSpeedMultiplier = 2.0f;
    public float upwardSpeedMultiplier = 1.2f;
    public float ledgeDetectionRadius = 0.2f;
    public float ledgeDetectionOffsetX = 0.5f;
    public float ledgeDetectionOffsetY = -0.5f;
    public LayerMask groundLayer;
    public float pushUpSpeed = 10.0f;
    public float pushUpTime = 1.0f; // Duration of the push-up action

    private Rigidbody2D rb;
    private bool isGrounded = false;
    private bool isPushingUp = false;
    private bool hasPushedUp = false; // New flag to track if push-up has been performed
    public float pushOnX = 0.3f;
    private Vector2 ledgePosition;

    private PlayerController playerController;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        HandleCoyoteTime();
        HandleJumpBuffer();
        ApplyFallSpeedMultiplier();
        HandleEdgeDetection();
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
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * jumpApexModifier * upwardSpeedMultiplier);
            }

            isGrounded = false;
            hasPushedUp = false; // Reset the flag when jumping
        }
        else
        {
            jumpBufferCounter = jumpBufferTime;
        }
    }

    void HandleEdgeDetection()
    {
        if (isPushingUp || hasPushedUp) return; // Check if already pushed up

        // Right side ledge detection
        Vector2 circlePosRight = new Vector2(transform.position.x + ledgeDetectionOffsetX, transform.position.y + ledgeDetectionOffsetY);
        Collider2D hitRight = Physics2D.OverlapCircle(circlePosRight, ledgeDetectionRadius, groundLayer);

        if (!isGrounded && hitRight != null)
        {
            if (hitRight.CompareTag("Ground"))
            {
                Debug.Log("Ledge detected on the right side.");
                ledgePosition = new Vector2(circlePosRight.x, circlePosRight.y);
                StartCoroutine(PushUpCoroutine());
                hasPushedUp = true; // Set the flag after push-up
                return; // Exit the method after detecting a ledge on the right
            }
        }

        // Left side ledge detection
        Vector2 circlePosLeft = new Vector2(transform.position.x - ledgeDetectionOffsetX, transform.position.y + ledgeDetectionOffsetY);
        Collider2D hitLeft = Physics2D.OverlapCircle(circlePosLeft, ledgeDetectionRadius, groundLayer);

        if (!isGrounded && hitLeft != null)
        {
            if (hitLeft.CompareTag("Ground"))
            {
                Debug.Log("Ledge detected on the left side.");
                ledgePosition = new Vector2(circlePosLeft.x, circlePosLeft.y);
                StartCoroutine(PushUpCoroutine());
                hasPushedUp = true; // Set the flag after push-up
            }
        }
    }

    IEnumerator PushUpCoroutine()
    {
        isPushingUp = true;
        Vector2 startPosition = transform.position;
        Vector2 targetPosition;
        if (playerController.IsFacingRight())
        {
            targetPosition = new Vector2(startPosition.x + pushOnX, startPosition.y + pushUpSpeed);
        }
        else
        {
            targetPosition = new Vector2(startPosition.x - pushOnX, startPosition.y + pushUpSpeed);
        }
        float elapsedTime = 0f;

        while (elapsedTime < pushUpTime)
        {
            transform.position = Vector2.Lerp(startPosition, targetPosition, (elapsedTime / pushUpTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        isPushingUp = false;
    }

    public void SetGroundedState(bool grounded)
    {
        isGrounded = grounded;
        if (grounded)
        {
            rb.isKinematic = false;
            hasPushedUp = false; // Reset the flag when grounded
        }
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    void OnDrawGizmos()
    {
        Vector2 circlePosRight = new Vector2(transform.position.x + ledgeDetectionOffsetX, transform.position.y + ledgeDetectionOffsetY);
        Vector2 circlePosLeft = new Vector2(transform.position.x - ledgeDetectionOffsetX, transform.position.y + ledgeDetectionOffsetY);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(circlePosRight, ledgeDetectionRadius);
        Gizmos.DrawWireSphere(circlePosLeft, ledgeDetectionRadius);
    }
}
