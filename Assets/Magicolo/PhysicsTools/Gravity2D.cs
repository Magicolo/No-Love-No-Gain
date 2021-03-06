﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;
using Magicolo.PhysicsTools;

namespace Magicolo
{
	[RequireComponent(typeof(Rigidbody2D))]
	[AddComponentMenu("Magicolo/Physics/Gravity2D")]
	public class Gravity2D : GravityBase
	{
		bool _rigidbody2DCached;
		Rigidbody2D _rigidbody2D;
		public Rigidbody2D Rigidbody2D
		{
			get
			{
				_rigidbody2D = _rigidbody2DCached ? _rigidbody2D : this.FindComponent<Rigidbody2D>();
				_rigidbody2DCached = true;
				return _rigidbody2D;
			}
		}

		void FixedUpdate()
		{
			Rigidbody2D.AddForce(Force * Rigidbody2D.mass);
		}
	}
}

