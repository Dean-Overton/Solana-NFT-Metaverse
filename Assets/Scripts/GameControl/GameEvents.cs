using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;
    private void Awake()
    {
        current = this;
    }

    //DIALOGUE
    public event Action onDialogueStart;
    public void DialogueStart ()
    {
        if (onDialogueStart != null)
            onDialogueStart();
    }
    public event Action<string> onDialogueEnd; //Requires string parameter to easily find who was talking
    public void DialogueEnd(string nameOfNPC)
    {
        if (onDialogueEnd != null)
            onDialogueEnd(nameOfNPC);
    }

    //Questing
    public event Action onQuestStart;
    public void QuestStart()
    {
        if (onQuestStart != null)
            onQuestStart();
    }
    public event Action onQuestComplete;
    public void CompleteQuest()
    {
        if (onQuestComplete != null)
            onQuestComplete();
    }

    //Item Pickup Tracking for quest
    public event Action<ItemObject> onItemPickup;
    public void PickUpItem(ItemObject item)
    {
        if (onItemPickup != null)
            onItemPickup(item);
    }
}
