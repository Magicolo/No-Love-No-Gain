using UnityEngine;
using System.Collections;

public class GameObjectBuilder {

    private GameObject go;
    public GameObject GameObject {
        get { return go; }
    }

    public GameObjectBuilder(string name, Transform parent = null) {
        go = new GameObject(name);
        
        go.transform.parent = parent;
    }


    #region SpriteRenderer
    public GameObjectBuilder addSprite(Sprite sprite, int orderInLayer = 0)
    {
        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.sortingOrder = orderInLayer;
        return this;
    }

    public GameObjectBuilder addSprite(Sprite sprite, Color color, Material material, int sortingLayerId, int orderInLayer)
    {
        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.color = color;
        sr.material = material;
        sr.sortingLayerID = sortingLayerId;
        sr.sortingOrder = orderInLayer;
        return this;
    }
    #endregion


}
