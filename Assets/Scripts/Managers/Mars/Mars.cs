using UnityEngine;
using System.Collections;
using Magicolo;
using Rick;

public class Mars : Singleton<Mars>
{
	[Disable]
	public BaseSpawner[] spawners;


	void Start()
	{
		spawners = Object.FindObjectsOfType<BaseSpawner>();
	}


	void Update()
	{
		foreach (var spawner in spawners)
		{
			spawner.Update();
		}
	}
}
