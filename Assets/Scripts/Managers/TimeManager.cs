using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TimeManager : Singleton<TimeManager>
{
	public class TimeChannel
	{
		public float Time;
		public float DeltaTime;
		public float FixedDeltaTime;
		public float TimeScale;

		TimeChannels _channel;
		public TimeChannels Channel { get { return _channel; } }

		public TimeChannel(TimeChannels channel)
		{
			_channel = channel;
		}
	}

	public enum TimeChannels
	{
		UI,
		World,
		Player,
		Enemies,
		Count
	}

	TimeChannel[] _channels;

	protected override void Awake()
	{
		base.Awake();

		_channels = new TimeChannel[(int)TimeChannels.Count];

		for (int i = 0; i < _channels.Length; i++)
		{
			TimeChannel timeChannel = new TimeChannel((TimeChannels)i);

			_channels[i] = timeChannel;
		}
	}

	void Update()
	{
		for (int i = 0; i < _channels.Length; i++)
		{
			TimeChannel timeChannel = _channels[i];

			timeChannel.DeltaTime = Time.deltaTime * timeChannel.TimeScale;
			timeChannel.Time += timeChannel.DeltaTime;
		}
	}

	void FixedUpdate()
	{
		for (int i = 0; i < _channels.Length; i++)
		{
			TimeChannel timeChannel = _channels[i];

			timeChannel.DeltaTime = Time.fixedDeltaTime * timeChannel.TimeScale;
		}
	}

	public TimeChannel GetTimeChannel(TimeChannels channel)
	{
		return _channels[(int)channel];
	}

	public float GetDeltaTime(TimeChannels channel)
	{
		return _channels[(int)channel].DeltaTime;
	}

	public float GetFixedDeltaTime(TimeChannels channel)
	{
		return _channels[(int)channel].FixedDeltaTime;
	}

	public float GetTime(TimeChannels channel)
	{
		return _channels[(int)channel].Time;
	}

	public float GetTimeScale(TimeChannels channel)
	{
		return _channels[(int)channel].TimeScale;
	}

	public void SetTimeScale(TimeChannels channel, float timeScale)
	{
		_channels[(int)channel].TimeScale = timeScale;
	}
}
