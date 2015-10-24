using UnityEngine;
using System.Collections;
using Magicolo;
using Rick;

public class Civile : DamageableBase, IPoolable, ICopyable<Civile>
{
	public float MaxHeath;

	public float MovementSpeed;
	public float MovementSpeedMin;
	public float MovementSpeedMax;

	[DoNotCopy]
	private Rigidbody2D body;

	[DoNotCopy]
	public bool LeftEntrance;

	public LayerMask FlipWhenSeeing;
	public SingleRayCast GroundRayCast;
	[Disable, DoNotCopy]
	public GameObject ForwardGroundGameObject;

	void Start()
	{
		body = GetComponent<Rigidbody2D>();
		MovementSpeed = Random.Range(MovementSpeedMin, MovementSpeedMax);
	}

	public void Copy(Civile reference)
	{
		MaxHeath = reference.MaxHeath;
		MovementSpeed = reference.MovementSpeed;
		MovementSpeedMin = reference.MovementSpeedMin;
		MovementSpeedMax = reference.MovementSpeedMax;
		FlipWhenSeeing = reference.FlipWhenSeeing;
		GroundRayCast = reference.GroundRayCast;
	}

	public void OnCreate()
	{
		MovementSpeed = Random.Range(MovementSpeedMin, MovementSpeedMax);
		LeftEntrance = false;
		Health = MaxHeath;
	}

	public void OnRecycle()
	{

	}

	void FixedUpdate()
	{
		body.AccelerateTowards(new Vector2(this.MovementSpeed * transform.forward.z, 0), 100, Kronos.World.DeltaTime, axes: Axes.X);
	}

	void Update()
	{
		CheckFloorInFront();
	}

	private void CheckFloorInFront()
	{
		ForwardGroundGameObject = GroundRayCast.GetHitGameObject(transform.position);
		if (!ForwardGroundGameObject || FlipWhenSeeing.Contains(ForwardGroundGameObject.layer))
		{
			transform.Rotate(new Vector3(0, 180, 0));
			GroundRayCast.FlipX();
		}
	}

	public override void Die()
	{
		BehaviourPool<Civile>.Recycle(this);
	}

	void OnDrawGizmos()
	{
		GroundRayCast.DrawGizmos();
	}
}
