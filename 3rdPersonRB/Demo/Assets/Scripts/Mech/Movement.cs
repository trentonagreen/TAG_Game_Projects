using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody rbDrone;
    Transform cameraT;
    Quaternion startRot;
    Vector3 startVel;

    public bool lockCursor;

    public float moveForwardSpeed = 500;
    //public float tiltAmountForward = 0f;
    //public float tiltVelocityForward;

    public float wantRotationY;
    public float currRotationY;
    public float rotateAmountByKeys = 3.0f;
    public float rotationVelocityY;
    public float rotateSpeed = .3f;

    public Vector3 smoothDampVelocity;

    public float sideMovementSpeed = 300f;
    public float tiltAmountSide;
    public float tiltVelocitySide;

    private void Awake()
    {
        rbDrone = GetComponent<Rigidbody>();

        startRot = Quaternion.Euler(rbDrone.rotation.x, rbDrone.rotation.y, rbDrone.rotation.z);
        startVel = rbDrone.velocity;
    }

    private void Start()
    {
        cameraT = Camera.main.transform;

        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void Update()
    {
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            rbDrone.drag = 0;

            //rbDrone.constraints = RigidbodyConstraints.None;
            //rbDrone.constraints = RigidbodyConstraints.FreezePositionY;
        }

        if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
        {
            rbDrone.drag = 100;
            //rbDrone.constraints = RigidbodyConstraints.FreezePosition;
        }
    }

    private void FixedUpdate()
    {
        MoveForward();
        MoveRotation();
        MovementSmoothingClamp();
        MoveLeftRight();

        rbDrone.rotation = Quaternion.Euler(new Vector3(cameraT.eulerAngles.x, currRotationY, rbDrone.rotation.z));
    }

    void Freeze()
    {
        rbDrone.constraints = RigidbodyConstraints.FreezePosition;
    }

    void MoveForward()
    {
        if (System.Math.Abs(Input.GetAxis("Vertical") - Mathf.Epsilon) > 0f)
        {
            Vector3 moveForce = Vector3.forward * Input.GetAxis("Vertical") * moveForwardSpeed;
            rbDrone.AddRelativeForce(moveForce.x, 0, moveForce.z, ForceMode.Force);

            //rbDrone.AddRelativeForce(Vector3.forward * Input.GetAxis("Vertical") * moveForwardSpeed);

            //rbDrone.MovePosition(transform.position + transform.forward * Time.deltaTime);
            //rbDrone.MovePosition(transform.position + Vector3.forward * Input.GetAxis("Vertical") * Time.deltaTime * moveForwardSpeed);
            //tiltAmountForward = Mathf.SmoothDamp(tiltAmountForward, 20 * Input.GetAxis("Vertical"), ref tiltVelocityForward, 0);
        }
    }

    void MoveLeftRight()
    {
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
        {
            Vector3 sideForce = Vector3.right * Input.GetAxis("Horizontal") * sideMovementSpeed;
            rbDrone.AddRelativeForce(sideForce.x, 0, sideForce.z, ForceMode.Force);
            
            //rbDrone.AddRelativeForce(Vector3.right * Input.GetAxis("Horizontal") * sideMovementSpeed);

            //rbDrone.MovePosition(transform.position + Vector3.right * Input.GetAxis("Horizontal") * Time.deltaTime * sideMovementSpeed);
            //tiltAmountSide = Mathf.SmoothDamp(tiltAmountSide, -20 * Input.GetAxis("Horizontal"), ref tiltVelocitySide, 0.1f);
        }
        else
        {
            //tiltAmountSide = Mathf.SmoothDamp(tiltAmountSide, 0, ref tiltVelocitySide, 0.1f);
        }
    }

    void MoveRotation()
    {
        if (Input.GetKey(KeyCode.J))
        {
            wantRotationY -= rotateAmountByKeys;
        }
        if (Input.GetKey(KeyCode.L))
        {
            wantRotationY += rotateAmountByKeys;
        }

        //currRotationY = Mathf.SmoothDampAngle(currRotationY, cameraT.eulerAngles.y, ref rotationVelocityY, 0.2f);
        currRotationY = Mathf.SmoothDampAngle(currRotationY, cameraT.eulerAngles.y, ref rotationVelocityY, rotateSpeed);
    }

    void MovementSmoothingClamp()
    {
        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
        {
            rbDrone.velocity = Vector3.ClampMagnitude(rbDrone.velocity, Mathf.Lerp(rbDrone.velocity.magnitude, 10.0f, Time.deltaTime * 5f));
        }
        if (Mathf.Abs(Input.GetAxis("Vertical")) > 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.2f)
        {
            rbDrone.velocity = Vector3.ClampMagnitude(rbDrone.velocity, Mathf.Lerp(rbDrone.velocity.magnitude, 10.0f, Time.deltaTime * 5f));
        }
        if (Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) > 0.2f)
        {
            rbDrone.velocity = Vector3.ClampMagnitude(rbDrone.velocity, Mathf.Lerp(rbDrone.velocity.magnitude, 5.0f, Time.deltaTime * 5f));
        }
        if (Mathf.Abs(Input.GetAxis("Vertical")) < 0.2f && Mathf.Abs(Input.GetAxis("Horizontal")) < 0.2f)
        {
            rbDrone.velocity = Vector3.SmoothDamp(rbDrone.velocity, Vector3.zero, ref smoothDampVelocity, 0.95f);
        }
    }
}
