using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscoPlayer : MonoBehaviour
{
    public Animator anim;
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetBool("Dancing", true);
        }
        if (Input.GetKeyUp(KeyCode.Space)){
            anim.SetBool("Dancing", false);
        }
    }
}
