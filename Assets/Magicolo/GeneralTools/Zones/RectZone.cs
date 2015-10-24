﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Magicolo;
using Magicolo.GeneralTools;

namespace Magicolo
{
	[AddComponentMenu("Magicolo/General/Zones/Rect Zone")]
	public class RectZone : MonoBehaviourExtended
	{
		[SerializeField]
		Rect _rect = new Rect(0f, 0f, 1f, 1f);
		[SerializeField]
		bool _draw = true;

		public Rect LocalRect { get { return new Rect(_rect.center, _rect.size); } }
		public Rect WorldRect
		{
			get
			{
				Rect rect = LocalRect;
				rect.center += transform.position.ToVector2();
				return rect;
			}
		}

		void OnDrawGizmos()
		{
			if (!_draw)
				return;

			Vector3 position = transform.position + _rect.position.ToVector3();
			Vector3 size = _rect.size;
			Gizmos.color = new Color(1f, 0f, 0f, 0.6f);
			Gizmos.DrawWireCube(position, size);
			Gizmos.color = new Color(1f, 0f, 0f, 0.15f);
			Gizmos.DrawCube(position, size);
		}
	}
}