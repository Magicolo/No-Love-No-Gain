using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo {
	[System.Serializable]  
	public class KeyboardButton {

		[SerializeField] string name = "";
		public string Name {
			get {
				return name;
			}
		}

		[SerializeField] KeyCode key;
		public KeyCode Key {
			get {
				return key;
			}
			set {
				key = value;
			}
		}
		
		public KeyboardButton(string name, KeyCode button) {
			this.name = name;
			this.key = button;
		}
	}
}