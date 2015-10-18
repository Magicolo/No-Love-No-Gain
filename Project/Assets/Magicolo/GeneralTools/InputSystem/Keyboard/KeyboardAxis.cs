using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo {
	[System.Serializable]  
	public class KeyboardAxis {

		[SerializeField] string name = "";
		public string Name {
			get {
				return name;
			}
		}
    	
		[SerializeField] string axis;
		public string Axis {
			get {
				return axis;
			}
			set {
				axis = value;
				lastValue = 0;
			}
		}
		
		float lastValue;
		public float LastValue {
			get {
				return lastValue;
			}
			set {
				lastValue = value;
			}
		}
		
		public KeyboardAxis(string name, string axis) {
			this.name = name;
			this.axis = axis;
		}
	}
}