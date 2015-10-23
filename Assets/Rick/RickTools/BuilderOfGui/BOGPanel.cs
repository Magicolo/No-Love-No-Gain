using UnityEngine;
using System.Collections;

public class BOGPanel : MonoBehaviour {

	public BOGCanevas canvas;
	public BOGPanel previousPanel;
	
	void Start () {
	
	}
	
	void Update () {
	
	}
	
	public void handleCommand(string actionCommand) {
		if(actionCommand.Equals("Back")){
			canvas.switchPanels(this,previousPanel);
		}
	}
}
