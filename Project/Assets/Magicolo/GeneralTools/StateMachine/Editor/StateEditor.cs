using System;
using Magicolo;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Magicolo.EditorTools;

namespace Magicolo.GeneralTools {
	[CustomEditor(typeof(State), true), CanEditMultipleObjects]
	public class StateEditor : CustomEditorBase {

		State state;
		
		public override void OnEnable() {
			base.OnEnable();
			
			state = (State)target;
			
			if (state.Machine == null) {
				Type layerType = StateMachineUtility.GetLayerTypeFromState(state);
				StateMachine machine = state.GetOrAddComponent<StateMachine>();
				StateMachineUtility.AddLayer(machine, layerType, machine);
			}
		}
	}
}