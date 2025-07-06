// using UnityEngine;

// public class PlayerAttack : MonoBehaviour
// {
//     public int attackDamage = 10;
//     public float attackRange = 2f;

//     public LayerMask enemLapyer;

//     void Update()
//     {
//         // Attack with left mouse button
//         if (Input.GetMouseButtonDown(0))
//         {
//             Attack();
//         }
//     }

//     void Attack()
//     {
//         // Find all enemies in range
//         Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange);

//         // Damage each enemy found
//         foreach (Collider2D enemy in hitEnemies)
//         {
//             // Check if it's an enemy
//             EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
//             if (enemyHealth != null)
//             {
//                 enemyHealth.TakeDamage(attackDamage);
//                 Debug.Log("Hit enemy for " + attackDamage + " damage");
//             }
//         }
//     }

//     // Draw the attack range in the editor
//     void OnDrawGizmosSelected()
//     {
//         Gizmos.color = Color.red;
//         Gizmos.DrawWireSphere(transform.position, attackRange);
//     }
// }