using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

[RequireComponent(typeof(InputSystem), typeof(Animator), typeof(Gravity2D))]
public class Character2DMotion : StateLayer
{
	public GroundCastSettings2D RaySettings;

	[Disable]
	public Collider2D Ground;

	[SerializeField, Disable]
	float _horizontalAxis;
	public float HorizontalAxis
	{
		get { return _horizontalAxis; }
		set
		{
			_horizontalAxis = value;
			Animator.SetFloat(AbsHorizontalAxisHash, AbsHorizontalAxis);
		}
	}

	public float AbsHorizontalAxis { get { return Mathf.Abs(_horizontalAxis); } }

	[SerializeField, Disable]
	float _moveVelocity;
	public float MoveVelocity
	{
		get { return _moveVelocity; }
		set
		{
			_moveVelocity = value;
			Animator.SetFloat(AbsMoveVelocityHash, AbsMoveVelocity);
		}
	}

	public float AbsMoveVelocity { get { return Mathf.Abs(_moveVelocity); } }

	[SerializeField, Disable]
	bool _grounded;
	public bool Grounded
	{
		get { return _grounded; }
		set
		{
			if (_grounded != value)
			{
				_grounded = value;
				Animator.SetBool(GroundedHash, _grounded);
			}
		}
	}

	[SerializeField, Disable]
	bool _jumping;
	public bool Jumping
	{
		get { return _jumping; }
		set
		{
			if (_jumping != value)
			{
				_jumping = value;
				Animator.SetBool(JumpingHash, _jumping);
			}
		}
	}

	[SerializeField, Disable]
	float _friction = 1f;
	public float Friction { get { return _friction; } set { _friction = value; } }

	[Disable]
	public int GroundedHash = Animator.StringToHash("Grounded");
	[Disable]
	public int JumpingHash = Animator.StringToHash("Jumping");
	[Disable]
	public int AbsMoveVelocityHash = Animator.StringToHash("AbsMoveVelocity");
	[Disable]
	public int AbsHorizontalAxisHash = Animator.StringToHash("AbsHorizontalAxis");

	bool _animatorCached;
	Animator _animator;
	public Animator Animator
	{
		get
		{
			_animator = _animatorCached ? _animator : this.FindComponent<Animator>();
			_animatorCached = true;
			return _animator;
		}
	}

	bool _rigidbodyCached;
	Rigidbody2D _rigidbody;
	public Rigidbody2D Rigidbody
	{
		get
		{
			_rigidbody = _rigidbodyCached ? _rigidbody : this.FindComponent<Rigidbody2D>();
			_rigidbodyCached = true;
			return _rigidbody;
		}
	}

	bool _inputSystemCached;
	InputSystem _inputSystem;
	public InputSystem InputSystem
	{
		get
		{
			_inputSystem = _inputSystemCached ? _inputSystem : this.FindComponent<InputSystem>();
			_inputSystemCached = true;
			return _inputSystem;
		}
	}

	bool _gravityCached;
	Gravity _gravity;
	public Gravity Gravity
	{
		get
		{
			_gravity = _gravityCached ? _gravity : this.FindComponent<Gravity>();
			_gravityCached = true;
			return _gravity;
		}
	}

	public override void OnUpdate()
	{
		base.OnUpdate();

		RaySettings.Angle = Gravity.Angle - 90;
		Ground = RaySettings.GetGround(transform.position, Vector3.down);

		if (Ground == null)
			Grounded = false;
		else
		{
			Grounded = true;
			Friction = Ground.sharedMaterial == null ? 1 : Ground.sharedMaterial.friction;
		}
	}

	public void Enable()
	{
		IState[] states = GetActiveStates();

		for (int i = 0; i < states.Length; i++)
			states[i].SwitchState("Idle");
	}

	public void Disable()
	{
		IState[] states = GetActiveStates();

		for (int i = 0; i < states.Length; i++)
			states[i].SwitchState<EmptyState>();
	}
}
