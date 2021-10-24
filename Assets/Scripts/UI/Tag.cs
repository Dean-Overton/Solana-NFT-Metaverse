using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Tag : MonoBehaviour
{
    public Text tagText;

    public void ShowTag () {
        tagText.gameObject.SetActive(true);
    }
    public void HideTag () {
        tagText.gameObject.SetActive(false);
    }
    public void ChangeTag (string newTagText) {
        tagText.text = newTagText;
    }
}
