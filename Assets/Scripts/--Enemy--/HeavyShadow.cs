using UnityEngine;

public class HeavyShadow : EnemyBase
{
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private float detectionRange = 12f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 3f;

    private Transform player;
    private CandleLight candleScript;
    private float lastAttackTime = 0f;

    private void Start()
    {
        base.Start(); // يستدعي تهيئة animator من EnemyBase
        player = GameObject.FindGameObjectWithTag("Player").transform;
        candleScript = FindFirstObjectByType<CandleLight>();
    }

    private void Update()
    {
        if (player == null || candleScript == null)
            return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            if (candleScript.IsCandleLit())
            {
                animator?.SetBool("isMoving", false);
                return;
            }

            Vector2 direction = (player.position - transform.position).normalized;
            transform.position += (Vector3)direction * speed * Time.deltaTime;

            if (direction.x > 0)
                transform.localScale = new Vector3(1, 1, 1);
            else
                transform.localScale = new Vector3(-1, 1, 1);

            animator?.SetBool("isMoving", true);

            if (distance <= attackRange && Time.time >= lastAttackTime + attackCooldown)
            {
                Attack();
            }
        }
    }

    private void Attack()
    {
        lastAttackTime = Time.time;
        animator?.SetTrigger("attack");
        Debug.Log("Heavy Shadow attacked!");
    }
}
