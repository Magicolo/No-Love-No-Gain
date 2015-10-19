
#if UNITY_EDITOR 
using System.IO;
using UnityEngine;
using UnityEditor;

using Magicolo.EditorTools;
using RickEditor.Editor;

namespace RickTools.MapLoader{

    public abstract class MapWindowBase<T> : CustomWindowBase<T> where T : MapWindowBase<T>  {
	
		public TiledToUnityLinker linker;
		public SerializedObject linkerSO;
		
		public string linkerPath = "assets";
		public bool dataChanged = true;
	
		string[] linkersPathFound;
		
		
		protected GUIStyle labelTextureIconStyle;
		protected GUIStyle labelDontResizeStyle;
		
		protected virtual void OnEnable(){
			if(linker == null){
				refreshLinkerList();
			}
		}
		
		void OnGUI() {
			if(labelTextureIconStyle == null){
				labelTextureIconStyle = GUI.skin.label;
				labelTextureIconStyle.margin = new RectOffset(0,0,0,0);
				labelTextureIconStyle.padding = new RectOffset(0,0,0,0);
				labelTextureIconStyle.border = new RectOffset(0,0,0,0);
				labelTextureIconStyle.stretchWidth = false;
				labelTextureIconStyle.stretchHeight = false;
				labelTextureIconStyle.fixedWidth =16;
				
				labelDontResizeStyle = GUI.skin.label;
				labelTextureIconStyle.padding = new RectOffset(3,3,3,3);
				labelDontResizeStyle.stretchWidth = false;
				labelDontResizeStyle.stretchHeight = false;
				labelTextureIconStyle.fixedWidth = 64;
			}
			
			
			if(linker == null){
				showLinkerSelectionPanel();
			}else{
				linkerSO = new SerializedObject(linker);
				EditorGUI.BeginChangeCheck();
				showGUI();
				if (EditorGUI.EndChangeCheck() || dataChanged){
					EditorUtility.SetDirty(linker);
					dataChanged = false;
				} 
				linkerSO.ApplyModifiedProperties();
			}
		}

		void showLinkerSelectionPanel(){
			GUILayout.BeginHorizontal();
			if (GUILayout.Button ("Create New Linker")) {
				createNewLinker();
			}
			if (GUILayout.Button ("Refresh Linker list")) {
				refreshLinkerList();
			}
			GUILayout.EndHorizontal();
			CustomEditorBase.Separator();
			
			showLinkerList();
		}

		void refreshLinkerList(){
			linkersPathFound = AssetDatabase.FindAssets("t:TiledToUnityLinker");
		}

		void showLinkerList(){
			if(linkersPathFound == null) return;
			
			foreach (var guid in linkersPathFound) {
				string path = AssetDatabase.GUIDToAssetPath(guid);
				string name = Path.GetFileNameWithoutExtension(path);
				if (GUILayout.Button ("Open " + name)) {
					linkerPath = path;
					loadNewLinker();
					onLinkerLoaded();
				}
			}
		}
		
		
		protected abstract void showGUI();
		
		
	
		protected abstract void onLinkerLoaded();
		
		void loadNewLinker() {
			TiledToUnityLinker loadedLinker = (TiledToUnityLinker)AssetDatabase.LoadAssetAtPath(linkerPath, typeof(TiledToUnityLinker));
			if(loadedLinker != null){
				linker = loadedLinker;
			}else{
				Debug.LogError("Selected file not a Linker!");
			}
		}
	
		
		void createNewLinker() {
			TiledToUnityLinker newLinker = (TiledToUnityLinker)ScriptableObject.CreateInstance("TiledToUnityLinker");
			string path = findNameForNewLinker();
			AssetDatabase.CreateAsset(newLinker, path);
		}
		
		string findNameForNewLinker() {
			
			Object foundLinker = AssetDatabase.LoadAssetAtPath("Assets/newLinker.asset", typeof(TiledToUnityLinker));
			if(foundLinker == null){
				return "Assets/newLinker.asset";
			}else{
				int index = 0;
				while( assetExist("Assets/newLinker (" + (++index) +").asset") );
				
				return "Assets/newLinker (" + index +").asset";
			}
		}
		
		bool assetExist(string path){
			return AssetDatabase.LoadAssetAtPath(path, typeof(TiledToUnityLinker)) != null;
		}
	}
}
#endif