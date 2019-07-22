using UnityEngine;

public abstract class Controller : MonoBehaviour {

	public Character character;

	public int teamID = -1;

	private void Awake() {
		AwakeInit();
	}

	protected virtual void AwakeInit() {
		character = GetComponent<Character>();
	}

	void Start() {
		character.Possessed(this);
	}

	void Update() {
		Vector2 movementInput = GetInputs();

		if (character != null) {
			character.ReceiveInput(movementInput);
			character.Jump(GetJump());

		}
	}

	protected abstract Vector2 GetInputs();

	protected abstract bool GetJump();
}
