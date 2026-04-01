using System.Diagnostics.Tracing;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private InputActionReference inputActionsRef;
    [SerializeField] private Animator animator;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpTime = 0.4f; // max ammount of time allowed in air
    [SerializeField] private int attackPower;
    [SerializeField] private int healthPoints;
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
        //moveInput = Input.GetAxis("Horizontal");
        moveDir = inputActionsRef.action.ReadValue<Vector2>();

        animator.SetFloat("horizontalVelocity", Mathf.Abs(moveDir.x)); // get the absolute value of the Vector2 and apply that to the Animator perameter

        Debug.Log($"Is player grounded = {IsPlayerGrounded()}");
        Debug.Log($"Move Direction = {moveDir}");

        SetAnimations();


        if(moveDir.y >= 0.1f)
        {
            Debug.Log("jump");
            Jump();
            isJumping = true;
        }


    }

    private void FixedUpdate()
    {


        // get input
        // move character
        rb.linearVelocity = new Vector2(moveDir.x * moveSpeed, rb.linearVelocity.y);
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


    void SetAnimations()
    {
        if (moveDir.x > 0)
        {
            Debug.Log("Moving Right");
            gameObject.transform.localScale = new Vector3(1,1,1);
        }
        else if (moveDir.x < 0)
        {
            Debug.Log("Moving Left");
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            Debug.Log("Idle");
        }
    }
}
