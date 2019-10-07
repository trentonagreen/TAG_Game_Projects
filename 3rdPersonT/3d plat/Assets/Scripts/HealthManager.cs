using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour {

	public int currentHealth;
	public int maxHealth;

	// Use this for initialization
	void Start () {
		currentHealth = maxHealth;
	}

	public void HurtPlayer(int damage) {
		currentHealth -= damage;
	}

	public void HealPlayer(int healAmount) {
		currentHealth += healAmount;

		if(currentHealth > maxHealth) {
			currentHealth = maxHealth;
		}
	}
}
