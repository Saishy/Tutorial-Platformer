using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{

	protected override Vector2 GetInputs() {
		return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
	}

	protected override bool GetJump() {
		return Input.GetButton("Jump");
	}

}
