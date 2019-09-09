using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour {

	public PlayerController controller;
	public Transform iceEffect;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.GetKey(KeyCode.G)) {
            GameObject.Find("IceCollider").GetComponent<Collider>().enabled = true;
            IceShooter();
        }
        else {
            GameObject.Find("IceCollider").GetComponent<Collider>().enabled = false;
        }
	}

	void IceShooter() {
        Transform effect = Instantiate(iceEffect, transform.position, transform.rotation) as Transform;
        Destroy(effect.gameObject, .5f);
    }
}
