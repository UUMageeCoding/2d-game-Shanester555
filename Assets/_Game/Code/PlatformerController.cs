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
    public float moveInput;
    private bool isFacingRight = true;
    private bool turnAround = true;
    // jump variables
    public float coyoteTime = 0.1f;
    public bool canJump = false;
    private bool doubleJumpUnlock = false;
    public bool canDoubleJump = false;
    // dash variables
    private bool dashUnlock = false;
    public bool canDash = true;
    public bool isDashing = false;
    // wall jump variables
    private bool wallJumpUnlock = false;
    private bool isWallSliding;
    private float wallSlideSpeed = 2f;
    private bool isWallJumping;
    private float wallJumpDirection;
    private float wallJumpTime = 0.2f;
    private float wallJumpCounter;
    [SerializeField] private float wallJumpDuration = 0.2f;
    [SerializeField] private Vector2 wallJumpPower = new Vector2(8f, 20f);
    // checkpoint variables
    public Vector3 respawnPoint;
    // moving platform variables
    private GameObject currentOneWayPlatform;
    [SerializeField] private CapsuleCollider2D playerCollider;
    // animation variables
    private Animator animator;
    // sound effects for player
    [SerializeField] private AudioClip jumpSoundClip;
    [SerializeField] private AudioClip damageSoundClip;
    [SerializeField] private AudioClip dashSoundClip;
    [SerializeField] private AudioClip collectableSoundClip;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        // Set to Dynamic with gravity
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 3f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        respawnPoint = transform.position;
        GameManager.timeIsRunning = true;
        GameManager.timePassed = 0;
        GameManager.collectableCount = 0;

    }

    void Update()
    {
        // Get horizontal input
        if (GameManager.canMove)
        {
            moveInput = Input.GetAxisRaw("Horizontal");
        }
        else
        {
            moveInput = 0;
        }

        // animation triggers
        if (IsGrounded() && moveInput != 0)
        {
            animator.SetTrigger("Running");
        }
        else if (IsGrounded() && moveInput == 0)
        {
            animator.SetTrigger("Idle");
        }
        else if (rb.linearVelocity.y > 0 && !IsGrounded())
        {
            animator.SetTrigger("Jump");
        }
        else if (rb.linearVelocity.y < 0 && !IsGrounded())
        {
            animator.SetTrigger("Fall");
        }

        // Lets player phase down through one way platforms
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentOneWayPlatform != null)
            {
                StartCoroutine(DisableCollision());
            }
        }

        // calls wall jump functions
        if (wallJumpUnlock)
        {
            WallSlide();
            WallJump();
        }

        // prevents the player from accidentally using their double jump while wall jumping
        if (IsWalled() && !canDoubleJump)
        {
            canDoubleJump = false;
        }

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
            if (doubleJumpUnlock)
            {
                canDoubleJump = true;
            }
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
            SoundFXManager.instance.PlaySoundFXClip(jumpSoundClip, transform, .6f, 0);
        }

        // allows player to double jump midair once
        if (Input.GetButtonDown("Jump") && !canJump && canDoubleJump && !IsWalled())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            SoundFXManager.instance.PlaySoundFXClip(jumpSoundClip, transform, .6f, 0);
            canDoubleJump = false;
        }

        // can only dash on the ground
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && IsGrounded() && dashUnlock)
        {
            StartCoroutine(Dash());
        }

        // resets dash and speed if player lands after dash ends or turns around mid dash
        if (IsGrounded() && !isDashing || IsWalled() && !isDashing)
        {
            moveSpeed = maxSpeed;
            tr.emitting = false;
            canDash = true;
        }
        else if (isFacingRight == turnAround)
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
        turnAround = !isFacingRight;
        SoundFXManager.instance.PlaySoundFXClip(dashSoundClip, transform, .5f, 0);
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
    private void WallJump()
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

        // allows players to wall jump again after set amount of time
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

    // stops wall jump function while refreshing player's double jump
    // need to ensure player can keep their double jump when not using it and falling off of a wall instead of only gaining it coming off of a wall jump
    private void StopWallJumping()
    {
        isWallJumping = false;
        if (doubleJumpUnlock)
        {
            canDoubleJump = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // sends player back to their last checkpoint
        if (collision.tag == "KillPlayer")
        {
            SoundFXManager.instance.PlaySoundFXClip(damageSoundClip, transform, 1f, 0);
            transform.position = respawnPoint;
        }
        // sets the player respawn point to the latest checkpoint
        else if (collision.tag == "Checkpoint")
        {
            respawnPoint = transform.position;
        }
        // unlocks various abilities for the player to use
        else if (collision.tag == "Dash")
        {
            dashUnlock = true;
            Destroy(collision.gameObject);
        }
        else if (collision.tag == "DoubleJump")
        {
            doubleJumpUnlock = true;
            Destroy(collision.gameObject);
        }
        else if (collision.tag == "WallJump")
        {
            wallJumpUnlock = true;
            Destroy(collision.gameObject);
        }
        // incriments the total amount of collectables the player has by 1
        else if (collision.tag == "Collectable")
        {
            GameManager.collectableCount++;
            SoundFXManager.instance.PlaySoundFXClip(collectableSoundClip, transform, 1f, 0);
            Destroy(collision.gameObject);
        }
    }

    // sets the one way platform the player stands on to the current one in code
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = collision.gameObject;
        }
    }

    // makes current one way platform empty if player isn't standing on one
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = null;
        }
    }

    // disables the collision between the player and current one way platform exclusively
    private IEnumerator DisableCollision()
    {
        BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();

        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }
}