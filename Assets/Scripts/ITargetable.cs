using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetable {

	bool CanBeTargetOf(Character character);

	void TakeDamage(float damage, Character character);
}
