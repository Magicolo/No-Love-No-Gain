using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Magicolo;

public class PlayerFist : MonoBehaviourExtended
{
	public enum FistStates
	{
		Idle,
		Punching
	}

	public Player Player;
	public SpriteRenderer Sprite;
	public Collider2D Collider;

	FistStates _state;
	float _punchCounter;
	Vector2 _targetPosition;

	void Start()
	{
		SetState(FistStates.Idle);
	}

	void Update()
	{
		switch (_state)
		{
			case FistStates.Idle:
				break;
			case FistStates.Punching:
				UpdatePunching();
				break;
		}

		transform.localPosition = transform.localPosition.Lerp(_targetPosition.x, Kronos.Player.DeltaTime * Player.Stats.AttackSpeed * 2f, Axes.X);
	}

	void UpdatePunching()
	{
		_punchCounter -= Kronos.Player.DeltaTime;

		if (_punchCounter <= 0f)
			SetState(FistStates.Idle);
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		Player.Punch(collision);

		SetState(FistStates.Idle);
	}

	void SetState(FistStates state)
	{
		_state = state;

		switch (_state)
		{
			case FistStates.Idle:
				Collider.enabled = false;
				_targetPosition.x = 0f;
				break;
			case FistStates.Punching:
				Collider.enabled = true;
				_punchCounter = 0.5f / Player.Stats.AttackSpeed;
				break;
		}
	}

	public void Punch()
	{
		_targetPosition = _targetPosition.SetValues(Player.Stats.Range, Axes.X);

		SetState(FistStates.Punching);
	}
}
