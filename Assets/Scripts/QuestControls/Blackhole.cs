    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole : MonoBehaviour
{
    [SerializeField] private float pullRange = 5f;
    [SerializeField] private float duration = 5f;

    // Update is called once per frame
    void FixedUpdate()
    {
        Collider2D[] effectedColliders = Physics2D.OverlapCircleAll(transform.position, pullRange);
        if (effectedColliders.Length == 0)
            return;
        foreach (Collider2D col in effectedColliders) {
            Rigidbody2D rB = null; 
            if (col.gameObject.TryGetComponent<Rigidbody2D>(out rB)) {
                if (rB.gameObject != this.gameObject)
                    PullObject(col.gameObject, rB);
            }
        }
    }
    private void Update() {
        duration -= Time.deltaTime;
        if (duration <= 0)
            Die();
    }
    private void Die () {
        Destroy(gameObject);
    }
    public void PullObject (GameObject effectedGameObject, Rigidbody2D rB) {
        Vector2 direction = transform.position - effectedGameObject.transform.position;
        float distance = direction.magnitude;

        float forceMagnitude = (GetComponent<Rigidbody2D>().mass * rB.mass) / distance;

        Vector2 pullForce = direction.normalized * forceMagnitude;
        rB.AddForce(pullForce, ForceMode2D.Force);
    }
    private void OnCollisionEnter2D(Collision2D other) {
        Rigidbody2D rB = other.gameObject.GetComponent<Rigidbody2D>();
        if (rB) //kill only rigidbodies
            Destroy(other.gameObject);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, pullRange);
    }
}
