using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyMovement : MonoBehaviour
{
    Rigidbody rbDrone;
    Transform cameraT;

    public bool isL2Down = false;
    public bool isL2Up = true;

    public KeepOriginalPos orignalPos;


    void Awake()
    {
        rbDrone = GetComponent<Rigidbody>();
    }

    void Update()
    {
        EnableButtonForL2();

        //rbDrone.constraints = RigidbodyConstraints.FreezeRotationZ;

        /*
            CONTROLLER INPUT
        */
        

        

        if (Input.GetButton("PS4_L1") || isL2Down)
        {
            rbDrone.drag = 0;
            rbDrone.constraints = RigidbodyConstraints.None;
            //rbDrone.constraints = RigidbodyConstraints.FreezeRotationZ;
        }
        else
        {
            rbDrone.constraints = RigidbodyConstraints.FreezePositionY;
        }
    }

    void FixedUpdate()
    {
        FlyUpDown();
        
        rbDrone.AddForce(orignalPos.orignalTransUp * upForce);
    }

    void FreezeY()
    {
        rbDrone.constraints = RigidbodyConstraints.FreezePositionY;
    }

    void Freeze()
    {
        rbDrone.constraints = RigidbodyConstraints.FreezePosition;
    }

    public float upForce;
    void FlyUpDown() 
    {
        if (Input.GetKey(KeyCode.I) || Input.GetButton("PS4_L1"))
        {
            rbDrone.velocity = rbDrone.velocity;
            upForce = 4000;
            if(Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
            {
                upForce = 3600;
            }
        }
        else if(Input.GetKey(KeyCode.K) || isL2Down)
        {
            rbDrone.velocity = rbDrone.velocity;
            upForce = -2000;
        }
        else if(!Input.GetKey(KeyCode.I) && !Input.GetKey(KeyCode.K) && Input.GetButtonDown("PS4_L1") && (Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.2f))
        {
            upForce = 98.1f;
        }
        else
        {
            upForce = 0;
            rbDrone.velocity = new Vector3(rbDrone.velocity.x, Mathf.Lerp(rbDrone.velocity.y, 0, Time.deltaTime * 5), rbDrone.velocity.z);
        }
    }

    void EnableButtonForL2()
    {
        float foo = Input.GetAxisRaw("PS4_L2");
        if (foo < 0)
        {
            if (!isL2Up) 
            {
                isL2Up = true; 
            }
            if (isL2Down)  
            {
                isL2Down = false;  
            }
        }
        else if (foo > 0)
        {
            if (isL2Up)
            {
                isL2Up = false;
            }
            if (!isL2Down)
            {
                isL2Down = true;
            }
        }
        else if (foo == 0)
        {
            if (isL2Up)
            {
                isL2Down = false;
            }
            if (isL2Down)
            {
                isL2Up = false;
            }
        }
    }

}
