using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour {
	public List<Attack> attacks;

	public Character owner;

	public void Initialize(Character character) {
		owner = character;

		for (int i = 0; i < attacks.Count; i++) {
			attacks[i].Initialize(this);
		}
	}

	public void Tick() {
		foreach (Attack attack in attacks) {
			attack.Tick();
		}
	}

	public void Fire(int attackIndex) {
		attacks[attackIndex].Fire();
	}

	public int GetAvailableAttack() {
		for (int i = 0; i < attacks.Count; i++) {
			if (attacks[i] != null && attacks[i].CheckIfCanUse()) {
				return i;
			}
		}

		return -1;
	}

	public int ChooseAttackInRange(float distance) {
		for (int i = 0; i < attacks.Count; i++) {
			if (attacks[i] != null && attacks[i].range > distance && attacks[i].CheckIfCanUse()) {
				return i;
			}
		}

		return -1;
	}

	public bool IsAttacking() {
		foreach (Attack atk in attacks) {
			if (atk.IsUsing()) {
				return true;
			}
		}
		return false;
	}

}
