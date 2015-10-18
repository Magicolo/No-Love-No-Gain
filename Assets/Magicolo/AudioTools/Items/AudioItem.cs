using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Magicolo;
using Magicolo.AudioTools;

namespace Magicolo
{
	[Serializable]
	public abstract class AudioItem : IPoolable, ICopyable<AudioItem>
	{
		public enum AudioStates
		{
			Waiting,
			Playing,
			Paused,
			Stopping,
			Stopped,
		}

		public enum AudioTypes
		{
			Source,
			Dynamic,
			MixContainer,
			RandomContainer,
			EnumeratorContainer,
			SwitchContainer,
			SequenceContainer,
		}

		protected int _id;
		protected string _name;
		protected AudioStates _state;
		protected AudioSpatializer _spatializer;
		protected AudioItem _parent;
		protected double _scheduledTime;
		protected bool _scheduleStarted;
		protected AudioModifier _volumeModifier;
		protected AudioModifier _pitchModifier;
		protected FloatTweener _rampVolumeTweener;
		protected FloatTweener _rampParentVolumeTweener;
		protected FloatTweener _rampPitchTweener;
		protected FloatTweener _rampParentPitchTweener;
		protected FloatTweener _fadeTweener;
		protected AudioStates _pausedState;
		protected bool _hasFaded;
		protected bool _break;
		protected List<AudioDelayedOption> _delayedOptions = new List<AudioDelayedOption>();

		protected readonly Action _stopImmediate;
		protected readonly Func<float> _getDeltaTime;
		protected readonly Action<float> _setVolumeRampModifier;
		protected readonly Action<float> _setVolumeParentModifier;
		protected readonly Action<float> _setPitchRampModifier;
		protected readonly Action<float> _setPitchParentModifier;
		protected readonly Action<float> _setVolumeFadeModifier;

		public string Name { get { return _name; } }
		public int Id { get { return _id; } }
		public AudioStates State { get { return _state; } }
		public AudioSpatializer Spatializer { get { return _spatializer; } }
		public bool IsPlaying { get { return (_state == AudioStates.Playing || _state == AudioStates.Stopping) && (_scheduledTime <= 0d || _scheduleStarted); } }
		public abstract AudioSettingsBase Settings { get; }
		public abstract AudioTypes Type { get; }

		public event Action<AudioItem> OnPlay;
		public event Action<AudioItem> OnPause;
		public event Action<AudioItem> OnResume;
		public event Action<AudioItem> OnStopping;
		public event Action<AudioItem> OnStop;
		public event Action<AudioItem> OnUpdate;
		public event Action<AudioItem, AudioStates, AudioStates> OnStateChanged;

		protected AudioItem()
		{
			_stopImmediate = StopImmediate;
			_getDeltaTime = GetDeltaTime;
			_setVolumeRampModifier = value => _volumeModifier.RampModifier = value;
			_setVolumeParentModifier = value => _volumeModifier.ParentModifier = value;
			_setPitchRampModifier = value => _pitchModifier.RampModifier = value;
			_setPitchParentModifier = value => _pitchModifier.ParentModifier = value;
			_setVolumeFadeModifier = value => _volumeModifier.FadeModifier = value;
		}

		protected virtual void Initialize(int id, string name, AudioSpatializer spatializer, AudioItem parent)
		{
			_id = id;
			_name = name;
			_spatializer = spatializer;
			_parent = parent;

			if (_parent == null)
				AudioManager.Instance.ItemManager.Activate(this);

			SetState(AudioStates.Waiting);
		}

		protected virtual void SetState(AudioStates state)
		{
			_break |= state == AudioStates.Stopping || state == AudioStates.Stopped;

			RaiseStateChangeEvent(_state, (_state = state));
		}

		protected virtual void RaisePlayEvent()
		{
			if (OnPlay != null)
				OnPlay(this);
		}

		protected virtual void RaisePauseEvent()
		{
			if (OnPause != null)
				OnPause(this);
		}

		protected virtual void RaiseResumeEvent()
		{
			if (OnResume != null)
				OnResume(this);
		}

		protected virtual void RaiseStoppingEvent()
		{
			if (OnStopping != null)
				OnStopping(this);
		}

		protected virtual void RaiseStopEvent()
		{
			if (OnStop != null)
				OnStop(this);
		}

		protected virtual void RaiseUpdateEvent()
		{
			if (OnUpdate != null)
				OnUpdate(this);
		}

		protected virtual void RaiseStateChangeEvent(AudioStates oldState, AudioStates newState)
		{
			if (OnStateChanged != null)
				OnStateChanged(this, oldState, newState);
		}

		protected virtual void FadeIn()
		{
			if (_hasFaded)
				return;

			_hasFaded = true;
			_fadeTweener.Ramp(0f, 1f, Settings.FadeIn, _setVolumeFadeModifier, ease: Settings.FadeInEase, getDeltaTime: _getDeltaTime);
		}

