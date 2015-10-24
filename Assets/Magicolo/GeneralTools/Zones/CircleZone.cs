using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Magicolo;

public class CircleZone : MonoBehaviourExtended
{
	[SerializeField]
	Circle _circle;
	[SerializeField]
	bool _draw = true;

	public Circle LocalCircle { get { return _circle; } }
	public Circle WorldCircle
	{
		get
		{
			Circle circle = new Circle(_circle);
			circle.Position += transform.position.ToVector2();
			return circle;
		}
	}

	void OnDrawGizmos()
#if UNITY_EDITOR
	{
		if (!_draw)
			return;

		Vector3 position = transform.position + _circle.Position.ToVector3();
		UnityEditor.Handles.color = new Color(1f, 0f, 0f, 0.75f);
		UnityEditor.Handles.DrawWireDisc(position, Vector3.back, _circle.Radius);
		UnityEditor.Handles.color = new Color(1f, 0f, 0f, 0.1f);
		UnityEditor.Handles.DrawSolidDisc(position, Vector3.back, _circle.Radius);
#endif
	}
}
