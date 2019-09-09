using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Air : MonoBehaviour {

	// Use this for initialization
	public PlayerController controller;
	public Transform airEffect;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.GetKey(KeyCode.H)) {
            GameObject.Find("AirCollider").GetComponent<Collider>().enabled = true;
            AirShooter();
        }
        else {
            GameObject.Find("AirCollider").GetComponent<Collider>().enabled = false;
        }
	}

	void AirShooter() {
        Transform effect = Instantiate(airEffect, transform.position, transform.rotation) as Transform;
        Destroy(effect.gameObject, .5f);
    }
}
