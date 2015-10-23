using UnityEngine;
using System.Collections;

public class GameObjectFactory {

	public static GameObject clone(GameObject original){
		GameObject go = (GameObject)Object.Instantiate(original);
		go.name = original.name;
		return go;
	}
	
	public static GameObject createClone(GameObject original, Transform parent){
		GameObject go = (GameObject)Object.Instantiate(original);
		go.name = original.name;
		go.transform.parent = parent;
		return go;
	}
	
	public static GameObject createClone(GameObject original, Transform parent, Vector3 position){
		GameObject go = (GameObject)Object.Instantiate(original);
		go.name = original.name;
		go.transform.parent = parent;
		go.transform.position = position;
		return go;
	}
}
