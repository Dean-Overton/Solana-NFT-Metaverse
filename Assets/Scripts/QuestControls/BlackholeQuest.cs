using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackholeQuest : MonoBehaviour
{
    public int totalEnemyKills = 0;
    private void Update() {
        if (totalEnemyKills >= 20) {
            GetComponent<QuestManager>().QuestComplete();
        }
    }
}
