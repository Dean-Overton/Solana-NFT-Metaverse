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

    }
}
