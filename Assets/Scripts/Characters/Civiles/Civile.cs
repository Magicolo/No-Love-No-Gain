using UnityEngine;
using System.Collections;
using Magicolo;
using Rick;

public class Civile : MonoBehaviour, IPoolable, ICopyable<Civile>
{

	public float MovementSpeed;
	public float MovementSpeedMin;
	public float MovementSpeedMax;

	public Vector2 RaycastDirection;
	public float RaycastDistance;
	public LayerMask Mask;
	[Disable]
	public GameObject GameObjectOnNextFoot;

	private Rigidbody2D body;

	void Start()
	{
		body = GetComponent<Rigidbody2D>();
		MovementSpeed = Random.Range(MovementSpeedMin, MovementSpeedMax);
	}

	void ICopyable<Civile>.Copy(Civile reference)
	{
		MovementSpeedMin = reference.MovementSpeedMin;
	}

	void IPoolable.OnCreate()
	{
		MovementSpeed = Random.Range(MovementSpeedMin, MovementSpeedMax);
	}

	void IPoolable.OnRecycle()
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
		RaycastHit2D hit = Physics2D.Raycast(transform.position, RaycastDirection, RaycastDistance, Mask, 0);
		//Debug.DrawRay(transform.position, RaycastDirection, Color.red);
		GameObjectOnNextFoot = null;
		if (hit)
		{
			GameObjectOnNextFoot = hit.collider.gameObject;

		}
		else
		{
			//transform.FlipLocalScale(Axes.X);
			transform.Rotate(new Vector3(0, 180, 0));
			Debug.Log(this.transform.forward);
		}
	}
}