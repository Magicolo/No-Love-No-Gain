using UnityEngine;
using System.Collections;
using Magicolo;
using Rick;

public class BuildingBase : DamageableBase
{

	[Disable]
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
