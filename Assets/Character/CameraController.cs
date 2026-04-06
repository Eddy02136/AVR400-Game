using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float maxY, minY, sensitivity, zoomSpeed, targetDistance, maxDistance, minDistance, zoomSensitivity;
    float currentAngle;

    float distance;

    Vector3 dir;

    // Start is called before the first frame update
    void Start()
    {
        currentAngle = Mathf.Atan2(transform.localPosition.y, Mathf.Abs(transform.localPosition.z));
        distance = transform.localPosition.magnitude;
    }

    // Update is called once per frame
    void LateUpdate()
    {

        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        transform.parent.Rotate(Vector3.up, mouseX, Space.World);

        targetDistance = Mathf.Clamp(targetDistance - Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity * Time.deltaTime, minDistance, maxDistance);

        transform.localPosition *= distance / transform.localPosition.magnitude;

        float input = Input.GetAxis("Mouse Y");

        float newAngle = Mathf.Clamp(currentAngle - input * sensitivity * Time.deltaTime, minY, maxY);

        dir = transform.localPosition;

        RaycastHit hit;
        int layerMask = 1 << 7;
        if (Physics.SphereCast(transform.parent.position, 0.5f, -transform.forward, out hit, targetDistance, layerMask))
        {
            distance = hit.distance;
        }
        else
            distance = Mathf.Lerp(distance, targetDistance, zoomSpeed * Time.deltaTime);


        dir = Quaternion.AngleAxis(newAngle - currentAngle, Vector3.right) * dir;

        transform.localPosition = dir;
        transform.Rotate(Vector3.right, newAngle - currentAngle);

        currentAngle = newAngle;

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1f);

        Gizmos.DrawWireSphere(transform.parent.position, 1f);

        Gizmos.DrawRay(transform.parent.position, -transform.forward * distance);
    }


}
