using System.Collections;
using System.Runtime.CompilerServices;
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
    
    private Rigidbody2D rb;
    private bool isGrounded;
    private float moveInput;
    // jump variables
    public float coyoteTime = 0.1f;
    public bool canJump = false;
    public bool canDoubleJump = false;
    // dash variables
    public bool canDash = true;
    public bool isDashing = false;
    
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

        // Check if grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // coyote time counts down while midair
        coyoteTime -= Time.deltaTime;

        // resets values when player is on the ground
        if (isGrounded)
        {
            coyoteTime = 0.1f;
            canDoubleJump = true;
        }

        // allows player to briefly jump if they still have coyote time
        if (isGrounded || coyoteTime > 0)
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

        // ca only dash on the ground
        if (Input.GetKeyDown(KeyCode.E) && canDash && isGrounded)
        {
            StartCoroutine(Dash());
        }

        // resets dash and speed if player stops moving or lands after dash ends (might try to add turning around as well)
        if (isGrounded && !isDashing)
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
        // Apply horizontal movement
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
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
}