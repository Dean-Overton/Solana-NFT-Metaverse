using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintManager : MonoBehaviour
{
	public Text importantHint;
	public Text unImportantHint;
	public float timeBetweenHints = 5f;

	public Animator animator;

	private Queue<Hint> hintsQueue;

	void Start()
	{
		hintsQueue = new Queue<Hint>();
	}
	private void Update() {
		if (hintsQueue.Count > 0) //Should not start new dialogue if there is already sentences in queue
			DequeueAndShowHint();
	}
	public void AddHint(string hintText, bool important)
	{
		Hint newHint = new Hint();
		newHint.text = hintText;
		newHint.important = important;
		hintsQueue.Enqueue(newHint);
	}
	public void DequeueAndShowHint()
	{
		Hint hintToShow = hintsQueue.Dequeue();
		StartCoroutine(ShowHint(hintToShow));
	}
	IEnumerator ShowHint (Hint hintToShow) {
		animator.SetTrigger("FadeOut");
		if (hintToShow.important) {
			importantHint.text = hintToShow.text;
			animator.SetTrigger("ImportantHint");
		} else {
			unImportantHint.text = hintToShow.text;
			animator.SetTrigger("UnImportantHint");
		}
		yield return new WaitForSeconds(timeBetweenHints);
	}
}
public class Hint{
	public string text;
	public bool important;
}

