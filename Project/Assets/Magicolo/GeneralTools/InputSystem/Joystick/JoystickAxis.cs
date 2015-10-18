using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo {
	[System.Serializable]
	public class JoystickAxis {

		[SerializeField] string name = "";
		public string Name {
			get {
				return name;
			}
		}
    	
		[SerializeField] Joysticks joystick;
		public Joysticks Joystick {
			get {
				return joystick;
			}
			set {
				joystick = value;
				
				axisName = InputSystem.JoystickInputToAxis(joystick, axis);
				lastValue = 0;
			}
		}

		[SerializeField, PropertyField] JoystickAxes axis;
		public JoystickAxes Axis {
			get {
				return axis;
			}
			set {
				axis = value;
				
				axisName = InputSystem.JoystickInputToAxis(joystick, axis);
				lastValue = 0;
			}
		}

		[SerializeField] string axisName;
		public string AxisName {
			get {
				return axisName;
			}
			set {
				axisName = value;
				
				joystick = InputSystem.AxisToJoystick(axisName);
				axis = InputSystem.AxisToJoystickAxis(axisName);
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
		
		public JoystickAxis(string name, Joysticks joystick, JoystickAxes axis) {
			this.name = name;
			this.joystick = joystick;
			this.axis = axis;
		
			axisName = InputSystem.JoystickInputToAxis(joystick, axis);
		}
		
		public JoystickAxis(string name, string axisName) {
			this.name = name;
			this.axisName = axisName;
		
			joystick = InputSystem.AxisToJoystick(axisName);
			axis = InputSystem.AxisToJoystickAxis(axisName);
		}
	}
}