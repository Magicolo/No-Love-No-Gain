using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

namespace Magicolo
{
	[AddComponentMenu("Magicolo/General/Smooth/Move")]
	public class SmoothMove : MonoBehaviourExtended
	{
		[Mask]
		public TransformModes Mode = TransformModes.Position;
		[Mask(Axes.XYZ)]
		public Axes Axes = Axes.XYZ;
		public bool Culling = true;

		[Slider(BeforeSeparator = true)]
		public float Randomness;
		public Vector3 Speed = Vector3.one;

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
					transform.TranslateLocal(Speed, Axes);

				if (Mode.Contains(TransformModes.Rotation))
					transform.RotateLocal(Speed, Axes);

				if (Mode.Contains(TransformModes.Scale))
					transform.ScaleLocal(Speed, Axes);
			}
		}

		public void ApplyRandomness()
		{
			Speed += Speed.SetValues(new Vector3(Random.Range(-Randomness * Speed.x, Randomness * Speed.x), Random.Range(-Randomness * Speed.y, Randomness * Speed.y), Random.Range(-Randomness * Speed.z, Randomness * Speed.z)), Axes);
		}
	}
}