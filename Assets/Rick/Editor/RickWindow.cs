using Magicolo.EditorTools;
using UnityEditor;
using UnityEngine;
using Magicolo;
using UnityEngine.UI;

namespace RickEditor.Editor{
    public class RickWindow<T> : CustomWindowBase<T> where T : RickWindow<T> 
    {
    
	
		bool inited = false;
		public GUIStyle simpleButton;
		public GUIStyle simpleSelect;
		public GUIStyle prefixLabelStyle;
		public GUIStyle selectStyle;
		
		public void OnGUI(){
			if(!inited){
				inited = true;
				initStyle();
			}
		}
		
		void OnEnable(){
			inited = false; // TEMPORAIRE POUR DEBUGER LE STRYLE PLUS VITE TSER YO
		}

		void initStyle() {
			simpleButton = new GUIStyle(GUI.skin.button);
			simpleButton.wordWrap = false;
			simpleButton.stretchWidth = false;
			
			prefixLabelStyle = new GUIStyle("boldLabel");
			
			selectStyle = new GUIStyle("popup");
			selectStyle.wordWrap = true;
		}
		
		
		protected bool makeButton(string text){
			return GUILayout.Button(new GUIContent(text), simpleButton);
		}
		
		protected int makeSelect(string title, int selectedIndex, Object[] objects){
			string[] names = new string[objects.Length];
			int index = 0;
			foreach (var obj in objects) {
				string name = obj.name;
				for (int i = 0; i < index; i++) {
					if(names[i].Equals(name)){
						name = name + " (" + index + ")" ;
						break;
					}
				}
				names[ index++ ] = name;
			}
			return RickEditorGUI.Popup(title, selectedIndex, names);
		}
		
		protected void makePrefix(string text){
			EditorGUILayout.PrefixLabel(text);
		}
		
		protected string makeField(string title, string value){
			return EditorGUILayout.TextField(title,value);
		}
		
		protected bool makeBool(string title, bool value){
			return EditorGUILayout.Toggle(title, value);
		}
			
		protected R makeObjectField<R>(string title, Object obj, bool allowSceneOject = true) where R : Object{
			return (R) EditorGUILayout.ObjectField(title, obj,typeof(R),allowSceneOject);
		}
		
		public void makeHelp(string text){
			EditorGUILayout.HelpBox(text, MessageType.Info);
		}
		
		protected int makeSelect(string title, int selectedIndex, string[] choicesText){
			/*Rect rect = getBaseRect();
			rect = EditorGUI.PrefixLabel(rect, new GUIContent(title), prefixLabelStyle);*/
			
		//	int newSelectedIndex = EditorGUI.Popup(rect,selectedIndex,choicesText,selectStyle);
			int newSelectedIndex = EditorGUILayout.Popup(title,selectedIndex,choicesText,selectStyle);
			//EditorGUILayout.Popup(
			
			return newSelectedIndex;
		}
		
		protected void makeSeparator(){
			CustomEditorBase.Separator();
		}
		
		
		protected void BeginHorizontal(){
			GUILayout.BeginHorizontal();
		}
		
		protected void EndHorizontal(){
			GUILayout.EndHorizontal();
		}
		
		
		static Rect getBaseRect(){
			Rect rect = EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(" ");
			EditorGUILayout.EndHorizontal();
			return rect;
		}
	
		
		
	}
}