
//
// PLAYER CONTROLLER
//
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;       // Horizontal speed
    public float jumpForce = 12f;      // Jumping power

    [Header("Input Settings")]
    public KeyCode leftKey = KeyCode.A;     // Move left
    public KeyCode rightKey = KeyCode.D;    // Move right
    public KeyCode jumpKey = KeyCode.Space; // Jump

    [Header("Ground Check Settings")]
    public Transform groundCheck;      // Empty GameObject at player's feet
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;      // Layer that counts as "ground"

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private bool isGrounded;
    private float moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        HandleInput();
        HandleJump();
        UpdateAnimations();
    }

    void FixedUpdate()
    {
        // Apply horizontal movement
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    /// <summary>
    /// Handles left/right input (supports both custom keys and Unity axes).
    /// </summary>
    private void HandleInput()
    {
        float axisInput = Input.GetAxisRaw("Horizontal"); // works with keyboard & controller

        bool leftPressed = Input.GetKey(leftKey);
        bool rightPressed = Input.GetKey(rightKey);

        if (leftPressed && !rightPressed)
            moveInput = -1f;
        else if (rightPressed && !leftPressed)
            moveInput = 1f;
        else
            moveInput = axisInput; // fall back to axis (for controllers/joysticks)

        // Flip sprite based on movement
        if (moveInput != 0)
            spriteRenderer.flipX = moveInput < 0;
    }

    /// <summary>
    /// Handles jumping input & ground detection.
    /// </summary>
    private void HandleJump()
    {
        // Check if player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Jump on key press (keyboard OR controller "Jump" button)
        if (isGrounded && (Input.GetKeyDown(jumpKey) || Input.GetButtonDown("Jump")))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    /// <summary>
    /// Updates animator parameters to control transitions.
    /// </summary>
    private void UpdateAnimations()
    {
        animator.SetBool("isRunning", moveInput != 0);
        animator.SetBool("isJumping", !isGrounded && rb.velocity.y > 0.1f);
        animator.SetBool("isFalling", !isGrounded && rb.velocity.y < -0.1f);
    }
}

//
// CAMERA FOLLOW
//
public class CameraFollow : MonoBehaviour
{
    public Transform target;           // Player to follow
    public float smoothSpeed = 0.125f; // Smoothing factor
    public Vector3 offset;             // Camera offset from the player

    void LateUpdate()
    {
        if (target == null) return;

        // Desired position based on player position + offset
        Vector3 desiredPosition = target.position + offset;

        // Smoothly interpolate camera position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Apply position (keep original Z so camera stays back)
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
    }
}
