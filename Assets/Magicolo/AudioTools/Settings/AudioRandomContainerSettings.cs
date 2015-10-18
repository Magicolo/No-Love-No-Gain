
using UnityEngine;
using System.Collections;
using System;
using Magicolo;
using System.Collections.Generic;

namespace Magicolo.AudioTools
{
	/// <summary>
	/// Container that will play a random source based on the Weights array.
	/// </summary>
	public class AudioRandomContainerSettings : AudioContainerSettings, ICopyable<AudioRandomContainerSettings>
	{
		public List<float> Weights = new List<float>();

		public override AudioItem.AudioTypes Type { get { return AudioItem.AudioTypes.RandomContainer; } }

		public override void Recycle()
		{
			Pool<AudioRandomContainerSettings>.Recycle(this);
		}

		public override AudioSettingsBase Clone()
		{
			return Pool<AudioRandomContainerSettings>.Create(this);
		}

		public void Copy(AudioRandomContainerSettings reference)
		{
			base.Copy(reference);

			CopyHelper.CopyTo(reference.Weights, ref Weights);
		}
	}
}