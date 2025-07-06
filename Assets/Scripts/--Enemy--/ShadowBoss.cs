using UnityEngine;

public class BossEnemy : EnemyBase
{
    [Header("Phase Settings")]
    [SerializeField] private float normalSpeed = 2f;
    [SerializeField] private float phase2Speed = 4f;
    private float currentSpeed;

    private Transform player;

    [Header("Dash Settings (Phase 2)")]
    [SerializeField] private float dashForce = 10f;
    [SerializeField] private float dashCooldown = 3f;
    [SerializeField] private float dashDuration = 0.2f;

    private float nextDashTime = 0f;
    private bool isDashing = false;
    private float dashEndTime = 0f;

    protected override void Start()
    {
        base.Start();
        currentSpeed = normalSpeed;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (player == null || currentHealth <= 0) return;

        // حركة تتبع اللاعب (لو ما كان يداش)
        if (!isDashing)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            transform.position += (Vector3)(dir * currentSpeed * Time.deltaTime);

            // قلب الزعيم حسب الاتجاه
            if (dir.x > 0.01f)
                transform.localScale = new Vector3(1, 1, 1);
            else if (dir.x < -0.01f)
                transform.localScale = new Vector3(-1, 1, 1);
        }

        // إذا دخل فيز 2، نفعل الداش
        if (isInPhase2 && Time.time >= nextDashTime && !isDashing)
        {
            StartDash();
        }

        // إيقاف الداش بعد مدته
        if (isDashing && Time.time >= dashEndTime)
        {
            isDashing = false;
        }
    }

    protected override void EnterPhase2()
    {
        base.EnterPhase2();
        currentSpeed = phase2Speed;

        // Reset الداش
        nextDashTime = Time.time + dashCooldown;
    }

    private void StartDash()
    {
        isDashing = true;
        dashEndTime = Time.time + dashDuration;
        nextDashTime = Time.time + dashCooldown;

        Vector2 dashDir = (player.position - transform.position).normalized;

        // نتحرك دفعة واحدة (اندفاع)
        transform.position += (Vector3)(dashDir * dashForce);
    }
}


// using UnityEngine;

// public class ShadowBoss : EnemyBase
// {
//     [SerializeField] private float speed = 2.5f;
//     [SerializeField] private float attackCooldown = 3f;
//     [SerializeField] private float maxHealth = 100f;
//     [SerializeField] private GameObject shadowBlastPrefab;
//     [SerializeField] private Transform castPoint;

//     private float currentHealth;
//     private float lastAttackTime;
//     private Transform player;
//     private CandleLight candle;
//     private Animator animator;

//     private void Start()
//     {
//         currentHealth = maxHealth;
//         player = GameObject.FindGameObjectWithTag("Player").transform;
//         candle = FindObjectOfType<CandleLight>();
//         animator = GetComponent<Animator>();
//     }

//     private void Update()
//     {
//         if (player == null) return;

//         float distance = Vector2.Distance(transform.position, player.position);

//         // إذا الشمعة مشتعلة، تراجع أو تباطؤ
//         if (candle != null && candle.IsCandleLit())
//         {
//             animator?.SetBool("isRetreating", true);
//             // تراجع بسيط
//             transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * 0.5f * Time.deltaTime);
//             return;
//         }

//         // اقترب من اللاعب
//         transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
//         animator?.SetBool("isRetreating", false);

//         if (Time.time >= lastAttackTime + attackCooldown)
//         {
//             Attack();
//             lastAttackTime = Time.time;
//         }
//     }

//     private void Attack()
//     {
//         animator?.SetTrigger("attack");
//         if (shadowBlastPrefab && castPoint)
//         {
//             Instantiate(shadowBlastPrefab, castPoint.position, Quaternion.identity);
//         }
//     }

//     public void TakeDamage(float amount)
//     {
//         currentHealth -= amount;
//         if (currentHealth <= 0f)
//         {
//             Die();
//         }
//     }

//     private void Die()
//     {
//         animator?.SetTrigger("die");
//         Destroy(gameObject, 1.5f);
//     }
// }
