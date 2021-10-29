using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meatworks : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other) {
        Rigidbody2D rB = other.gameObject.GetComponent<Rigidbody2D>();
        if (rB) //kill only rigidbodies
            Destroy(other.gameObject);
    }
}
