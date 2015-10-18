using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo.GeneralTools;

namespace Magicolo {
	public interface IState : IStateMachineCallable, IStateMachineStateable, IStateMachineSwitchable {
		
		IStateLayer Layer { get; }
		IStateMachine Machine { get; }
	}
}

