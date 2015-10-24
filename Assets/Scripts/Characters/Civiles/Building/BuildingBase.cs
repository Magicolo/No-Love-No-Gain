using UnityEngine;
using System.Collections;
using System;
using Rick;

public class BuildingBase : DamageableBase
{
	public override void Die()
	{
		gameObject.Remove();
	}
}
