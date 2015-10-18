using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo.GeneralTools {
	public interface IStateMachineStateable {

		T GetState<T>() where T : IState;
		
		IState GetState(System.Type stateType);
		
		IState GetState(string stateName);
		
		IState[] GetStates();
		
		bool ContainsState<T>() where T : IState;
		
		bool ContainsState(System.Type stateType);
		
		bool ContainsState(string stateName);
	}
}