using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Magicolo;

public class Kronos : Singleton<Kronos>
{
	public class TimeChannel
	{
		public float Time;
		public float DeltaTime;
		public float FixedDeltaTime;
		public float TimeScale = 1f;

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
		Enemy,
		Count
	}

	List<TimeChannel> _channels = new List<TimeChannel>();

	public static TimeChannel UI = new TimeChannel(TimeChannels.UI);
	public static TimeChannel World = new TimeChannel(TimeChannels.World);
	public static TimeChannel Player = new TimeChannel(TimeChannels.Player);
	public static TimeChannel Enemy = new TimeChannel(TimeChannels.Enemy);

	protected override void Awake()
	{
		base.Awake();

		_channels.Add(UI);
		_channels.Add(World);
		_channels.Add(Player);
		_channels.Add(Enemy);
	}

	void Update()
	{
		for (int i = 0; i < _channels.Count; i++)
		{
			TimeChannel timeChannel = _channels[i];
			timeChannel.DeltaTime = Time.deltaTime * timeChannel.TimeScale;
			timeChannel.Time += timeChannel.DeltaTime;
		}
	}

	void FixedUpdate()
	{
		for (int i = 0; i < _channels.Count; i++)
		{
			TimeChannel timeChannel = _channels[i];
			timeChannel.FixedDeltaTime = Time.fixedDeltaTime * timeChannel.TimeScale;
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
