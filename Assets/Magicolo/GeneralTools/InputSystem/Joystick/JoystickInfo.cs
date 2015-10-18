using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;
using Magicolo.GeneralTools;

namespace Magicolo {
	[System.Serializable]
	public class JoystickInfo : ControllerInfo {

		[SerializeField, PropertyField] Joysticks joystick;
		public Joysticks Joystick {
			get {
				return joystick;
			}
			set {
				joystick = value;
				
				UpdateButtons();
				UpdateAxes();
			}
		}
		
		[SerializeField] List<JoystickButton> buttons = new List<JoystickButton>();
		[SerializeField] List<JoystickAxis> axes = new List<JoystickAxis>();
		
		Dictionary<string, List<JoystickButton>> nameButtonDict;
		Dictionary<string, List<JoystickButton>> NameButtonDict {
			get {
				if (nameButtonDict == null) {
					BuildNameButtonDict();
				}
				
				return nameButtonDict;
			}
		}
		
		Dictionary<string, List<JoystickAxis>> nameAxisDict;
		Dictionary<string, List<JoystickAxis>> NameAxisDict {
			get {
				if (nameAxisDict == null) {
					BuildNameAxisDict();
				}
				
				return nameAxisDict;
			}
		}
		
		public JoystickInfo(string name, Joysticks joystick, JoystickButton[] buttons, JoystickAxis[] axes, IInputListener[] listeners)
			: base(name, listeners) {
			
			this.joystick = joystick;
			this.buttons = new List<JoystickButton>(buttons);
			this.axes = new List<JoystickAxis>(axes);
		}
		
		public void UpdateInput() {
			if (!HasListeners()) {
				return;
			}
			
			foreach (string buttonName in NameButtonDict.Keys) {
				foreach (JoystickButton button in NameButtonDict[buttonName]) {
					if (Input.GetKeyDown(button.Key)) {
						SendInput(button.Name, ButtonStates.Down);
						break;
					}
					
					if (Input.GetKeyUp(button.Key)) {
						SendInput(button.Name, ButtonStates.Up);
						break;
					}
					
					if (Input.GetKey(button.Key)) {
						SendInput(button.Name, ButtonStates.Pressed);
						break;
					}
				}
			}
			
			foreach (JoystickAxis axis in axes) {
				float currentValue = Input.GetAxis(axis.AxisName);
					
				if ((axis.LastValue != 0 && currentValue == 0) || currentValue - axis.LastValue != 0) {
					axis.LastValue = currentValue;
					
					SendInput(axis.Name, currentValue);
				}
			}
		}
		
		public JoystickButton[] GetButtons() {
			return buttons.ToArray();
		}
		
		public JoystickButton[] GetButtons(string buttonName) {
			return NameButtonDict[buttonName].ToArray();
		}
		
		public string[] GetButtonNames() {
			return NameButtonDict.GetKeyArray();
		}
		
		public void SetButtons(JoystickButton[] buttons) {
			this.buttons = new List<JoystickButton>(buttons);
			
			BuildNameButtonDict();
		}
		
		public void CopyButtons(JoystickInfo info) {
			SetButtons(info.GetButtons());
		}
		
		public void SwitchButtons(JoystickInfo info) {
			JoystickButton[] otherButtons = info.GetButtons();
			
			info.SetButtons(GetButtons());
			SetButtons(otherButtons);
		}
		
		public void AddButton(JoystickButton button) {
			buttons.Add(button);
			
			if (!NameButtonDict.ContainsKey(button.Name)) {
				NameButtonDict[button.Name] = new List<JoystickButton>();
			}

			NameButtonDict[button.Name].Add(button);
		}
		
		public void AddButtons(JoystickButton[] buttons) {
			foreach (JoystickButton button in buttons) {
				AddButton(button);
			}
		}
		
		public void RemoveButton(JoystickButton button) {
			buttons.Remove(button);
			
			if (NameButtonDict.ContainsKey(button.Name)) {
				NameButtonDict[button.Name].Remove(button);
			}
		}
	
		public void RemoveButtons(JoystickButton[] buttons) {
			foreach (JoystickButton button in buttons) {
				RemoveButton(button);
			}
		}
		
		public JoystickAxis[] GetAxes() {
			return axes.ToArray();
		}
		
		public JoystickAxis[] GetAxes(string axisName) {
			return NameAxisDict[axisName].ToArray();
		}
		
		public string[] GetAxisNames() {
			return NameAxisDict.GetKeyArray();
		}
		
		public void SetAxes(JoystickAxis[] axes) {
			this.axes = new List<JoystickAxis>(axes);
			
			BuildNameAxisDict();
		}
		
		public void CopyAxes(JoystickInfo info) {
			SetAxes(info.GetAxes());
		}

		public void SwitchAxes(JoystickInfo info) {
			JoystickAxis[] otherAxes = info.GetAxes();
			
			info.SetAxes(GetAxes());
			SetAxes(otherAxes);
		}
		
		public void AddAxis(JoystickAxis axis) {
			axes.Add(axis);
			
			if (!NameAxisDict.ContainsKey(axis.Name)) {
				NameAxisDict[axis.Name] = new List<JoystickAxis>();
			}

			NameAxisDict[axis.Name].Add(axis);
		}
		
		public void AddAxes(JoystickAxis[] axes) {
			foreach (JoystickAxis axis in axes) {
				AddAxis(axis);
			}
		}
		
		public void RemoveAxis(JoystickAxis axis) {
			axes.Remove(axis);
			
			if (NameAxisDict.ContainsKey(axis.Name)) {
				NameAxisDict[axis.Name].Remove(axis);
			}
		}

		public void RemoveAxes(JoystickAxis[] axes) {
			foreach (JoystickAxis axis in axes) {
				RemoveAxis(axis);
			}
		}
		
		public void CopyInput(JoystickInfo info) {
			CopyButtons(info);
			CopyAxes(info);
		}
		
		public void SwitchInput(JoystickInfo info) {
			SwitchButtons(info);
			SwitchAxes(info);
		}
		
		void UpdateButtons() {
			foreach (JoystickButton button in buttons) {
				button.Joystick = Joystick;
//				button.Key = InputSystem.GetKeyFromJoystickInput(Joystick, InputSystem.GetJoystickButtonFromKey(button.Key));
			}
		}

		void UpdateAxes() {
			foreach (JoystickAxis axis in axes) {
				axis.Joystick = Joystick;
//				axis.AxisName = InputSystem.GetAxisFromJoystickInput(Joystick, InputSystem.GetJoystickAxisFromAxis(axis.AxisName));
			}
		}
		
		void BuildNameButtonDict() {
			nameButtonDict = new Dictionary<string, List<JoystickButton>>();
			
			foreach (JoystickButton button in buttons) {
				if (!nameButtonDict.ContainsKey(button.Name)) {
					nameButtonDict[button.Name] = new List<JoystickButton>();
				}
				
				nameButtonDict[button.Name].Add(button);
			}
		}

		void BuildNameAxisDict() {
			nameAxisDict = new Dictionary<string, List<JoystickAxis>>();
			
			foreach (JoystickAxis axis in axes) {
				if (!nameAxisDict.ContainsKey(axis.Name)) {
					nameAxisDict[axis.Name] = new List<JoystickAxis>();
				}
				
				nameAxisDict[axis.Name].Add(axis);
			}
		}
	}
}