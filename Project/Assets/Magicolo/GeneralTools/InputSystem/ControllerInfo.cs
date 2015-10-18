using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo {
	[System.Serializable]
	public abstract class ControllerInfo : INamable {

		[SerializeField] string name = "";
		public string Name {
			get {
				return name;
			}
			set {
				name = value;
			}
		}
		
		[SerializeField] List<MonoBehaviour> listenerReferences = new List<MonoBehaviour>();
		
		List<IInputListener> listeners = new List<IInputListener>();
		
		protected ControllerInfo(string name, IInputListener[] listeners) {
			this.name = name;
			this.listeners = new List<IInputListener>(listeners);
		
			SetListeners();
		}
		
		public IInputListener[] GetListeners() {
			return listeners.ToArray();
		}
		
		public void SetListeners(IInputListener[] listeners) {
			this.listeners = new List<IInputListener>(listeners);
			
			SetListeners();
		}
		
		public void SetListeners() {
			listeners = new List<IInputListener>();
			
			foreach (MonoBehaviour listenerReference in listenerReferences) {
				IInputListener listener = listenerReference as IInputListener;
				
				if (listener != null) {
					listeners.Add(listener);
				}
			}
		}
		
		public void AddListener(IInputListener listener) {
			if (!listeners.Contains(listener)) {
				listeners.Add(listener);
			}
		}
		
		public void AddListeners(IInputListener[] listeners) {
			foreach (IInputListener listener in listeners) {
				AddListener(listener);
			}
		}
		
		public void RemoveListener(IInputListener listener) {
			listeners.Remove(listener);
		}
		
		public void RemoveListeners(IInputListener[] listeners) {
			foreach (IInputListener listener in listeners) {
				RemoveListener(listener);
			}
		}
		
		public bool HasListeners() {
			return listeners.Count > 0;
		}
	
		public void CopyListeners(ControllerInfo info) {
			SetListeners(info.GetListeners());
		}
		
		public void SwitchListeners(ControllerInfo info) {
			IInputListener[] otherListeners = info.GetListeners();
			
			info.SetListeners(GetListeners());
			SetListeners(otherListeners);
		}
	
		public void SendInput(string inputName, ButtonStates state) {
			foreach (IInputListener listener in listeners.ToArray()) {
				listener.OnButtonInput(new ButtonInput(Name, inputName, state));
			}
		}
		
		public void SendInput(string inputName, float value) {
			foreach (IInputListener listener in listeners) {
				listener.OnAxisInput(new AxisInput(Name, inputName, value));
			}
		}
	}
}