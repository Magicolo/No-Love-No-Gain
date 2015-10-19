using UnityEngine;
using System.Collections;

public static class Interpolation {

	public static Vector3 smoothStep(Vector3 fromV, Vector3 toV, float t){
		float tpro = t = t*t * (3f - 2f*t);
		return Vector3.Lerp(fromV, toV, tpro);
	}
	
	public static float smoothStep(float fromfloat, float toFloat, float t){
		float tpro = t = t*t * (3f - 2f*t);
		return Mathf.Lerp(fromfloat, toFloat, tpro);
	}
	
	public static Color smoothStep(Color from, Color to, float t){
		float tpro = t = t*t * (3f - 2f*t);
		return Color.Lerp(from, to, tpro);
	}
	
	public static Quaternion smoothStep(Quaternion from, Quaternion to, float t){
		float tpro = t = t*t * (3f - 2f*t);
		return Quaternion.Lerp(from, to, tpro);
	}
}
