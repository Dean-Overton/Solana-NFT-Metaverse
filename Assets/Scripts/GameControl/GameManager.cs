using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Player Inputs")]
    public GameObject movementJoystick;

    public GameObject player;

    public QuestObject currentQuest;
    public GameObject questProgressUI;

    [Header("Scripts")]
    private UI_Inventory uIInventory;

    public static GameManager current;

    private void Awake()
    {
        current = this;
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        GameEvents.current.onDialogueStart += OnDialogueStart;
        //GameEvents.current.onDialogueEnd += OnDialogueEnd;
    }

    void OnDialogueStart ()
    {
        PausePlayerMovement(true);
    }
    void OnDialogueEnd()
    {
        PausePlayerMovement(false);
    }
    public void PausePlayerMovement (bool pause)
    {
        if (pause)
            movementJoystick.SetActive(false);
        else
            movementJoystick.SetActive(true);
    }
}
[System.Serializable]
public class QuestObject
{
    public string questName;
    [Range(0, 100)]
    public float questProgressPercentage;
}
