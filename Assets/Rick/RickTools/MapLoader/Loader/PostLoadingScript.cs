using UnityEngine;
using System.Collections.Generic;
using RickTools.MapLoader;

public abstract class PostLoadingScript : MonoBehaviour
{
    public abstract void init(MapData mapData);

    public abstract void addObject(string objectGroupName, int id, string objName, float x, float y, Dictionary<string, string> properties);

    public abstract void addObjectGroups(string objectGroupName, Color color, Dictionary<string, string> properties);
}
