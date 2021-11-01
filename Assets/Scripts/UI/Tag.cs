using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Tag : MonoBehaviour
{
    public Text tagText;

    private void Start() {
        oldText = tagText.text;
    }
    public void ShowTag () {
        tagText.gameObject.SetActive(true);
    }
    public void HideTag () {
        tagText.gameObject.SetActive(false);
    }
    public void ChangeTag (string newTagText) {
        tagText.text = newTagText;
    }
    private string oldText;
    public void ChangeTagTemp (string newTagText) { //Temporary tag
        oldText = tagText.text;
        tagText.text = newTagText;
    }
    public void ChangeTagTempBack () { //Temporary tag revert
        tagText.text = oldText;
    }
}
