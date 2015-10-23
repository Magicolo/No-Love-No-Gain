using UnityEngine;
using System.Collections;
using Magicolo;

public class CrabPince : MonoBehaviour
{
	[Disable]
	public float Timer;

	public float MinCoolDown;
	public float MaxCoolDown;

	public float AttackTime;
	public float AttackDamage;

	private SpriteRenderer Sprite;

	public enum CrabPinceState { CoolDown, Attacking };
	public CrabPinceState state;

	void Start()
	{
		this.Sprite = GetComponent<SpriteRenderer>();
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
		this.Sprite.enabled = false;
		state = CrabPinceState.CoolDown;
	}


	private void PrepareAttack()
	{
		Timer = AttackTime;
		this.Sprite.enabled = true;
		state = CrabPinceState.Attacking;
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if (state == CrabPinceState.Attacking)
		{
			IDamageable damagable = other.GetComponent<IDamageable>();

			if (damagable != null)
				damagable.Damage(AttackDamage, DamageSources.Crabs);
		}
	}
}
