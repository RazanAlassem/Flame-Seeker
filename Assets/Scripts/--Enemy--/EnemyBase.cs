using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] protected int maxHealth = 1;
    protected int currentHealth;

    [Header("Animation")]
    [SerializeField] protected string dieTrigger = "die";
    [SerializeField] protected string phase2Trigger = "phase2";
    protected Animator animator;

    [Header("Death Settings")]
    [SerializeField] protected float deathDelay = 1.5f;

    [Header("Phase Control (Boss Only)")]
    [SerializeField] protected bool isBoss = false;
    [SerializeField] protected int phase2Threshold = 3;
    protected bool isInPhase2 = false;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public virtual void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"{gameObject.name} took damage, HP: {currentHealth}");

        if (isBoss && !isInPhase2 && currentHealth == phase2Threshold)
        {
            EnterPhase2();
            return;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void EnterPhase2()
    {
        isInPhase2 = true;
        Debug.Log($"{gameObject.name} entered Phase 2!");
        if (animator != null && !string.IsNullOrEmpty(phase2Trigger))
        {
            animator.SetTrigger(phase2Trigger);
        }

        // يمكنك هنا تفعيل سلوك جديد، مثل سرعة أعلى أو ضربات إضافية
    }

    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} died.");
        if (animator != null && !string.IsNullOrEmpty(dieTrigger))
        {
            animator.SetTrigger(dieTrigger);
        }

        Destroy(gameObject, deathDelay);
    }
}
