#if UNITY_EDITOR 
using Magicolo.EditorTools;
using UnityEngine;
using UnityEditor;
using RickEditor.Editor;
using Magicolo;
using System.IO;

namespace RickTools.MapLoader{
	[System.Serializable]
	public class MapLoaderWindow : MapWindowBase<MapLoaderWindow> {
		
		public string selectedPath;
		FileInfo[] files;
		bool[] selectedFiles;
		bool selectAllToglle;
		bool separatePrefabByTiledLayers;
		
		public bool loadToPrefab = false;
		public string prefabFolder = "Assets/Resources/Prefab/Map";
		
		
		[MenuItem("Rick's Tools/Map Loader/Map Loader")]
		public static void Create() {
			CreateWindow("Map Loader", new Vector2(275, 250));
		}

		protected override void onLinkerLoaded() {
			hideFlags = HideFlags.HideAndDontSave;
		}
		
		protected override void OnEnable(){
			base.OnEnable();
			if(!string.IsNullOrEmpty(selectedPath)){
				loadMapsInfos(selectedPath);
			}
		}
		
		protected override void showGUI() {
			string path = RickEditorGUI.FolderPath("Change Map folder", "", RickEditorGUI.rootFolder);
			if(!string.IsNullOrEmpty(path)){
				loadMapsInfos(path);
			}
			
			if(files != null){
				showMapFiles();
				showOutputPanel();
			}
		}

		void loadMapsInfos(string path){
			var info = new DirectoryInfo(path);
			if(info.Exists){
				selectedPath = path;
				files = info.GetFiles("*.tmx");
				selectedFiles = new bool[files.Length];
				for (int i = 0; i < files.Length; i++) {
					selectedFiles[i] = true;
				}
			}
		}
		
		
		void showMapFiles() {
			CustomEditorBase.Separator();
			RickEditorGUI.Label("INPUT :", selectedPath);
			GUILayout.Space(4);
			if(selectAllToglle){
				selectAllToglle = RickEditorGUI.Toggle("Select all",true);
				if(!selectAllToglle){
					for (int i = 0; i < selectedFiles.Length; i++) {
						selectedFiles[i] = false;
					}
				}
			}else{
				selectAllToglle = RickEditorGUI.Toggle("Select all",false);
				if(selectAllToglle){
					for (int i = 0; i < selectedFiles.Length; i++) {
						selectedFiles[i] = true;
					}
				}
			}
			GUILayout.Space(8);
			int index = 0;
			foreach (FileInfo fileInfo in files) {
				if(fileInfo.Exists){
					selectedFiles[index] = RickEditorGUI.Toggle(fileInfo.Name, selectedFiles[index]);
				}
				index++;
			}	
		}

		void showOutputPanel() {
			CustomEditorBase.Separator();
			RickEditorGUI.Label("Output","");
			GUILayout.Space(4);
			
			const string seprateTooltip = "Determine whether the Tiles are loaded in Parent GameObjects based on the Tile's Layer or all tiles are put in the same parent.";
			separatePrefabByTiledLayers = RickEditorGUI.Toggle("Use Layers", seprateTooltip, separatePrefabByTiledLayers);
			loadToPrefab = RickEditorGUI.Toggle("Load As Prefab", loadToPrefab);
			if (loadToPrefab) {
				prefabFolder = RickEditorGUI.FolderPath("Asset Prefab Folder", prefabFolder, RickEditorGUI.assetFolder);
			}
			
			showButton();
		}
		
		void showButton() {
			if (selectedFiles == null || selectedFiles.Length == 0) {
				GUI.enabled = false;
			}
			
			if (GUILayout.Button("Load All Map")) {
				loadMaps();
			}
			
			GUI.enabled = true;
		}

		void loadMaps() {
			MapLoaderControler mapLoaderControler = new MapLoaderControler();
			
			MapLoaderOptions options = MapLoaderOptions.NONE;
			if(separatePrefabByTiledLayers) options |= MapLoaderOptions.SEPARATE_PREFAB_BY_TILED_LAYERS;
			
			mapLoaderControler.loadToPrefab = loadToPrefab;
			mapLoaderControler.prefabRoot = "Assets" + prefabFolder;
			mapLoaderControler.options = options;
			
			
			int index = 0;
			int loadedMap = 0;
			System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
			stopWatch.Start();
			foreach (FileInfo file in files) {
				if(selectedFiles[index]){
					loadedMap++;
					mapLoaderControler.loadFile(linker, file);
				}
				index++;
			}
			float time = stopWatch.ElapsedMilliseconds / 1000f;
			Debug.Log(string.Format("Loaded {0} maps in {1} s.", loadedMap, time));
		}
	}
}
#endif