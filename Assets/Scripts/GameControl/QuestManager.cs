using UnityEngine.SceneManagement;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public GameObject questPanel;
    public GameObject deathPanel;

    public GameObject player;
    private void Start() {
        player = FindObjectOfType<Player>().gameObject;
        Time.timeScale = 1;
    }
    void Update()
    {
        if (!player)
            deathPanel.SetActive(true);
    }
    public void QuestComplete () {
        questPanel.SetActive(true);
        Time.timeScale = 0;
    }
    public void RestartScene () {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void BackToMainland () {
        SceneManager.LoadScene(0);
    }
}
