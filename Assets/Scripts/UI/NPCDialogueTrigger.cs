using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NPCDialogueTrigger : MonoBehaviour
{
    [Header("Dialogue Properties")]
    public Dialogue dialogue;
    public Text playerTextLabel;

    [SerializeField] private bool askProximityTrigger = false;
    [SerializeField] private float proximityRadius = 3f;

    [SerializeField] private bool useColliderTrigger = false;
    [SerializeField] private bool onClickTrigger = false;

    [SerializeField] private Vector3 cameraOffset;

    private DialogueManager dialogueMan;

    private Transform player;
    private void Start()
    {
        dialogueMan = FindObjectOfType<DialogueManager>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (useColliderTrigger && askProximityTrigger)
            Debug.LogError("Having the collider used to both ask and start the dialogue is not good.");

        GameEvents.current.onDialogueStart += OnDialogueBegin;
    }
    private bool withinAskProximity = false;
    string textBefore;
    private void Update()
    {
        if (!dialogueMan.isCurrentlyInDialogue)
        {
            if (Vector3.Distance(transform.position, player.position) < proximityRadius) //Proximity sense because on trigger was bugging out with on click handlers
            {
                if (!withinAskProximity) //To help with sensing on entering and not just staying.
                {
                    withinAskProximity = true;
                    textBefore = playerTextLabel.text;

                    if (useColliderTrigger)
                        TriggerStartDialogue();
                    else if (askProximityTrigger)
                        TriggerAskDialogue();
                }
            } else
            {
                if (withinAskProximity)
                {
                    withinAskProximity = false;
                    playerTextLabel.text = textBefore;
                }
            }
                if (onClickTrigger)
            {
#if UNITY_EDITOR
                if (Input.GetMouseButtonDown(0))
                {
                    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

                    RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
                    if (hit.collider != null)
                    {
                        if (hit.collider.name == gameObject.name)
                        {
                            TriggerStartDialogue();
                        }
                    }
                }
#else
                if (Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Began))
                {
                    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

                    RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
                    if (hit.collider != null)
                    {
                        if (hit.collider.name == gameObject.name)
                        {
                            TriggerStartDialogue();
                        }
                    }
                }
#endif
            }
        }
    }
    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        textBefore = playerTextLabel.text;
        if (useColliderTrigger && collision.gameObject.name == "Zen") {
            TriggerStartDialogue();
        } else if (askProximityTrigger && collision.gameObject.name == "Zen") {
            TriggerAskDialogue();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        playerTextLabel.text = textBefore;
    }*/
    public void TriggerAskDialogue ()
    {
        playerTextLabel.text = "Tap To Talk...";
        playerTextLabel.gameObject.SetActive(true);
    }
    public void TriggerStartDialogue()
	{
        //TODO: Start NPC talking nonsense animation and sound

        if (FindObjectOfType<GameManager>().player.transform.position.x < transform.position.x) //Ensuring npc is facing the player when talking
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        else
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);

		dialogueMan.StartDialogue(dialogue);
	}
    public void OnDialogueBegin ()
    {
        //Camera.main.GetComponent<CameraFollow>().target = transform;
        //Camera.main.GetComponent<CameraFollow>().offset = cameraOffset;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, proximityRadius);
    }
}
