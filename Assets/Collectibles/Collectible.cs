using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{

    public GameObject itemPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        GameManager.GM.collectibleHelpText.enabled = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            GameManager.GM.hintsCollected++;
            GetComponent<Collider>().enabled = false;
            GameManager.GM.collectibleHelpText.enabled = false;
            Destroy(gameObject, 0.5f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameManager.GM.collectibleHelpText.enabled = false;
    }
}
