using UnityEngine;
using System.Collections.Generic;

namespace RickTools.MapLoader{
	[System.Serializable]
	public class TiledTilesetData  {
	
		public string name;
		public string imagePath;
		
		public int nbTileX;
		public int nbTileY;
		
		public List<TiledTileData> tiles = new List<TiledTileData>();
		
		
		public TiledTilesetData(string name){
			this.name = name;
		}
	
		public void setNbTiles(int nbTileX, int nbTileY) {
			int totalTiles = nbTileX * nbTileY;
			
			if(tiles.Count > totalTiles){
				for (int index = totalTiles; index < tiles.Count; index++) {
					tiles.RemoveAt(index);
				}
			}else if(tiles.Count < totalTiles){
				for (int id = tiles.Count; id < totalTiles; id++) {
					tiles.Add(new TiledTileData(id));
				}
			}
			this.nbTileX = nbTileX;
			this.nbTileY = nbTileY;
		}
		
		public TiledTileData getTile(int row, int col) {
			return tiles[row*nbTileX + col];
		}
	}
}