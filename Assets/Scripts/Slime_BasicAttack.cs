using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_BasicAttack : Attack {

	protected override void FireStart() {
		base.FireStart();
		
		attackManager.owner.SetAnimationTrigger("Attack");

		attackManager.owner.CanMove += ReturnFalse;

		StartCoroutine(DoAttack());
	}

	protected override void FireEnd() {
		base.FireEnd();

		attackManager.owner.CanMove -= ReturnFalse;
	}

	IEnumerator DoAttack() {
		yield return new WaitForSeconds(0.3102f);

		Debug.Log("Take This!");
	}
}
