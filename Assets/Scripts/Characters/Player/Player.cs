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
	public PlayerFist[] Fists;
	[Empty(BeforeSeparator = true)]
	public Rigidbody2D Rigidbody;
	public Gravity2D Gravity;
	public Animator Animator;
	public InputHandler InputHandler;

	float _currentSpeed;
	int _direction = 1;
	float _jumpCounter;
	float _jumpIncrement;
	bool _jumpButtonPressed;
	Vector2 _jumpDirection;
	float _attackSpeedCounter;
	int _currentFist;
	int _isMovingHash = Animator.StringToHash("IsMoving");
	int _isGroundedHash = Animator.StringToHash("IsGrounded");
	int _isJumpingHash = Animator.StringToHash("IsJumping");
	int _isFallingHash = Animator.StringToHash("IsFalling");
	int _isPunchingHash = Animator.StringToHash("IsPunching");
	int _isHurtHash = Animator.StringToHash("IsHurt");
	int _isHuggingHash = Animator.StringToHash("IsHugging");

	public bool IsMoving { get { return Mathf.Abs(_currentSpeed) > Stats.MoveSpeed / 1000f; } }
	public bool IsGrounded { get; set; }
	public bool IsJumping { get { return _jumpCounter > 0f && _jumpButtonPressed; } }
	public bool IsFalling { get { return !IsJumping && !IsGrounded; } }
	public bool IsPunching { get; set; }
	public bool IsHurt { get; set; }
	public bool IsHugging { get; set; }

	void Start()
	{
		Health = Stats.MaxHealth;
	}

	void Update()
	{
		UpdatePunch();
		UpdateAnimator();
	}

	void FixedUpdate()
	{
		UpdateGrounded();
		UpdateMotion();
		UpdateJump();
	}

	void UpdatePunch()
	{
		_attackSpeedCounter -= Kronos.Player.DeltaTime;

		if (_attackSpeedCounter <= 0f && InputHandler.GetButtonDown("Punch"))
		{
			Vector3 targetPosition = transform.position + new Vector3(Stats.Range * _direction, 0f, 0f);
			Fists[_currentFist].Punch(targetPosition);
			_attackSpeedCounter = 1f / Stats.AttackSpeed;
			_currentFist = (_currentFist + 1) % Fists.Length;
			IsPunching = true;
		}

		IsPunching &= _attackSpeedCounter > 0f;
	}

	void UpdateGrounded()
	{
		GroundedSettings.Angle = Gravity.Angle - 90;
		IsGrounded = _jumpCounter <= 0f && GroundedSettings.HasHit(transform.position, Vector3.down, Application.isEditor);
	}

	void UpdateMotion()
	{
		_currentSpeed = InputHandler.GetAxis("MotionX") * Stats.MoveSpeed;
		_direction = _currentSpeed == 0f ? _direction : _currentSpeed.Sign();

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

		transform.RotateLocalTowards(_direction == 1 ? 0f : 180f, Kronos.Player.DeltaTime * Stats.RotateSpeed, axes: Axes.Y);
	}

	void UpdateJump()
	{
		_jumpButtonPressed = InputHandler.GetButtonPressed("Jump");

		if (IsGrounded && InputHandler.GetButtonDown("Jump"))
			Jump();

		if (_jumpButtonPressed)
			_jumpCounter -= Time.fixedDeltaTime;
		else
			_jumpCounter = 0f;

		if (_jumpCounter > 0f)
			Rigidbody.Accelerate(_jumpDirection * _jumpIncrement * (_jumpCounter / Stats.JumpMaxDuration), Kronos.Player.FixedDeltaTime, Axes.XY);
	}

	void UpdateAnimator()
	{
		Animator.SetBool(_isMovingHash, IsMoving);
		Animator.SetBool(_isGroundedHash, IsGrounded);
		Animator.SetBool(_isJumpingHash, IsJumping);
		Animator.SetBool(_isFallingHash, IsFalling);
		Animator.SetBool(_isPunchingHash, IsPunching);
		Animator.SetBool(_isHurtHash, IsHurt);
		Animator.SetBool(_isHuggingHash, IsHugging);
	}

	Vector2 GetKnockback(Transform target)
	{
		return (target.position - transform.position).normalized * Stats.Knockback;
	}

	public void Punch(Collider2D collision)
	{
		IDamageable damageable = collision.FindComponent<IDamageable>();

		if (damageable != null && damageable.CanBeDamagedBy(DamageSources.Player))
			damageable.Damage(Stats.Damage, DamageSources.Player, GetKnockback(collision.transform));
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

		IsGrounded = false;
	}

	public override void Damage(float damage, DamageSources damageSource, Vector2 knockback = default(Vector2))
	{
		base.Damage(damage, damageSource, knockback);

		Rigidbody.AddForce(knockback);
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