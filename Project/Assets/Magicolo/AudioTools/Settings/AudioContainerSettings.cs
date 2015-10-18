using UnityEngine;
using System.Collections;
using System;
using Magicolo;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Magicolo.AudioTools
{
	public abstract class AudioContainerSettings : AudioSettingsBase, ICopyable<AudioContainerSettings>
	{
		public List<AudioContainerSourceData> Sources = new List<AudioContainerSourceData>();

		public override void OnCreate()
		{
			base.OnCreate();

			Pool<AudioContainerSourceData>.CreateElements(Sources);
		}

		public override void OnRecycle()
		{
			base.OnRecycle();

			Pool<AudioContainerSourceData>.RecycleElements(Sources);
		}

		public void Copy(AudioContainerSettings reference)
		{
			base.Copy(reference);

			CopyHelper.CopyTo(reference.Sources, ref Sources);
		}
	}
}