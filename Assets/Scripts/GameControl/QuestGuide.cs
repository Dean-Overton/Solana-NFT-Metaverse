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
    private void TransitionToQuestScene () {
        if (nameOfQuestScene == "" || nameOfQuestScene == null) {
            Debug.LogError("No scene for guide to redirect to!");
            return;
        }
        FindObjectOfType<SceneLoader>().LoadSceneByName(nameOfQuestScene);
    }
}
