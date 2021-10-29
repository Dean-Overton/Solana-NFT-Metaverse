using UnityEngine;
using System.Collections;

public class BlackHolePower : MonoBehaviour
{
    [SerializeField] private GameObject blackholePrefab;

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            transform.parent.parent.GetComponent<Animator>().SetTrigger("CastSpell");
            StartCoroutine("SpawnBlackHole");
        }
    }
    public IEnumerator SpawnBlackHole()
    {
        Vector3 worldPoint = Input.mousePosition;
        worldPoint.z = Mathf.Abs(Camera.main.transform.position.z);
        //worldPoint.z = 11f;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(worldPoint);
        mouseWorldPosition.z = 0f;
        yield return new WaitForSeconds(0.5f);
        GameObject hole = Instantiate(blackholePrefab, mouseWorldPosition, Quaternion.identity);
        hole.GetComponent<Blackhole>().playerSpawned = true;
    }
}
