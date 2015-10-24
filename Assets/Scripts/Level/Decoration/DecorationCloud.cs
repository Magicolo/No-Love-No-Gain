using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Magicolo;

[DoNotCopy]
public class DecorationCloud : MonoBehaviourExtended, IPoolable, ICopyable<DecorationCloud>
{
	public SmoothOscillate SmoothOscillate;
	public SmoothMove SmoothMove;

	public void OnCreate()
	{
		SmoothOscillate.ApplyRandomness();
		SmoothMove.ApplyRandomness();
	}

	public void OnRecycle()
	{
	}

	public void Copy(DecorationCloud reference)
	{
		CopyHelper.CopyTo(reference.SmoothOscillate, ref SmoothOscillate);
		CopyHelper.CopyTo(reference.SmoothMove, ref SmoothMove);
	}
}
