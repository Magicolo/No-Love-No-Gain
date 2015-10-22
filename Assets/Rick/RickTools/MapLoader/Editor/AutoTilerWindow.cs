#if UNITY_EDITOR 
using System;
using System.Xml;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using UnityEditor;
using Magicolo;

using Magicolo.EditorTools;
using RickEditor.Editor;

namespace RickTools.MapLoader{
	[System.Serializable]
	public class AutoTilerWindow : MapWindowBase<AutoTilerWindow> {
	
		const int TILE_EMPTY = 0;
		const int TILE_CENTER = 4;
		const int TILE_STRAIGHT = 2;
		const int TILE_CORNER_INSIDE = 1;
		const int TILE_CORNER_OUTSIDE = 3;
		
		public AutotileData currentAutotile;
		public int currentAutotileIndex;
		
		List<Texture2D> previewTextures;
		
		AutoTileShowerHelper shower = new AutoTileShowerHelper();
		
		public int selectedAutoTileIndex;
		public Vector2 scrollView;
		
		
		
		[MenuItem("Rick's Tools/Map Loader/AutoTiler")]
		public static void Create() {
			CreateWindow("AutoTiler", new Vector2(275, 250));
		}
	
	
		protected override void OnEnable(){
			base.OnEnable();
			if(shower == null){
				shower = new AutoTileShowerHelper();
				shower.loadTextures();
			}else{
				loadTextures();
			}
			
			
		}	
		
		protected override void onLinkerLoaded(){
			
		}

		void loadTextures() {
			Sprite[] sprites = Resources.LoadAll<Sprite>("MapLoader/autotileExemple");
			previewTextures = TextureUtils.texturesFromSprites(sprites);
			shower.loadTextures();
		}
		

		
		protected override void showGUI() {
			if(previewTextures == null){
				loadTextures();
			}
			showSelectionButtons();
			if(currentAutotile != null){
				CustomEditorBase.Separator();
				scrollView = EditorGUILayout.BeginScrollView(scrollView);
				showDataSection();
				CustomEditorBase.Separator();
				showInputSection();
				CustomEditorBase.Separator();
				showPreviewSection();
				CustomEditorBase.Separator();
				showOutputSection();
				EditorGUILayout.EndScrollView();
			}
		}
		
		void showSelectionButtons() {
			GUILayout.BeginHorizontal();
			
			string[] autoTileChoices = new string[linker.autotiles.Count];
			for (int index = 0; index < linker.autotiles.Count; index++) {
				autoTileChoices[index] = linker.autotiles[index].name;
				if( String.IsNullOrEmpty(autoTileChoices[index]) ) autoTileChoices[index] = "* NO NAME * (" + index +")";
			}
			GUI.changed = false;
			selectedAutoTileIndex = RickEditorGUI.Popup("Editing Tileset", selectedAutoTileIndex,autoTileChoices);
			if(GUI.changed){
				switchAutoTile(selectedAutoTileIndex);
			}
			if(CustomEditorBase.Button(new GUIContent("Delete"))){
				linker.autotiles.Remove(currentAutotile);
				
				selectedAutoTileIndex = selectedAutoTileIndex - 1;
				if(selectedAutoTileIndex >= 0){
					switchAutoTile(selectedAutoTileIndex);
				}else{
					currentAutotile = null;
				}
			}
			
			if(CustomEditorBase.Button(new GUIContent("New Autotile"))){
				currentAutotile = new AutotileData();
				linker.autotiles.Add(currentAutotile);
				selectedAutoTileIndex = linker.autotiles.Count - 1;
				switchAutoTile(selectedAutoTileIndex);
			}
			GUILayout.EndHorizontal();
		}

		void switchAutoTile(int index) {
			AutotileData autotile = linker.autotiles[index];
			currentAutotile = autotile;
			currentAutotileIndex = index;
			shower.setCurrentautoTile(autotile);
		}		
		
		void showDataSection() {
			GUI.changed = false;
			currentAutotile.name = EditorGUILayout.TextField("Name",currentAutotile.name);
			if(GUI.changed) dataChanged = true;
		}
		
		void showInputSection() {
			RickEditorGUI.Label("Input","");
			RickEditorGUI.Label("","Modes placeHolder");
			GUI.changed = false;
			showInputLine("Center", 		 previewTextures[TILE_CENTER]		 , "center");
			showInputLine("Outside Corner",previewTextures[TILE_CORNER_OUTSIDE], "cornerOutside");
			showInputLine("Side",			 previewTextures[TILE_STRAIGHT]		 , "side");
			showInputLine("Inside Corner", previewTextures[TILE_CORNER_INSIDE] , "cornerInside");
			if(GUI.changed){
				dataChanged = true;
				shower.loadTextures();
			}
			/*if(GUILayout.Button("Autoload Based on Center")){
			   	autoLoadBasedOnCenter();
			}*/
		}

