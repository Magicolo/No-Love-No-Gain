using UnityEngine;
using System.Collections;

public static class Vector2Extentions {

    public static Vector3 ToVector3(this Vector2 vector2) {
        return new Vector3(vector2.x, vector2.y);
    }
}
