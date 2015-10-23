using System.Collections.Generic;
using System;
using System.Xml.Linq;
using System.Linq;

namespace Rick.TiledMapLoader{
	public abstract class TiledMapTilesetLoader : TiledLoader{
	
		protected string name;
		protected string imageSource;
		protected string imageSourceAtRessourceLevel;
		
		protected int tileWidth;
		protected int tileHeight;
		
		protected int nbTileX;
		protected int nbTileY;
		
		XDocument document;
		XElement tilesetElement;
		
		
		public void loadFromFile(string fileName){
			string text = readAllText(fileName);
			document = XDocument.Parse(text);
			tilesetElement = document.Element("tileset");
	        loadTileset();
		}
		
		string readAllText(string fileName) {
			string[] lines = System.IO.File.ReadAllLines(fileName);
			string outString = "";
			foreach (var line in lines) {
				outString += line + "\n";
			}
			return outString;
		}
		
		public void loadFromElement(XElement tilesetElement){
			this.tilesetElement = tilesetElement;
	        loadTileset();
		}
		
		
		void loadTileset(){
			loadTilesetProperties();
			beforeAll();
			loadTiles();
	        afterAll();
		}
	
		void loadTilesetProperties() {
			name = tilesetElement.Attribute("name").Value;
			tileWidth = parseInt(tilesetElement.Attribute("tilewidth").Value);
			tileHeight = parseInt(tilesetElement.Attribute("tileheight").Value);
			
			XElement imageElement = tilesetElement.Element("image");
			int width = parseInt(imageElement.Attribute("width").Value);
			int height = parseInt(imageElement.Attribute("height").Value);
			imageSource = imageElement.Attribute("source").Value;
			imageSourceAtRessourceLevel = imageSource.Substring(imageSource.IndexOf("Resources", StringComparison.Ordinal) + 10);
			imageSourceAtRessourceLevel = imageSourceAtRessourceLevel.Remove(imageSourceAtRessourceLevel.Length - 4);
			
			nbTileX = width / tileWidth;
			nbTileY = height / tileHeight;
		}
		
		
		void loadTiles() {
			var tiles = tilesetElement.Elements("tile");
			if(tiles != null){
				foreach (var tile in tiles) {
					loadTile(tile);
				}
			}
		}
	
		void loadTile(XElement tile) {
			int id = parseInt(tile.Attribute("id").Value);
			Dictionary<string, string> properties = makePropertiesDictionary(tile.Element("properties"));	
			loadTile(id,properties);
		}
	
		protected abstract void beforeAll();
		
		protected abstract void loadTile(int id, Dictionary<string, string> properties);
		
		protected abstract void afterAll();
	
	}
}