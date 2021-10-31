using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NPCDialogueTrigger : MonoBehaviour
{
    [Header("Dialogue Properties")]
    public Dialogue dialogue;

    [SerializeField] private Vector3 cameraOffset;

    private DialogueManager dialogueMan;

    private Transform player;
    private void Start()
    {
        dialogueMan = FindObjectOfType<DialogueManager>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        GameEvents.current.onDialogueStart += OnDialogueBegin;
    }
    public void TriggerTipTag ()
    {
        GetComponent<Tag>().ChangeTagTemp("Talk (T)");
    }
    public void TriggerStartDialogue()
	{
        //TODO: **Start NPC talking nonsense animation and sound**
		dialogueMan.StartDialogue(dialogue);
	}
    public void OnDialogueBegin ()
    {
        //Camera.main.GetComponent<CameraFollow>().target = transform;
        //Camera.main.GetComponent<CameraFollow>().offset = cameraOffset;
    }
}
