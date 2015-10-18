using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo {
	[System.Serializable]
	public class SphereCastAllSettings {
		
		public Vector3 offset;
		public float radius = 1;
		public LayerMask layerMask;
		
		public bool HasHit(Vector3 origin, bool debug = false) {
			return GetHits(origin, debug).Length > 0;
		}
		
		public RaycastHit[] GetHits(Vector3 origin, bool debug = false) {
			RaycastHit[] hits = Physics.SphereCastAll(origin + offset, radius, Vector3.forward, Mathf.Infinity, layerMask);
			
			if (debug) {
				DrawRays(origin);
			}
			
			return hits;
		}
		
		public void DrawRays(Vector3 origin) {
			DrawRays(origin, 16);
		}
		
		public void DrawRays(Vector3 origin, int amountOfRays) {
			for (int i = 0; i < amountOfRays; i++) {
				Debug.DrawRay(origin + offset, Vector2.up.Rotate(i * (360 / amountOfRays)) * radius, Color.yellow);
			}
		}
	}
	
	[System.Serializable]
	public class GroundCastSettings {
		
		public Vector3 offset;
		[Range(-90, 90)] public float spread = 30;
		[Min] public float distance = 1;
		[Range(0, 360)] public float angle;
		public LayerMask layerMask;
		
		public bool HasHit(Vector3 origin, Vector3 direction, bool debug = false) {
			return GetHits(origin, direction, debug).Length > 0;
		}
		
		public Collider GetGround(Vector2 origin, Vector2 direction, bool debug = false) {
			RaycastHit[] hits = GetHits(origin, direction, debug);
			
			return hits.Length > 0 ? hits[0].collider : null;
		}
		
		public T GetGround<T>(Vector2 origin, Vector2 direction, bool debug = false) where T : Collider {
			return (T)GetGround(origin, direction, debug);
		}
		
		public RaycastHit[] GetHits(Vector3 origin, Vector3 direction, bool debug = false) {
			List<RaycastHit> hits = new List<RaycastHit>();
			float adjustedDistance = distance / Mathf.Cos(spread * Mathf.Deg2Rad);
			Vector3 adjustedOrigin = origin + (angle == 0 ? offset : offset.Rotate(angle));
			Vector3 adjustedDirection = angle == 0 ? direction : direction.Rotate(angle);
		
			RaycastHit hit;
			if (Physics.Raycast(adjustedOrigin, adjustedDirection, out hit, distance, layerMask)) {
				hits.Add(hit);
			}

			if (Physics.Raycast(adjustedOrigin, adjustedDirection.Rotate(spread), out hit, adjustedDistance, layerMask)) {
				hits.Add(hit);
			}
			
			if (Physics.Raycast(adjustedOrigin, adjustedDirection.Rotate(-spread), out hit, adjustedDistance, layerMask)) {
				hits.Add(hit);
			}
		
			if (debug) {
				DrawRays(origin, direction);
			}
		
			return hits.ToArray();
		}
		
		public void DrawRays(Vector3 origin, Vector3 direction) {
			float adjustedDistance = distance / Mathf.Cos(spread * Mathf.Deg2Rad);
			Vector3 adjustedOrigin = origin + offset.Rotate(angle);
			Vector3 adjustedDirection = angle == 0 ? direction : direction.Rotate(angle);
			
			Debug.DrawRay(adjustedOrigin, adjustedDirection * distance, Color.green);
			Debug.DrawRay(adjustedOrigin, adjustedDirection.Rotate(spread) * adjustedDistance, Color.green);
			Debug.DrawRay(adjustedOrigin, adjustedDirection.Rotate(-spread) * adjustedDistance, Color.green);
		}
	}
	
	[System.Serializable]
	public class GroundCastSettings2D {
		
		public Vector2 offset;
		[Range(-90, 90)] public float spread = 30;
		[Min] public float distance = 1;
		[Range(0, 360)] public float angle;
		public LayerMask layerMask;
		
		public bool HasHit(Vector2 origin, Vector2 direction, bool debug = false) {
			return GetHits(origin, direction, debug).Length > 0;
		}
		
		public Collider2D GetGround(Vector2 origin, Vector2 direction, bool debug = false) {
			RaycastHit2D[] hits = GetHits(origin, direction, debug);
			
			return hits.Length > 0 ? hits[0].collider : null;
		}
		
		public T GetGround<T>(Vector2 origin, Vector2 direction, bool debug = false) where T : Collider2D {
			return (T)GetGround(origin, direction, debug);
		}
		
		public RaycastHit2D[] GetHits(Vector2 origin, Vector2 direction, bool debug = false) {
			List<RaycastHit2D> hits = new List<RaycastHit2D>();
			float adjustedDistance = distance / Mathf.Cos(spread * Mathf.Deg2Rad);
			Vector2 adjustedOrigin = origin + (angle == 0 ? offset : offset.Rotate(angle));
			Vector2 adjustedDirection = angle == 0 ? direction : direction.Rotate(angle);
		
			RaycastHit2D hit;
			
			hit = Physics2D.Raycast(adjustedOrigin, adjustedDirection, distance, layerMask, 0);
			if (hit) {
				hits.Add(hit);
			}
			
			hit = Physics2D.Raycast(adjustedOrigin, adjustedDirection.Rotate(spread), adjustedDistance, layerMask, 0);
			if (hit) {
				hits.Add(hit);
			}
			
			hit = Physics2D.Raycast(adjustedOrigin, adjustedDirection.Rotate(-spread), adjustedDistance, layerMask, 0);
			if (hit) {
				hits.Add(hit);
			}
			
			if (debug) {
				DrawRays(origin, direction);
			}
		
			return hits.ToArray();
		}
		
		public void DrawRays(Vector2 origin, Vector2 direction) {
			float adjustedDistance = distance / Mathf.Cos(spread * Mathf.Deg2Rad);
			Vector2 adjustedOrigin = origin + (angle == 0 ? offset : offset.Rotate(angle));
			Vector2 adjustedDirection = angle == 0 ? direction : direction.Rotate(angle);
			
			Debug.DrawRay(adjustedOrigin, adjustedDirection * distance, Color.green);
			Debug.DrawRay(adjustedOrigin, adjustedDirection.Rotate(spread) * adjustedDistance, Color.green);
			Debug.DrawRay(adjustedOrigin, adjustedDirection.Rotate(-spread) * adjustedDistance, Color.green);
		}
	}
}
