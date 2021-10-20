using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[ExecuteAlways]
public class SortingScript : MonoBehaviour
{
    public List<SpriteRenderer> sRS = new List<SpriteRenderer>();
    public float offsetY;
    private Animator anim;

    private void Awake()
    {
        //anim = GetComponent<Animator>();
    }
    void Update()
    {
        foreach (SpriteRenderer sR in sRS)
        {
            sR.sortingOrder = (int)((transform.position.y + offsetY) * -100f + sRS.IndexOf(sR));
        }
    }

    public void Die ()
    {
        Destroy(gameObject);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(new Vector3(transform.position.x - 1f, transform.position.y + offsetY, 0f), new Vector3(transform.position.x + 1f, transform.position.y + offsetY, 0f));
    }
}
