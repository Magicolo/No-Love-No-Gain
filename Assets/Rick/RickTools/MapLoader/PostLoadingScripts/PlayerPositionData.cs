using UnityEngine;
using System.Collections;
using Magicolo;

public class PlayerPositionData : MonoBehaviour
{
	public SerializableStringVector2Dictionary positions = new SerializableStringVector2Dictionary();
	public Color drawColor;

	void OnDrawGizmos()
	{
#if UNITY_EDITOR
		foreach (var key in positions.keys)
		{
			UnityEditor.Handles.color = drawColor;
			Vector3 textPosition = positions[key].ToVector3() + new Vector3(0.5f, 0.5f, 0);
			UnityEditor.Handles.Label(textPosition, key);

			Gizmos.color = drawColor;
			Gizmos.DrawSphere(positions[key], 0.4f);
		}
#endif
	}
}
