﻿using UnityEngine;
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
	public float MaxHealth = 25f;

	[Disable]
	public GameObject CrabTarget;

	[Disable]
	public Vector2 targetMovement;

	[Disable]
	public CrabStates CrabState;

	public Animator Animator;
	public Rigidbody2D Body;

	public float MouvementSpeed = 2;

	public LayerMask FlipWhenSeeing;
	public SingleRayCast GroundRayCast;
	[Disable]
	public GameObject ForwardGroundGameObject;

	int _isMovingHash = Animator.StringToHash("IsMoving");

	public override void Die()
	{
		gameObject.Destroy();
	}

	public override void Damage(float damage, DamageSources damageSource, Vector2 knockback = default(Vector2))
	{
		base.Damage(damage, damageSource, knockback);

		Body.AddForce(knockback, ForceMode2D.Impulse);
		CrabTarget = null;
	}

	void Start()
	{
		Health = MaxHealth;
		Pinces = GetComponentsInChildren<CrabPince>();
		Animator = GetComponent<Animator>();
		Body = GetComponent<Rigidbody2D>();
		Animator.SetBool(_isMovingHash, true);
	}

	void Update()
	{
		ForwardGroundGameObject = GroundRayCast.GetHitGameObject(transform.position);
		switch (CrabBehavior)
		{
			case CrabBehaviors.AimlessWalks: AimlessWalk(); break;
			case CrabBehaviors.AttackBuilding: AttackBuilding(); break;
			case CrabBehaviors.AttackPlayer: AttackPlayer(); break;
			case CrabBehaviors.AttackPopulation: AttackPopulation(); break;
		}
	}

	private void AttackPopulation()
	{
		CheckTargetIsActiveOrGetClosest<Civile>();

		if (CrabTarget != null)
			SetTargetMovement(CrabTarget.transform.position);
	}

	private void AttackPlayer()
	{
		CheckTargetIsActiveOrGetClosest<Player>();

		if (CrabTarget != null)
			SetTargetMovement(CrabTarget.transform.position);
	}

	private void AttackBuilding()
	{
		CheckTargetIsActiveOrGetClosest<Building>();

		if (CrabTarget != null)
		{
			BuildingBase topBuildingBase = CrabTarget.GetComponentsInChildren<BuildingBase>().Last();

			Vector3 targetMovementForBuilding = topBuildingBase.transform.position.SetValues(transform.position.y, Axes.Y);
			float distanceToBuilding = transform.position.y - topBuildingBase.transform.position.y;
			bool isInTheWidthOfBuilding = transform.position.x.IsBetween(topBuildingBase.BoxCollider.GetTopLeftCorner().x, topBuildingBase.BoxCollider.GetTopRightCorner().x);

			if (distanceToBuilding >= 0.4)
			{
				if (!isInTheWidthOfBuilding)
					SetTargetMovement(targetMovementForBuilding);
			}
			else
			{
				if (!isInTheWidthOfBuilding && Body.velocity.y <= 0)
					this.CrabBehavior = CrabBehaviors.AimlessWalks;
				else
					SetTargetMovement(targetMovementForBuilding);
			}
		}
		else
		{
			targetMovement = Vector2.zero;
		}
	}

	private void AimlessWalk()
	{
		if (!ForwardGroundGameObject || FlipWhenSeeing.Contains(ForwardGroundGameObject.layer))
		{
			transform.Rotate(new Vector3(0, 180, 0));
			GroundRayCast.FlipX();
		}
		targetMovement = new Vector2(this.MouvementSpeed * transform.forward.z, 0);
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

	private void SetTargetMovement(Vector3 target)
	{
		if (CrabTarget)
		{
			targetMovement = MouvementSpeed * (target - transform.position).normalized;
			targetMovement = targetMovement.SetValues(0, Axes.Y);
		}
		else
		{
			targetMovement = Vector2.zero;
		}
	}


	void FixedUpdate()
	{
		Body.AccelerateTowards(targetMovement, 100, Kronos.Enemy.DeltaTime, axes: Axes.X);
	}

	void OnDrawGizmos()
	{
		GroundRayCast.DrawGizmos();
	}
}