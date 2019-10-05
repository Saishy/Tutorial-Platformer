
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Character {

	ParticleSystem _particleSystem;

	

	float currentGravity;

	protected override void AwakeInit() {
		base.AwakeInit();
		_particleSystem = GetComponent<ParticleSystem>();
	}

	protected override void StartInit() {
		base.StartInit();
		_movementComponent.JumpEvent += Jumped;
		_movementComponent.LandEvent += Landed;
    }

	protected override void Animate()
    {
		//movementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetButton("Jump") ? 1f : 0f);
		//_movementComponent.MovementInput = movementInput;

		_animator.SetBool("bGrounded", _movementComponent.IsGrounded);
		_animator.SetInteger("SpeedX", (int)_movementComponent.MovementInput.x);

		if (_movementComponent.CurrentVelocity.x != 0) {
			if (_movementComponent.CurrentVelocity.x < 0) {
				ChangeLookDirection(true);
			} else {
				ChangeLookDirection(false);
			}
		}
	}

	void Jumped() {
		_particleSystem.Play();

	}

	void Landed() {
		_particleSystem.Play();

	}

	protected override void Death() {
		Destroy(gameObject);
	}
}
