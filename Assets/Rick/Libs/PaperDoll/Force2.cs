using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo {
	[System.Serializable]
	public class Force2 {

		[SerializeField, PropertyField(typeof(RangeAttribute), 0, 360)]
		float angle = 90;
		public float Angle {
			get {
				return angle;
			}
			set {
				angle = value % 360;
				force = (Vector2.right.Rotate(angle) * strength).Round(0.0001F);
				direction = force.normalized;

				hasChanged = true;
			}
		}
	
		[SerializeField, PropertyField(typeof(MinAttribute))]
		float strength = 20;
		public float Strength {
			get {
				return strength;
			}
			set {
				strength = value;
				force = (Vector2.right.Rotate(angle) * strength).Round(0.0001F);
				direction = force.normalized;

				hasChanged = true;
			}
		}

		[SerializeField, PropertyField]
		Vector2 direction = new Vector2(0, -1);
		public Vector2 Direction {
			get {
				return direction;
			}
			set {
				direction = value.normalized.Round(0.0001F);
				force = direction * strength;
				angle = direction.Angle();

				hasChanged = true;
			}
		}
		
		[SerializeField, PropertyField]
		Vector2 force = new Vector2(0, -20);
		public Vector2 Force {
			get {
				return force;
			}
			set {
				force = value;
				strength = force.magnitude;
				direction = force.normalized;
				angle = direction.Angle();
				
				hasChanged = true;
			}
		}
		
		Vector2 left;
		public Vector2 Left {
			get {
				if (hasChanged) {
					UpdateForces();
				}
				
				return right;
			}
		}
		
		Vector2 right;
		public Vector2 Right {
			get {
				if (hasChanged) {
					UpdateForces();
				}
				
				return right;
			}
		}
		
		bool hasChanged = true;
		
		public Force2(float angle, float strength) {
			Angle = angle;
			Strength = strength;
		}
		
		public Force2(Vector2 direction, float strength) {
			Direction = direction;
			Strength = strength;
		}
		
		public Force2(Vector2 vector) {
			Force = vector;
		}
		
		void UpdateForces() {
			left = force.Rotate(90);
			right = -left;
			
			hasChanged = false;
		}
		
		public static implicit operator Vector2(Force2 force) {
			return force.Force;
		}
		
		public static implicit operator Vector3(Force2 force) {
			return force.Force;
		}
		
		public static implicit operator Force2(Vector2 vector) {
			return new Force2(vector);
		}
		
		public static implicit operator Force2(Vector3 vector) {
			return new Force2(vector);
		}
	}
}
