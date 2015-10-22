using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using RickTools.MapLoader;
using Magicolo;

public class PLSPositionPlayer : PostLoadingScript
{

	MapData mapData;
	PlayerPositionData playerPositionData;

	public override void init(MapData mapData)
	{
		playerPositionData = mapData.gameObject.AddComponent<PlayerPositionData>();
	}

	public override void addObject(string objectGroupName, int id, string objName, float x, float y, Dictionary<string, string> properties)
	{
		playerPositionData.positions[objName] = new Vector2(x, y);
	}

	public override void addObjectGroups(string objectGroupName, Color color, Dictionary<string, string> properties)
	{
		if (objectGroupName == "Positions")
		{
			playerPositionData.drawColor = color;
		}
	}


}
