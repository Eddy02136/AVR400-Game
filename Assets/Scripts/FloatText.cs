using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatText : MonoBehaviour
{

    public Camera playercamera;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 direction = playercamera.transform.position - transform.position;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(-direction, Vector3.up);
        }
    }
}
