using UnityEngine;
using System.Collections;
using Magicolo;
using System;

public abstract class DamageableBase : MonoBehaviour, IDamageable
{
	public float Hp;

	protected abstract void OnDamaged();

	public abstract void Die();

	public abstract bool CanBeDamagedBy(DamageSources damageSource);

	public virtual void Damage(float damage, DamageSources damageSource, Vector2 knockback = default(Vector2))
	{
		if (CanBeDamagedBy(damageSource))
		{
			Hp -= damage;

			if (Hp < 0)
				Die();
			else
				OnDamaged();
		}
	}
}
