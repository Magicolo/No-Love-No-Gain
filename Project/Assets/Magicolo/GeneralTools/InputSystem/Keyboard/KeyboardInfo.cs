using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;
using Magicolo.GeneralTools;

namespace Magicolo {
	[System.Serializable]  
	public class KeyboardInfo : ControllerInfo {

		[SerializeField] List<KeyboardButton> buttons = new List<KeyboardButton>();
		[SerializeField] List<KeyboardAxis> axes = new List<KeyboardAxis>();
		
		Dictionary<string, List<KeyboardButton>> nameButtonDict;
		Dictionary<string, List<KeyboardButton>> NameButtonDict {
			get {
				if (nameButtonDict == null) {
					BuildNameButtonDict();
				}
				
				return nameButtonDict;
			}
		}
		
		Dictionary<string, List<KeyboardAxis>> nameAxisDict;
		Dictionary<string, List<KeyboardAxis>> NameAxisDict {
			get {
				if (nameAxisDict == null) {
					BuildNameAxisDict();
				}
				
				return nameAxisDict;
			}
		}
		
		public KeyboardInfo(string name, KeyboardButton[] buttons, KeyboardAxis[] axes, IInputListener[] listeners)
			: base(name, listeners) {
			
			this.buttons = new List<KeyboardButton>(buttons);
			this.axes = new List<KeyboardAxis>(axes);
		
			BuildNameButtonDict();
			BuildNameAxisDict();
		}
		
		public void UpdateInput() {
			if (!HasListeners()) {
				return;
			}
			
			foreach (string buttonName in NameButtonDict.Keys) {
				foreach (KeyboardButton button in NameButtonDict[buttonName]) {
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
			
			foreach (KeyboardAxis axis in axes) {
				float currentValue = Input.GetAxis(axis.Axis);
					
				if ((axis.LastValue != 0 && currentValue == 0) || currentValue - axis.LastValue != 0) {
					axis.LastValue = currentValue;
					
					SendInput(axis.Name, currentValue);
				}
			}
		}
		
		public KeyboardButton[] GetButtons() {
			return buttons.ToArray();
		}
		
		public KeyboardButton[] GetButtons(string buttonName) {
			return NameButtonDict[buttonName].ToArray();
		}
		
		public string[] GetButtonNames() {
			return NameButtonDict.GetKeyArray();
		}
		
		public void SetButtons(KeyboardButton[] buttons) {
			this.buttons = new List<KeyboardButton>(buttons);
			
			BuildNameButtonDict();
		}
		
		public void CopyButtons(KeyboardInfo info) {
			SetButtons(info.GetButtons());
		}
		
		public void SwitchButtons(KeyboardInfo info) {
			KeyboardButton[] otherButtons = info.GetButtons();
			
			info.SetButtons(GetButtons());
			SetButtons(otherButtons);
		}
		
		public void AddButton(KeyboardButton button) {
			buttons.Add(button);
			
			if (!NameButtonDict.ContainsKey(button.Name)) {
				NameButtonDict[button.Name] = new List<KeyboardButton>();
			}

			NameButtonDict[button.Name].Add(button);
		}
		
		public void AddButtons(KeyboardButton[] buttons) {
			foreach (KeyboardButton button in buttons) {
				AddButton(button);
			}
		}
		
		public void RemoveButton(KeyboardButton button) {
			buttons.Remove(button);
			
			if (NameButtonDict.ContainsKey(button.Name)) {
				NameButtonDict[button.Name].Remove(button);
			}
		}
	
		public void RemoveButtons(KeyboardButton[] buttons) {
			foreach (KeyboardButton button in buttons) {
				RemoveButton(button);
			}
		}
		
		public KeyboardAxis[] GetAxes() {
			return axes.ToArray();
		}
		
		public KeyboardAxis[] GetAxes(string axisName) {
			return NameAxisDict[axisName].ToArray();
		}
		
		public string[] GetAxisNames() {
			return NameAxisDict.GetKeyArray();
		}
		
		public void SetAxes(KeyboardAxis[] axes) {
			this.axes = new List<KeyboardAxis>(axes);
			
			BuildNameAxisDict();
		}
		
		public void CopyAxes(KeyboardInfo info) {
			SetAxes(info.GetAxes());
		}

		public void SwitchAxes(KeyboardInfo info) {
			KeyboardAxis[] otherAxes = info.GetAxes();
			
			info.SetAxes(GetAxes());
			SetAxes(otherAxes);
		}
		
		public void AddAxis(KeyboardAxis axis) {
			axes.Add(axis);
			
			if (!NameAxisDict.ContainsKey(axis.Name)) {
				NameAxisDict[axis.Name] = new List<KeyboardAxis>();
			}

			NameAxisDict[axis.Name].Add(axis);
		}
		
		public void AddAxes(KeyboardAxis[] axes) {
			foreach (KeyboardAxis axis in axes) {
				AddAxis(axis);
			}
		}
		
		public void RemoveAxis(KeyboardAxis axis) {
			axes.Remove(axis);
			
			if (NameAxisDict.ContainsKey(axis.Name)) {
				NameAxisDict[axis.Name].Remove(axis);
			}
		}

		public void RemoveAxes(KeyboardAxis[] axes) {
			foreach (KeyboardAxis axis in axes) {
				RemoveAxis(axis);
			}
		}
		
		public void CopyInput(KeyboardInfo info) {
			CopyButtons(info);
			CopyAxes(info);
		}
		
		public void SwitchInput(KeyboardInfo info) {
			SwitchButtons(info);
			SwitchAxes(info);
		}
		
		void BuildNameButtonDict() {
			nameButtonDict = new Dictionary<string, List<KeyboardButton>>();
			
			foreach (KeyboardButton key in buttons) {
				if (!nameButtonDict.ContainsKey(key.Name)) {
					nameButtonDict[key.Name] = new List<KeyboardButton>();
				}
				
				nameButtonDict[key.Name].Add(key);
			}
		}

		void BuildNameAxisDict() {
			nameAxisDict = new Dictionary<string, List<KeyboardAxis>>();
			
			foreach (KeyboardAxis axis in axes) {
				if (!nameAxisDict.ContainsKey(axis.Name)) {
					nameAxisDict[axis.Name] = new List<KeyboardAxis>();
				}
				
				nameAxisDict[axis.Name].Add(axis);
			}
		}
	}
}