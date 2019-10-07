using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isPlayerInRange;

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
        Transform target = playerTarget.transform;
        Vector3 direction = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        Vector3 relpos = transform.position - target.position;
        relpos.y = 0;

        if(!isPlayerInRange)
        {
            anim.SetBool("isChasing", true);
            rb.MovePosition(direction);
            rb.MoveRotation(Quaternion.LookRotation(relpos, Vector3.up));
        }
        else
        {
            anim.SetBool("isChasing", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Player entered enemy range");
            isPlayerInRange = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Player is STILL in enemy range");
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Player has exited enemy range");
            isPlayerInRange = false;
        }
    }
}
