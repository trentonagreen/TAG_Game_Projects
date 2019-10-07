using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< HEAD
public class Enemy : MonoBehaviour
{
    public bool isPlayerInRange;
=======
/*
 *  Root motion animation is going in the opposite direction
 */

public class Enemy : MonoBehaviour
{
    public bool isPlayerInRange;
    public bool isAttacking;
>>>>>>> 23e11841298b94f49a567da5fc65e77ba45a0e4e

    public int moveSpeed;

    public GameObject playerTarget;

    Rigidbody rb;
    Animator anim;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
<<<<<<< HEAD
=======
        #region Movement and Rotation to chase player
>>>>>>> 23e11841298b94f49a567da5fc65e77ba45a0e4e
        Transform target = playerTarget.transform;
        Vector3 direction = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        Vector3 relpos = transform.position - target.position;
        relpos.y = 0;

<<<<<<< HEAD
        if(!isPlayerInRange)
=======
        if(!isPlayerInRange && !isAttacking)
>>>>>>> 23e11841298b94f49a567da5fc65e77ba45a0e4e
        {
            anim.SetBool("isChasing", true);
            rb.MovePosition(direction);
            rb.MoveRotation(Quaternion.LookRotation(relpos, Vector3.up));
        }
        else
        {
            anim.SetBool("isChasing", false);
        }
<<<<<<< HEAD
=======
        #endregion

        #region Attack Anims
        if(isPlayerInRange)
        {
            isAttacking = true;

            anim.SetTrigger("Attack1");
            anim.applyRootMotion = true;
        }
        else
        {
            isAttacking = false;
        }
        #endregion
>>>>>>> 23e11841298b94f49a567da5fc65e77ba45a0e4e
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Player entered enemy range");
            isPlayerInRange = true;
        }
    }

<<<<<<< HEAD
=======
    /*
>>>>>>> 23e11841298b94f49a567da5fc65e77ba45a0e4e
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Player is STILL in enemy range");
            isPlayerInRange = true;
        }
    }
<<<<<<< HEAD
=======
    */
>>>>>>> 23e11841298b94f49a567da5fc65e77ba45a0e4e

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Player has exited enemy range");
            isPlayerInRange = false;
        }
    }
}
