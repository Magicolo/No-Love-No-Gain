using UnityEngine;
using System.Collections;
using Magicolo;

public class Crabs : DamageableBaseBase
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
		this.Destroy();
	}

	void Start()
	{
		Pinces = GetComponentsInChildren<CrabPince>();
		Animator = GetComponent<Animator>();
		Body = GetComponent<Rigidbody2D>();
	}

	public override void OnDamaged()
	{
		CrabTarget = null;
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
		if (CrabTarget && CrabTarget.gameObject.activeSelf)
			CrabTarget = null;

		if (CrabTarget == null)
		{
			T thing = transform.GetClosest<T>(Object.FindObjectsOfType<T>());
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
	}

	private void AttackPlayer()
	{
		if (CrabTarget == null)
		{
			CrabTarget = transform.GetClosest<Player>(Object.FindObjectsOfType<Player>()).gameObject;
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
		Body.AccelerateTowards(targetMovement, 100, Kronos.Enemy.DeltaTime);
	}
}