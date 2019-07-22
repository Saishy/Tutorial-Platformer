using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(MovementComponent))]
[RequireComponent(typeof(AttackManager))]
[RequireComponent(typeof(SpriteRenderer))]

public abstract class Character : MonoBehaviour {

	protected SpriteRenderer _spriteRenderer;
	protected MovementComponent _movementComponent;
	protected Rigidbody2D _rb;
	protected AttackManager _attackManager;
	protected Animator _animator;
	protected Controller _controller;

	public SmartEvent<System.Func<bool>> CanMove = new SmartEvent<System.Func<bool>>();
	public SmartEvent<System.Func<bool>> CanJump = new SmartEvent<System.Func<bool>>();

	[SerializeField]
	protected float speed;
	[SerializeField]
	protected float jumpSpeed;

	public AttackManager AttackManager {
		get {
			return _attackManager;
		}
	}

	public int TeamID {
		get {
			if (_controller == null) {
				return -1;
			}

			return _controller.teamID;
		}
	}

	void Awake() {
		AwakeInit();
	}

	protected virtual void AwakeInit() {
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_rb = GetComponent<Rigidbody2D>();
		_movementComponent = GetComponent<MovementComponent>();
		_attackManager = GetComponent<AttackManager>();
		_animator = GetComponent<Animator>();
	}

	private void Start() {
		StartInit();
	}

	protected virtual void StartInit() {
		_movementComponent.MovementSpeed = speed;
		_movementComponent.JumpSpeed = jumpSpeed;

		_attackManager.Initialize(this);
	}

	private void Update() {
		Animate();
		_attackManager.Tick();
	}

	protected abstract void Animate();

	public virtual void SetAnimationTrigger(string animName) {
		_animator.SetTrigger(animName);
	}
	public virtual void SetAnimationBool(string animName, bool animBool) {
		_animator.SetBool(animName, animBool);
	}
	public virtual void SetAnimationInt(string animName, int animInt) {
		_animator.SetInteger(animName, animInt);
	}

	public Vector2 GetCenterPosition() {
		return _movementComponent.CenterCollider;
	}

	public void Possessed(Controller controller) {
		_controller = controller;
	}

	public void UnPossessed(Controller controller) {
		if (_controller == controller) {
			_controller = null;
		}
	}

	public virtual void ReceiveInput(Vector2 input) {
		foreach (System.Func<bool> action in CanMove.Actions) {
			if (!action()) {
				input = Vector2.zero;
			}
		}

		_movementComponent.MovementInput = input;
	}

	public virtual void Jump(bool bJumping) {
		foreach (System.Func<bool> action in CanJump.Actions) {
			if (!action()) {
				bJumping = false;
			}
		}

		_movementComponent.JumpInput = bJumping;
	}

	public virtual void Fire(int attackIndex) {
		_attackManager.Fire(attackIndex);
	}



}
