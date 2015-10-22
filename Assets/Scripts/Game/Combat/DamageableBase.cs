using UnityEngine;
using System.Collections;
using Magicolo;
using System;

public abstract class DamageableBase : MonoBehaviour, IDamageable
{
	public float Health { get; set; }

	protected virtual void OnDamaged() { }

	public abstract void Die();

	public abstract bool CanBeDamagedBy(DamageSources damageSource);

	public virtual void Damage(float damage, DamageSources damageSource, Vector2 knockback = default(Vector2))
	{
		if (CanBeDamagedBy(damageSource))
		{
			Health -= damage;

			if (Health < 0)
				Die();
			else
				OnDamaged();
		}
	}
}
