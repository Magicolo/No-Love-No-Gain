using UnityEngine;
using System.Collections;

public interface IDamageable {


    void Damage(float damage, DamageType attackType, Vector2 knowbackForce = default (Vector2));
	
}
