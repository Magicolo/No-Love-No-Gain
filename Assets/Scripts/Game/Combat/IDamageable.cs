using UnityEngine;
using System.Collections;

public interface IDamageable {

    void Damage(Transform source, float damage, float knockbackForce, DamageType attackType);
	
}
