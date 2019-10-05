using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hornet_BasicAttack : Attack
{
	[SerializeField]
	GameObject prefabSpike;

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
		yield return new WaitForSeconds(0.2f);

		GameObject spike = Instantiate(prefabSpike, transform.position, Quaternion.identity);
		Projectile spikeScript = spike.GetComponent<Projectile>();
		spikeScript.Direction = attackManager.owner.IsLookingRight() ? Vector2.right : Vector2.left;
		spikeScript.Speed = 4f;
		spikeScript.Duration = 3f;
		spikeScript.Damage = damage;
		spikeScript.Shoot(attackManager.owner);
	}

}
