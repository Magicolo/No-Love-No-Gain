using Magicolo.EditorTools;
using UnityEditor;
using UnityEngine;
using Magicolo;
using UnityEngine.UI;
using System;

namespace RickEditor.Editor{
	
	public class RickEditorGUI : CustomEditorBase {
	
		public static GUIStyle prefixLabelStyle = new GUIStyle("boldLabel");
		public static GUIStyle pathButtonStyle = makePathButtonStyle();
		
		public static string rootFolder = Application.dataPath.Substring(0, Application.dataPath.Length - 6);
		public static string assetFolder = Application.dataPath;
		public static string resourcesFolder = Application.dataPath + "/Resources/";
		
		static GUIStyle makePathButtonStyle() {
			GUIStyle buttonStyle = new GUIStyle("TL SelectionButton");
			/*buttonStyle.font = EditorStyles.miniFont;
			buttonStyle.fontStyle = FontStyle.Italic;
			buttonStyle.normal.textColor = EditorStyles.label.normal.textColor;*/
			return buttonStyle;
		}


        public static void Label(GUIContent prefixLabel, GUIContent label){
			Rect rect = makePrefixLabelAndReturnRect(prefixLabel);
			
			EditorGUI.LabelField(rect, label);
		}
		
		public static void Label(string prefixLabel, string label){
			Label(new GUIContent(prefixLabel), new GUIContent(label));
		}



        public static bool CheckBox(string prefixLabel, bool value)
        {
            return CheckBox(new GUIContent(prefixLabel), value);
        }
        
        public static bool CheckBox(GUIContent prefix, bool value)
        {
            Rect rect = getBaseRect();
            rect = EditorGUI.PrefixLabel(rect, prefix, prefixLabelStyle);
            return EditorGUI.Toggle(rect, value);
        }


        public static GameObject GamebjectField(string label, GameObject gameObject, bool allowSceneObjects = true)
        {
            return (GameObject) ObjectField(label, gameObject, typeof(GameObject), allowSceneObjects);
        }

        public static UnityEngine.Object ObjectField(string prefixLabel, UnityEngine.Object obj, System.Type objType, bool allowSceneObjects){
			Rect rect = getBaseRect();
			rect = EditorGUI.PrefixLabel(rect, new GUIContent(prefixLabel), prefixLabelStyle);
            UnityEngine.Object selectedObject = EditorGUI.ObjectField(rect, obj, objType, allowSceneObjects);
			return selectedObject;
		}
		
		
		public static int Popup(string prefixLabel, int selectedIndex, string[] choicesText){
			Rect rect = getBaseRect();
			rect = EditorGUI.PrefixLabel(rect, new GUIContent(prefixLabel), prefixLabelStyle);
			
			int newSelectedIndex = EditorGUI.Popup(rect,selectedIndex,choicesText);
			
			return newSelectedIndex;
		}
		
		
		public static bool Toggle(GUIContent prefixLabel, bool value){
			Rect rect = getBaseRect();
			
			rect = EditorGUI.PrefixLabel(rect, prefixLabel, prefixLabelStyle);
			bool guiValue = EditorGUI.Toggle(rect,value);
			
			return guiValue;
		}
		public static bool Toggle(string prefixLabel, bool value){	
			return Toggle(new GUIContent(prefixLabel),value);
		}
		public static bool Toggle(string prefixLabel, string tooltip, bool value){	
			return Toggle(new GUIContent(prefixLabel,tooltip),value);
		}
		
		
		
		#region FilePath
		public static string FilePath(GUIContent label, string path, string relativeTo, string extension = null, FilePathOptions options = FilePathOptions.NONE){
			Rect rect = getBaseRect();
			
			rect = EditorGUI.PrefixLabel(rect, label, prefixLabelStyle);
			string returnPath = FilePathButton(rect, path, relativeTo, extension);
			if(!hasFilePathFlag(options, FilePathOptions.KEEP_EXTENTION) && returnPath != null)
            {
				returnPath = returnPath.Replace("." + extension, "");
			}
			
			return returnPath;
		}
		
