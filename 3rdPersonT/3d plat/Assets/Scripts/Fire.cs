using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {

	public PlayerController controller;
	public Transform fireEffect;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Input.GetKey(KeyCode.F)) {
            GameObject.Find("FireCollider").GetComponent<Collider>().enabled = true;
            FireShooter();
        }
        else {
            GameObject.Find("FireCollider").GetComponent<Collider>().enabled = false;
        }
	}

	void FireShooter() {
        Transform effect = Instantiate(fireEffect, transform.position, transform.rotation) as Transform;
        Destroy(effect.gameObject, .5f);
    }
}
