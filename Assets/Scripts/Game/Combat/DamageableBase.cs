using UnityEngine;
using System.Collections;
using Magicolo;
using System;

public abstract class DamageableBase : MonoBehaviour, IDamageable {


    public float Hp;
    public DamageType VulnerableDamageType;

    public void Damage(Transform source, float damage, float knockbackForce, DamageType attackType)
    {
        if (CanBeDamagedBy(attackType))
        {
            Hp -= damage;
            if (Hp < 0)
            {
                Die();
            } else {
                TakeDamage();
            }
        }
        
    }

    internal abstract void TakeDamage();

    public bool CanBeDamagedBy(DamageType attack)
    {
        return VulnerableDamageType.MatchOneFrom(attack);
    }

    public abstract void Die();
}
