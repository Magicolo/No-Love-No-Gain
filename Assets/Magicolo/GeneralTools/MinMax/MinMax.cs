using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Magicolo;

[Serializable]
public struct MinMax
{
	[SerializeField]
	float _min;

	[SerializeField]
	float _max;

	public float Min { get { return _min; } set { _min = value; } }
	public float Max { get { return _max; } set { _max = value; } }

	public MinMax(float min, float max)
	{
		_min = min;
		_max = max;
	}
}
