using UnityEngine;
using System.Collections;

public class ResourcesUtils {

    public static Sprite loadSprite(string path, string spriteFileName, int spriteIndex)
    {
        return loadSprite(path + "/" + spriteFileName, spriteIndex);
    }

    public static Sprite loadSprite(string spriteFilePath, int spriteIndex)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>(spriteFilePath);
        if (sprites != null && sprites.Length > spriteIndex)
        {
            return sprites[spriteIndex];
        }
        else
        {
            Debug.Log("Not found");
            return null;
        }
    }
}
