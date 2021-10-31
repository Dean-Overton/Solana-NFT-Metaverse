using UnityEngine;
using System.Collections;

public class BlackHolePower : MonoBehaviour
{
    [SerializeField] private GameObject blackholePrefab;
    [SerializeField] private float timeBetweenCast = 3f;
    public bool canCast = true;

    float time;
    private void Start() {
        time = 0f;
    }
    private void Update() {
        time -= Time.deltaTime;
        if (time <= 0 && canCast) {
            if (Input.GetMouseButtonDown(0)) {
                transform.parent.parent.GetComponent<Animator>().SetTrigger("CastSpell");
                Vector3 worldPoint = Input.mousePosition;
                worldPoint.z = Mathf.Abs(Camera.main.transform.position.z);
                //worldPoint.z = 11f;
                Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(worldPoint);
                mouseWorldPosition.z = 0f;
                GameObject hole = Instantiate(blackholePrefab, mouseWorldPosition, Quaternion.identity);
                hole.GetComponent<Blackhole>().playerSpawned = true;
                time = timeBetweenCast;
            }
        }
    }
}
