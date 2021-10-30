using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGuideSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public QuestGuideObject[] questGuidesToSpawn;
    Dictionary<GameObject, QuestGuideObject> currentGuides = new Dictionary<GameObject, QuestGuideObject>();
    public float timeBetweenSpawns = 10f;
    public Queue<GameObject> guideSpawnQueue = new Queue<GameObject>();
    float time;
    private void Start() {
        foreach (QuestGuideObject guide in questGuidesToSpawn)
            currentGuides.Add(SpawnGuide(guide.questGuidePrefab), guide);
        time = timeBetweenSpawns;
    }
    private void Update() {
        while (currentGuides.ContainsKey(null)) {
            QuestGuideObject guide;
            if (currentGuides.TryGetValue(null, out guide)) {
                guideSpawnQueue.Enqueue(guide.questGuidePrefab);
            }
        }
        if (guideSpawnQueue.Count > 0) {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                SpawnGuide(guideSpawnQueue.Dequeue());
                time = timeBetweenSpawns;
            }
        }
    }
    public GameObject SpawnGuide (GameObject guidePrefab) {
        if (!guidePrefab) {
            Debug.LogError("No enemy prefab passed to spawn!");
            return null;
        }
        if (spawnPoints.Length == 0)
            return null;
        int randSpawnIndex = Random.Range(0, spawnPoints.Length);
        GameObject guide = Instantiate(guidePrefab, spawnPoints[randSpawnIndex].position, Quaternion.identity, transform);
        return guide;
    }
}
[System.Serializable]
public class QuestGuideObject {
    public string questName;
    public GameObject questGuidePrefab;
}

