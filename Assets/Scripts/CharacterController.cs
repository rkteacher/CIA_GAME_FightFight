using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private Rigidbody2D rb;
    private float moveInput;

    public float moveSpeed;
    public float jumpForce;
    [SerializeField] private float jumpTime = 0.4f; // max ammount of time allowed in air
    private float airTime = 0;


    [SerializeField] private float raycastDistance = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    //public bool isGrounded {  get; private set; }

    private float inputVectorX;
    private float inputVectorY;

    private Vector2 moveDir;

    private bool isJumping;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Movement input
        moveInput = Input.GetAxis("Horizontal");

        // Ground check
        //isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Jump
        if (Input.GetButtonDown("Jump") && IsPlayerGrounded())
        {
            //rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            Debug.Log("jump");

            Jump();
            isJumping = true;
        }

        // Attack
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Attack();
        }
    }

    private void FixedUpdate()
    {


        // get input
        // move character

    }

    /// <summary>
    /// Cast a ray downwards and esstablish if the character is on a valid ground surface
    /// </summary>
    /// <returns></returns>
    private bool IsPlayerGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastDistance, groundLayer);

        // For debugging: draws the ray in the scene view
        Debug.DrawRay(transform.position, Vector2.down * raycastDistance, hit.collider != null ? Color.green : Color.red);

        // If the ray hits a collider on the specified groundLayer, the player is grounded
        return hit.collider != null;
    }


    void Jump()
    {

        if(airTime < jumpTime || isJumping)
        {
            airTime += Time.deltaTime;

            rb.linearVelocity = Vector2.up * jumpForce;
        }
        else
        {
            isJumping = false;
        }
    }
}
