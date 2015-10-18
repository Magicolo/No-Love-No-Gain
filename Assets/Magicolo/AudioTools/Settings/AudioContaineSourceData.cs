using UnityEngine;
using System.Collections;
using System;
using Magicolo;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Magicolo.AudioTools
{
	[Serializable]
	public class AudioContainerSourceData : IPoolable, ICopyable<AudioContainerSourceData>
	{
		public AudioSettingsBase Settings;
		public List<AudioOption> Options = new List<AudioOption>();

		public void OnCreate()
		{
			if (Settings != null)
				Settings = Settings.Clone();

			Pool<AudioOption>.CreateElements(Options);
		}

		public void OnRecycle()
		{
			if (Settings != null)
				Settings.Recycle();

			Pool<AudioOption>.RecycleElements(Options);
		}

		public void Copy(AudioContainerSourceData reference)
		{
			Settings = reference.Settings;
			CopyHelper.CopyTo(reference.Options, ref Options);
		}
	}
}