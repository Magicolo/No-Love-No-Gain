using UnityEngine;
using System.Collections.Generic;

namespace RickTools.MapLoader{
	[System.Serializable]
	public class TiledToUnityLinker : ScriptableObject {
	
		public int savedTime = 0;
		public List<TiledTilesetData> tilesets = new List<TiledTilesetData>();
		public List<AutotileData> autotiles = new List<AutotileData>();
		public List<GameObject> allMapsPrefabsToHave = new List<GameObject>();
        public GameObject postLoadingScript;
	
		public TiledTilesetData getOrCreateTileset(string name) {
			TiledTilesetData tileset = getTileset(name);
			if(tileset == null){
				tileset = new TiledTilesetData(name);
				tilesets.Add(tileset);
			}
			return tileset;
		}
		
		public TiledTilesetData getTileset(string name){
			foreach (var tileset in tilesets) {
				if(tileset.name == name){
					return tileset;
				}
			}	
			return null;
		}
	}
}