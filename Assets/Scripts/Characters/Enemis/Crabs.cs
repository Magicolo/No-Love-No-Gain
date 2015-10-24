using UnityEngine;
using System.Collections;
using Magicolo;
using System;

public class Crabs : DamageableBase
{
	[Disable]
	public CrabPince[] Pinces;

	public enum CrabStates { Idle, Charging, Attacking };

	public enum CrabBehaviors { AttackPlayer, AttackPopulation, AttackBuilding, AimlessWalks }
	public CrabBehaviors CrabBehavior;

	[Disable]
	public GameObject CrabTarget;

	[Disable]
	public Vector2 targetMovement;

	[Disable]
	public CrabStates CrabState;

	public Animator Animator;
	public Rigidbody2D Body;

	public float MouvementSpeed = 2;

	public override void Die()
	{
		gameObject.Destroy();
	}

	public override void Damage(float damage, DamageSources damageSource, Vector2 knockback = default(Vector2))
	{
		base.Damage(damage, damageSource, knockback);

		Body.AddForce(knockback);
		CrabTarget = null;
	}

	void Start()
	{
		Pinces = GetComponentsInChildren<CrabPince>();
		Animator = GetComponent<Animator>();
		Body = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		switch (CrabBehavior)
		{
			case CrabBehaviors.AimlessWalks: AimlessWalk(); break;
			case CrabBehaviors.AttackBuilding: AttackBuilding(); break;
			case CrabBehaviors.AttackPlayer: AttackPlayer(); break;
			case CrabBehaviors.AttackPopulation: AttackPopulation(); break;
		}
	}

	private void CheckTargetIsActiveOrGetClosest<T>() where T : MonoBehaviour
	{
		if (CrabTarget && !CrabTarget.activeSelf)
			CrabTarget = null;

		if (CrabTarget == null)
		{
			T thing = transform.GetClosest<T>(FindObjectsOfType<T>());
			if (thing)
			{
				CrabTarget = thing.gameObject;
			}
		}
	}

	private void AttackPopulation()
	{
		CheckTargetIsActiveOrGetClosest<Civile>();
		if (CrabTarget)
		{
			targetMovement = MouvementSpeed * (CrabTarget.transform.position - transform.position).normalized;
			targetMovement = targetMovement.SetValues(0, Axes.Y);
		}
		else
		{
			targetMovement = Vector2.zero;
		}
	}

	private void AttackPlayer()
	{
		if (CrabTarget == null)
		{
			Player player = transform.GetClosest(FindObjectsOfType<Player>());

			CrabTarget = player == null ? null : player.gameObject;
		}
	}

	private void AttackBuilding()
	{

	}

	private void AimlessWalk()
	{

	}

	void FixedUpdate()
	{
		Body.AccelerateTowards(targetMovement, 100, Kronos.Enemy.DeltaTime, axes: Axes.X);
	}
}