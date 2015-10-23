using UnityEngine;
using System.Collections;

public static class BoxCollider2DExtentions {

    public static Vector3[] GetCornersWorldPositions(this BoxCollider2D box) 
    {
        Vector3[] corners = new Vector3[4];
        corners[0] = box.transform.position + new Vector3(-box.size.x / 2, -box.size.y / 2);
        corners[1] = box.transform.position + new Vector3(-box.size.x / 2, box.size.y / 2);
        corners[2] = box.transform.position + new Vector3(box.size.x / 2, -box.size.y / 2);
        corners[3] = box.transform.position + new Vector3(box.size.x / 2, box.size.y / 2);

        return corners;
    }
}
