using Managers;
using ScriptableObjects;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement2D : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 7f;
    public float jumpForce = 12f;
    public LayerMask groundLayers;
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;

    [Header("Jump Tuning")]
    [Tooltip("Extra gravity multiplier when player is falling after jumping")]
    public float jumpFallMultiplier = 3f;
    [Tooltip("Extra gravity multiplier when player falls without jumping (e.g. walks off ledge)")]
    public float normalFallMultiplier = 1f;

    [Header("Dash")]
    public float dashForce = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isDashing;
    private bool canDash = true;
    private bool hasJumped = false;
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
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayers);

        HandleInput();
        HandleDashTimers();
        HandleBetterJump();

        // Reset jump flag when touching ground again
        if (isGrounded && hasJumped)
            hasJumped = false;
    }

    private void HandleInput()
    {
        if (InputManager.Instance.InputEnabled)
        {
            horizontalInput = BugManager.Instance.currentBugs.Contains(BugManager.Bugs.ReverseInput) ? -1 * Input.GetAxisRaw("Horizontal") : Input.GetAxisRaw("Horizontal");

            if (Input.GetButtonDown("Jump") && isGrounded && ShouldJump())
            {
                Jump();
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
                StartDash();
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
            rb.linearVelocity = dashDirection * dashForce;
        else
            Move();
    }

    private bool ShouldJump()
    {
        // If the bug is not active, always jump
        if (!BugManager.Instance.currentBugs.Contains(BugManager.Bugs.JumpMightFail))
            return true;

        // If the bug is active, only jump 70% of the time
        float chance = Random.value; // gives 0.0–1.0
        return chance > 0.3f; // 30% fail chance
    }

    private void Move()
    {
        float targetSpeed = horizontalInput * moveSpeed;
        rb.linearVelocity = new Vector2(targetSpeed, rb.linearVelocity.y);
    }

    private void Jump()
    {
        hasJumped = true;
        canDash = true;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void HandleBetterJump()
    {
        if (isDashing) return;

        // Apply extra gravity based on whether we jumped or fell naturally
        if (rb.linearVelocity.y < 0)
        {
            float multiplier = hasJumped ? jumpFallMultiplier : normalFallMultiplier;

            rb.linearVelocity += Vector2.up * (Physics2D.gravity.y * (multiplier) * Time.deltaTime);
        }
    }

    private void StartDash()
    {
        isDashing = true;
        canDash = false;
        dashTimeLeft = dashDuration;
        dashDirection = new Vector2(horizontalInput != 0 ? horizontalInput : transform.localScale.x, 0).normalized;
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

    public void UpdatePlayerVersion(UnityVersionData newVersion)
    {
        if (newVersion.updateJump) jumpForce = newVersion.jumpPower;
        if (newVersion.updateSpeed) moveSpeed = newVersion.speed;
    }
}
