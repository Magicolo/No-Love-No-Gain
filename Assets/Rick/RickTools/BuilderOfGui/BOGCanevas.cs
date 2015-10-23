using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BOGCanevas : MonoBehaviour {

	public List<BOGPanel> panels = new List<BOGPanel>();

	public void handleCommand(string actionCommand) {
		//throw new System.NotImplementedException();
		
	}

	public void switchPanels(BOGPanel panelFrom, BOGPanel panelTo) {
		panelFrom.gameObject.SetActive(false);
		panelTo.gameObject.SetActive(true);
	}
}
