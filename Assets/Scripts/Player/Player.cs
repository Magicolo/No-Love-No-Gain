using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Magicolo;

public class Player : DamageableBase
{
	public CharacterStats Stats;
	public GroundCastSettings2D GroundedSettings;
	[Empty(BeforeSeparator = true)]
	public Rigidbody2D Rigidbody;
	public Gravity2D Gravity;
	public Animator Animator;
	public SpriteRenderer Renderer;
	public InputHandler InputHandler;

	float _motionX;
	float _currentSpeed;
	float _jumpCounter;
	float _jumpIncrement;
	Vector2 _jumpDirection;

	public bool IsMoving { get; set; }
	public bool IsGrounded { get; set; }
	public bool IsJumping { get; set; }

	void Start()
	{
		Hp = Stats.MaxHealth;
	}

	void Update()
	{
		UpdateGrounded();
	}

	void FixedUpdate()
	{
		UpdateMotion();
		UpdateJump();
	}

	void OnTriggerEnter2D(Collider2D collision)
	{

	}

	void UpdateGrounded()
	{
		GroundedSettings.Angle = Gravity.Angle - 90;
		IsGrounded = GroundedSettings.HasHit(transform.position, Vector3.down, Application.isEditor);
	}

	void UpdateMotion()
	{
		_currentSpeed = InputHandler.GetAxis("MotionX") * Stats.MoveSpeed;

		if (Gravity.Angle == 90f)
			Rigidbody.AccelerateTowards(_currentSpeed, Stats.MoveAcceleration, Kronos.Player.FixedDeltaTime, axes: Axes.X);
		else if (Gravity.Angle == 180f)
			Rigidbody.AccelerateTowards(-_currentSpeed, Stats.MoveAcceleration, Kronos.Player.FixedDeltaTime, axes: Axes.Y);
		else if (Gravity.Angle == 270f)
			Rigidbody.AccelerateTowards(-_currentSpeed, Stats.MoveAcceleration, Kronos.Player.FixedDeltaTime, axes: Axes.X);
		else if (Gravity.Angle == 0f)
			Rigidbody.AccelerateTowards(_currentSpeed, Stats.MoveAcceleration, Kronos.Player.FixedDeltaTime, axes: Axes.Y);
		else
		{
			Vector3 relativeVelocity = Gravity.WorldToRelative(Rigidbody.velocity);
			relativeVelocity.x = Mathf.Lerp(relativeVelocity.x, _currentSpeed, Kronos.Player.FixedDeltaTime * Stats.MoveAcceleration);

			Rigidbody.SetVelocity(Gravity.RelativeToWorld(relativeVelocity));
		}
	}

	void UpdateJump()
	{
		IsJumping = InputHandler.GetButtonPressed("Jump");

		if (IsGrounded && InputHandler.GetButtonDown("Jump"))
			Jump();

		if (!IsJumping)
		{
			_jumpCounter = 0f;
			return;
		}

		_jumpCounter -= Time.fixedDeltaTime;

		if (_jumpCounter > 0)
			Rigidbody.Accelerate(_jumpDirection * _jumpIncrement * (_jumpCounter / Stats.JumpMaxDuration), Axes.XY);
	}

	public void Jump()
	{
		if (Gravity.Angle == 90)
			Rigidbody.SetVelocity(Stats.JumpMinHeight, Axes.Y);
		else if (Gravity.Angle == 180)
			Rigidbody.SetVelocity(Stats.JumpMinHeight, Axes.X);
		else if (Gravity.Angle == 270)
			Rigidbody.SetVelocity(-Stats.JumpMinHeight, Axes.Y);
		else if (Gravity.Angle == 0)
			Rigidbody.SetVelocity(-Stats.JumpMinHeight, Axes.X);
		else
		{
			Vector2 relativeVelocity = Gravity.WorldToRelative(Rigidbody.velocity);
			relativeVelocity.y = Stats.JumpMinHeight;
			Rigidbody.SetVelocity(Gravity.RelativeToWorld(relativeVelocity));
		}

		_jumpIncrement = (Stats.JumpMaxHeight - Stats.JumpMinHeight) / Stats.JumpMaxDuration;
		_jumpDirection = -Gravity.Direction;
		_jumpCounter = Stats.JumpMaxDuration;
	}

	public override bool CanBeDamagedBy(DamageSources damageSource)
	{
		switch (damageSource)
		{
			case DamageSources.Player:
				return true;
			case DamageSources.Crabs:
				return true;
			case DamageSources.Population:
				return true;
		}

		return false;
	}

	public override void Die()
	{

	}
}
