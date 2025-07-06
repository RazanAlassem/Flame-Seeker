using UnityEngine;

public class CrawlerEnemy : EnemyBase
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackCooldown = 1.5f;

    private float lastAttackTime = 0f;
    private Transform player;
    private CandleLight candleScript;

    private void Start()
    {
        base.Start(); // تهيئة animator من EnemyBase
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        candleScript = FindFirstObjectByType<CandleLight>();
    }

    private void Update()
    {
        if (player == null || candleScript == null)
            return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            if (!candleScript.IsCandleLit())
            {
                Vector2 direction = (player.position - transform.position).normalized;
                transform.position += (Vector3)direction * speed * Time.deltaTime;

                // اتجاه الوجه
                transform.localScale = new Vector3(direction.x > 0 ? 1 : -1, 1, 1);

                if (distance <= attackRange)
                {
                    animator?.SetBool("isMoving", false); // يوقف الحركة وقت الهجوم
                    Attack();
                }
                else
                {
                    animator?.SetBool("isMoving", true);
                }
            }
            else
            {
                animator?.SetBool("isMoving", false);
            }
        }
        else
        {
            animator?.SetBool("isMoving", false);
        }
    }

    private void Attack()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            animator?.SetTrigger("attack");
            Debug.Log("Crawler attacks player!");
        }
    }
}