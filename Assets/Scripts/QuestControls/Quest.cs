using UnityEngine;
using UnityEngine.UI;

public class Quest : MonoBehaviour
{
    private void OnEnable()
    {
        GameManager.current.questProgressUI.SetActive(true);
        UpdateQuestProgress(0f);
    }
    public void UpdateQuestProgress(float percentage)
    {
        GameManager.current.currentQuest.questProgressPercentage = percentage;
        GameManager.current.questProgressUI.GetComponentInChildren<Text>().text = string.Concat(Mathf.Round(percentage).ToString(), "%");
    }
}
