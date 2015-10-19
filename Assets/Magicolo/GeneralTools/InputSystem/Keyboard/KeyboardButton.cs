using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;
using Magicolo.GeneralTools;

namespace Magicolo
{
	[System.Serializable]
	public class KeyboardButton : ButtonBase
	{
		public KeyboardButton(string name, KeyCode key) : base(name, key)
		{

		}
	}
}