using System.Collections;
using UnityEngine;

public class PlatformerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float maxSpeed = 7f;
    [SerializeField] public float maxVelocity = 20f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float dashPower = 1.5f;
    [SerializeField] private float dashTime = 1f;
    [SerializeField] private TrailRenderer tr;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    private Rigidbody2D rb;
    private float moveInput;
    private bool isFacingRight = true;
    // jump variables
    public float coyoteTime = 0.1f;
    public bool canJump = false;
    public bool canDoubleJump = false;
    // dash variables
    public bool canDash = true;
    public bool isDashing = false;
    // wall jump variables
    private bool isWallSliding;
    private float wallSlideSpeed = 2f;
    private bool isWallJumping;
    private float wallJumpDirection;
    private float wallJumpTime = 0.2f;
    private float wallJumpCounter;
    private float wallJumpDuration = 0.4f;
    private Vector2 wallJumpPower = new Vector2(8f, 16f);
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        // Set to Dynamic with gravity
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 3f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        // Get horizontal input
        moveInput = Input.GetAxisRaw("Horizontal");

        // calls wall jump functions
        WallSlide();
        WallJump();

        // only uses the flip function if player isn't wall jumping
        if (!isWallJumping)
        {
            Flip();
        }

        // coyote time counts down while midair
        coyoteTime -= Time.deltaTime;

        // resets values when player is on the ground
        if (IsGrounded())
        {
            coyoteTime = 0.1f;
            canDoubleJump = true;
        }

        // allows player to briefly jump if they still have coyote time
        if (IsGrounded() || coyoteTime > 0)
        {
            canJump = true;
        }
        else
        {
            canJump = false;
        }

        // Jump input
        if (Input.GetButtonDown("Jump") && canJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        // allows player to double jump midair once
        if (Input.GetButtonDown("Jump") && !canJump && canDoubleJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            canDoubleJump = false;
        }

        // can only dash on the ground
        if (Input.GetKeyDown(KeyCode.E) && canDash && IsGrounded())
        {
            StartCoroutine(Dash());
        }

        // resets dash and speed if player stops moving or lands after dash ends (might try to add turning around as well)
        if (IsGrounded() && !isDashing || IsWalled() && !isDashing)
        {
            moveSpeed = maxSpeed;
            tr.emitting = false;
            canDash = true;
        }
        else if (rb.linearVelocity.magnitude < 5 && isDashing)
        {
            moveSpeed = maxSpeed;
            tr.emitting = false;
            isDashing = false;
            canDash = true;
        }
    }

    // Coroutine to dash 
    private IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;
        moveSpeed *= dashPower;
        tr.emitting = true;
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
    }
    
    void FixedUpdate()
    {
        // Apply horizontal movement when player isn't wall jumping
        if (!isWallJumping)
        {
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }
        rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxVelocity);
    }
    
    // Visualise ground check in editor
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    // flips player on the x axis
    private void Flip()
    {
        if (isFacingRight && moveInput < 0f || !isFacingRight && moveInput > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    // checks if player is grounded
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }


    // checks if player is on a wall
    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }


    // lets players slide down walls
    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && moveInput != 0f)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, -wallSlideSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    // lets players wall jump if they are wall sliding
    private void WallJump() // tweak values to feel better to play, far to slow/unresponsive
        // maybe less time jumping away from wall and no cooldown, akin to hollow knight?
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpDirection = -transform.localScale.x;
            wallJumpCounter = wallJumpTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpCounter > 0f)
        {
            isWallJumping = true;
            rb.linearVelocity = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y);
            wallJumpCounter = 0f;

            if (transform.localScale.x != wallJumpDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }
}