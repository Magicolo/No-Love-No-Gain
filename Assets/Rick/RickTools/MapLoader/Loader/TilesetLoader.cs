using System.Collections.Generic;
using System;
using System.Linq;
using Rick.TiledMapLoader;

namespace RickTools.MapLoader{
	public class TilesetLoader : TiledMapTilesetLoader{
	
		
		TiledToUnityLinker linker;
		TiledTilesetData tileset;
		
		public TilesetLoader(TiledToUnityLinker linker){
			this.linker = linker;
		}
		
		
		protected override void beforeAll() {
			tileset = linker.getOrCreateTileset(name);
			
			tileset.setNbTiles(nbTileX,nbTileY);
			tileset.imagePath = imageSourceAtRessourceLevel;
		}
	
		protected override void loadTile(int id, Dictionary<string, string> properties) {
			TiledTileData tile = tileset.tiles[id];
			
			tile.properties = properties;
		}
		
		protected override void afterAll() {
			
		}
	
	}
}