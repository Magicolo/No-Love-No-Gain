﻿using UnityEngine;
using System.Collections;

public static class BoxCollider2DExtentions
{

	public static Vector3[] GetCornersWorldPositions(this BoxCollider2D box)
	{
		Vector3[] corners = new Vector3[4];
		corners[0] = box.transform.position + new Vector3(-box.size.x / 2, -box.size.y / 2);
		corners[1] = box.transform.position + new Vector3(-box.size.x / 2, box.size.y / 2);
		corners[2] = box.transform.position + new Vector3(box.size.x / 2, -box.size.y / 2);
		corners[3] = box.transform.position + new Vector3(box.size.x / 2, box.size.y / 2);

		return corners;
	}

	public static Vector2 GetTopLeftCorner(this BoxCollider2D collider)
	{
		float top = collider.offset.y + (collider.size.y / 2f);
		float left = collider.offset.x - (collider.size.x / 2f);
		return collider.transform.TransformPoint(new Vector2(left, top));
	}
	public static Vector2 GetTopRightCorner(this BoxCollider2D collider)
	{
		float top = collider.offset.y + (collider.size.y / 2f);
		float right = collider.offset.x + (collider.size.x / 2f);
		return collider.transform.TransformPoint(new Vector2(right, top));
	}
	public static Vector2 GetBottomLeftCorner(this BoxCollider2D collider)
	{
		float btm = collider.offset.y - (collider.size.y / 2f);
		float left = collider.offset.x - (collider.size.x / 2f);
		return collider.transform.TransformPoint(new Vector2(left, btm));
	}
	public static Vector2 GetBottomRightCorner(this BoxCollider2D collider)
	{
		float btm = collider.offset.y - (collider.size.y / 2f);
		float right = collider.offset.x + (collider.size.x / 2f);
		return collider.transform.TransformPoint(new Vector2(right, btm));
	}

	//TODO code fucking pas objtimal
	public static Vector2 GetRandomPoint(this BoxCollider2D collider)
	{
		float top = collider.GetTopLeftCorner().y;
		float left = collider.GetTopLeftCorner().x;
		float btm = collider.GetBottomRightCorner().y;
		float right = collider.GetBottomRightCorner().x;
		return new Vector2(Random.Range(left, right), Random.Range(top, btm));
	}
}