		/*void autoLoadBasedOnCenter() {
			int index = Int32.Parse(currentAutotile.center.name.Split(new char[]{'_'})[1]);
			var found = AssetDatabase.FindAssets("t:Texture2D " + currentAutotile.center.texture.name);
			
			if(found.Length >= 1){
				string path = AssetDatabase.GUIDToAssetPath(found[0]);
				var assetsFound = AssetDatabase.LoadAllAssetsAtPath(path);
				//tilesFileName = assetsFound[0].name;
				/*foreach (var f in assetsFound) {
					Debug.Log(f.name);
				}*/
				/*currentAutotile.cornerOutside = (Sprite)assetsFound[index + 2];
				currentAutotile.side = (Sprite)assetsFound[index + 3];
				currentAutotile.cornerInside = (Sprite)assetsFound[index + 4];
				dataChanged = true;
				shower.loadTextures();
			}
			
		}*/
		
		void showInputLine(string labelName, Texture2D previewTexture, string propertyName){
			GUILayout.BeginHorizontal();
			
			EditorGUILayout.LabelField(labelName);
			GUILayout.Label(new GUIContent(previewTexture), labelTextureIconStyle);
			EditorGUILayout.PropertyField(linkerSO.FindProperty("autotiles").GetArrayElementAtIndex(currentAutotileIndex).FindPropertyRelative(propertyName), new GUIContent(propertyName),true);
			
			GUILayout.EndHorizontal();
		}

		void showPreviewSection() {
			RickEditorGUI.Label("Preview","");
			
			shower.show();
		}
		
		void showOutputSection() {
			RickEditorGUI.Label("Output-Tiled AutoTiles","");
			currentAutotile.tilesFileName = EditorGUILayout.TextField("TilesetName" , currentAutotile.tilesFileName);
			if(GUILayout.Button("Export")){
			   //autoTileFilePath = EditorUtility.SaveFilePanel("Autotile file", RickEditorGUI.rootFolder, "autotile","tmx");
			   currentAutotile.autoTileFilePath = "G:/Workspaces/unity/Razzefs-Balafa/Project/Maps/autotile.tmx";
			   export();
			}
			
			CustomEditorBase.Separator();
			RickEditorGUI.Label("Output-Create Prefab","");
			currentAutotile.basePrefab = (GameObject)EditorGUILayout.ObjectField("Base prefab", currentAutotile.basePrefab, typeof(GameObject), false);
			currentAutotile.outputAssetFolder = RickEditorGUI.FolderPath("Destination folder",currentAutotile.outputAssetFolder, RickEditorGUI.assetFolder);
			if(GUILayout.Button("Make Copies")){
				makeCopie();
			}
		}

		void export() {
			List<int> idsCenter = getIdsFrom(currentAutotile.center);
			List<int> idsSide = getIdsFrom(currentAutotile.side);
			List<int> idsCornerIn = getIdsFrom(currentAutotile.cornerInside);
			List<int> idsCornerOut = getIdsFrom(currentAutotile.cornerOutside);
			
			SimpleAutoTileExporter exporter = new SimpleAutoTileExporter(currentAutotile.tilesFileName, idsCenter, idsSide, idsCornerIn, idsCornerOut);
			
			XmlDocument doc = exporter.generateDocument();
			doc.Save(currentAutotile.autoTileFilePath);
		}

		List<int> getIdsFrom(List<Sprite> sprites) {
			List<int> ids = new List<int>();
			foreach (var sprite in sprites) {
				int newId = Int32.Parse(sprite.name.Split('_')[1]);
				ids.Add(newId);
			}
			return ids;
		}
				
		void makeCopie() {
			if(currentAutotile.basePrefab != null){
				makeCloneFor("Center", currentAutotile.center);
				makeCloneFor("Side", currentAutotile.side);
				makeCloneFor("Outside", currentAutotile.cornerOutside);
				makeCloneFor("Inside", currentAutotile.cornerInside);
			}
		}

		void makeCloneFor(string name, List<Sprite> sprite) {
			for (int i = 0; i < sprite.Count; i++) {
				makeCloneFor(name + i, sprite[i]);
			}
		}
		
		void makeCloneFor(string name, Sprite sprite) {
		GameObject tileCenter = GameObjectExtend.createClone(currentAutotile.basePrefab);
			SpriteRenderer sr = tileCenter.GetComponentInChildren<SpriteRenderer>();
			if(sr != null){
				sr.sprite = sprite;
			}
			string path = "Assets" + currentAutotile.outputAssetFolder + "/" + currentAutotile.name + "_" + name + ".prefab";
			PrefabUtility.CreatePrefab(path,tileCenter);
			tileCenter.Remove();
		}
	}
}
#endif