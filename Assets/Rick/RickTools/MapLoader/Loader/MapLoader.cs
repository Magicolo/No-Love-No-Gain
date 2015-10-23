using System.IO;
using UnityEngine;
using System.Collections.Generic;
using Rick.TiledMapLoader;
using Magicolo;
using System;

namespace RickTools.MapLoader{
	[System.Serializable]
	public class MapLoader : TiledMapLoader {
	
		public TiledToUnityLinker linker;
		MapLoaderStatistics statistics;
		
		public List<TiledTileData> tiles = new List<TiledTileData>();
		[Disable] public List<int> lastIdForTileset = new List<int>();
		[Disable] public List<string> tilesetNames = new List<string>();
		
		public Transform parent;
		public Transform tileParent;
        MapData data;

        public MapLoaderOptions options;
        private GameObject postLoadingScriptGo;

		public MapLoader(TiledToUnityLinker linker, MapLoaderOptions options = MapLoaderOptions.NONE){
			if(linker == null) Debug.LogError("The linker provided is null");
			this.linker = linker;
			
			this.options = options;
		}
		
		public GameObject loadFromFile(FileInfo file){
			statistics = new MapLoaderStatistics();
			string name = file.Name.Split(new []{'.'})[0];
			
			createParent(name);
            
			tiles.Clear();
			
	        base.loadFromFile(file.FullName);
	        statistics.showToConsole();
	        return parent.gameObject;
		}

        private void initPLS()
        {
            if (linker.postLoadingScript != null)
            {
                postLoadingScriptGo = GameObjectFactory.createClone(linker.postLoadingScript,null);
                foreach (var item in postLoadingScriptGo.GetComponents<PostLoadingScript>())
                {
                    item.init(data);
                }
            }
        }

        void createLayersParent(){
			
		}
		void createParent(string name){
			GameObject parentGo = new GameObject(name);
			parent = parentGo.transform;
			tileParent = parent;
		}

		
		protected override void addExternalTileset(int firstGridId, string source){
			if(linker == null) Debug.LogError("The linker provided is null");
			
			string[] splited = source.Split(new []{'/'});
			string tilesetName = splited[splited.Length-1];
			
			var tileset = linker.getTileset(tilesetName);
			
			if(tileset == null){
				Debug.LogError("The tileset \"" + tilesetName + "\" is unknown to the linker.");
			}else{
				lastIdForTileset.Add( tiles.Count + tileset.tiles.Count - 1);
				tilesetNames.Add(tilesetName);
				tiles.AddRange(tileset.tiles);
			}
		}
		
		protected override void addInternalTileset(int firstGridId,string name, string source){
			Debug.LogError(String.Format("Maploader does not support internal Tileset. Export the tileset {0} and import it to the linker.",name));
		}
	
		protected override void afterAll() {
            if (postLoadingScriptGo != null) {
                postLoadingScriptGo.Remove();
            }
		}
		
		protected override void afterMapAttributesLoaded() {
			data = parent.GetOrAddComponent<MapData>();
			data.width = mapWidth;
			data.height = mapHeight;
			data.filePath = fileName;
			data.linker = linker;
            initPLS();
        }

        protected override void addObjectGroups(string objectGroupName, Color color, Dictionary<string, string> properties)
        {
            if (postLoadingScriptGo != null)
            {
                foreach (var item in postLoadingScriptGo.GetComponents<PostLoadingScript>())
                {
                    item.addObjectGroups(objectGroupName, color, properties);
                }
            }
        }

        protected override void addObject(string objectGroupName, int id, string objName, float x, float y, Dictionary<string, string> properties)
        {
            if (postLoadingScriptGo != null)
            {
                foreach (var item in postLoadingScriptGo.GetComponents<PostLoadingScript>())
                {
                    item.addObject(objectGroupName, id, objName, x, y, properties);
                }   
            }
		}
		
