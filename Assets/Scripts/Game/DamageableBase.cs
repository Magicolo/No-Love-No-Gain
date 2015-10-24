using UnityEngine;
using System.Collections;
using Magicolo;
using System;

public abstract class DamageableBase : MonoBehaviour, IDamageable
{
	[Serializable]
	public class CanBeDamaged
	{
		public bool Player;
		public bool Crabs;
		public bool Population;
	}

	public CanBeDamaged DamagedBy;

	public float Health { get; set; }

	public abstract void Die();

	public virtual bool CanBeDamagedBy(DamageSources damageSource)
	{
		switch (damageSource)
		{
			case DamageSources.Player: return DamagedBy.Player;
			case DamageSources.Crabs: return DamagedBy.Crabs;
			case DamageSources.Population: return DamagedBy.Population;
			default: return false;
		}
	}
	public virtual void Damage(float damage, DamageSources damageSource, Vector2 knockback = default(Vector2))
	{
		if (CanBeDamagedBy(damageSource))
		{
			Health -= damage;

			if (Health < 0)
				Die();
		}
	}
}
