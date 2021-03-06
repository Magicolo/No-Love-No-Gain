using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class Character3DStick : StateLayer
{
	[Layer]
	public int StickyLayer;

	public Gravity Gravity { get { return Layer.Gravity; } }
	public bool Jumping { get { return Layer.Jumping; } }
	public bool Grounded { get { return Layer.Grounded; } }
	public float HorizontalAxis { get { return Layer.HorizontalAxis; } }
	public Rigidbody Rigidbody { get { return Layer.Rigidbody; } }
	new public Character3DMotion Layer { get { return ((Character3DMotion)base.Layer); } }
}
