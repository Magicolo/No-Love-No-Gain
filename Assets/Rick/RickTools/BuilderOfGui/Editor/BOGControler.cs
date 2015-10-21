#if UNITY_EDITOR 
using System;
using System.Xml;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using UnityEditor;
using Magicolo;
using UnityEngine.UI;
using Magicolo.EditorTools;
using RickEditor.Editor;

namespace RickTools.BOG{
	[System.Serializable]
	public class BOGControler {
		
		public bool disableAllPanelExceptWorkingOne;
		
		public BOGCanevas currentCanvas;
		public BOGPanel currentPanel;
		
		//Panel's buttons
		public BOGButton closeButton;
		public BOGButton backButton;
		

	
	
		public void switchToCanvas(BOGCanevas canvas) {
			currentCanvas = canvas;
		}

		public int createNewBOGCanvas() {
			GameObject newCanvas = instantiatePrefab("Rick/Canvas");
			switchToCanvas(newCanvas.GetComponent<BOGCanevas>());
			currentPanel = null;
			
			var canvas = UnityEngine.Object.FindObjectsOfType<BOGCanevas>();
			int index = -1;
			while( canvas[++index] !=  newCanvas.GetComponent<BOGCanevas>() );
			return index;
		}

		public void switchToPanel(BOGPanel bOGPanel) {
			if(disableAllPanelExceptWorkingOne && currentPanel != null){
				currentPanel.gameObject.SetActive(false);
			}
			currentPanel = bOGPanel;
			currentPanel.gameObject.SetActive(true);
		}

		public int createNewBOGPanel() {
			GameObject newPanel = instantiatePrefab("Rick/Panel");
			
			setTransformStuff(newPanel, currentCanvas.transform, Vector2.zero, Vector2.one );
			BOGPanel bogPanel = newPanel.GetComponent<BOGPanel>();
			
			currentCanvas.panels.Add(bogPanel);
			bogPanel.canvas = currentCanvas;
			switchToPanel(bogPanel);
			
			var panels = currentCanvas.gameObject.GetComponentsInChildren<BOGPanel>();
			int index = -1;
			while( panels[++index] !=  newPanel.GetComponent<BOGPanel>() );
			return index;
		}

		public void createCloseButton() {
			crateButton("Close");
		}

		public void createBackButton() {
			crateButton("Back");
		}
		
		public BOGButton crateButton(string actionCommand){
			GameObject buttonObj = instantiatePrefab("Rick/Button");
			
			setTransformStuff(buttonObj, currentPanel.transform, Vector2.zero );
			
			Button button = buttonObj.GetComponent<Button>();
			BOGButton bogButton = buttonObj.GetComponent<BOGButton>();
			bogButton.panel = currentPanel;
			bogButton.makeUnityButtonConnection(actionCommand);
			return bogButton;
		}

		void setTransformStuff(GameObject obj, Transform parent, Vector2 anchoredPosition) {
			obj.transform.SetParent(parent);
		
			RectTransform recTransform = obj.transform.GetComponent<RectTransform>();
			recTransform.position = new Vector3(0,0,0);
			recTransform.anchoredPosition = anchoredPosition;
		}	
		void setTransformStuff(GameObject obj, Transform parent, Vector2 anchoredPosition, Vector2 sizeDelta) {
			obj.transform.SetParent(parent);
		
			RectTransform recTransform = obj.transform.GetComponent<RectTransform>();
			recTransform.position = new Vector3(0,0,0);
			recTransform.anchoredPosition = anchoredPosition;
			recTransform.sizeDelta = sizeDelta;
		}		
		
		
		GameObject instantiatePrefab(string path){
			GameObject prefab = Resources.Load<GameObject>(path);
			GameObject newObject = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
			PrefabUtility.DisconnectPrefabInstance(newObject);
			return newObject;
		}
	}
}
#endif