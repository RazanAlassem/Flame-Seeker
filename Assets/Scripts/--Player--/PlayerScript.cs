using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 700f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundRadius = 0.2f;
    [SerializeField] private LayerMask layerGround;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip powerupSound;

    [Header("Combat")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Health")]
    [SerializeField] private int maxHealth = 3;
    private int currentHealth;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 12f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    private bool isDashing = false;
    private float dashTimeLeft;
    private float lastDashTime;

    private Rigidbody2D body;
    private Animator animator;
    private AudioSource audioSource;
    private CandleLight candleScript;

    private float moveInput;
    private bool isGrounded;
    private bool canAttack = true;
    private bool isAlive = true;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        candleScript = FindObjectOfType<CandleLight>();
        currentHealth = maxHealth;

        if (groundCheckPoint == null)
        {
            GameObject check = new GameObject("GroundCheck");
            check.transform.SetParent(transform);
            check.transform.localPosition = new Vector3(0, -0.5f, 0);
            groundCheckPoint = check.transform;
        }
    }

    private void Update()
    {
        if (!isAlive) return;

        moveInput = Input.GetAxis("Horizontal");

        if (moveInput > 0.01f) transform.localScale = Vector3.one;
        else if (moveInput < -0.01f) transform.localScale = new Vector3(-1, 1, 1);

        if (!isDashing)
        {
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
                Jump();

            if (Input.GetKeyDown(KeyCode.F) && canAttack)
                Attack();

            if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= lastDashTime + dashCooldown)
                StartDash();
        }

        if (candleScript != null && !candleScript.IsCandleLit())
        {
            canAttack = false;
        }
        else
        {
            canAttack = true;
        }

        UpdateAnimationState();
    }

    private void FixedUpdate()
    {
        if (!isAlive) return;

        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundRadius, layerGround);

        if (isDashing)
        {
            float dashDirection = transform.localScale.x;
            body.linearVelocity = new Vector2(dashDirection * dashSpeed, 0f);
            dashTimeLeft -= Time.fixedDeltaTime;

            if (dashTimeLeft <= 0f)
            {
                EndDash();
            }
        }
        else
        {
            float targetVelocityX = moveInput * speed;
            body.linearVelocity = new Vector2(targetVelocityX, body.linearVelocity.y);
        }
    }

    private void StartDash()
    {
        isDashing = true;
        dashTimeLeft = dashDuration;
        lastDashTime = Time.time;

        // Disable attack during dash
        canAttack = false;

        // Optional: play dash animation or sound
        // animator.SetTrigger("dash");
        // PlaySound(dashSound);
    }

    private void EndDash()
    {
        isDashing = false;
        canAttack = true;
    }

    private void Jump()
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x, 0);
        body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        PlaySound(jumpSound);
    }

    private void Attack()
    {
        animator.SetTrigger("attack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.TryGetComponent(out EnemyBase enemyScript))
            {
                enemyScript.TakeDamage(1);
            }
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void OnCandleExtinguished()
    {
        Die();
    }

    private void Die()
    {
        isAlive = false;
        animator.SetTrigger("die");
        body.linearVelocity = Vector2.zero;
        this.enabled = false;

        Debug.Log("Player Died");
    }

    private void PlaySound(AudioClip clip, float pitch = 1.0f)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(clip);
        }
    }

    private void UpdateAnimationState()
    {
        if (animator == null) return;
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetFloat("Speed", Mathf.Abs(moveInput));
        animator.SetBool("IsDashing", isDashing);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }

        if (groundCheckPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheckPoint.position, groundRadius);
        }
    }
}

// using UnityEngine;
// using UnityEngine.UI;

// public class PlayerScript : MonoBehaviour
// {
//     [Header("Movement")]
//     [SerializeField] private float speed = 5f;
//     [SerializeField] private float jumpForce = 700f;

//     [Header("Ground Check")]
//     [SerializeField] private Transform groundCheckPoint;
//     [SerializeField] private float groundRadius = 0.2f;
//     [SerializeField] private LayerMask layerGround;

//     [Header("Sound Effects")]
//     [SerializeField] private AudioClip jumpSound;
//     [SerializeField] private AudioClip powerupSound;

//     [Header("Power-ups")]
//     [SerializeField] private float powerupDuration = 5f;
//     [SerializeField] private float speedBoostMultiplier = 2f;
//     [SerializeField] private float doubleJumpForce = 600f;

//     [Header("Dash Settings")]
//     [SerializeField] private float dashForce = 15f;
//     [SerializeField] private float dashDuration = 0.2f;
//     [SerializeField] private float dashCooldown = 1f;

//     private bool isDashing = false;
//     private float dashTime;
//     private float nextDashTime = 0f;
//     private float originalGravity;

//     // Component references
//     private Rigidbody2D body;
//     private Animator animator;
//     private AudioSource audioSource;

