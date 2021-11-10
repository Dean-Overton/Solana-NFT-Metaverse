using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFollowMouse : MonoBehaviour
{
    // Start is called before the first frame update
    void Update()
    {         
        Vector3 worldPoint = Input.mousePosition;
        worldPoint.z = Mathf.Abs(Camera.main.transform.position.z);
        //  Debug.Log(worldPoint);
            
        //worldPoint.z = 11f;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(worldPoint);
        mouseWorldPosition.z = 0f;
        transform.position = mouseWorldPosition;
    }
}
