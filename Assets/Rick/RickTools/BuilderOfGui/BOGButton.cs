using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BOGButton : MonoBehaviour {
	
	public BOGPanel panel;
	public string actionCommand;

	public void makeUnityButtonConnection(string command) {
		Button button = GetComponent<Button>();
		
		actionCommand = command;
		
		#if UNITY_EDITOR
		UnityEditor.Events.UnityEventTools.AddVoidPersistentListener(button.onClick, () => panel.handleCommand(actionCommand));
		#endif
	}
	
	void Start () {
	
	}
	
	
	void Update () {
	
	}
}
