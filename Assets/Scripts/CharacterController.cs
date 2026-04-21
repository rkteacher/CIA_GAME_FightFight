using System.Collections;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private InputActionReference movementActionRef;
    [SerializeField] private InputActionReference attackActionRef;
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
    private float yDeathrange = -6f;

    //public bool isGrounded {  get; private set; }

    private float inputVectorX;
    private float inputVectorY;

    private Vector2 moveDir;

    private bool isJumping;

    [Header("Attack Perameters")]
    [SerializeField] float attackDelay = 0.5f;
    [SerializeField] BoxCollider2D col_MediumAttack;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col_MediumAttack.enabled = false;
    }
    private void OnEnable()
    {
        attackActionRef.action.Enable();
        attackActionRef.action.performed += OnAttackPerformed;
    }

    private void OnDisable()
    {
        attackActionRef.action.performed -= OnAttackPerformed;
        attackActionRef.action.Disable();
    }

    private void Update()
    {
        // Movement input
        //moveInput = Input.GetAxis("Horizontal");
        moveDir = movementActionRef.action.ReadValue<Vector2>();
        
        Vector2 movementDirection = rb.linearVelocity.normalized;

        animator.SetFloat("horizontalVelocity", Mathf.Abs(moveDir.x)); // get the absolute value of the Vector2 and apply that to the Animator perameter

        Debug.Log($"Is player grounded = {IsPlayerGrounded()}");
        Debug.Log($"Move Direction = {moveDir}");

        SetAnimations();



        animator.SetFloat("verticalVelocity", movementDirection.y);
        if (moveDir.y >= 0.1f && IsPlayerGrounded())
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
        CheckResetRange();
    }

    /// <summary>
    /// Checks if the game object's position the class is attached to falls below a specific range. If it does it calls upon the GameManager to reset the scene. 
    /// </summary>
    void CheckResetRange()
    {
        if(gameObject.transform.position.y <= yDeathrange)
        {
            GameManager.instance.ResetScene();
        }
    }

    /// <summary>
    /// Cast a ray downwards and esstablish if the character is on a valid ground surface
    /// </summary>
    /// <returns></returns>
    private bool IsPlayerGrounded()
    {
        Vector2 origin = (Vector2)transform.position + Vector2.down * 0.6f;
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, raycastDistance, groundLayer);


        // For debugging: draws the ray in the scene view
        Debug.DrawRay(origin, Vector2.down * raycastDistance, hit.collider != null ? Color.green : Color.red);

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

    

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Atatack Action pressed");
        Attack();
    }

    private void Attack()
    {
        animator.SetTrigger("isAttacking");
        StartCoroutine(Attack_CoRoutine());
    }

    IEnumerator Attack_CoRoutine()
    {
        col_MediumAttack.enabled = true;
        yield return new WaitForSeconds(attackDelay);
        col_MediumAttack.enabled = false;
        yield return null;
    }

    void SetAnimations()
    {
        if (moveDir.x > 0)
        {
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


    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.gameObject.tag == "Hazzard")
        {
            healthPoints--;
            CheckHealth();
        }
    }

    void CheckHealth()
    {
        if(healthPoints <= 0)
        {
            animator.SetBool("isDead", true);
            healthPoints = 0;
        }
    }
}
