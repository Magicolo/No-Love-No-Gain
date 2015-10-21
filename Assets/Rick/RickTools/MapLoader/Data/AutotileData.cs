using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AutotileData{

	public string name = "";
	
	//Source Srite
	public List<Sprite> center = new List<Sprite>();
	public List<Sprite> side = new List<Sprite>();
	public List<Sprite> cornerInside = new List<Sprite>();
	public List<Sprite> cornerOutside = new List<Sprite>();
	
	/* Prefab copie */
	public GameObject basePrefab;
	public string outputAssetFolder;
	
	/* Make Tiled autotile */
	public string autoTileFilePath;
	public string tilesFileName;
}
