using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    // Update is called once per frame

    void Update()
    {         
        if (Input.GetMouseButtonDown(0)) {
            Vector3 worldPoint = Input.mousePosition;
            worldPoint.z = Mathf.Abs(Camera.main.transform.position.z);
            //  Debug.Log(worldPoint);
            
            //worldPoint.z = 11f;
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(worldPoint);
            mouseWorldPosition.z = 0f;
            transform.parent.parent.position = mouseWorldPosition;
        }
    }
}
