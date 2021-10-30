using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackholeQuest : MonoBehaviour
{  
    public int totalEnemyKills = 0;
    public GameObject payPanel;
    private void Update() {
        if (!Payment.current.folkOwnership) {
            if (totalEnemyKills >= 20) {
                payPanel.SetActive(true);
                Time.timeScale = 0;
            }
        } else {
            if (totalEnemyKills >= 100) {
                GetComponent<QuestManager>().QuestComplete();
            }
        }
    }
    public void PayToKeepPlaying () {
        Payment.current.PromptPay();
        Payment.current.onPaymentSuccessful += HasPayedForPlaying;
    }
    public void HasPayedForPlaying () {
        payPanel.SetActive(false);
        Time.timeScale = 1;
    }
}
