using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerRot : MonoBehaviour
{
    Transform cameraT;

    void Start()
    {
        cameraT = Camera.main.transform;
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(new Vector3(cameraT.eulerAngles.x, cameraT.eulerAngles.y, cameraT.eulerAngles.z));
    }
}
