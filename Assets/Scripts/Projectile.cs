using UnityEngine;

public class Projectile : MonoBehaviour {
	Rigidbody2D _rb;
	SpriteRenderer _spriteRenderer;

	public Vector2 Direction { get; set; }
	public float Speed { get; set; }
	public float Aceleration { get; set; }
	public float MaxSpeed { get; set; }
	public float Duration { get; set; }

	public Character Owner { get; protected set; }

	public virtual float Damage { get; set; }

	protected bool bFired;

	void Awake() {
		_rb = GetComponent<Rigidbody2D>();
		_spriteRenderer = GetComponent<SpriteRenderer>();
	}

	void Update() {
		if (!bFired) {
			return;
		}

		if (Aceleration != 0) {
			Speed += Aceleration * Time.deltaTime;
			Speed = Mathf.Clamp(Speed, -MaxSpeed, MaxSpeed);
		}

		Duration -= Time.deltaTime;
		if (Duration <= 0) {
			UnSpawn();
		}
	}

	public void Shoot(Character owner) {
		Owner = owner;

		_spriteRenderer.flipX = Direction.x > 0;

		_rb.velocity = Direction * Speed;

		bFired = true;
	}

	void OnContact(ITargetable target) {
		if (!target.CanBeTargetOf(Owner)) {
			return;
		}

		Explode(target);
	}

	void Explode(ITargetable target) {
		target.TakeDamage(Damage, Owner);
		UnSpawn();
	}

	void Spawn() {

	}

	void UnSpawn() {
		Destroy(gameObject);
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (!bFired) {
			return;
		}

		ITargetable target = collision.gameObject.GetComponent<ITargetable>();

		if (target != null) {
			OnContact(target);
		}
	}
}
