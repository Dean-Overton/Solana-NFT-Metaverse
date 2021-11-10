using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandScript : MonoBehaviour
{
    private void Die () {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Rigidbody2D rB = other.gameObject.GetComponent<Rigidbody2D>();
        if (rB) { //kill only rigidbodies
            if (other.gameObject.GetComponent<Player>())
                other.gameObject.GetComponent<Player>().Die();
        }
    }
}
