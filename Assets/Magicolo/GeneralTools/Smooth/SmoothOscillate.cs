using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Magicolo
{
	[AddComponentMenu("Magicolo/General/Smooth/Oscillate")]
	public class SmoothOscillate : MonoBehaviourExtended
	{
		[Mask]
		public TransformModes Mode = TransformModes.Position;
		[Mask(Axes.XYZ)]
		public Axes Axes = Axes.XYZ;
		public bool Culling = true;

		[Slider(BeforeSeparator = true)]
		public float FrequencyRandomness;
		public Vector3 Frequency = Vector3.one;

		[Slider(BeforeSeparator = true)]
		public float AmplitudeRandomness;
		public Vector3 Amplitude = Vector3.one;

		[Slider(BeforeSeparator = true)]
		public float CenterRandomness;
		public Vector3 Center;

		bool _rendererCached;
		Renderer _renderer;
		public Renderer Renderer
		{
			get
			{
				_renderer = _rendererCached ? _renderer : GetComponent<Renderer>();
				_rendererCached = true;
				return _renderer;
			}
		}

		void Awake()
		{
			ApplyRandomness();
		}

		void Update()
		{
			if (Mode == TransformModes.None || Axes == Axes.None)
				return;

			if (!Culling || Renderer.isVisible)
			{
				if (Mode.Contains(TransformModes.Position))
					transform.OscillateLocalPosition(Frequency, Amplitude, Center, Time.time, Axes);

				if (Mode.Contains(TransformModes.Rotation))
					transform.OscillateLocalEulerAngles(Frequency, Amplitude, Center, Time.time, Axes);

				if (Mode.Contains(TransformModes.Scale))
					transform.OscillateLocalScale(Frequency, Amplitude, Center, Time.time, Axes);
			}
		}

		public void ApplyRandomness()
		{
			Frequency += Frequency.SetValues(new Vector3(Random.Range(-FrequencyRandomness * Frequency.x, FrequencyRandomness * Frequency.x), Random.Range(-FrequencyRandomness * Frequency.y, FrequencyRandomness * Frequency.y), Random.Range(-FrequencyRandomness * Frequency.z, FrequencyRandomness * Frequency.z)), Axes);
			Amplitude += Amplitude.SetValues(new Vector3(Random.Range(-AmplitudeRandomness * Amplitude.x, AmplitudeRandomness * Amplitude.x), Random.Range(-AmplitudeRandomness * Amplitude.y, AmplitudeRandomness * Amplitude.y), Random.Range(-AmplitudeRandomness * Amplitude.z, AmplitudeRandomness * Amplitude.z)), Axes);
			Center += Center.SetValues(new Vector3(Random.Range(-CenterRandomness * Center.x, CenterRandomness * Center.x), Random.Range(-CenterRandomness * Center.y, CenterRandomness * Center.y), Random.Range(-CenterRandomness * Center.z, CenterRandomness * Center.z)), Axes);
		}
	}
}
