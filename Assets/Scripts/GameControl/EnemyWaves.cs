using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyInstantiater))]
public class EnemyWaves : MonoBehaviour
{
    [Tooltip("List of enemies to spawn in order.")]
    public List<GameObject> enemiesToSpawn = new List<GameObject>();
    
    [Tooltip("An optional field. Leave blank to have a continous spawning of enemies")]
    public Wave[] waves;

    public float timeTillFirstWave = 5f;
    public float timeBetweenWaves = 5f;

    private EnemyInstantiater enemySpawnScript;
    private void Awake() {
        enemySpawnScript = GetComponent<EnemyInstantiater>();
    }
    void Start()
    {
        time = timeTillFirstWave;
    }
    float time;
    bool spawned = false;
    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            if (waves.Length > 0) {
                //Wave functionality
            } else {
                if (spawned)
                    return;
                for (int i = 0; i < enemiesToSpawn.Count; i++) {
                    enemySpawnScript.enemieSpawnQueue.Enqueue(enemiesToSpawn[i]);
                }
                spawned = true;
            }
        }
    }
}
[System.Serializable]
public class Wave
{
    public int waveNum;
    public int amount;
}
