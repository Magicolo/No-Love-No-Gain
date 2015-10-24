using UnityEngine;
using System.Collections.Generic;
using Magicolo;
using Rick;

[System.Serializable]
public class SingleRayCast
{

	[SerializeField, PropertyField, Min(0), Max(360)]
	float angle;
	public float Angle
	{
		get { return angle; }
		set
		{
			angle = value;
			RaycastDirection = new Vector2(1, 0).Rotate(angle);
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

	public RaycastHit GetHit(Vector3 origin)
	{
		Vector3 adjustedOrigin = (origin + offset.ToVector3());
		if (ShowRayCast)
			Debug.DrawRay(adjustedOrigin, RaycastDirection * RaycastDistance, Color.green);

		RaycastHit hit;
		if (Physics.Raycast(adjustedOrigin, RaycastDirection, out hit, RaycastDistance, Mask, 0))
		{
			Debug.Log("YES");
		}
		return hit;
	}

	public GameObject getHitGameObject(Vector3 origin)
	{
		RaycastHit hit = GetHit(origin);

		if (hit.collider)
			return hit.collider.gameObject;
		else
			return null;

	}
}
