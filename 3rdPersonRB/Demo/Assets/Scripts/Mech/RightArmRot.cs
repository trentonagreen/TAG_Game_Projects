using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightArmRot : MonoBehaviour
{
    Transform cameraT;
    float initRotX;

    void Start()
    {
        cameraT = Camera.main.transform;
    }

    void Update()
    {
        initRotX = cameraT.eulerAngles.x - 90f;

        transform.localRotation = Quaternion.Euler(new Vector3(initRotX, 90, -115));
    }
}
