using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Basic Movement Settings")]
    public float walkSpeed;
    public float runSpeed;
    [SerializeField]
    private float speed;

    [Header("Rotate Settings")]
    private Vector3 dirPos;
    private Vector3 storPos;
    private Vector3 rotDir;

    [Header("Slope Settings")]
    public bool isOnSlope;
    public float groundAngle;
    public LayerMask groundLM;
    RaycastHit hitSlopeInfo;

    Rigidbody rb;
    Transform cam;
    Vector3 direction;

    Collider mCollider;
    Vector3 mSize;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main.transform;

        mCollider = GetComponent<Collider>();
        mSize = mCollider.bounds.size;
    }

    private void Update()
    {
        var hor = Input.GetAxis("Horizontal");
        var ver = Input.GetAxis("Vertical");

        #region Run/Sprint
        if (Input.GetButton("PS4_Circle"))
        {
            speed = runSpeed;
        }
        else if(hor == 0.0f && ver == 0.0f)
        {
            speed = 0;
        }
        else
        {
            speed = walkSpeed;
        }
        #endregion
    }

    private void FixedUpdate()
    {
        var hor = Input.GetAxis("Horizontal");
        var ver = Input.GetAxis("Vertical");

        // stops character from spinning from root motion animation
        if (hor == 0 && ver == 0)
        {
            rb.angularVelocity = Vector3.zero;
        }

        #region Slope Check
        isOnSlope = OnSlope();
        CalcSlopeAngle();
        CalculateForwardRay();
        CheckSlope();
        DrawSlopeDebugLines();
        #endregion

        #region Walking Movement
        Vector3 forward = cam.forward;
        Vector3 right = cam.right;

        forward.y = 0f;
        right.y = 0f;

        direction = hor * right + ver * forward;
        direction = direction.normalized * speed * Time.deltaTime;

        if (isOnSlope)
        {
            Vector3 tmp = Vector3.Cross(hitSlopeInfo.normal, direction);
            direction = Vector3.Cross(tmp, hitSlopeInfo.normal);
        }
        
        rb.MovePosition(transform.position + direction);
        #endregion

        #region Rotation
        dirPos = transform.position + (hor * right) + (forward * ver);
        rotDir = dirPos - transform.position;
        rotDir.y = 0;

        if(hor != 0 || ver!= 0)
        {
            float angle = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(rotDir));

            if(angle != 0)
            {
                rb.MoveRotation(Quaternion.LookRotation(rotDir));
            }
        }

        #endregion
   
    }

    #region Slope Check Functions

    // returns whether or not player is on a slope or flat ground
    bool OnSlope()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, mSize.y))
        {
            if(hit.normal != Vector3.up)
            {
                return true;
            }
        }

        return false;
    }

    void CalculateForwardRay()
    {
        if (!isOnSlope)
        {
            direction = transform.forward;
        }
        else
        {
            direction = Vector3.Cross(hitSlopeInfo.normal, -transform.right);
        }
    }

    void CalcSlopeAngle()
    {
        if(isOnSlope)
        {
            groundAngle = Vector3.Angle(hitSlopeInfo.normal, transform.forward);
        }
    }

    void CheckSlope()
    {
        Physics.Raycast(transform.position + Vector3.up, -Vector3.up * 2, out hitSlopeInfo, mSize.y, groundLM);
    }

    void DrawSlopeDebugLines()
    {
        Debug.DrawLine(transform.position, transform.position + direction * mSize.y, Color.blue);
        Debug.DrawLine(transform.position + Vector3.up, transform.position - Vector3.up * mSize.y, Color.green);
    }
    #endregion
}
