using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
    [Header("Enemy Properties")]
    [SerializeField] private float health = 10f;
    [SerializeField] private float playerAttackDistance = 1f;
    [SerializeField] private Transform handPos;

    private Transform target;
    private Movement enemyMovementScript;
    private void Start() {
        target = FindObjectOfType<Player>().transform;
    }
    private void Awake() {
        enemyMovementScript = GetComponent<Movement>();
    }
    private void FixedUpdate() {
        MoveTowardsPlayer();
    }
    private void Update()
    {
        if (health <= 0)
            Die();
    }
    public void MoveTowardsPlayer () {
        if (target) {
            Vector3 movementDirection = (target.transform.position - transform.position).normalized;
            enemyMovementScript.MoveWithInput(movementDirection);
        }
    }
    public void Attack () {

    }
    public void Die () {
        Destroy(gameObject);
    }
    private void OnDrawGizmosSelected()
    {
        if (!handPos)
            Debug.LogError("No hand transform");
        Gizmos.DrawWireSphere(handPos.position, playerAttackDistance);
    }
}
