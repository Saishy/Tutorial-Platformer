using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartEvent<T>
{

	private readonly HashSet<T> _actions = new HashSet<T>();

	public HashSet<T> Actions { get { return _actions; } }

	public static SmartEvent<T> operator +(SmartEvent<T> smartEvent, T action) {
		smartEvent._actions.Add(action);
		return smartEvent;
	}

	public static SmartEvent<T> operator -(SmartEvent<T> smartEvent, T action) {
		smartEvent._actions.Remove(action);
		return smartEvent;
	}
}
