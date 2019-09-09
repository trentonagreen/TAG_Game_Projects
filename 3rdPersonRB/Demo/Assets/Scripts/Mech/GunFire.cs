using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFire : MonoBehaviour
{
    Rigidbody rbDrone;

    public Animator animator;
    public bool isR2Down = false;
    public bool isR2Up = true;
    public float lFireRate = 5f;
    public float rFireRate = 2f;
    public float lTimer = 0.0f;
    public float rTimer = 0.0f;
    public bool canLShoot;
    public bool canRShoot;

    public GameObject _ammoPrefabSmall;
    public Transform _ammoSpawnPointSmall;
    public float firePower = 7500f;

    public GameObject _ammoPrefabBig;
    public Transform _ammoSpawnPointBig;

    private void Awake()
    {
        rbDrone = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        animator.SetBool("isRShooting", false);
        animator.SetBool("isLShooting", false);
    }

    private void Update()
    {
        EnableButtonForR2();

        /*
            Controls fire rate 
        */
        lTimer += Time.deltaTime;
        rTimer += Time.deltaTime;

        if ((Input.GetKey(KeyCode.Space) || Input.GetButton("PS4_R1")) && rTimer > rFireRate)
        {
            animator.SetBool("isRShooting", true);

            Fire();
            rTimer = 0;
        }
        if (Input.GetButtonUp("PS4_R1"))
        {
            animator.SetBool("isRShooting", false);
            canRShoot = false;
        }

        if (!isR2Down)
        {
            animator.SetBool("isLShooting", false);
        }

        //animator.SetBool("isRShooting", false);

        //  ENABLE FOR FAST BULLET SHOOT

        if (isR2Down && lTimer > lFireRate)
        {
            animator.SetBool("isLShooting", true);

            FireBig();
            lTimer = 0;
        }
    }

    public void Fire()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("ArmRShootIdle"))
        {
            //if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
            //{
            GameObject ammo = Instantiate(_ammoPrefabSmall, _ammoSpawnPointSmall.transform.position, Quaternion.identity) as GameObject;
            Rigidbody ammoRB = ammo.GetComponent<Rigidbody>();
            ammoRB.AddForce(transform.forward * firePower);
            //}
        }

        //Destroy(ammo, 2f);
    }

    public void FireBig()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("ArmLShootIdle"))
        {
            GameObject ammo = Instantiate(_ammoPrefabBig, _ammoSpawnPointBig.transform.position, Quaternion.identity) as GameObject;
            Rigidbody ammoRB = ammo.GetComponent<Rigidbody>();
            ammoRB.AddForce(transform.forward * firePower);
        }
    }

    void EnableButtonForR2()
    {
        float foo = Input.GetAxisRaw("PS4_R2");
        if (foo < 0)
        {
            if (!isR2Up)
            {
                isR2Up = true;
            }
            if (isR2Down)
            {
                isR2Down = false;
            }
        }
        else if (foo > 0)
        {
            if (isR2Up)
            {
                isR2Up = false;
            }
            if (!isR2Down)
            {
                // FireBig() for buttondown effect, remove for button
                //animator.SetBool("isLShooting", true);

                //FireBig();
                isR2Down = true;
            }
        }
        else if (foo == 0)
        {
            if (isR2Up)
            {
                isR2Down = false;
            }
            if (isR2Down)
            {
                isR2Up = false;
            }
        }
    }
}
