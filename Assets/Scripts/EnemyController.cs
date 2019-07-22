using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Controller {

	bool bGoingRight = true;
	Enemy enemy;

	protected override void AwakeInit() {
		base.AwakeInit();

		enemy = character as Enemy;
	}

	void Update() {
		Vector2 movementInput = GetInputs();

		if (character != null) {
			character.ReceiveInput(movementInput);
			character.Jump(GetJump());

			Character target = enemy.AcquireTarget();
			if (target != null) {
				int attackTemp = character.AttackManager.ChooseAttackInRange((target.GetCenterPosition() - character.GetCenterPosition()).magnitude);
				if (attackTemp >= 0) {
					character.Fire(attackTemp);
				}
			}
		}
	}

	protected override Vector2 GetInputs() {
		if (enemy.CheckIfNeedChangeDirection()) {
			bGoingRight = !bGoingRight;
		}

		return bGoingRight ? Vector2.right : Vector2.left;
	}

	protected override bool GetJump() {
		return false;
	}
}
