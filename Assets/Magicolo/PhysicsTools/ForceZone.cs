using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;
using Magicolo.PhysicsTools;

namespace Magicolo
{
	[AddComponentMenu("Magicolo/Physics/Force Zone")]
	public class ForceZone : PhysicsZone
	{
		public Vector2 Force;
		[Range(0, 1)]
		public float Damping;
		[Range(0, 1)]
		public float DistanceScaling;

		bool _colliderCached;
		Collider _collider;
		public Collider Collider
		{
			get
			{
				_collider = _colliderCached ? _collider : this.FindComponent<Collider>();
				_colliderCached = true;
				return _collider;
			}
		}

		void FixedUpdate()
		{
			foreach (Rigidbody body in RigidbodyCountDict.GetKeyArray())
			{
				Vector2 adjustedForce = Force;
				float adjustedDamping = Damping;

				if (DistanceScaling > 0)
				{
					Bounds zoneBounds = Collider.bounds;
					Vector3 bodyPosition = body.transform.position;
					float xAttenuation = Mathf.Clamp01(Mathf.Abs(zoneBounds.center.x - bodyPosition.x) / zoneBounds.extents.x) * DistanceScaling;
					float yAttenuation = Mathf.Clamp01(Mathf.Abs(zoneBounds.center.y - bodyPosition.y) / zoneBounds.extents.y) * DistanceScaling;
					float attenuation = 1 - (xAttenuation + yAttenuation) / 2;
					attenuation *= attenuation;

					adjustedForce *= attenuation;
					adjustedDamping *= attenuation;
				}

				body.AddForce(Force);

				if (adjustedDamping > 0)
					body.SetVelocity(body.velocity * (1 - adjustedDamping));
			}
		}
	}
}