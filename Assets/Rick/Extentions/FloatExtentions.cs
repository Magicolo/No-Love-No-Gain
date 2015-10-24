using UnityEngine;
using System.Collections;

public static class FloatExtentions
{

	public static float scale(this float f, float oldMin, float oldMax, float newMin, float newMax)
	{
		float to1 = (f - oldMin) / (oldMax - oldMin);
		return to1 * (newMax - newMin) + newMin;
	}

	public static bool IsBetween(this float f, float min, float max)
	{
		return f > min && f < max;
	}
}
