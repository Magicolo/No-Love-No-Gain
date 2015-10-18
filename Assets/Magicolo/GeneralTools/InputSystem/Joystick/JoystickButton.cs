using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo {
	[System.Serializable]
	public class JoystickButton {

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
				
				key = InputSystem.JoystickInputToKey(joystick, button);
			}
		}

		[SerializeField, PropertyField] JoystickButtons button;
		public JoystickButtons Button {
			get {
				return button;
			}
			set {
				button = value;
				
				key = InputSystem.JoystickInputToKey(joystick, button);
			}
		}

		[SerializeField] KeyCode key;
		public KeyCode Key {
			get {
				return key;
			}
			set {
				key = value;
				
				joystick = InputSystem.KeyToJoystick(key);
				button = InputSystem.KeyToJoystickButton(key);
			}
		}
		
		public JoystickButton(string name, Joysticks joystick, JoystickButtons button) {
			this.name = name;
			this.joystick = joystick;
			this.button = button;
			
			key = InputSystem.JoystickInputToKey(joystick, button);
		}
		
		public JoystickButton(string name, KeyCode key) {
			this.name = name;
			this.key = key;
			
			joystick = InputSystem.KeyToJoystick(key);
			button = InputSystem.KeyToJoystickButton(key);
		}
	}
}