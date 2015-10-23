using UnityEngine;
using System.Collections;
using Magicolo;
using System;

public class PaperDoolLegs : PaperDollPart
{

	//float distToGround;
	public GroundCastSettings2D raySettings;
	public Force2 gravity;

	[Disable]
	public Collider2D ground;
	[Disable]
	public bool grounded;

	[Min]
	public float minHeight = 1;
	[Min]
	public float maxHeight = 30;
	[Min]
	public float duration = 0.25F;

	[Disable]
	public float counter;
	[Disable]
	public float increment;
	[Disable]
	public Vector2 direction;

	[Disable]
	public bool jumping;


	public float walkSpeed = 0.5f;
	public float dashSpeed = 2;
	public float dashTimeBetweenTaps = 1f;
	[Disable]
	public float countDownLastMouvementX = 0;
	float moveVelocity;

	[Disable]
	public bool dashing;

	public bool debug;


	public Rigidbody2D body;

	void Start()
	{
		base.Init();
		//distToGround = GetComponent<BoxCollider2D>().size.y / 2 + 0.5f;
	}



	public bool isGrounded()
	{
		return grounded;
	}

	internal void StartJump()
	{
		if (grounded)
		{
			counter = duration;
			increment = (maxHeight - minHeight) / duration;
			direction = -gravity.Direction;
			setJumpForce();
			jumping = true;
		}
	}

	private void setJumpForce()
	{
		if (gravity.Angle == 90)
		{
			body.SetVelocity(minHeight, Axes.Y);
		}
		else if (gravity.Angle == 180)
		{
			body.SetVelocity(minHeight, Axes.X);
		}
		else if (gravity.Angle == 270)
		{
			body.SetVelocity(-minHeight, Axes.Y);
		}
		else if (gravity.Angle == 0)
		{
			body.SetVelocity(-minHeight, Axes.X);
		}
		else
		{
			Vector3 velocity = body.velocity.Rotate(-gravity.Angle + 90);
			velocity.y = minHeight;
			velocity = velocity.Rotate(gravity.Angle - 90);

			body.SetVelocity(velocity);
		}
	}

	public override void Update()
	{
		throw new NotImplementedException();
	}

	/*public override void Update()
	{
		raySettings.angle = gravity.Angle - 90;
		ground = raySettings.GetGround(transform.position, Vector3.down, debug);
		grounded = ground != null;

		if (jumping)
		{
			keepJumping();
		}

		HandleMovementX();

	}

	private void keepJumping()
	{
		
		if (counter > 0)
		{
			counter -= Time.fixedDeltaTime;
			body.Accelerate(direction * increment * (counter / duration), Axes.XY);
		}
		else
		{
			//SwitchState("Falling");
		}
	}


	private void HandleMovementX() {
		if (countDownLastMouvementX > 0)
			countDownLastMouvementX -= Time.deltaTime;

		if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
			handleDash();
			transform.localRotation = Quaternion.Euler(0, 180, 0);
		} else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
			handleDash();
			transform.localRotation = Quaternion.Euler(0, 0, 0);
		}

		if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow)) {
			PlayAnimation("Idle");
			dashing = false;
		} else if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow)) {
			PlayAnimation("Idle");
			dashing = false;
		}

		float xMovement = 0;
		if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
			xMovement = -1;
		} else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
			xMovement = 1;
		}

		//transform.position += new Vector3(xMovement, 0, 0) * speed * Time.deltaTime;
		if (xMovement != 0) 
			{
			float speed = (dashing) ? dashSpeed : walkSpeed;
			moveX(xMovement, speed);

		} else {
			moveX(xMovement, 0);
		}
		
	}


	private void moveX(float direction, float speed)
	{
		float currentSpeed = direction * speed;// * (1F / Mathf.Max(Mathf.Sqrt(Layer.Friction), 0.0001F));
		//float currentAcceleration = Mathf.Max(Layer.Friction, 0.0001F) * acceleration;
		float currentAcceleration = 1;

		if (gravity.Angle == 90)
		{
			body.AccelerateTowards(currentSpeed, currentAcceleration, Axes.X);
			moveVelocity = body.velocity.x;
		}
		else if (gravity.Angle == 180)
		{
			body.AccelerateTowards(-currentSpeed, currentAcceleration, Axes.Y);
			moveVelocity = body.velocity.y;
		}
		else if (gravity.Angle == 270)
		{
			body.AccelerateTowards(-currentSpeed, currentAcceleration, Axes.X);
			moveVelocity = body.velocity.x;
		}
		else if (gravity.Angle == 0)
		{
			body.AccelerateTowards(currentSpeed, currentAcceleration, Axes.Y);
			moveVelocity = body.velocity.y;
		}
		else
		{
			Vector3 velocity = body.velocity.Rotate(gravity.Angle + 90);
			velocity.x = Mathf.Lerp(velocity.x, currentSpeed, Time.fixedDeltaTime * currentSpeed * currentAcceleration);
			moveVelocity = velocity.x;
			velocity = velocity.Rotate(gravity.Angle - 90);

			body.SetVelocity(velocity);
		}
	}


	private void handleDash()
	{
		if (countDownLastMouvementX > 0)
		{
			if (jumping)
			{

			} else {
				dashing = true;
				PlayAnimation("Walk");
			}
			
		} else {
			countDownLastMouvementX = dashTimeBetweenTaps;
			PlayAnimation("Walk");
		}
	}*/


}
