using UnityEngine;
using System.Collections;

public class SmoothLineRendererOscillate : MonoBehaviour {

	public float widthAmplitude = 1;
	public float widthFrequency = 1;
	public float widthOffset = 0;
	
	LineRenderer lineRenderer;
	
	void Start () {
		lineRenderer = GetComponent<LineRenderer>();
	}
	
	
	void Update () {
		float width = widthOffset + widthAmplitude * Mathf.Sin(widthFrequency * Time.time);
		lineRenderer.SetWidth(width,width);
	}
}
