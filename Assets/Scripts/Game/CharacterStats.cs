using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Magicolo;

[Serializable]
public class CharacterStats
{
	[Min]
	public float MaxHealth = 10f;
	[Min]
	public float Damage = 5f;
	[Min]
	public float AttackSpeed = 1f;
	[Min]
	public float Range = 1f;
	[Min(BeforeSeparator = true)]
	public float MoveSpeed = 10f;
	[Min]
	public float MoveAcceleration = 10f;
	[Min(BeforeSeparator = true)]
	public float JumpMinHeight = 3f;
	[Min]
	public float JumpMaxHeight = 5f;
	[Min]
	public float JumpMaxDuration = 0.5f;
}
