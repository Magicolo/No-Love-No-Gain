using UnityEngine;
using Magicolo;


namespace RickTools.MapLoader{
	
	public class MapData : MonoBehaviour {
	
		public string filePath;
		public TiledToUnityLinker linker;
		public int width;
		public int height;
		
		
		#region reload
		#if UNITY_EDITOR
		
		[ButtonAttribute("Reload Map","reload")]
		public bool reloadBtn;
		
		void reload(){
			MapLoaderControler mapLoaderControler = new MapLoaderControler();
			GameObject go = mapLoaderControler.loadFile(linker, new System.IO.FileInfo(filePath));
			go.transform.parent = transform.parent;
			gameObject.Remove();
		}
		#endif
		#endregion
		
		public bool isWithinMap(Vector3 position){
			return position.x > 0 && position.y > 0 && position.x < width && position.y < height;
		}
	}
}