		public static string FilePath(string labelText, string path, string relativeTo, string extension = null, FilePathOptions options = FilePathOptions.NONE){
			return FilePath(new GUIContent(labelText), path, relativeTo, extension, options);
		}
		
		static string FilePathButton(Rect rect, string path, string relativeTo, string extension) {
			if (GUI.Button(rect, path.ToGUIContent(), pathButtonStyle)) {
				path = EditorUtility.OpenFilePanel("Select File", relativeTo + path, extension);
				
				if (!string.IsNullOrEmpty(relativeTo)) {
					if (path.StartsWith(relativeTo)) {
						path = path.Substring(relativeTo.Length);
					}
					else if (!string.IsNullOrEmpty(path)) {
						Logger.LogWarning(string.Format("The relative directory ({0}) does not contain the selected file ({1}).", relativeTo, path));
						path = "";
					}
				}
			}
			
			return path ;
		}

		
		static bool hasFilePathFlag(FilePathOptions options, FilePathOptions flag) {
			return (options & flag) == flag;
		}
		
		[System.Flags]
		public enum FilePathOptions{
			NONE = 0,
			KEEP_EXTENTION = 1
		}
		#endregion
		
		
		#region FolderPath
		public static string FolderPath(string labelText, string path, string relativeTo){
			return FolderPath(new GUIContent(labelText), path, relativeTo);
		}
		
		public static string FolderPath(GUIContent label, string path, string relativeTo){
			Rect rect = getBaseRect();
			
			rect = EditorGUI.PrefixLabel(rect, label, prefixLabelStyle);
			string returnPath = FolderPathButton(rect, path,relativeTo);
			
			return returnPath;
		}
		
		private static string FolderPathButton(Rect rect, string path, string relativeTo) {
			if (GUI.Button(rect, path.ToGUIContent(), pathButtonStyle)) {
				path = EditorUtility.OpenFolderPanel("Select Folder", relativeTo + path, "");
				
				if (!string.IsNullOrEmpty(relativeTo)) {
					if (path.StartsWith(relativeTo)) {
						path = path.Substring(relativeTo.Length);
					}
					else if (!string.IsNullOrEmpty(path)) {
						Logger.LogWarning(string.Format("The relative directory ({0}) does not contain the selected folder ({1}).", relativeTo, path));
						path = "";
					}
				}
			}
			
			return path;
		}
		#endregion
		
		
		
		static Rect makePrefixLabelAndReturnRect(string prefixLabel){
			return makePrefixLabelAndReturnRect ( new GUIContent(prefixLabel) );
		}
		
		static Rect makePrefixLabelAndReturnRect(GUIContent prefixLabel){
			Rect rect = EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(" ");
			EditorGUILayout.EndHorizontal();
			rect = EditorGUI.PrefixLabel(rect, new GUIContent(prefixLabel), prefixLabelStyle);
			//rect = EditorGUI.PrefixLabel(rect, new GUIContent(":"), prefixLabelStyle);
			return rect;
		}
		
		static Rect getBaseRect(){
			Rect rect = EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(" ");
			EditorGUILayout.EndHorizontal();
			return rect;
		}
		
		public static void makeInfoLabel(string infoTitle, string info){
			EditorGUILayout.BeginHorizontal();
			makeLabel(infoTitle, new GUIStyle("boldLabel"), 13);
			makeLabel(info, new GUIStyle("label"), 13);
			GUILayout.Space(5);
			EditorGUILayout.EndHorizontal();
		}
		
		public static void makeLabel(string text, GUIStyle style, int offset){
			makeLabel(new GUIContent(text), style, offset);
		}
		
		public static void makeLabel(GUIContent guiContent, GUIStyle style, int offset){
			Vector2 size = style.CalcSize(guiContent);
			EditorGUILayout.LabelField(guiContent, style, GUILayout.Width(size.x + offset));
		}
	}
}

