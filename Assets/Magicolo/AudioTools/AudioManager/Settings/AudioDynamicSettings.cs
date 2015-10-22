using UnityEngine;
using System.Collections;
using System;
using Magicolo;
using System.Runtime.Serialization;
using Magicolo.AudioTools;

namespace Magicolo.AudioTools
{
	public class AudioDynamicSettings : AudioContainerSettings, ICopyable<AudioDynamicSettings>
	{
		public override AudioItem.AudioTypes Type { get { return AudioItem.AudioTypes.Dynamic; } }

		public static AudioDynamicSettings Default = CreateInstance<AudioDynamicSettings>();

		public override void Recycle()
		{
			Pool<AudioDynamicSettings>.Recycle(this);
		}

		public override AudioSettingsBase Clone()
		{
			return Pool<AudioDynamicSettings>.Create(this);
		}

		public void Copy(AudioDynamicSettings reference)
		{
			base.Copy(reference);

		}
	}
}