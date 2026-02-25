using UnityEngine;
using Interlife.Core;
using System.Collections;

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

        [Header("Game Feel")]
        [SerializeField] private float coyoteTime = 0.15f;
        [SerializeField] private float jumpBufferTime = 0.15f;
        
        [Header("Physics Settings")]
        [SerializeField] private float upwardGravityMultiplier = 1f;
        [SerializeField] private float downwardGravityMultiplier = 3f;
        [SerializeField] private float groundCheckRadius = 0.2f;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Transform groundCheckTransform;

        [Header("References")]
        [SerializeField] private InputReader inputReader;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private GhostTrail ghostTrail;

        private Rigidbody2D rb;
        private Vector2 moveInput;
        private bool isGrounded;
        private bool canDash = true;
        private bool isDashing = false;
        private float defaultGravityScale;
        private float coyoteTimeCounter;
        private float jumpBufferCounter;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
            if (ghostTrail == null) ghostTrail = GetComponent<GhostTrail>();
            
            defaultGravityScale = rb.gravityScale;
            
            // Aseguramos que el Rigidbody esté configurado para interpolación (fluidez)
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }

        private void OnEnable()
        {
            if (inputReader == null)
            {
                Debug.LogError("InputReader not assigned in VoidController!");
                return;
            }

            inputReader.MoveEvent += OnMove;
            inputReader.JumpEvent += OnJump;
            inputReader.JumpCancelledEvent += OnJumpCancelled;
            inputReader.DashEvent += OnDash;
        }

        private void OnDisable()
        {
            if (inputReader != null)
            {
                inputReader.MoveEvent -= OnMove;
                inputReader.JumpEvent -= OnJump;
                inputReader.JumpCancelledEvent -= OnJumpCancelled;
                inputReader.DashEvent -= OnDash;
            }
        }

        private void Update()
        {
            CheckGround();
            HandleInputBuffer();
            CheckJumpLogic();
            ApplyAsymmetricGravity();
            FlipSprite();
        }

        private void HandleInputBuffer()
        {
            // Coyote Time
            if (isGrounded) coyoteTimeCounter = coyoteTime;
            else coyoteTimeCounter -= Time.deltaTime;

            // Jump Buffer
            jumpBufferCounter -= Time.deltaTime;
        }

        private void CheckJumpLogic()
        {
            if (jumpBufferCounter > 0 && coyoteTimeCounter > 0)
            {
                PerformJump();
            }
        }

        private void FixedUpdate()
        {
            if (isDashing) return;
            ApplyMovement();
        }

        private void CheckGround()
        {
            if (groundCheckTransform == null) return;
            
            isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, groundCheckRadius, groundLayer);
            
            // Debug para ver qué está detectando como suelo
            if (Input.GetKeyDown(KeyCode.G)) // Tecla G para debug manual si es necesario
            {
                Collider2D hit = Physics2D.OverlapCircle(groundCheckTransform.position, groundCheckRadius, groundLayer);
                if (hit != null) Debug.Log($"GroundCheck hit: {hit.name} on layer {LayerMask.LayerToName(hit.gameObject.layer)}");
                else Debug.Log("GroundCheck hit nothing");
            }

            if (isGrounded && !isDashing)
            {
                canDash = true;
            }
        }

        private void OnMove(Vector2 input) => moveInput = input;

        private void ApplyMovement()
        {
            // Usamos linearVelocity para Unity 6
            rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
        }

        private void FlipSprite()
        {
            if (moveInput.x != 0 && !isDashing && spriteRenderer != null)
            {
                spriteRenderer.flipX = moveInput.x < 0;
            }
        }

        private void OnJump()
        {
            jumpBufferCounter = jumpBufferTime;
        }

        private void PerformJump()
        {
            // Detener el buffering
            jumpBufferCounter = 0;
            coyoteTimeCounter = 0;

            // Fórmula física: v = sqrt(h * -2 * g)
            float gravity = Physics2D.gravity.y * rb.gravityScale;
            float jumpForce = Mathf.Sqrt(jumpHeight * -2 * gravity);
            
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0); // Reset vertical
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            
            Debug.Log("Jump Performed with Game Feel");
        }

        private void OnJumpCancelled()
        {
            // Salto variable: si soltamos el botón antes del pico, reducimos la velocidad vertical
            if (rb.linearVelocity.y > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
            }
        }

        private void ApplyAsymmetricGravity()
        {
            if (isDashing) return;

            if (rb.linearVelocity.y > 0) // Ascendiendo
            {
                rb.gravityScale = defaultGravityScale * upwardGravityMultiplier;
            }
            else if (rb.linearVelocity.y < 0) // Cayendo
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

        private IEnumerator ExecuteDash()
        {
            isDashing = true;
            canDash = false;
            
            float originalGravity = rb.gravityScale;
            rb.gravityScale = 0;
            
            // Dirección basada en hacia donde mira el sprite (flipX true = izquierda)
            float dashDirection = spriteRenderer != null && spriteRenderer.flipX ? -1f : 1f;
            
            // Si el jugador está quieto, usamos localScale por si acaso, o simplemente movInput
            if (moveInput.x != 0) dashDirection = Mathf.Sign(moveInput.x);

            // Velocidad constante para cubrir la distancia exacta en el tiempo exacto
            rb.linearVelocity = new Vector2(dashDirection * (dashDistance / dashDuration), 0);
            
            // Visual Trail
            if (ghostTrail != null) ghostTrail.StartTrail();

            yield return new WaitForSeconds(dashDuration);
            
            if (ghostTrail != null) ghostTrail.StopTrail();

            rb.gravityScale = originalGravity;
            isDashing = false;
            
            // Pequeño reset de velocidad tras el dash para evitar tirones
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            
            Debug.Log("Dash Executed with Shadow Trail");
        }

        private void OnDrawGizmosSelected()
        {
            if (groundCheckTransform != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckRadius);
            }
        }
    }
}
