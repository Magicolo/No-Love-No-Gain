using UnityEngine;
using System.Collections;
using System;
using Magicolo;
using Magicolo.AudioTools;
using System.Collections.Generic;

namespace Magicolo.AudioTools
{
	/// <summary>
	/// Container that will only play the sources that correspond to the value stored in AudioManager.Instance.States[StateName].
	/// </summary>
	public class AudioSwitchContainerSettings : AudioContainerSettings, ICopyable<AudioSwitchContainerSettings>
	{
		public string SwitchName;
		public List<int> SwitchValues = new List<int>();

		public override AudioItem.AudioTypes Type { get { return AudioItem.AudioTypes.SwitchContainer; } }

		public override void Recycle()
		{
			Pool<AudioSwitchContainerSettings>.Recycle(this);
		}

		public override AudioSettingsBase Clone()
		{
			return Pool<AudioSwitchContainerSettings>.Create(this);
		}

		public void Copy(AudioSwitchContainerSettings reference)
		{
			base.Copy(reference);

			SwitchName = reference.SwitchName;
			CopyHelper.CopyTo(reference.SwitchValues, ref SwitchValues);
		}
	}
}