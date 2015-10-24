using UnityEngine;
using System.Collections;
using System;
using Rick;

public class BuildingBase : DamageableBase
{

	public BoxCollider2D BoxCollider;

	void Start()
	{
		BoxCollider = GetComponent<BoxCollider2D>();
	}
	public override void Die()
	{
		gameObject.Remove();
	}
}
