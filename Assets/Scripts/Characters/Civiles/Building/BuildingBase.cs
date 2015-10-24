using UnityEngine;
using System.Collections;
using System;
using Rick;

public class BuildingBase : DamageableBase
{
	public override bool CanBeDamagedBy(DamageSources damageSource)
	{
		return false;
	}

	public override void Die()
	{
		gameObject.Remove();
	}

	public override void OnDamaged()
	{
		//throw new NotImplementedException();
	}
}
