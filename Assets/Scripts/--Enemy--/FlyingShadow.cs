using UnityEngine;

public class FlyingShadow : EnemyBase
{
    [Header("Flying Settings")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private float detectionRange = 15f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float hoverHeight = 2f; // ارتفاع الطيران عن اللاعب
    [SerializeField] private float sineFrequency = 2f; // تردد التموج
    [SerializeField] private float sineAmplitude = 0.5f; // سعة التموج

    private Transform player;
    private CandleLight candleScript;
    private Vector3 startPos;
    private Vector3 targetPosition;

    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        candleScript = FindObjectOfType<CandleLight>();
        startPos = transform.position;
    }

    private void Update()
    {
        if (player == null || candleScript == null)
            return;

        float distance = Vector2.Distance(transform.position, player.position);

        // هل اللاعب داخل نطاق الكشف؟
        if (distance <= detectionRange)
        {
            // الشمعة مطفأة؟ لاحق اللاعب
            if (!candleScript.IsCandleLit())
            {
                ChasePlayer();
                
                // هاجم إذا قريب
                if (distance <= attackRange)
                {
                    Attack();
                }
            }
            else
            {
                // ابتعد عن اللاعب إذا الشمعة شغالة
                MoveAwayFromPlayer();
            }
        }
        else
        {
            // عد إلى الموقع الأصلي أو اطفو في المكان
            Hover();
        }

        // تموج أفقي أثناء الطيران
        ApplyHoverEffect();
    }

    private void ChasePlayer()
    {
        // طيران فوق اللاعب بارتفاع معين
        Vector3 targetPos = player.position + Vector3.up * hoverHeight;
        Vector2 direction = (targetPos - transform.position).normalized;
        transform.position += (Vector3)direction * speed * Time.deltaTime;

        // قلب الشكل حسب الاتجاه
        if (direction.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (direction.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        animator?.SetBool("isMoving", true);
    }

    private void MoveAwayFromPlayer()
    {
        // ابتعد عن اللاعب بسرعة أقل
        Vector2 direction = (transform.position - player.position).normalized;
        transform.position += (Vector3)direction * speed * 0.5f * Time.deltaTime;

        animator?.SetBool("isMoving", true);
    }

    private void Hover()
    {
        // عودة تدريجية للموقع الأصلي أو بقاء في المكان
        Vector2 direction = (startPos - transform.position).normalized;
        if (Vector2.Distance(transform.position, startPos) > 0.5f)
        {
            transform.position += (Vector3)direction * speed * 0.3f * Time.deltaTime;
        }

        animator?.SetBool("isMoving", false);
    }

    private void ApplyHoverEffect()
    {
        // تموج أفقي أثناء الطيران لإعطاء تأثير الطيران
        float sineOffset = Mathf.Sin(Time.time * sineFrequency) * sineAmplitude;
        transform.position += new Vector3(0, sineOffset * Time.deltaTime, 0);
    }

    private void Attack()
    {
        // تفعيل أنميشن الهجوم إن وجد
        animator?.SetTrigger("attack");

        // TODO: تنقص دم اللاعب إذا عندك نظام صحة
        Debug.Log("Flying Shadow attacks player!");
    }
}