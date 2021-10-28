using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHolePower : MonoBehaviour
{
    [SerializeField]
    private GameObject blackholePrefab;

    private void FixedUpdate() {
        if (Input.GetMouseButtonUp(0)) {
            Vector3 worldPoint = Input.mousePosition;
            worldPoint.z = Mathf.Abs(Camera.main.transform.position.z);
            //worldPoint.z = 11f;
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(worldPoint);
            mouseWorldPosition.z = 0f;
            Instantiate(blackholePrefab, mouseWorldPosition, Quaternion.identity);
        }
    }
}
