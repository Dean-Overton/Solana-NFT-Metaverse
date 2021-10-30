using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
    [Header("Enemy Properties")]
    [SerializeField] private float health = 10f;
    [SerializeField] private GameObject bloodPrefab;
    [SerializeField] private int attackDamage = 5;
    [SerializeField] private float timeBetweenAttacks = 4f;

    private Transform target;
    private Movement enemyMovementScript;
    private void Start() {
        if (FindObjectOfType<Player>()) {
            playerScript = FindObjectOfType<Player>();
            target = playerScript.transform;
        }
        time = timeBetweenAttacks;

    }
    private void Awake() {
        enemyMovementScript = GetComponent<Movement>();
    }
    private void FixedUpdate() {
        MoveTowardsPlayer();
    }
    float time;
    private void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0) {
            if (playerReach) {
                AttackPlayer();
            }
        }

        if (health <= 0)
            Die();
    }
    public void MoveTowardsPlayer () {
        if (target) {
            Vector3 movementDirection = (target.transform.position - transform.position).normalized;
            if (Vector3.Distance(transform.position, target.transform.position) < 2) {
                enemyMovementScript.MoveWithInput(Vector3.zero);
            } else {
                enemyMovementScript.MoveWithInput(movementDirection);
            }
        }
    }
    public void Die () {
        Instantiate(bloodPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    private bool playerReach = false;
    Player playerScript;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            playerReach = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") {
            playerReach = false;
        }
    }
    public void AttackPlayer () {
        playerScript.DamagePlayer(attackDamage);
        time = timeBetweenAttacks;
        GetComponent<Animator>().SetTrigger("Attack");
    }
}
