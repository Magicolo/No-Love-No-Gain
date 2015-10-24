using UnityEngine;
using System.Collections.Generic;
using Magicolo;
using Rick;

public abstract class DamageableBaseBase : DamageableBase
{

	public DamagedBy DamagedBy;

	public override bool CanBeDamagedBy(DamageSources damageSource)
	{
		switch (damageSource)
		{
			case DamageSources.Player: return DamagedBy.Player;
			case DamageSources.Crabs: return DamagedBy.Crabs;
			case DamageSources.Population: return DamagedBy.Population;
			default: return false;
		}
	}
}

[System.Serializable]
public class DamagedBy
{
	public bool Player;
	public bool Crabs;
	public bool Population;
}
