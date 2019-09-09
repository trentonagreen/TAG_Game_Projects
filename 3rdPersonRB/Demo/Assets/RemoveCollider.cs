using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveCollider : MonoBehaviour
{
    public Enemy enemy;

    Collider coll;

    private void Start()
    {
        coll = GetComponent<Collider>();
    }

    /*
    void Update()
    {
        if(enemy.isPlayerInRange)
        {
            coll.enabled = false;
        }
        else
        {
            coll.enabled = true;
        }
    }
    */
}
