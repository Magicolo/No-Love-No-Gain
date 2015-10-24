using UnityEngine;
using System.Collections;
using Magicolo;

public class CrabPince : MonoBehaviour
{
	public Crabs Crab;
	public string AnimationBool;
	[Disable]
	public float Timer;

	public float MinCoolDown;
	public float MaxCoolDown;

	public float AttackTime;
	public float AttackDamage;

	public enum CrabPinceState { CoolDown, Attacking };
	public CrabPinceState state;

	int _animationHash;

	void Awake()
	{
		_animationHash = Animator.StringToHash(AnimationBool);
	}

	void Start()
	{
		GoInCoolDown();
	}

	void Update()
	{
		Timer -= Kronos.Enemy.DeltaTime;

		switch (this.state)
		{
			case CrabPinceState.CoolDown: CooDown(); break;
			case CrabPinceState.Attacking: Attack(); break;
		}

		Crab.Animator.SetBool(_animationHash, state == CrabPinceState.Attacking);
	}

	private void Attack()
	{
		if (Timer <= 0)
		{
			GoInCoolDown();
		}
	}

	private void CooDown()
	{
		if (Timer < 0)
		{
			PrepareAttack();
		}
	}

	private void GoInCoolDown()
	{
		Timer = Random.Range(MinCoolDown, MaxCoolDown);
		state = CrabPinceState.CoolDown;
	}


	private void PrepareAttack()
	{
		Timer = AttackTime;
		state = CrabPinceState.Attacking;
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if (state == CrabPinceState.Attacking)
		{
			IDamageable damagable = other.GetComponent<IDamageable>();

			if (damagable != null)
			{
				damagable.Damage(AttackDamage, DamageSources.Crabs);
				GoInCoolDown();
			}
		}
	}
}
