using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    int totalHints;

    public int hintsCollected;

    float timer;

    public TextMeshProUGUI hintCollectedText, timerText, collectibleHelpText;

    public static GameManager GM;

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
    }

    // Update is called once per frame
    void Update()
    {
        System.TimeSpan t = System.TimeSpan.FromSeconds(timer);
        timer += Time.deltaTime;

        hintCollectedText.text = "Hints Collected: " + hintsCollected + "/" + totalHints;
        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", t.Minutes, t.Seconds, t.Milliseconds);
    }
}