		protected virtual void FadeOut()
		{
			_fadeTweener.Ramp(_volumeModifier.FadeModifier, 0f, Settings.FadeOut, _setVolumeFadeModifier, ease: Settings.FadeOutEase, getDeltaTime: _getDeltaTime, endCallback: _stopImmediate);
		}

		protected virtual float GetDeltaTime()
		{
			return Application.isPlaying ? Time.unscaledDeltaTime : 0.01f;
		}

		protected virtual void Spatialize()
		{
			if (_parent == null)
				_spatializer.Spatialize();
		}

		protected virtual void UpdateOptions()
		{
			for (int i = 0; i < _delayedOptions.Count; i++)
			{
				AudioDelayedOption delayedOption = _delayedOptions[i];

				if (delayedOption.Update())
				{
					ApplyOptionNow(delayedOption.Option, delayedOption.Recycle);
					_delayedOptions.RemoveAt(i--);
					Pool<AudioDelayedOption>.Recycle(delayedOption);
				}
			}
		}

		protected void UpdateTweeners()
		{
			if (_state == AudioStates.Stopped)
				return;

			_rampVolumeTweener.Update();
			_rampParentVolumeTweener.Update();
			_rampPitchTweener.Update();
			_rampParentPitchTweener.Update();
			_fadeTweener.Update();
		}

		protected void UpdateRTPCs()
		{
			if (_state == AudioStates.Stopped)
				return;

			float volumeRtpc = 1f;
			float pitchRtpc = 1f;

			for (int i = 0; i < Settings.RTPCs.Count; i++)
			{
				AudioRTPC rtpc = Settings.RTPCs[i];

				switch (rtpc.Type)
				{
					case AudioRTPC.RTPCTypes.Volume:
						volumeRtpc *= rtpc.GetAdjustedValue();
						break;
					case AudioRTPC.RTPCTypes.Pitch:
						pitchRtpc *= rtpc.GetAdjustedValue();
						break;
				}
			}

			_volumeModifier.RTPCModifier = volumeRtpc;
			_pitchModifier.RTPCModifier = pitchRtpc;
		}

		public virtual void Update()
		{
			if (_state == AudioStates.Stopped)
				return;

			if (_scheduledTime > 0d)
				_scheduleStarted |= _scheduledTime <= AudioSettings.dspTime;

			Spatialize();
			UpdateOptions();
			UpdateRTPCs();
			UpdateTweeners();

			RaiseUpdateEvent();
		}

		public abstract void Play();
		public abstract void PlayScheduled(double time);
		public abstract void Pause();
		public abstract void Resume();
		public abstract void Stop();
		public abstract void StopImmediate();
		public virtual void ApplyOption(AudioOption option, bool recycle = true)
		{
			if (option.Delay > 0f)
				ApplyOptionDelayed(option, recycle);
			else
				ApplyOptionNow(option, recycle);
		}
		protected virtual void ApplyOptionDelayed(AudioOption option, bool recycle)
		{
			AudioDelayedOption delayedOption = Pool<AudioDelayedOption>.Create(AudioDelayedOption.Default);
			delayedOption.Initialize(option, recycle, _getDeltaTime);
			_delayedOptions.Add(delayedOption);
		}
		protected abstract void ApplyOptionNow(AudioOption option, bool recycle);
		public virtual double GetScheduledTime() { return _scheduledTime; }
		public abstract void SetScheduledTime(double time);
		public abstract double RemainingTime();
		public abstract void Break();
		public abstract void SetRTPCValue(string name, float value);
		public virtual AudioItem GetParent() { return _parent; }
		public abstract List<AudioItem> GetChildren();
		public virtual void ClearCallbacks()
		{
			OnPlay = null;
			OnPause = null;
			OnResume = null;
			OnStopping = null;
			OnStop = null;
			OnUpdate = null;
			OnStateChanged = null;
		}

		public virtual float GetVolumeScale()
		{
			if (_state == AudioStates.Stopped)
				return 0f;

			return _volumeModifier.Value;
		}
		protected virtual void SetVolumeScale(float volume, float time, TweenManager.Ease ease, bool fromSelf)
		{
			if (_state == AudioStates.Stopped)
				return;

			if (fromSelf)
			{
				_rampVolumeTweener.Stop();

				if (time > 0f)
					_rampVolumeTweener.Ramp(_volumeModifier.RampModifier, volume, time, _setVolumeRampModifier, ease, _getDeltaTime);
				else
					_volumeModifier.RampModifier = volume;
			}
			else
			{
				_rampParentVolumeTweener.Stop();

				if (time > 0f)
					_rampParentVolumeTweener.Ramp(_volumeModifier.ParentModifier, volume, time, _setVolumeParentModifier, ease, _getDeltaTime);
				else
					_volumeModifier.ParentModifier = volume;
			}
		}
		public virtual void SetVolumeScale(float volume, float time, TweenManager.Ease ease = TweenManager.Ease.Linear) { SetVolumeScale(volume, time, ease, false); }
		public virtual void SetVolumeScale(float volume) { SetVolumeScale(volume, 0f, TweenManager.Ease.Linear, false); }

