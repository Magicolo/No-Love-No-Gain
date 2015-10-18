using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo {
	public interface IInputListener {
		
		void OnButtonInput(ButtonInput input);
		
		void OnAxisInput(AxisInput input);
	}
}
