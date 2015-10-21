using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Magicolo;
using UnityEngine.Audio;
using Magicolo.GeneralTools;
using Magicolo.AudioTools;

namespace Magicolo
{
	public class AudioDynamicData : IPoolable, ICopyable<AudioDynamicData>
	{
		public enum PlayModes
		{
			Now,
			After
		}

		public PlayModes PlayMode = PlayModes.After;
		public double Delay;
		public Action<AudioItem> OnInitialize;

		public static AudioDynamicData Default = new AudioDynamicData();

		public void OnCreate()
		{
		}

		public void OnRecycle()
		{
		}

		public void Copy(AudioDynamicData reference)
		{
			PlayMode = reference.PlayMode;
			Delay = reference.Delay;
			OnInitialize = reference.OnInitialize;
		}
	}
}