		public virtual float GetPitchScale()
		{
			if (_state == AudioStates.Stopped)
				return 0f;

			return _pitchModifier.Value;
		}
		protected virtual void SetPitchScale(float pitch, float time, TweenManager.Ease ease, bool fromSelf)
		{
			if (_state == AudioStates.Stopped)
				return;

			if (fromSelf)
			{
				_rampPitchTweener.Stop();

				if (time > 0f)
					_rampPitchTweener.Ramp(_pitchModifier.RampModifier, pitch, time, _setPitchRampModifier, ease, _getDeltaTime);
				else
					_pitchModifier.RampModifier = pitch;
			}
			else
			{
				_rampParentPitchTweener.Stop();

				if (time > 0f)
					_rampParentPitchTweener.Ramp(_pitchModifier.ParentModifier, pitch, time, _setPitchParentModifier, ease, _getDeltaTime);
				else
					_pitchModifier.ParentModifier = pitch;
			}
		}
		public virtual void SetPitchScale(float pitch, float time, TweenManager.Ease ease = TweenManager.Ease.Linear) { SetPitchScale(pitch, time, ease, false); }
		public virtual void SetPitchScale(float pitch) { SetPitchScale(pitch, 0f, TweenManager.Ease.Linear, false); }

		public virtual void OnCreate()
		{
			_volumeModifier = Pool<AudioModifier>.Create(AudioModifier.Default);
			_pitchModifier = Pool<AudioModifier>.Create(AudioModifier.Default);
			_fadeTweener = Pool<FloatTweener>.Create(FloatTweener.Default);
			_rampVolumeTweener = Pool<FloatTweener>.Create(FloatTweener.Default);
			_rampParentVolumeTweener = Pool<FloatTweener>.Create(FloatTweener.Default);
			_rampPitchTweener = Pool<FloatTweener>.Create(FloatTweener.Default);
			_rampParentPitchTweener = Pool<FloatTweener>.Create(FloatTweener.Default);
			Pool<AudioDelayedOption>.CreateElements(_delayedOptions);
		}

		public virtual void OnRecycle()
		{
			Pool<AudioModifier>.Recycle(ref _volumeModifier);
			Pool<AudioModifier>.Recycle(ref _pitchModifier);
			Pool<FloatTweener>.Recycle(ref _fadeTweener);
			Pool<FloatTweener>.Recycle(ref _rampVolumeTweener);
			Pool<FloatTweener>.Recycle(ref _rampParentVolumeTweener);
			Pool<FloatTweener>.Recycle(ref _rampPitchTweener);
			Pool<FloatTweener>.Recycle(ref _rampParentPitchTweener);

			// Only the AudioItem root should recycle the spatializer as it is shared with it's children
			if (_parent == null)
				Pool<AudioSpatializer>.Recycle(ref _spatializer);

			RecycleDelayedOptions();
			ClearCallbacks();
		}

		void RecycleDelayedOptions()
		{
			for (int i = 0; i < _delayedOptions.Count; i++)
			{
				AudioDelayedOption delayedOption = _delayedOptions[i];

				if (delayedOption.Recycle)
					Pool<AudioOption>.Recycle(delayedOption.Option);

				Pool<AudioDelayedOption>.Recycle(delayedOption);
			}

			_delayedOptions.Clear();
		}

		public void Copy(AudioItem reference)
		{
			_id = reference._id;
			_name = reference._name;
			_state = reference._state;
			_spatializer = reference._spatializer;
			_parent = reference._parent;
			_scheduledTime = reference._scheduledTime;
			_scheduleStarted = reference._scheduleStarted;
			_volumeModifier = reference._volumeModifier;
			_pitchModifier = reference._pitchModifier;
			_rampVolumeTweener = reference._rampVolumeTweener;
			_rampParentVolumeTweener = reference._rampParentVolumeTweener;
			_rampPitchTweener = reference._rampPitchTweener;
			_rampParentPitchTweener = reference._rampParentPitchTweener;
			_fadeTweener = reference._fadeTweener;
			_pausedState = reference._pausedState;
			_hasFaded = reference._hasFaded;
			_break = reference._break;
			CopyHelper.CopyTo(reference._delayedOptions, ref _delayedOptions);
			OnPlay = reference.OnPlay;
			OnPause = reference.OnPause;
			OnResume = reference.OnResume;
			OnStopping = reference.OnStopping;
			OnStop = reference.OnStop;
			OnUpdate = reference.OnUpdate;
			OnStateChanged = reference.OnStateChanged;
		}

		public override string ToString()
		{
			return string.Format("{0}({1}, {2})", GetType(), Name, _state);
		}
	}
}
