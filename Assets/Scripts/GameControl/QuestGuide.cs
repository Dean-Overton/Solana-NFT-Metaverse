using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGuide : MonoBehaviour
{
    [SerializeField]private string nameOfQuestScene;
    [SerializeField]private bool triggerQuestOnDialogueEnd = false;

    private void Start() {
        if (triggerQuestOnDialogueEnd) {
            GameEvents.current.onDialogueEnd += TransitionToQuestScene;
        }
    }
    private void TransitionToQuestScene (string nameOfNPC) {
        if (nameOfNPC != gameObject.name)
            return;
        if (nameOfQuestScene == "" || nameOfQuestScene == null) {
            Debug.LogError("No scene for guide to redirect to!");
            return;
        }
        Debug.Log(nameOfQuestScene);
        FindObjectOfType<SceneLoader>().LoadSceneByName(nameOfQuestScene);
    }
}
