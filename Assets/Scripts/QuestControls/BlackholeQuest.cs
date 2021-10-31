using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackholeQuest : MonoBehaviour
{  
    public int totalEnemyKills = 0;
    public GameObject buriedShepardsCrook;
    public GameObject payPanel;
    private bool hasPaidOrOwned = false;
    private void Update() {
        if (!Payment.current._folkOwnership) {
            if (!hasPaidOrOwned) {
                if (totalEnemyKills >= 20) {
                    payPanel.SetActive(true);
                    Time.timeScale = 0;
                }
            }
        }
        if (totalEnemyKills >= 100) {
            GetComponent<QuestManager>().QuestComplete();
        }
    }
    public void PayToKeepPlaying () {
        Payment.current.onPaymentSuccessful += HasPayedForPlaying;
        //Payment.current.PromptPay();
        HasPayedForPlaying();
    }
    public void HasPayedForPlaying () {
        hasPaidOrOwned = true;
        payPanel.SetActive(false);
        Time.timeScale = 1;
        FindObjectOfType<BlackHolePower>().canCast = true;
    }
}
