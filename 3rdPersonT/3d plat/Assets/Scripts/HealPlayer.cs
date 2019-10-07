using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPlayer : MonoBehaviour {

	public int amountToHeal = 1;

	private void OnTriggerEnter(Collider other) {
		if(other.gameObject.CompareTag("Player")) {
			FindObjectOfType<HealthManager>().HealPlayer(amountToHeal);
		}
	}
}
