using UnityEngine;
using System.Collections;
using Magicolo;
using RickTools.MapLoader;

[System.Serializable]
public class LevelCycleMenager : MonoBehaviour {

	public string playerPrefMapIndexKey;
	
	public string mapPrefabFolder;
	public string inGameMapName;
	public string endOfCycleMapName;
	
	
	[Disable] public MapData[] currentMapPack;
	public int currentMapIndex = -1;
	
	[Disable] public GameObject currentMapGO;
	[Disable] public MapData currentMapData;
	
	public static LevelCycleMenager instance;
	
	[Disable] public bool nextMapOnLevelWasLoaded = false;
	[Disable] public bool refreshed = false;

	public bool levelLoaded;
	
	void Awake(){
		if(LevelCycleMenager.instance != null && LevelCycleMenager.instance != this){
			gameObject.Remove();
			return;
		} 
		LevelCycleMenager.instance = this;
		DontDestroyOnLoad(this);
	}
	
	[Button("Load Map Pack", "loadMapPack")]
	public bool loadMapPackBtn;
	
	public void loadMapPack(){
		currentMapPack = Resources.LoadAll<MapData>(mapPrefabFolder);
	}
	
	
	
	[Button("Load Next Map", "nextMap")]
	public bool loadNextMapBtn;
	
	public void nextMap(){
		levelLoaded = false;
		if(hasNextMap()){
			nextMapOnLevelWasLoaded = true;
			Application.LoadLevel(inGameMapName);
		}else{
			endMapPack();
		}
		
	}

	void OnLevelWasLoaded(int level) {
		if(Application.loadedLevelName == inGameMapName && nextMapOnLevelWasLoaded){
			loadNextMap();
			nextMapOnLevelWasLoaded = false;
		}
    }
	
	public bool hasNextMap() {
		return currentMapPack.Length != 0 && currentMapIndex + 1 < currentMapPack.Length;
	}
	
	void loadNextMap() {
		currentMapIndex++;	
		setCurrentMapIndexInPlayerPref();
		loadMap(currentMapPack[currentMapIndex]);		
	}

	void setCurrentMapIndexInPlayerPref() {
		if( !string.IsNullOrEmpty(playerPrefMapIndexKey) ){
			int maxLevel = PlayerPrefs.GetInt(playerPrefMapIndexKey,1);
			if(maxLevel < currentMapIndex){
				PlayerPrefs.SetInt(playerPrefMapIndexKey,currentMapIndex);
			}
		}
		
	}
	
	
	public void endMapPack() {
		clearCurrentMap();
		Application.LoadLevel(endOfCycleMapName);
	}

	
	void loadMap(MapData mapData) {
		if(currentMapGO != null){
			clearCurrentMap();
		}
		
		currentMapGO = GameObjectExtend.createClone(mapData.gameObject);
		currentMapData = currentMapGO.GetComponent<MapData>();
		
		levelLoaded = true;
	}
	
	void clearCurrentMap() {
		currentMapGO.Remove();
		currentMapData = null;
	}
}
