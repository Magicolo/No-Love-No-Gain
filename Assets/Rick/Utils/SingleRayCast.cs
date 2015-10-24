using UnityEngine;
using System.Collections.Generic;
using Magicolo;
using Rick;

[System.Serializable]
public class SingleRayCast
{

	[SerializeField, PropertyField(typeof(ClampAttribute), 0f, 360f)]
	float angle;
	public float Angle
	{
		get { return angle; }
		set
		{
			angle = value;
			RaycastDirection = Vector2.right.Rotate(angle);
		}
	}
	[Disable]
	public Vector2 RaycastDirection;
	public float RaycastDistance;
	public LayerMask Mask;

	public bool ShowRayCast;
	public Transform OriginTransform; // A checker, c'est intressant le drawGizmos mais il faut avoir toujours un transform... P-e classe enfant specilisé

	public Vector2 offset;


	public void DrawGizmos()
	{
		if (ShowRayCast)
		{
			Vector3 position = (OriginTransform.position + offset.ToVector3());
			Gizmos.DrawLine(position, position + (RaycastDirection * RaycastDistance).ToVector3());
		}
	}

	public RaycastHit2D GetHit(Vector3 origin)
	{
		Vector3 adjustedOrigin = (origin + offset.ToVector3());
		if (ShowRayCast)
			Debug.DrawRay(adjustedOrigin, RaycastDirection * RaycastDistance, Color.green);

		return Physics2D.Raycast(adjustedOrigin, RaycastDirection, RaycastDistance, Mask, 0);
	}

	public GameObject GetHitGameObject(Vector3 origin)
	{
		RaycastHit2D hit = GetHit(origin);
		if (hit.collider)
			return hit.collider.gameObject;
		else
			return null;

	}

	public void FlipX()
	{
		RaycastDirection = Vector2.Reflect(RaycastDirection, Vector2.right);
		angle = RaycastDirection.Angle();
		offset = new Vector2(-offset.x, offset.y);
	}
}
