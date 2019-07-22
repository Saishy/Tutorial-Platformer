using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

	public AttackManager attackManager;

	[SerializeField]
	protected float cooldown;
	[SerializeField]
	protected float duration;
	[SerializeField]
	protected bool bCooldownOnFinish;

	[Header("AI")]
	public float range;

	private float _lastTimeUsed;
	protected bool bUsing;

	public Collider2D _col2D;

	private float usingTimer;

	public void Initialize(AttackManager attackManager) {
		this.attackManager = attackManager;
	}

	public virtual void Tick() {
		if (bUsing) {
			usingTimer -= Time.deltaTime;

			if (usingTimer <= 0) {
				FireEnd();
				if (bCooldownOnFinish) {
					StartCooldown();
				}
			}
		}
	}

	/// <summary>Checks if out of cooldown.</summary>
	/// <returns></returns>
	public virtual bool CheckIfOutOfCooldown() {
		return (Time.timeSinceLevelLoad - _lastTimeUsed) >= cooldown;
	}

	public virtual bool CheckIfCanUse() {
		return !bUsing && CheckIfOutOfCooldown();
	}

	void StartCooldown() {
		_lastTimeUsed = Time.timeSinceLevelLoad;
	}

	public virtual void Fire() {
		if (!CheckIfCanUse()) {
			return;
		}

		FireStart();
		if (!bCooldownOnFinish) {
			StartCooldown();
		}
	}

	protected virtual void FireStart() {
		bUsing = true;
		usingTimer = duration;
	}

	protected virtual void FireEnd() {
		bUsing = false;
	}

	protected virtual bool ReturnFalse() {
		return false;
	}
}
