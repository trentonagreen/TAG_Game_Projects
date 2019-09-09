using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public float mouseSensitivity = 100f;
    public Transform target;
    public float distFromTarget = 2f;
    public Vector2 pitchRange = new Vector2(-25, 85);
    public float rotSmoothTime = .1f;

    Vector3 rotSmoothVel;
    Vector3 currRot;

    private float yaw;
    private float pitch;

    void LateUpdate()
    {
        //yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        //pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        yaw += Input.GetAxis("PS4_RightAnalogHorizontal") * mouseSensitivity;
        pitch += Input.GetAxis("PS4_RightAnalogVertical") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, pitchRange.x, pitchRange.y);

        currRot = Vector3.SmoothDamp(currRot, new Vector3(pitch, yaw), ref rotSmoothVel, rotSmoothTime);
        transform.eulerAngles = currRot;

        transform.position = target.position - transform.forward * distFromTarget;
    }
}