//     // Game state variables
//     private float moveInput;
//     private bool isGrounded;

//     // Power-up variables
//     private bool hasSpeedBoost = false;
//     private bool hasDoubleJump = false;
//     private bool canDoubleJump = false;
//     private float speedBoostEndTime = 0;
//     private float doubleJumpEndTime = 0;

//     private void Awake()
//     {
//         body = GetComponent<Rigidbody2D>();
//         animator = GetComponent<Animator>();
//         audioSource = GetComponent<AudioSource>();

//         if (audioSource == null)
//         {
//             audioSource = gameObject.AddComponent<AudioSource>();
//         }

//         if (groundCheckPoint == null)
//         {
//             GameObject check = new GameObject("GroundCheck");
//             check.transform.SetParent(transform);
//             check.transform.localPosition = new Vector3(0, -0.5f, 0);
//             groundCheckPoint = check.transform;
//         }
//     }

//     private void Update()
//     {
//         moveInput = Input.GetAxis("Horizontal");

//         if (moveInput > 0.01f)
//         {
//             transform.localScale = Vector3.one;
//         }
//         else if (moveInput < -0.01f)
//         {
//             transform.localScale = new Vector3(-1, 1, 1);
//         }

//         CheckPowerupExpiration();

//         if (Input.GetKeyDown(KeyCode.Space))
//         {
//             if (isGrounded)
//             {
//                 Jump();
//             }
//             else if (hasDoubleJump && canDoubleJump)
//             {
//                 DoubleJump();
//             }
//         }

//         // Start Dash
//         if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= nextDashTime && !isDashing)
//         {
//             StartDash();
//         }

//         // Stop Dash
//         if (isDashing && Time.time >= dashTime)
//         {
//             isDashing = false;
//             body.gravityScale = originalGravity;
//         }

//         UpdateAnimationState();
//     }

//     private void FixedUpdate()
//     {
//         isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundRadius, layerGround);

//         if (isGrounded && hasDoubleJump)
//         {
//             canDoubleJump = true;
//         }

//         float currentSpeed = hasSpeedBoost ? speed * speedBoostMultiplier : speed;

//         if (!isDashing)
//         {
//             float targetVelocityX = moveInput * currentSpeed;
//             body.linearVelocity = new Vector2(targetVelocityX, body.linearVelocity.y);
//         }
//     }

//     private void Jump()
//     {
//         body.linearVelocity = new Vector2(body.linearVelocity.x, 0);
//         body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
//         PlaySound(jumpSound);
//     }

//     private void DoubleJump()
//     {
//         body.linearVelocity = new Vector2(body.linearVelocity.x, 0);
//         body.AddForce(Vector2.up * doubleJumpForce, ForceMode2D.Impulse);
//         canDoubleJump = false;
//         PlaySound(jumpSound, 1.2f);
//     }

//     private void StartDash()
//     {
//         isDashing = true;
//         dashTime = Time.time + dashDuration;
//         nextDashTime = Time.time + dashCooldown;

//         originalGravity = body.gravityScale;
//         body.gravityScale = 0;

//         float dashDirection = transform.localScale.x > 0 ? 1f : -1f;
//         body.linearVelocity = new Vector2(dashDirection * dashForce, 0f);
//     }

//     private void UpdateAnimationState()
//     {
//         if (animator == null) return;

//         animator.SetBool("IsGrounded", isGrounded);
//         animator.SetFloat("Speed", Mathf.Abs(moveInput));
//         animator.SetBool("IsDashing", isDashing);
//     }

//     private void PlaySound(AudioClip clip, float pitch = 1.0f)
//     {
//         if (audioSource != null && clip != null)
//         {
//             audioSource.pitch = pitch;
//             audioSource.PlayOneShot(clip);
//         }
//     }

//     public void ActivatePowerup(string powerupType)
//     {
//         audioSource.PlayOneShot(powerupSound, 0.2f);

//         switch (powerupType.ToLower())
//         {
//             case "speedboost":
//                 hasSpeedBoost = true;
//                 speedBoostEndTime = Time.time + powerupDuration;
//                 break;

//             case "doublejump":
//                 hasDoubleJump = true;
//                 canDoubleJump = true;
//                 doubleJumpEndTime = Time.time + powerupDuration;
//                 break;
//         }
//     }

//     private void CheckPowerupExpiration()
//     {
//         if (hasSpeedBoost && Time.time > speedBoostEndTime)
//         {
//             hasSpeedBoost = false;
//         }

//         if (hasDoubleJump && Time.time > doubleJumpEndTime)
//         {
//             hasDoubleJump = false;
//             canDoubleJump = false;
//         }
//     }

//     private void OnDrawGizmos()
//     {
//         if (groundCheckPoint != null)
//         {
//             Gizmos.color = Color.yellow;
//             Gizmos.DrawWireSphere(groundCheckPoint.position, groundRadius);
//         }
//     }
// }
