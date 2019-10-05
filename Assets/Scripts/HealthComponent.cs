using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
	public float MaxHealth;

	public float CurrentHealth { get; protected set; }

	public SmartEvent<System.Action> OnDeath = new SmartEvent<System.Action>();
	public SmartEvent<System.Action> OnDamage = new SmartEvent<System.Action>();
	public SmartEvent<System.Action> OnHealing = new SmartEvent<System.Action>();

	private void Awake() {
		CurrentHealth = MaxHealth;
	}

	public void TakeDamage(float damage) {
		CurrentHealth -= damage;
		foreach (System.Action action in OnDamage.Actions) {
			action();
		}
		if (CurrentHealth <= 0) {
			Death();
		}
	}

	void Death() {
		foreach (System.Action action in OnDeath.Actions) {
			action();
		}
	}

	public void ReceiveHealing(float heal) {
		CurrentHealth += heal;
		if (CurrentHealth > MaxHealth) {
			CurrentHealth = MaxHealth;
		}
		foreach (System.Action action in OnHealing.Actions) {
			action();
		}
	}
}
