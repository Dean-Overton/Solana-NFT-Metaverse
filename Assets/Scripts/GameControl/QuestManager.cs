using UnityEngine.SceneManagement;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public GameObject questPanel;
    public GameObject deathPanel;

    public GameObject player;
    private void Start() {
        player = FindObjectOfType<Player>().gameObject;
    }
    void Update()
    {
        if (!player)
            deathPanel.SetActive(true);
    }
    public void QuestComplete () {
        questPanel.SetActive(true);
    }
    public void RestartScene () {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void BackToMainland () {
        SceneManager.LoadScene(0);
    }
}
