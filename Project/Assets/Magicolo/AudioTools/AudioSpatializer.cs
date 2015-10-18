using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Magicolo;

namespace Magicolo.AudioTools
{
	public class AudioSpatializer : IPoolable, ICopyable<AudioSpatializer>
	{
		public enum SpatializeModes
		{
			None,
			Static,
			Dynamic
		}

		Vector3 _position;
		Transform _follow;
		Func<Vector3> _getPosition;
		SpatializeModes _spatializeMode;

		readonly List<Transform> _sources = new List<Transform>();

		public static AudioSpatializer Default = new AudioSpatializer();

		public SpatializeModes SpatializeMode { get { return _spatializeMode; } }
		public Vector3 Position { get { return _position; } }

		public void Initialize(Vector3 position)
		{
			_position = position;
			_spatializeMode = SpatializeModes.Static;
		}

		public void Initialize(Transform follow)
		{
			_follow = follow;
			_position = _follow.position;
			_spatializeMode = SpatializeModes.Dynamic;
		}

		public void Initialize(Func<Vector3> getPosition)
		{
			_getPosition = getPosition;
			_position = getPosition();
			_spatializeMode = SpatializeModes.Dynamic;
		}

		public void Spatialize()
		{
			if (_spatializeMode == SpatializeModes.Dynamic)
			{
				if (_getPosition != null)
					_position = _getPosition();
				else if (_follow != null)
					_position = _follow.position;
				else
					_spatializeMode = SpatializeModes.Static;

				for (int i = 0; i < _sources.Count; i++)
					_sources[i].position = _position;
			}
		}

		public void AddSource(Transform source)
		{
			_sources.Add(source);
			source.position = _position;
		}

		public void RemoveSource(Transform source)
		{
			_sources.Remove(source);
		}

		public void OnCreate()
		{

		}

		public void OnRecycle()
		{
			_sources.Clear();
		}

		public void Copy(AudioSpatializer reference)
		{
			_position = reference._position;
			_follow = reference._follow;
			_getPosition = reference._getPosition;
			_spatializeMode = reference._spatializeMode;
		}
	}
}