using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Collectible : MonoBehaviour
{

    public GameObject itemPrefab;
    public string objectName;
    private TextMeshProUGUI floatText;

    public string dialogue;

    private bool playerInRange;

    // Start is called before the first frame update
    void Start()
    {

        floatText = GetComponentInChildren<TextMeshProUGUI>();
        floatText.text = "???";
        playerInRange = false;

        if (itemPrefab != null)
        {
            // Add item child
            GameObject item = Instantiate(itemPrefab, transform.position, transform.rotation);
            item.transform.SetParent(transform);
            item.transform.localPosition = Vector3.zero;

            // Position floatText above item
            Renderer rend = item.GetComponentInChildren<Renderer>();
            if (rend != null)
            {
                floatText.rectTransform.position = new Vector3(transform.position.x, rend.bounds.max.y + 0.5f, transform.position.z);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            GameManager.GM.hintsCollected++;
            GetComponent<Collider>().enabled = false;
            GameManager.GM.collectibleHelpText.enabled = false;
            GameManager.GM.StartDialogue(dialogue);
            GetComponent<AudioSource>().Play();
            Destroy(gameObject, 0.5f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameManager.GM.collectibleHelpText.enabled = true;
        floatText.text = objectName;
        playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        GameManager.GM.collectibleHelpText.enabled = false;
        playerInRange = false;
    }
}
