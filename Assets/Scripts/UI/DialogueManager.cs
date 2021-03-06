using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
	public Text nameText;
	public Text dialogueText;

	public Animator animator;

	public bool isCurrentlyInDialogue;

	private Queue<string> sentences;

	void Start()
	{
		sentences = new Queue<string>();
	}
	private void Update() {
		if (isCurrentlyInDialogue)
			if (Input.GetKeyDown(KeyCode.Space))
				DisplayNextSentence();
	}
	public void StartDialogue(Dialogue dialogue, string gameObjectName)
	{
		if (sentences.Count > 0) //Should not start new dialogue if there is already sentences in queue
			return;

		isCurrentlyInDialogue = true;

		currentTalking = gameObjectName;

		GameEvents.current.DialogueStart();

		nameText.text = dialogue.name;

		sentences.Clear();

		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}
		dialogueText.transform.parent.gameObject.SetActive(true);
		animator.SetTrigger("DialogueStart");

		DisplayNextSentence();
	}

	public void DisplayNextSentence()
	{
		if (sentences.Count == 0)
		{
			EndDialogue();
			return;
		}

		string sentence = sentences.Dequeue();
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
	}

	IEnumerator TypeSentence(string sentence)
	{
		string sentenceToType = string.Concat(sentence + "\n" + "(Spacebar) to continue...");
		dialogueText.text = "";
		foreach (char letter in sentenceToType.ToCharArray())
		{
			dialogueText.text += letter;
			yield return 0;
		}
	}
	string currentTalking;
	void EndDialogue()
	{
		isCurrentlyInDialogue = false;
		GameEvents.current.DialogueEnd(currentTalking);
		//Camera.main.GetComponent<CameraFollow>().target = GameObject.FindGameObjectWithTag("Player").transform;
		//Camera.main.GetComponent<CameraFollow>().offset = new Vector3(0, 1.5f, 0);
		animator.SetTrigger("DialogueEnd");
	}

}
