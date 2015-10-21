using UnityEngine;
using System.Collections;
using Magicolo;
using System;

public abstract class DamageableBase : MonoBehaviour, IDamageable {


	public float Hp;
	public DamageType VulnerableDamageType;
	
	internal abstract void TakeDamage();
	
	public bool CanBeDamagedBy(DamageType attack)
	{
		return VulnerableDamageType.CanBeDamagedBy(attack);
	}
	
	public abstract void Die();
	
	public void Damage(float damage, DamageType attackType, Vector2 knowback = default(Vector2))
	{
		if (CanBeDamagedBy(attackType))
		{
			Hp -= damage;
			if (Hp < 0)
			{
				Die();

			} 
			else
			{
				TakeDamage();
			}
		}
	}
}
