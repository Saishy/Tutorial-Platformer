using UnityEngine;

public class EnemyState {
	protected EnemyController _controller;

	public virtual AIStateNames StateName {
		get {
			return AIStateNames.NullStateID;
		}
	}

	public EnemyState(EnemyController caller) {
		_controller = caller;
	}

	public virtual void OnBegin(AIStateNames previous) {

	}

	public virtual void OnEnd(AIStateNames next) {

	}

	public virtual void MainUpdate() {

	}
}

public class EnemyStateMoving : EnemyState {

	public override AIStateNames StateName => AIStateNames.Moving;


	public EnemyStateMoving(EnemyController caller) : base(caller) {
	}

	public override void MainUpdate() {
		base.MainUpdate();
		Vector2 movementInput = GetInputs();

		if (_controller.character != null) {
			_controller.character.ReceiveInput(movementInput);
		}

		Character target = _controller.Enemy.AcquireTarget();
		if (target != null) {
			_controller.GoToState(AIStateNames.Chasing);
		}
	}

	protected Vector2 GetInputs() {
		if (_controller.Enemy.CheckIfNeedChangeDirection()) {
			_controller.character.ChangeLookDirection(!_controller.character.IsLookingRight());
		}

		return _controller.character.IsLookingRight() ? Vector2.right : Vector2.left;
	}


	public override void OnBegin(AIStateNames previous) {
		base.OnBegin(previous);
	}

	public override void OnEnd(AIStateNames next) {
		base.OnEnd(next);
	}
}

public class EnemyStateChasing : EnemyState {

	public override AIStateNames StateName => AIStateNames.Chasing;
	Vector2 vectorToTarget;


	public EnemyStateChasing(EnemyController caller) : base(caller) {
	}

	public override void MainUpdate() {
		base.MainUpdate();
		Character target = _controller.Enemy.AcquireTarget();
		if (target == null) {
			_controller.GoToState(AIStateNames.Moving);
			return;
		}

		vectorToTarget = (target.GetCenterPosition() - _controller.character.GetCenterPosition());
		Vector2 movementInput = GetInputs();

		if (_controller.character.AttackManager.ChooseAttackInRange(vectorToTarget.magnitude) >= 0) {
			_controller.GoToState(AIStateNames.Attacking);
			return;
		}

		if (_controller.character != null) {
			_controller.character.ReceiveInput(movementInput);
		}


	}

	protected Vector2 GetInputs() {
		if (vectorToTarget.x < 0) {
			_controller.character.ChangeLookDirection(false);
		} else {
			_controller.character.ChangeLookDirection(true);
		}

		return _controller.character.IsLookingRight() ? Vector2.right : Vector2.left;
	}


	public override void OnBegin(AIStateNames previous) {
		base.OnBegin(previous);
	}

	public override void OnEnd(AIStateNames next) {
		base.OnEnd(next);
	}
}


public class EnemyStateAttacking : EnemyState {

	public override AIStateNames StateName => AIStateNames.Attacking;

	public EnemyStateAttacking(EnemyController caller) : base(caller) {
	}

	public override void MainUpdate() {
		base.MainUpdate();
		
		Character target = _controller.Enemy.AcquireTarget();
		if (target == null) {
			if (!_controller.character.AttackManager.IsAttacking()) {
				_controller.GoToState(AIStateNames.Moving);
			}
			return;
		}

		Vector2 vectorToTarget = (target.GetCenterPosition() - _controller.character.GetCenterPosition());
		if(vectorToTarget.x < 0) {
			_controller.character.ChangeLookDirection(false);
		} else {
			_controller.character.ChangeLookDirection(true);
		}

		int attackTemp = _controller.character.AttackManager.ChooseAttackInRange(vectorToTarget.magnitude);
		if (attackTemp >= 0) {
			_controller.character.Fire(attackTemp);
		}

	}

	public override void OnBegin(AIStateNames previous) {
		base.OnBegin(previous);
		Debug.Log("StartAttack");
		_controller.character.ReceiveInput(Vector2.zero);
	}

	public override void OnEnd(AIStateNames next) {
		base.OnEnd(next);
	}
}

