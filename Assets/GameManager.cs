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

    public TextMeshProUGUI hintCollectedText, timerText, collectibleHelpText, dialogueText;

    public static GameManager GM;

    public GameObject door;

    public GameObject dialogueBg;

    private bool doorIsOpen;

    public float dialogueSpeed;

    private string dialogue;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
        totalHints = 10;
        hintsCollected = 0;

        if (GM == null)
        {
            GM = this;
        }
        collectibleHelpText.enabled = false;
        doorIsOpen = false;
        dialogueText.text = "";
        dialogueText.enabled = false;
        dialogueBg.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        System.TimeSpan t = System.TimeSpan.FromSeconds(timer);
        timer += Time.deltaTime;

        hintCollectedText.text = "Hints Collected: " + hintsCollected + "/" + totalHints;
        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", t.Minutes, t.Seconds, t.Milliseconds);

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

    IEnumerator TypeLine(string dialogue)
    {
        foreach (char c in dialogue)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(dialogueSpeed);
        }
    }
}
