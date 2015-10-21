using UnityEngine;
using System.Collections;

public interface IDamageable {


    void Damage(float damage, DamageType attackType, float knockbackForce = 0, Vector2 knowbackForce = default (Vector2));
	
}
