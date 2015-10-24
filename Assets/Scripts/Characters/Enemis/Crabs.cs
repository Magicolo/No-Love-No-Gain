using UnityEngine;
using System.Collections;
using Magicolo;
using System;

public class Crabs : DamageableBase
{
	[Disable]
	public CrabPince[] Pinces;

	public enum CrabStates { Idle, Charging, Attacking };

	[Disable]
	public CrabStates CrabState;

	public Animator Animator;

	public override void Die()
	{
		this.Destroy();
	}

	void Start()
	{
		Pinces = GetComponentsInChildren<CrabPince>();
		Animator = GetComponent<Animator>();
	}

	public override bool CanBeDamagedBy(DamageSources damageSource)
	{
		return false;
	}

	public override void OnDamaged()
	{

	}
}