		protected override void addLayer(string layerName, int width, int height, Dictionary<string, string> properties) {
			if(hasOption(MapLoaderOptions.SEPARATE_PREFAB_BY_TILED_LAYERS)){
				GameObject newParent = new GameObject(layerName);
				newParent.transform.parent = parent;
				tileParent = newParent.transform;
			}
		}
		
		protected override void addTile(int x, int y, int id, ulong flags) {
			int listTileId = id-1;
			if(listTileId > tiles.Count - 1){
				string error = String.Format("Unknown tile id {0}, last id is {1}.\nForgot to load a tileset in the linker?\nUnsuported tiled param?\n Used time : ", id, tiles.Count);
				statistics.addError(error);
				return;
			}
			
			TiledTileData tileData = tiles[id-1];
			
			if(tileData == null ){
				Debug.Log("Tile " + id +" is nulll !?!?");
				
			}else if(tileData.prefab == null){
				makeWarningForMissingTile(id, tileData.id);
				
			}else{
				//Debug.Log("fake on rajoute " + tileData.prefab.name + " (" + (id-1) + ")");
				
				Vector3 position = new Vector3(x,y);
				GameObject original = tileData.prefab;
				GameObject go = (GameObject)UnityEngine.Object.Instantiate(original);
				go.name = original.name;
				go.transform.position = position;
				go.transform.parent = tileParent;
				applyFlipFlags(go, flags);
			}
			
		}

		void applyFlipFlags(GameObject go, ulong flags) {
			if(flags == 0) return;
			bool horizontal = (flags & TiledMapLoader.FLIPPED_HORIZONTALLY_FLAG) == TiledMapLoader.FLIPPED_HORIZONTALLY_FLAG;
			bool vertical 	= (flags & TiledMapLoader.FLIPPED_VERTICALLY_FLAG) == FLIPPED_VERTICALLY_FLAG;
			bool diagonal 	= (flags & TiledMapLoader.FLIPPED_DIAGONALLY_FLAG) == FLIPPED_DIAGONALLY_FLAG;
			
			if(!horizontal & !vertical & diagonal) applyRotationFlip(go, 90, new Vector3(-1,1,1));
			if(!horizontal & vertical & !diagonal) applyRotationFlip(go, 180, new Vector3(-1,1,1));
			if(!horizontal & vertical & diagonal) applyRotationFlip(go, 90, new Vector3(1,1,1));
			if(horizontal & !vertical & !diagonal) applyRotationFlip(go, 0, new Vector3(-1,1,1));
			if(horizontal & !vertical & diagonal) applyRotationFlip(go, 270, new Vector3(1,1,1));
			if(horizontal & vertical & !diagonal) applyRotationFlip(go, 180, new Vector3(1,1,1));
			if(horizontal & vertical & diagonal) applyRotationFlip(go, 270, new Vector3(-1,1,1));
		}

		void applyRotationFlip(GameObject go, int rotation, Vector3 flip) {
			go.transform.Rotate(0,0,rotation);
			Vector3 s = go.transform.localScale;
			go.transform.localScale = new Vector3(s.x*flip.x, s.y*flip.y, s.z*flip.z);
		}
		
		
		void makeWarningForMissingTile(int overallId, int idInTileset){
			int index = 0;
			while(lastIdForTileset[index++] < overallId);
			
			string tilesetName = tilesetNames[index-1];
			
			statistics.addWarning(string.Format("Tile ID ({0}) in tileset ({1}) is missing. Used time",idInTileset, tilesetName));
		}
		
		protected override void loadMapProperty(Dictionary<string, string> properties) {
			
		}
		
		bool hasOption(MapLoaderOptions option){
			return (options & option) == option;
		}
	}
	
	[Flags]
	public enum MapLoaderOptions {
		NONE = 0,
		LOAD_AS_PREFAB = 1,
		SEPARATE_PREFAB_BY_TILED_LAYERS = 2
	}
}
