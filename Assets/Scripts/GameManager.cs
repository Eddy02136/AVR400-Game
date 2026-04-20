using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    int totalHints;

    public int hintsCollected;

    float timer;

    public TextMeshProUGUI hintCollectedText, timerText, collectibleHelpText, dialogueText, deadHint, deadTimer, winHint, winTimer;

    public static GameManager GM;

    public GameObject door;

    public GameObject dialogueBg, deadScreen, winScreen;

    private bool doorIsOpen;

    public float dialogueSpeed;

    private string dialogue = "Welcome, detective. Will you manage to uncover the mysteries surrounding this forest? Find the ten hints and it may put you on the right track.";

    private bool isFinished;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
        totalHints = 10;
        hintsCollected = 0;
        isFinished = false;

        if (GM == null)
        {
            GM = this;
        }
        collectibleHelpText.enabled = false;
        winScreen.SetActive(false);
        deadScreen.SetActive(false);
        doorIsOpen = false;
        StartDialogue(dialogue);
    }

    // Update is called once per frame
    void Update()
    {
        System.TimeSpan t = System.TimeSpan.FromSeconds(timer);
        timer += Time.deltaTime;

        if (!isFinished)
        {
            hintCollectedText.text = "Hints Collected: " + hintsCollected + "/" + totalHints;
            timerText.text = string.Format("{0:00}:{1:00}:{2:00}", t.Minutes, t.Seconds, t.Milliseconds);
        }

        if (hintsCollected >= totalHints && !doorIsOpen)
        {
            door.transform.Rotate(Vector3.up * (door.transform.rotation.y - 111f));
            doorIsOpen = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (dialogueText.text == dialogue)
            {
                StopAllCoroutines();
                dialogueText.text = "";
                dialogueBg.SetActive(false);
                dialogueText.enabled = false;
                if (hintsCollected >= 11)
                {
                    GameManager.GM.Win();
                }
            }
        }
    }

    public void StartDialogue(string dialogue)
    {
        this.dialogue = dialogue;
        dialogueBg.SetActive(true);
        dialogueText.enabled = true;
        dialogueText.text = "";
        StartCoroutine(TypeLine(dialogue));
    }

    public void Win()
    {
        isFinished = true;
        winHint.text = hintCollectedText.text;
        winTimer.text = "Time: " + timerText.text;
        hintCollectedText.enabled = false;
        timerText.enabled = false;
        dialogueBg.SetActive(false);
        dialogueText.enabled = false;
        StopAllCoroutines();
        Camera.main.GetComponent<CameraController>().enabled = false;
        GameObject.FindWithTag("Player").transform.GetComponent<PlayerController>().enabled = false;
        MonoBehaviour[] AIScript = GameObject.FindWithTag("Animals").transform.GetComponentsInChildren<AIBehavior>();
        foreach (AIBehavior behaviour in AIScript)
        {
            behaviour.enabled = false;
        }
        winScreen.SetActive(true);
    }

    public void GameOver()
    {
        isFinished = true;
        deadHint.text = hintCollectedText.text;
        deadTimer.text = "Time: "+ timerText.text;
        hintCollectedText.enabled = false;
        timerText.enabled = false;
        dialogueBg.SetActive(false);
        dialogueText.enabled = false;
        StopAllCoroutines();
        Camera.main.GetComponent<CameraController>().enabled = false;
        GameObject.FindWithTag("Player").transform.GetComponent<PlayerController>().enabled = false;
        MonoBehaviour[] AIScript = GameObject.FindWithTag("Animals").transform.GetComponentsInChildren<AIBehavior>();
        foreach (AIBehavior behaviour in AIScript) { 
            behaviour.enabled = false; 
        }
        deadScreen.SetActive(true);
    }

    IEnumerator TypeLine(string dialogue)
    {
        foreach (char c in dialogue)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(dialogueSpeed);
        }
    }
}
