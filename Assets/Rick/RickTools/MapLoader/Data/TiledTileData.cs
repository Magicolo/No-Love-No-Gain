using System.Collections.Generic;
using UnityEngine;

namespace RickTools.MapLoader{
	[System.Serializable]
	public class TiledTileData {
	
		public int id;
		public GameObject prefab;
	
		public Dictionary<string, string> properties;
	
		public TiledTileData(int id) {
			this.id = id;
		}
	}
}