using UnityEngine;
using Interlife.Core;

namespace Interlife.Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
    public class VoidController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveSpeed = 6f;
        [SerializeField] private float jumpHeight = 3.5f;
        [SerializeField] private float dashDistance = 4f;
        [SerializeField] private float dashDuration = 0.2f;
        
        [Header("Physics Settings")]
        [SerializeField] private float upwardGravityMultiplier = 1f;
        [SerializeField] private float downwardGravityMultiplier = 3f;
        [SerializeField] private float groundCheckRadius = 0.2f;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Transform groundCheckTransform;

        [Header("References")]
        [SerializeField] private InputReader inputReader;

        private Rigidbody2D rb;
        private Vector2 moveInput;
        private bool isGrounded;
        private bool canDash = true;
        private bool isDashing = false;

        private float defaultGravityScale;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            defaultGravityScale = rb.gravityScale;
        }

        private void OnEnable()
        {
            inputReader.MoveEvent += OnMove;
            inputReader.JumpEvent += OnJump;
            inputReader.JumpCancelledEvent += OnJumpCancelled;
            inputReader.DashEvent += OnDash;
        }

        private void OnDisable()
        {
            inputReader.MoveEvent -= OnMove;
            inputReader.JumpEvent -= OnJump;
            inputReader.JumpCancelledEvent -= OnJumpCancelled;
            inputReader.DashEvent -= OnDash;
        }

        private void Update()
        {
            CheckGround();
            ApplyAsymmetricGravity();
        }

        private void FixedUpdate()
        {
            if (isDashing) return;
            ApplyMovement();
        }

        private void CheckGround()
        {
            isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, groundCheckRadius, groundLayer);
            if (isGrounded) canDash = true;
        }

        private void OnMove(Vector2 input) => moveInput = input;

        private void ApplyMovement()
        {
            rb.velocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);
            
            // Flip sprite logic could go here
            if (moveInput.x != 0)
            {
                transform.localScale = new Vector3(Mathf.Sign(moveInput.x), 1, 1);
            }
        }

        private void OnJump()
        {
            if (isGrounded)
            {
                float jumpForce = Mathf.Sqrt(jumpHeight * -2 * (Physics2D.gravity.y * rb.gravityScale));
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                Debug.Log("Jump!");
            }
        }

        private void OnJumpCancelled()
        {
            if (rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            }
        }

        private void ApplyAsymmetricGravity()
        {
            if (isDashing) return;

            if (rb.velocity.y > 0)
            {
                rb.gravityScale = defaultGravityScale * upwardGravityMultiplier;
            }
            else if (rb.velocity.y < 0)
            {
                rb.gravityScale = defaultGravityScale * downwardGravityMultiplier;
            }
            else
            {
                rb.gravityScale = defaultGravityScale;
            }
        }

        private void OnDash()
        {
            if (canDash && !isDashing)
            {
                StartCoroutine(ExecuteDash());
            }
        }

        private System.Collections.IEnumerator ExecuteDash()
        {
            isDashing = true;
            canDash = false;
            
            float originalGravity = rb.gravityScale;
            rb.gravityScale = 0;
            
            float dashDirection = transform.localScale.x;
            rb.velocity = new Vector2(dashDirection * (dashDistance / dashDuration), 0);
            
            yield return new WaitForSeconds(dashDuration);
            
            rb.gravityScale = originalGravity;
            isDashing = false;
            
            Debug.Log("Dash executed");
        }
    }
}
