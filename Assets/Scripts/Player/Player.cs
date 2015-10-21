using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Magicolo;

public class Player : MonoBehaviourExtended
{
	[Min]
	public float MoveSpeed;
	[Min]
	public float MoveAcceleration;
	[Min(BeforeSeparator = true)]
	public float JumpMinHeight = 1;
	[Min]
	public float JumpMaxHeight = 30;
	[Min]
	public float JumpDuration = 0.25F;
	public GroundCastSettings2D GroundedSettings;
	[Empty(BeforeSeparator = true)]
	public Rigidbody2D Rigidbody;
	public Gravity2D Gravity;
	public Animator Animator;
	public SpriteRenderer Renderer;
	public InputHandler Input;

	float _motionX;
	float _currentSpeed;
	float _jumpCounter;
	float _jumpIncrement;
	Vector2 _jumpDirection;

	public bool IsMoving { get; set; }
	public bool IsGrounded { get; set; }
	public bool IsJumping { get; set; }

	void Update()
	{

	}

	void FixedUpdate()
	{
		UpdateGrounded();
		UpdateMotion();
		UpdateJump();
	}

	void UpdateGrounded()
	{
		GroundedSettings.Angle = Gravity.Angle - 90;
		IsGrounded = GroundedSettings.HasHit(transform.position, Vector3.down, Application.isEditor);
	}

	void UpdateMotion()
	{
		_currentSpeed = Input.GetAxis("MotionX") * MoveSpeed;

		if (Gravity.Angle == 90f)
			Rigidbody.AccelerateTowards(_currentSpeed, MoveAcceleration, Kronos.Player.FixedDeltaTime, axes: Axes.X);
		else if (Gravity.Angle == 180f)
			Rigidbody.AccelerateTowards(-_currentSpeed, MoveAcceleration, Kronos.Player.FixedDeltaTime, axes: Axes.Y);
		else if (Gravity.Angle == 270f)
			Rigidbody.AccelerateTowards(-_currentSpeed, MoveAcceleration, Kronos.Player.FixedDeltaTime, axes: Axes.X);
		else if (Gravity.Angle == 0f)
			Rigidbody.AccelerateTowards(_currentSpeed, MoveAcceleration, Kronos.Player.FixedDeltaTime, axes: Axes.Y);
		else
		{
			Vector3 relativeVelocity = Gravity.WorldToRelative(Rigidbody.velocity);
			relativeVelocity.x = Mathf.Lerp(relativeVelocity.x, _currentSpeed, Kronos.Player.FixedDeltaTime * MoveAcceleration);

			Rigidbody.SetVelocity(Gravity.RelativeToWorld(relativeVelocity));
		}
	}

	void UpdateJump()
	{
		IsJumping = Input.GetButtonPressed("Jump");

		if (IsGrounded && Input.GetButtonDown("Jump"))
			Jump();

		if (!IsJumping)
			return;

		_jumpCounter -= Time.fixedDeltaTime;

		if (_jumpCounter > 0)
			Rigidbody.Accelerate(_jumpDirection * _jumpIncrement * (_jumpCounter / JumpDuration), Axes.XY);
	}

	public void Jump()
	{
		if (Gravity.Angle == 90)
			Rigidbody.SetVelocity(JumpMinHeight, Axes.Y);
		else if (Gravity.Angle == 180)
			Rigidbody.SetVelocity(JumpMinHeight, Axes.X);
		else if (Gravity.Angle == 270)
			Rigidbody.SetVelocity(-JumpMinHeight, Axes.Y);
		else if (Gravity.Angle == 0)
			Rigidbody.SetVelocity(-JumpMinHeight, Axes.X);
		else
		{
			Vector2 relativeVelocity = Gravity.WorldToRelative(Rigidbody.velocity);
			relativeVelocity.y = JumpMinHeight;
			Rigidbody.SetVelocity(Gravity.RelativeToWorld(relativeVelocity));
		}

		_jumpIncrement = (JumpMaxHeight - JumpMinHeight) / JumpDuration;
		_jumpDirection = -Gravity.Direction;
		_jumpCounter = JumpDuration;
	}
}
