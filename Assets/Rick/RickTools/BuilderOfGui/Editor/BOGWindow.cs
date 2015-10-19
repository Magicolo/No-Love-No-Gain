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
using Rick;

namespace RickTools.BOG{
	[System.Serializable]
	public class BOGWindow : RickWindow<BOGWindow> {
		
		public BOGControler controler;
		
		public InclusiveComponentGetter<BOGCanevas> canvas = new InclusiveComponentGetter<BOGCanevas>();
		public int selectedCanvasIndex;
		public int selectedPanelIndex;
		
		[MenuItem("Rick's Tools/Builder Of Gui")]
		public static void Create() {
			BOGWindow bog = CreateWindow("Builder Of Gui", new Vector2(275, 250));
			bog.controler = new BOGControler();
		}
		
		new void OnGUI() {
			base.OnGUI();
			showBOGOption();
			showCanvasChoiceSection();
			if(controler.currentCanvas != null){
				showCanvasOptions();
				makeSeparator();
				showPanelChoiceSection();
				if(controler.currentPanel != null){
					showPanelOptions();
				}
				
			}
		}

		void showBOGOption() {
			controler.disableAllPanelExceptWorkingOne = makeBool("Disable all Panels except working one", controler.disableAllPanelExceptWorkingOne);
		}		
		
		void showCanvasChoiceSection() {
			BeginHorizontal();
			
			var sceneCanvas = canvas.getComponents();
			GUI.changed = false;
			selectedCanvasIndex = makeSelect("Working Canvas", selectedCanvasIndex, sceneCanvas);
			if(GUI.changed) controler.switchToCanvas(sceneCanvas[selectedCanvasIndex]);
			
			if(makeButton("New")){
				controler.createNewBOGCanvas();
				selectedCanvasIndex = 0;
			}
			EndHorizontal();
		}

		void showCanvasOptions() {
			controler.currentCanvas.name = makeField("Canvas name" , controler.currentCanvas.name);
		}		
		
		
		void showPanelChoiceSection() {
			BeginHorizontal();
			
			var panels = controler.currentCanvas.panels.ToArray();
			GUI.changed = false;
			selectedPanelIndex = makeSelect("Working Panel", selectedPanelIndex, panels);
			if(GUI.changed) controler.switchToPanel(panels[selectedPanelIndex]);
			
			if(makeButton("New")){
				selectedPanelIndex = controler.createNewBOGPanel();
			}
			EndHorizontal();
		}
		
		void showPanelOptions() {
			BOGPanel panel = controler.currentPanel;
			panel.name = makeField("Panel name" , panel.name);
			panel.previousPanel = makeObjectField<BOGPanel>("Previous panel", panel.previousPanel);
			showCloseButton();
			showBackButton();
		}	

		
		void showCloseButton() {
			BeginHorizontal();
			
			makePrefix("Close button");
			if(controler.closeButton == null){
				if(makeButton("Create")){
					controler.createCloseButton();
				}
			}
			
			EndHorizontal();
		}

		void showBackButton() {
			BeginHorizontal();
			
			makePrefix("Back button");
			if(controler.backButton == null){
				if(makeButton("Create")){
					controler.createBackButton();
				}
			}
			
			EndHorizontal();
		}
	}
}
#endif