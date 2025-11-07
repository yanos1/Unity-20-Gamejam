using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement2D : MonoBehaviour
{
    [Header("Movement")] public float moveSpeed = 7f;
    public float jumpForce = 12f;
    public LayerMask groundLayers;
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;

    [Header("Jump Tuning")] [Tooltip("Extra gravity multiplier when player is falling")]
    public float fallMultiplier = 2.5f;

    [Header("Dash")] public float dashForce = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public bool allowAirDash = false;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isDashing;
    private bool canDash = true;
    private float dashTimeLeft;
    private float dashCooldownTimer;
    private float horizontalInput;
    private Vector2 dashDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Input handling
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayers);

        // Jump input — fires only once per press
        if (Input.GetButtonDown("Jump") && isGrounded)
            Jump();

        // Dash input
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && (isGrounded || allowAirDash))
            StartDash();

        HandleDashTimers();
        HandleBetterJump();
    }

    private void FixedUpdate()
    {
        if (isDashing)
            rb.linearVelocity = dashDirection * dashForce;
        else
            Move();
    }

    private void Move()
    {
        float targetSpeed = horizontalInput * moveSpeed;
        rb.linearVelocity = new Vector2(targetSpeed, rb.linearVelocity.y);
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void HandleBetterJump()
    {
        if (isDashing) return;

        // Only apply stronger gravity while falling
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * (Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime);
        }
    }

    private void StartDash()
    {
        isDashing = true;
        canDash = false;
        dashTimeLeft = dashDuration;
        dashDirection = new Vector2(horizontalInput != 0 ? horizontalInput : transform.localScale.x, 0).normalized;

        // Optional: cancel vertical velocity
        rb.linearVelocity = Vector2.zero;
    }

    private void HandleDashTimers()
    {
        if (isDashing)
        {
            dashTimeLeft -= Time.deltaTime;
            if (dashTimeLeft <= 0)
            {
                isDashing = false;
                dashCooldownTimer = dashCooldown;
            }
        }
        else if (!canDash)
        {
            dashCooldownTimer -= Time.deltaTime;
            if (dashCooldownTimer <= 0)
                canDash = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}