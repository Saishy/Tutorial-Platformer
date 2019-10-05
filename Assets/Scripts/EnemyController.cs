using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateNames {
	public static readonly AIStateNames NullStateID = new AIStateNames(0); // Use this ID to represent a non-existing State in your system
	/// <summary>This character is spawning.</summary>
	public static readonly AIStateNames Spawning = new AIStateNames(1);
	/// <summary>Nothing to do...</summary>
	public static readonly AIStateNames Idle = new AIStateNames(2);
	/// <summary>We walking to somewhere.</summary>
	public static readonly AIStateNames Moving = new AIStateNames(3);
	/// <summary>We are chasing someone.</summary>
	public static readonly AIStateNames Chasing = new AIStateNames(4);
	/// <summary>We are at combat range, but not attacking.</summary>
	public static readonly AIStateNames Combat = new AIStateNames(5);
	/// <summary>We are doing an attack!</summary>
	public static readonly AIStateNames Attacking = new AIStateNames(6);
	/// <summary>Ded.</summary>
	public static readonly AIStateNames Dead = new AIStateNames(7);
	public int Value { get; protected set; }
	protected AIStateNames(int internalValue) {
		this.Value = internalValue;
	}
}

public class EnemyController : Controller {

	public EnemyCharacter Enemy { get; private set; }

	private EnemyState _currentState;
	private List<EnemyState> _listOfStates = new List<EnemyState>(8);

	protected override void AwakeInit() {
		base.AwakeInit();

		Enemy = character as EnemyCharacter;
		AddStates();
	}

	protected override void StartInit() {
		base.StartInit();
		GoToState(AIStateNames.Moving);
	}

	void Update() {
		if(_currentState != null) {
			_currentState.MainUpdate();
		}
		//Debug.Log(_currentState.StateName.Value);
		//if (character != null) {
		//	character.ReceiveInput(movementInput);
		//	character.Jump(GetJump());

		//Character target = enemy.AcquireTarget();
		//if (target != null) {
		//	int attackTemp = character.AttackManager.ChooseAttackInRange((target.GetCenterPosition() - character.GetCenterPosition()).magnitude);
		//	if (attackTemp >= 0) {
		//		character.Fire(attackTemp);
		//	}
		//}
		//}
	}

	protected override Vector2 GetInputs() {
		return Vector2.zero;
	}

	protected override bool GetJump() {
		return false;
	}

	public void AddStates() {
		_listOfStates.Add(new EnemyStateMoving(this));
		_listOfStates.Add(new EnemyStateChasing(this));
		_listOfStates.Add(new EnemyStateAttacking(this));
	}

	public void GoToState(AIStateNames newStateName) {
		foreach (EnemyState state in _listOfStates) {
			if(state.StateName == newStateName) {
				if(_currentState != null) {
					_currentState.OnEnd(newStateName);
				}
				EnemyState previousState = _currentState;
				_currentState = state;
				_currentState.OnBegin(previousState != null ? previousState.StateName : AIStateNames.NullStateID);

				return;
			}
		}
		Debug.LogWarning("EnemyController::GoToState a state with the given name was not found.");
	}
}
