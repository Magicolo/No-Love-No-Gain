using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;
using Magicolo.GeneralTools;

namespace Magicolo
{
	[System.Serializable]
	public class KeyboardAxis : AxisBase
	{
		public KeyboardAxis(string name, string axis) : base(name, axis)
		{
		}
	}
}