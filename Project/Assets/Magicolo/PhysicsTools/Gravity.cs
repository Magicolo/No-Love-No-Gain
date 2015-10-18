using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;
using Magicolo.PhysicsTools;

namespace Magicolo {
	[RequireComponent(typeof(Rigidbody))]
	[AddComponentMenu("Magicolo/Physics/Gravity")]
	public class Gravity : GravityBase {

		bool _rigidbodyCached;
		Rigidbody _rigidbody;
		new public Rigidbody rigidbody { 
			get { 
				_rigidbody = _rigidbodyCached ? _rigidbody : this.FindComponent<Rigidbody>();
				_rigidbodyCached = true;
				return _rigidbody;
			}
		}
		
		void FixedUpdate() {
			rigidbody.AddForce(Force * rigidbody.mass);
		}
	}
}

