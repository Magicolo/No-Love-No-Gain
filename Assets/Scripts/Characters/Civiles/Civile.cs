using UnityEngine;
using System.Collections;
using Magicolo;
using Rick;

public class Civile : DamageableBaseBase, IPoolable, ICopyable<Civile>
{

	public float MaxHeath;

	public float MovementSpeed;
	public float MovementSpeedMin;
	public float MovementSpeedMax;

	public Vector2 RaycastDirection;
	public float RaycastDistance;
	public LayerMask Mask;
	[Disable]
	public GameObject GameObjectOnNextFoot;

	[DoNotCopy]
	private Rigidbody2D body;

	[DoNotCopy]
	public bool LeftEntrance;

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
		RaycastDirection = reference.RaycastDirection;
		RaycastDistance = reference.RaycastDistance;
		Mask = reference.Mask;
		GameObjectOnNextFoot = reference.GameObjectOnNextFoot;
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
		body.AccelerateTowards(new Vector2(this.MovementSpeed * transform.forward.z, 0), 100, Kronos.World.DeltaTime);
	}

	void Update()
	{
		CheckFloorInFront();
	}

	private void CheckFloorInFront()
	{
		Vector2 rayDirection = RaycastDirection;
		if (transform.forward.z < 0)
		{
			rayDirection = new Vector2(RaycastDirection.x * -1, RaycastDirection.y);
		}
		RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, RaycastDistance, Mask, 0);
		Debug.DrawRay(transform.position, rayDirection, Color.red);
		GameObjectOnNextFoot = null;
		if (hit)
			GameObjectOnNextFoot = hit.collider.gameObject;
		else
			transform.Rotate(new Vector3(0, 180, 0));
	}

	public override void Die()
	{
		BehaviourPool<Civile>.Recycle(this);
	}

	public override void OnDamaged()
	{

	}
}
