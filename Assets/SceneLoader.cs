using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Animator swipeTransitionAnim;
    [Tooltip("Used to wait this amount of time till actually trying to load scene.")]
    [SerializeField] private float swipeTransitioWaitTime;

    IEnumerator LoadSceneByName (string name) {
        swipeTransitionAnim.SetTrigger("Start");

        yield return new WaitForSeconds (swipeTransitioWaitTime);

        SceneManager.LoadScene (name);
    }
}
