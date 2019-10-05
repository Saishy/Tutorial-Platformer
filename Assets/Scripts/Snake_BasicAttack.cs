using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake_BasicAttack : Attack
{
	protected static Collider2D[] colResults = new Collider2D[50];

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

		int count = _col2D.OverlapCollider(new ContactFilter2D(), colResults);

		for (int i = 0; i < count; i++) {
			ITargetable target = colResults[i].GetComponent<ITargetable>();
			if (target.CanBeTargetOf(attackManager.owner)) {
				target.TakeDamage(damage, attackManager.owner);
			}
		}
	}

}
