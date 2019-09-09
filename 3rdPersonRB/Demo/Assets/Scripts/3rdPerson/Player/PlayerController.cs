using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /* TODO
     *      - rolling on slopes
     *      - fix rolling attack to only be true after roll animation
     *          make similar to attack combo
     *      - improve attack combo anims, #2
     *      - wait to settrigger until attack is done
     *      
     *      - SEPERATE SCRIPT TO SMALLER ONES
     *          
     *  FINISHED
     *      - Basic Animations
     *          - Idle
     *          - Walk
     *          - Run
     *      - Hover Animations
     *      - Movement
     *      - Rotation
     *      - Camera
     *      - Strafe Camera
     *      - Strafe Rotation
     *      - Strafe Animations
     *      - Crouch
     *      - Crouch Animations
     *      - Fall Animation
     *      - Dash Mechanic 
     *      - Dash Animations
     *      - FIXED spinning after collision
     *      - Improved Slope Interaction
     *      - Added 1 Attack
     *      - Added Rolling
     *      - Jump Root Motion
     *      - Crouch Strafe Anims
     *      - Combo Attacks
     *      - Attack/Roll from strafe
     *      - Better Jump root anim
     *      - better slopes again
     *      - rolling attack
     */

    [Header("Speed Settings")]
    public float walk_speed;
    public float run_speed;
    [SerializeField]
    private float speed;

    [Header("Jump Settings")]
    public bool jumpEnable;
    public bool hasJumped;
    public bool isGrounded;
    public float jump_force;
    public ForceMode jump_force_type;

    [Header("Crouch Settings")]
    public bool crouchEnable;
    public bool isCrouching;
    public bool crouchToggle;
    public float crouch_speed;

    [Header("Hover Settings")]
    public bool hoverEnable;
    public float hover_force;
    public float clamp_hover;
    public ForceMode hover_force_type;

    [Header("Rotate Settings")]
    public float turn_speed;
    private Vector3 dir_pos;
    private Vector3 stor_dir;

    [Header("Animation Settings")]
    public Animator anim;
    public float InputMagn;

    [Header("Slope Settings")]
    [SerializeField]
    private bool onSlope;
    public float slope_force;
    public float slope_ray_leng;

    [Header("Strafe Settings")]
    [SerializeField]
    private bool isStrafe;

    [Header("Debug Settings")]
    public Transform ray_debugger;
    private Vector3 ray_start;

    [Header("Dash Settings")]
    public bool dashEnable;
    public bool canDash;
    public bool isDashing;
    public float dash_force;
    public float dash_cooldown;
    public float dash_timer;
    public float dash_duration;
    public float dash_start;
    public ForceMode dash_force_type;
    public GameObject dash_effect;

    [Header("Mesh Settings")]
    public SkinnedMeshRenderer surface;
    public SkinnedMeshRenderer joint;

    [Header("Attack Anims")]
    public bool attackEnable;
    public bool isAttacking;
    public AnimationClip attack;

    [Header("Rolling Anims")]
    public bool rollEnable;
    public bool isRolling;
    public AnimationClip roll;

    [Header("Rolling Attack")]
    public bool rollAttackEnable;
    public bool isRollAttacking;
    public AnimationClip rollAttack;

    [Header("Jumping Root Anim")]
    public bool jumpRootEnable;
    public bool isJumping;
    public AnimationClip jumpClip;

    //[Header("Debug Settings")]
    //public int r1Count;

    [Header("Attack Combo Settings")]
    public bool isComboAttacking;
    public bool attackComboEnable;
    public int attackCount;
    public bool canAttack;

    [Header("Better Slope Check Settings")]
    public LayerMask ground;
    public float maxGroundAngle = 120;
    public bool debug;
    public float groundAngle;
    RaycastHit hitSlopeInfo;
    public bool isGroundSlopeCheck;

    [Header("Lock on Target Settings")]
    public bool enableLockOn;
    public bool isLockedOnTarget;
    public bool lockOnToggle;
    public GameObject lockOnTarget;

    [Header("Improved Combo?")]
    public bool enableBetterCombo;
    public bool isBetterComboAttacking;
    public float attackRate;
    public string[] comboParams;
    public int comboIdx;
    public float resetTimer;
    
    Rigidbody rb;
    Transform cam;

    Collider m_Collider;
    Vector3 m_Size;

    Vector3 direction;
    Vector3 dir;

    private void Awake()
    {
        if(comboParams == null || (comboParams != null && comboParams.Length == 0))
        {
            comboParams = new string[] { "Attack1", "Attack2", "Attack3" };
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = Camera.main.transform;

        dash_effect.SetActive(false);

        m_Collider = GetComponent<Collider>();
        m_Size = m_Collider.bounds.size;
    }

    private void Update()
    {
        var hor = Input.GetAxis("Horizontal");
        var ver = Input.GetAxis("Vertical");

        #region Inputs

        #region Sprint
        if (Input.GetButton("PS4_X") && !jumpEnable && !jumpRootEnable && !hoverEnable)
        {
            speed = run_speed;
        }
        else if (hor == 0.0f && ver == 0.0f)
        {
            speed = 0;
        }
        else if(isCrouching)
        {
            speed = crouch_speed;
        }
        else
        {
            speed = walk_speed;
        }
        #endregion Sprint

        #region Strafe
        // can only strafe when locked on target
        if (Input.GetButtonDown("PS4_L1"))
        {
            Debug.Log("Pressing L1");
            isStrafe = true;
        }
        else
        {
            isStrafe = false;
        }
        #endregion Strafe

        #region Crouch

        if(Input.GetButtonDown("PS4_L3") && crouchEnable)
        {          
            crouchToggle = !crouchToggle;
        }

        if(crouchToggle)
        {
            isCrouching = true;
            anim.SetBool("isCrouching", isCrouching);
        }
        else
        {
            isCrouching = false;
            anim.SetBool("isCrouching", isCrouching);
        }

        #endregion Crouch

        #region Lock On
        if(enableLockOn && Input.GetButtonDown("PS4_R3"))
        {
            lockOnToggle = !lockOnToggle;
        }

        if (lockOnToggle)
        {
            isLockedOnTarget = true;
            isStrafe = true;
        }
        else if (!lockOnToggle && Input.GetButtonUp("PS4_L1"))
        {
            isLockedOnTarget = false;
            isStrafe = false;
        }

        #endregion Lock On

        #endregion

    }

    private void FixedUpdate()
    {

        var hor = Input.GetAxis("Horizontal");
        var ver = Input.GetAxis("Vertical");

        // stops the character from spinning after root motion animation
        if(hor == 0 && ver == 0)
        {
            rb.angularVelocity = Vector3.zero;
        }

        #region Slope Check
        onSlope = OnSlope();
        CalculateSlopeAngle();
        CheckSlope();
        DrawDebugLines();
        #endregion

        #region General Movement

        Vector3 forward = cam.forward;
        Vector3 right = cam.right;

        forward.y = 0f;
        right.y = 0f;

        direction = hor * right + ver * forward;
        //direction = hor * Vector3.right + ver * Vector3.forward;

        direction = direction * speed * Time.deltaTime;

        if(isAttacking || isRolling || isComboAttacking || isRollAttacking || groundAngle >= maxGroundAngle)
        {
            direction = Vector3.zero;
        }

        if(onSlope && !isStrafe)
        {
            direction = Vector3.Cross(hitSlopeInfo.normal, -transform.right);
            direction = direction * speed * Time.deltaTime;
        }

        if(onSlope && isStrafe)
        {
            Vector3 tmp = Vector3.Cross(hitSlopeInfo.normal, direction);
            direction = Vector3.Cross(tmp, hitSlopeInfo.normal);
        }

        rb.MovePosition(transform.position + direction);


        #endregion

        #region Rotation

        //Vector3 forward = cam.forward;
        //Vector3 right = cam.right;

        if(isStrafe && !isLockedOnTarget)
        {
            rb.MoveRotation(Quaternion.Euler(0, cam.eulerAngles.y, 0));
        }
        else if(isLockedOnTarget && isStrafe)
        {
            Transform target = lockOnTarget.transform;
            Vector3 relPos = target.position - transform.position;
            relPos.y = 0;

            rb.MoveRotation(Quaternion.LookRotation(relPos, Vector3.up));
        }
        else
        {
            dir_pos = transform.position + (right * hor) + (forward * ver);

            //Vector3 dir = dir_pos - transform.position;
            dir = dir_pos - transform.position;
            dir.y = 0;

            if (hor != 0 || ver != 0)
            {
                float angle = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(dir));

                if (angle != 0 && !isAttacking && !isRolling && !isComboAttacking)
                {
                    rb.MoveRotation(Quaternion.LookRotation(dir));
                }
            }
        }
        #endregion
       
        #region Inputs Physics Based

        #region Dash

        dash_timer += Time.deltaTime;

        if (dash_timer > dash_cooldown)
        {
            dash_timer = 0;
            canDash = true;
        }

        if (Input.GetButtonDown("PS4_Circle") && dashEnable && canDash == true)
        {
            forward = cam.forward;
            right = cam.right;

            isDashing = true;
            Vector3 dash_dir = hor * right + ver * forward;
            StartCoroutine(Dash(dash_dir, dash_force, dash_force_type));
            canDash = false;
            dash_timer = 0;
        }
        #endregion Dash

        #region Hover
        if ((Input.GetButton("PS4_X")) && hoverEnable == true)
        {
            rb.useGravity = false;
            Hover(hover_force, hover_force_type);

            anim.SetBool("isHover", true);
            anim.SetBool("isGrounded", false);
            isGrounded = false;
        }
        else
        {
            rb.useGravity = true;

            anim.SetBool("isHover", false);
        }
        #endregion Hover

        #region Jump
        if ((Input.GetButton("PS4_X")) && isGrounded == true && jumpEnable == true)
        {
            Jump(jump_force, jump_force_type);

            anim.SetBool("hasJumped", true);
            anim.SetBool("isGrounded", false);
            isGrounded = false;
        }
        else
        {
            anim.SetBool("hasJumped", false);
        }

        if(isGrounded)
        {
            anim.SetBool("isGrounded", true);
            anim.SetBool("hasJumped", false);
        }
        #endregion Jump

        #endregion

        #region Animations
        anim.SetFloat("InputX", hor, 0.0f, Time.deltaTime * 2f);
        anim.SetFloat("InputZ", ver, 0.0f, Time.deltaTime * 2f);

        #region Idle/Walk/Run anim
        InputMagn = new Vector2(hor, ver).sqrMagnitude;
        anim.SetFloat("InputMagnitude", InputMagn, 0.0f, Time.deltaTime);
        #endregion Idle/Walk/Run anim

        #region Strafe anim
        if (!isStrafe)
        {
            anim.SetBool("isStrafe", false);
        }
        else
        {
            anim.SetBool("isStrafe", true);
        }
        #endregion Strafe anim

        #region Falling anim

        if (rb.velocity.y < -10f)
        {
            anim.SetBool("isGrounded", false);
            isGrounded = false;
        }

        if (onSlope)
        {
            anim.SetBool("onSlope", true);
        }
        else
        {
            anim.SetBool("onSlope", false);
        }

        anim.SetFloat("JumpVelocity", rb.velocity.y);

        #endregion Jump and Falling anim

        #region Attack anims
        if (Input.GetButton("PS4_Square") && attackEnable)
        {
            isAttacking = true;
            anim.applyRootMotion = true;
            anim.SetBool("isAttacking", true);
            StartCoroutine(AttackAnim());
        }
        #endregion Attack anims

        #region Rolling anims
        if (rollEnable && Input.GetButton("PS4_Circle") && !isCrouching && !isStrafe && !isAttacking)
        {
            isRolling = true;
            anim.applyRootMotion = true;
            anim.SetBool("isRolling", true);
            StartCoroutine(RollingAnim());
        }
        #endregion Rolling anims

        #region Roll Attack
        if (rollAttackEnable && isRolling && Input.GetButtonDown("PS4_Square"))
        {
            isRollAttacking = true;
            anim.applyRootMotion = true;
            anim.SetBool("isSlideAttacking", true);
            StartCoroutine(RollAtackingAnim());
        }

        if(isRollAttacking)
        {
            anim.applyRootMotion = true;
        }
        #endregion Roll Attack

        #region Jump Anim
        if (jumpRootEnable && Input.GetButton("PS4_X"))
        {
            isJumping = true;
            anim.applyRootMotion = true;
            rb.useGravity = false;
            anim.SetBool("isJumpingRoot", true);
            StartCoroutine(JumpingAnim());
        }
        #endregion Jump Anim
     
        #region Attack Combo anims
        if (Input.GetButtonDown("PS4_Square") && attackComboEnable && !isCrouching && !isRolling)
        {
            ComboStart();
        }
        #endregion Attack Combo anims  

        #endregion

    }

    private void LateUpdate()
    {
        #region Better Attack Combo ?
        if (Input.GetButtonDown("PS4_Square") && enableBetterCombo && !isCrouching && !isRolling && comboIdx < comboParams.Length)
        {
            isBetterComboAttacking = true;
            Debug.Log(comboParams[comboIdx] + "trigger");

            anim.SetTrigger(comboParams[comboIdx]);       

            comboIdx++;

            resetTimer = 0f;

            if (isBetterComboAttacking)
            {
                anim.applyRootMotion = true;
            }
            else
            {
                anim.applyRootMotion = false;
            }
        }
        if (comboIdx > 0)
        {
            resetTimer += Time.deltaTime;
            if (resetTimer > attackRate)
            {
                anim.SetTrigger("ResetAttack");
                comboIdx = 0;

                isBetterComboAttacking = false;
            }

            if (isBetterComboAttacking)
            {
                anim.applyRootMotion = true;
            }
            else
            {
                anim.applyRootMotion = false;
            }
        }

        #endregion Better Attack Combo ?
    }

    #region Hover | Jump
    void Hover(float hover_force, ForceMode type)
    {
        rb.AddRelativeForce(transform.up * hover_force, type);

        // Clamps a max velocity to hover up
        rb.velocity = new Vector3(rb.velocity.x, Mathf.Clamp(rb.velocity.y, 0, clamp_hover), rb.velocity.z);
    }

    void Jump(float jump_force, ForceMode type)
    {
        rb.velocity = Vector3.zero;
        rb.AddRelativeForce(transform.up * jump_force, type);
    }

    #endregion

    #region IEnumerators
    IEnumerator Dash(Vector3 dash_dir, float dash_force, ForceMode type)
    {
        dash_dir = dash_dir.normalized;

        surface.enabled = false;
        joint.enabled = false;

        dash_effect.SetActive(true);

        rb.AddForce(dash_dir * dash_force, ForceMode.Impulse);
        yield return new WaitForSeconds(dash_duration);

        isDashing = false;

        surface.enabled = true;
        joint.enabled = true;

        rb.velocity = Vector3.zero;

        dash_effect.SetActive(false);
    }

    private IEnumerator AttackAnim()
    {
        yield return new WaitForSeconds(attack.length);

        isAttacking = false;
        anim.applyRootMotion = false;
        anim.SetBool("isAttacking", false);
    }

    private IEnumerator RollingAnim()
    {
        yield return new WaitForSeconds(roll.length);

        isRolling = false;
        anim.applyRootMotion = false;
        anim.SetBool("isRolling", false);
    }

    private IEnumerator JumpingAnim()
    {
        yield return new WaitForSeconds(jumpClip.length);

        isJumping = false;
        anim.applyRootMotion = false;
        rb.useGravity = true;
        anim.SetBool("isJumpingRoot", false);
    }

    private IEnumerator RollAtackingAnim()
    {
        yield return new WaitForSeconds(rollAttack.length);

        isRollAttacking = false;
        anim.applyRootMotion = false;
        anim.SetBool("isSlideAttacking", false);
    }
    #endregion

    #region Collision Functions

    private void OnCollisionEnter(Collision collision)
    {
        // stops rigidbody from spinning after collision
        if (collision.collider.tag == "Ground")
        {
            rb.angularVelocity = Vector3.zero;
            rb.velocity = Vector3.zero;
        }

    }

    private void OnCollisionStay(Collision collision)
    {
        if(isGrounded == false && collision.collider.tag == "Ground")
        {
            //anim.SetBool("isGrounded", true);
            rb.angularVelocity = Vector3.zero;
            rb.velocity = Vector3.zero;
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.collider.tag == "Ground")
        {
            isGrounded = false;
        }
    }

    #endregion

    #region Attack Combo Functions
    public void ComboStart()
    {
        if(canAttack && attackCount < 3)
        {           
            attackCount++;
            Debug.Log("Attack count increment: " + attackCount);
        }
        else
        {
            attackCount = 0;
        }

        if(attackCount == 1)
        {
            anim.SetInteger("AttackCount", 1);

            anim.applyRootMotion = true;
            isComboAttacking = true;
        }
    }

    public void ComboCheck()
    {
        canAttack = false;

        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack Combo 1") && attackCount == 1)
        {
            attackCount = 0;
            anim.SetInteger("AttackCount", 0);

            canAttack = true;            
            anim.applyRootMotion = false;
            isComboAttacking = false;
        }
        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack Combo 1") && attackCount >= 2)
        {
            anim.SetInteger("AttackCount", 2);
            canAttack = true;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack Combo 2") && attackCount == 2)
        {
            attackCount = 0;
            anim.SetInteger("AttackCount", 0);
            canAttack = true;          
            anim.applyRootMotion = false;
            isComboAttacking = false;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack Combo 2") && attackCount >= 3)
        {
            anim.SetInteger("AttackCount", 3);
            canAttack = true;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack Combo 3"))
        {
            attackCount = 0;
            anim.SetInteger("AttackCount", 0);
            canAttack = true;            
            anim.applyRootMotion = false;
            isComboAttacking = false;
        }
        else
        {
            attackCount = 0;
            anim.SetInteger("AttackCount", 0);
            canAttack = true;           
            anim.applyRootMotion = false;
            isComboAttacking = false;
        }
    }
    #endregion Attack Combo Functions

    #region Better Slope Check

    bool OnSlope()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, m_Size.y))
        {
            if (hit.normal != Vector3.up)
            {
                return true;
            }
        }

        return false;
    }

    void CalculateForwardRay()
    {
        if (!onSlope)
        {
            direction = transform.forward;
        }
        else {
            direction = Vector3.Cross(hitSlopeInfo.normal, -transform.right);
        }
           
    }

    void CalculateSlopeAngle()
    {
        if(onSlope)
        {
            groundAngle = Vector3.Angle(hitSlopeInfo.normal, transform.forward);
        }
            
    }

    void CheckSlope()
    {
        if(Physics.Raycast(transform.position + Vector3.up, -Vector3.up * 2, out hitSlopeInfo, m_Size.y, ground))
        {
            isGroundSlopeCheck = true;
        }
        else
        {
            isGroundSlopeCheck = false;
        }
    }

    void DrawDebugLines()
    {
        if(debug)
        {
            Debug.DrawLine(transform.position, transform.position + direction * m_Size.y * 10, Color.blue);
            Debug.DrawLine(transform.position + Vector3.up, transform.position - Vector3.up * m_Size.y, Color.green);
        }
    }

    #endregion
}
