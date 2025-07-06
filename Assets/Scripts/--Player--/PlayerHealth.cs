using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    // Reference to UI slider
    public Image healthBar;

    public Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();

        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }

        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = (float)currentHealth / maxHealth;
        }
    }

    void Die()
    {
        Debug.Log("Player died!");
        
        if (GameUIManager.Instance != null){
            GameUIManager.Instance.ShowGameOver();
        }
        animator.Play("Death");

        StartCoroutine(show());

    }

    IEnumerator show(){
        yield return new WaitForSecondsRealtime(1);
        Time.timeScale = 0f;
    }
}