using UnityEngine;
using System.Collections;

public enum DamageSources
{
	Player,
	Crabs,
	Population
}

public interface IDamageable
{
	bool CanBeDamagedBy(DamageSources damageSource);
	void Damage(float damage, DamageSources damageSource, Vector2 knockback = default(Vector2));
}
