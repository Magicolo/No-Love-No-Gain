using UnityEngine;
using System.Collections;
using Magicolo;
using System;
using System.Collections.Generic;

namespace Magicolo
{
	public abstract class AudioSettingsBase : ScriptableObject, INamable, IPoolable, IClonable<AudioSettingsBase>, ICopyable<AudioSettingsBase>
	{
		public enum PitchScaleModes
		{
			Ratio,
			Semitone
		}

		string _name;

		public string Name { get { return string.IsNullOrEmpty(_name) ? (_name = name) : _name; } set { _name = value; } }
		public abstract AudioItem.AudioTypes Type { get; }
		public bool Loop;
		[Min]
		public float FadeIn;
		public TweenManager.Ease FadeInEase = TweenManager.Ease.InQuad;
		[Min]
		public float FadeOut;
		public TweenManager.Ease FadeOutEase = TweenManager.Ease.OutQuad;
		[Range(0f, 5f)]
		public float VolumeScale = 1f;
		public PitchScaleModes PitchScaleMode;
		[Range(0.0001f, 5f)]
		public float PitchScale = 1f;
		[Range(0f, 1f)]
		public float RandomVolume;
		[Range(0f, 1f)]
		public float RandomPitch;
		public List<AudioRTPC> RTPCs = new List<AudioRTPC>();
		public List<AudioOption> Options = new List<AudioOption>();

		public AudioRTPC GetRTPC(string name)
		{
			for (int i = 0; i < RTPCs.Count; i++)
			{
				AudioRTPC rtpc = RTPCs[i];

				if (rtpc.Name == name)
					return rtpc;
			}

			return null;
		}

		public abstract void Recycle();
		public abstract AudioSettingsBase Clone();

		public virtual void OnCreate()
		{
			Pool<AudioRTPC>.CreateElements(RTPCs);
			Pool<AudioOption>.CreateElements(Options);
		}

		public virtual void OnRecycle()
		{
			Pool<AudioRTPC>.RecycleElements(RTPCs);
			Pool<AudioOption>.RecycleElements(Options);
		}

		public void Copy(AudioSettingsBase reference)
		{
			_name = reference._name;
			Loop = reference.Loop;
			FadeIn = reference.FadeIn;
			FadeInEase = reference.FadeInEase;
			FadeOut = reference.FadeOut;
			FadeOutEase = reference.FadeOutEase;
			VolumeScale = reference.VolumeScale;
			PitchScaleMode = reference.PitchScaleMode;
			PitchScale = reference.PitchScale;
			RandomVolume = reference.RandomVolume;
			RandomPitch = reference.RandomPitch;
			CopyHelper.CopyTo(reference.RTPCs, ref RTPCs);
			CopyHelper.CopyTo(reference.Options, ref Options);
		}
	}
}