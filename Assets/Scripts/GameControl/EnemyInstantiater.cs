using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInstantiater : MonoBehaviour
{
    public Transform[] spawnPoints;
    List<GameObject> spawned = new List<GameObject>();
    public float timeBetweenSpawns = 2f;
    public Queue<GameObject> enemieSpawnQueue = new Queue<GameObject>();
    float time;
    private void Start() {
        time = timeBetweenSpawns;
    }
    private void Update() {
        if (enemieSpawnQueue.Count > 0) {
            Debug.Log(time);
            time -= Time.deltaTime;
            if (time <= 0)
            {
                Debug.Log("spawn");
                SpawnEnemy(enemieSpawnQueue.Dequeue());
                time = timeBetweenSpawns;
            }
        }
    }
    public void SpawnEnemy (GameObject enemyPrefab) {
        if (!enemyPrefab) {
            Debug.LogError("No enemy prefab passed to spawn!");
            return;
        }
        int randSpawnIndex = Random.Range(0, spawnPoints.Length);
        GameObject enemy = Instantiate(enemyPrefab, spawnPoints[randSpawnIndex].position, Quaternion.identity, transform);
        spawned.Add(enemy);
    }
}

