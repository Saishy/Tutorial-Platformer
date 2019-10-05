using UnityEngine;


public class EnemyCharacter : Character {

	[Header("AI")]
	public float seeDistance;

	protected static Collider2D[] cacheTargets = new Collider2D[50];

	protected override void StartInit() {
		base.StartInit();
		_spriteRenderer.flipX = true;
	}

	protected override void Animate() {

	}

	public override void ReceiveInput(Vector2 input) {
		base.ReceiveInput(input);

		if (input.x > 0) {
			ChangeLookDirection(true);
		} else if (input.x < 0) {
			ChangeLookDirection(false);
		}
	}

	public bool CheckIfNeedChangeDirection() {

		if (Physics2D.Raycast(_movementComponent.CenterCollider, _movementComponent.MovementInput, _movementComponent.SizeCollider.x + (1f / 16f), 1 << 9)) {
			return true;
		} else if (!Physics2D.Raycast(_movementComponent.CenterCollider + ((_movementComponent.SizeCollider.x + (1f / 16f)) * _movementComponent.MovementInput), Vector2.down, _movementComponent.SizeCollider.y + (1f / 16f), 1 << 9)) {
			return true;
		}
		return false;
	}

	public Character AcquireTarget() {
		int hits = Physics2D.OverlapBoxNonAlloc(_movementComponent.CenterCollider, new Vector2(seeDistance*2, _movementComponent.SizeCollider.y), 0f, cacheTargets, 1 << 8 | 1 << 10);

		for (int i = 0; i < hits; i++) {
			Character hitCharacter = cacheTargets[i].gameObject.GetComponent<Character>();
			if (hitCharacter.TeamID != -1 && hitCharacter.TeamID != TeamID) {
				return hitCharacter;
			}
		}
		return null;
	}

	protected override void Death() {
		Destroy(gameObject);
	}
}
