using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class Character2DJumpFalling : State
{
	new public Character2DJump Layer { get { return ((Character2DJump)base.Layer); } }

	public override void OnUpdate()
	{
		base.OnUpdate();

		if (Layer.Grounded)
		{
			SwitchState("Idle");
			return;
		}
	}
}
