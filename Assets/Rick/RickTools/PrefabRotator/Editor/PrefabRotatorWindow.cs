using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

using Magicolo.EditorTools;
using RickEditor.Editor;


namespace RickTools{
	[System.Serializable]
	public class PrefabRotatorWindow : CustomWindowBase<PrefabRotatorWindow> {
	
		string prefabFilePath = "Assets";
			
		[MenuItem("Rick's Tools/Map Loader/Prefab Rotator")]
		public static void Create() {
			CreateWindow("Prefab Rotator", new Vector2(275, 250));
		}
		
		void OnGUI() {
			prefabFilePath = RickEditorGUI.FilePath("Prefab to rotate", prefabFilePath, RickEditorGUI.resourcesFolder, "prefab");
			if(CustomEditorBase.Button(new GUIContent("Make rotated prefab"))){
				string filePath = "Assets/Resources" + prefabFilePath;
				GameObject prefabToRotate = (GameObject)AssetDatabase.LoadAssetAtPath(filePath + ".prefab", typeof(GameObject));
				
				int dotIndex = filePath.LastIndexOf('/');
				string fileFolder = filePath.Substring(0,dotIndex);
				
				nameRotation(prefabToRotate, fileFolder, "90", new Vector3(0,0,90));
				nameRotation(prefabToRotate, fileFolder, "180", new Vector3(0,0,180));
				nameRotation(prefabToRotate, fileFolder, "-90", new Vector3(0,0,-90));
			}
		}

		void nameRotation(GameObject prefabToRotate, string fileFolder, string nameSuffix, Vector3 vector3){
			GameObject clone = GameObjectExtend.createClone(prefabToRotate);
			clone.transform.localRotation = Quaternion.Euler(vector3);
			clone.name += nameSuffix;
			
			PrefabUtility.CreatePrefab(fileFolder + "/" + clone.name + ".prefab", clone);
			Object.DestroyImmediate(clone);
		}
	}
}
