using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementComponent : MonoBehaviour
{
	public float MovementSpeed { get; set; }
	public float JumpSpeed { get; set; }
	public Vector2 MovementInput { get; set; }
	public bool JumpInput { get; set; }
	public bool BLookingRight { get; set; }

	private float _currentGravity;
	private bool _previousFrameGrounded;
	private bool _bJumping;

	private Vector2 _sizeCollider;
	private Rigidbody2D _rb;
	private Collider2D _collider;

	public bool IsGrounded { get; private set; }
	public Vector2 CurrentVelocity { get; private set; }
	public Vector2 SizeCollider { get { return _sizeCollider; } }
	public Vector2 CenterCollider { get { return _collider.bounds.center; } }

	public event System.Action JumpEvent;
	public event System.Action LandEvent;

	void Awake()
    {
		_rb = GetComponent<Rigidbody2D>();
		_collider = GetComponent<Collider2D>();

		Vector2 offset = _collider.offset;
		_sizeCollider = _collider.bounds.extents;
		_sizeCollider = new Vector2(_sizeCollider.x - offset.x, _sizeCollider.y - offset.y);
	}

	private void FixedUpdate() {
		CheckGround();

		_currentGravity += Physics2D.gravity.y * Time.deltaTime;

		if ((_previousFrameGrounded || IsGrounded) && !_bJumping) {
			_currentGravity = (JumpInput? 1f:0f) * JumpSpeed;

			if (JumpInput) {
				_bJumping = true;
				Jumped();
			}
		}

		if (!_previousFrameGrounded && IsGrounded) {
			Landed();
		}

		_rb.velocity = new Vector2(MovementInput.x * MovementSpeed, _currentGravity);
		CurrentVelocity = _rb.velocity;

		_previousFrameGrounded = IsGrounded;
	}

	protected virtual void CheckGround() {
		IsGrounded = false;

		RaycastHit2D rcastHit2D = Physics2D.Raycast(new Vector2(transform.position.x + _sizeCollider.x - (1f / 16f), transform.position.y), Vector2.down, _sizeCollider.y + (1f / 16f), 1 << 9);

		if (rcastHit2D) {
			IsGrounded = true;
			_bJumping = false;
			return;
		}

		rcastHit2D = Physics2D.Raycast(new Vector2(transform.position.x - _sizeCollider.x + (1f / 16f), transform.position.y), Vector2.down, _sizeCollider.y + (1f / 16f), 1 << 9);

		if (rcastHit2D) {
			IsGrounded = true;
			_bJumping = false;
			return;
		}
	}

	protected virtual void Jumped() {
		JumpEvent?.Invoke();
	}

	protected virtual void Landed() {
		LandEvent?.Invoke();
	}

}
//bool IsGrounded() {
//	Debug.DrawLine(new Vector2(transform.position.x + sizeCollider.x, transform.position.y), transform.position - new Vector3(-sizeCollider.x, sizeCollider.y + (1f / 16f)), Color.white);
//		return (
//	Physics2D.Raycast(new Vector2(transform.position.x + sizeCollider.x, transform.position.y), Vector2.down, sizeCollider.y + (1f / 16f), 1 << 9) ||
//	Physics2D.Raycast(new Vector2(transform.position.x - sizeCollider.x, transform.position.y), Vector2.down, sizeCollider.y + (1f / 16f), 1 << 9)
//	);
//}
