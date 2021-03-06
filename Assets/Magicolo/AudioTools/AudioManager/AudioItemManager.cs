﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Magicolo;

namespace Magicolo.AudioTools
{
	public class AudioItemManager
	{
		Dictionary<int, List<AudioItem>> _idActiveItems = new Dictionary<int, List<AudioItem>>();
		List<AudioItem> _toUpdate = new List<AudioItem>();

		public void Update()
		{
			for (int i = _toUpdate.Count; i-- > 0;)
				_toUpdate[i].Update();
		}

		public void Activate(AudioItem item)
		{
			_toUpdate.Add(item);
		}

		public void Deactivate(AudioItem item)
		{
			if (_idActiveItems.ContainsKey(item.Id))
				_idActiveItems[item.Id].Remove(item);

			_toUpdate.Remove(item);
		}

		public void TrimInstances(AudioItem item, int maxInstances)
		{
			if (!_idActiveItems.ContainsKey(item.Id))
				_idActiveItems[item.Id] = new List<AudioItem>();

			List<AudioItem> _activeItems = _idActiveItems[item.Id];

			if (maxInstances > 0)
				while (_activeItems.Count >= maxInstances)
					_activeItems.Pop().StopImmediate();

			_idActiveItems[item.Id].Add(item);
		}

		public AudioItem CreateItem(AudioSettingsBase settings)
		{
			AudioSpatializer spatializer = Pool<AudioSpatializer>.Create(AudioSpatializer.Default);

			return CreateItem(settings, spatializer, null);
		}

		public AudioItem CreateItem(AudioSettingsBase settings, Vector3 position)
		{
			AudioSpatializer spatializer = Pool<AudioSpatializer>.Create(AudioSpatializer.Default);
			spatializer.Initialize(position);

			return CreateItem(settings, spatializer, null);
		}

		public AudioItem CreateItem(AudioSettingsBase settings, Transform follow)
		{
			AudioSpatializer spatializer = Pool<AudioSpatializer>.Create(AudioSpatializer.Default);
			spatializer.Initialize(follow);

			return CreateItem(settings, spatializer, null);
		}

		public AudioItem CreateItem(AudioSettingsBase settings, Func<Vector3> getPosition)
		{
			AudioSpatializer spatializer = Pool<AudioSpatializer>.Create(AudioSpatializer.Default);
			spatializer.Initialize(getPosition);

			return CreateItem(settings, spatializer, null);
		}

		public AudioItem CreateItem(AudioSettingsBase settings, AudioSpatializer spatializer, AudioItem parent)
		{
			if (settings == null)
				return null;

			AudioItem item;

			switch (settings.Type)
			{
				default:
					AudioSourceItem sourceItem = Pool<AudioSourceItem>.Create(AudioSourceItem.Default);
					AudioSource source = ComponentPool<AudioSource>.Create(AudioManager.Instance.Reference);
					source.Copy(AudioManager.Instance.Reference, AudioManager.Instance.UseCustomCurves);
					sourceItem.Initialize((AudioSourceSettings)settings, source, spatializer, parent);
					item = sourceItem;
					break;
				case AudioItem.AudioTypes.MixContainer:
					AudioMixContainerItem mixContainerItem = Pool<AudioMixContainerItem>.Create(AudioMixContainerItem.Default);
					mixContainerItem.Initialize((AudioMixContainerSettings)settings, spatializer, parent);
					item = mixContainerItem;
					break;
				case AudioItem.AudioTypes.RandomContainer:
					AudioRandomContainerItem randomContainerItem = Pool<AudioRandomContainerItem>.Create(AudioRandomContainerItem.Default);
					randomContainerItem.Initialize((AudioRandomContainerSettings)settings, spatializer, parent);
					item = randomContainerItem;
					break;
				case AudioItem.AudioTypes.EnumeratorContainer:
					AudioEnumeratorContainerItem enumeratorContainerItem = Pool<AudioEnumeratorContainerItem>.Create(AudioEnumeratorContainerItem.Default);
					enumeratorContainerItem.Initialize((AudioEnumeratorContainerSettings)settings, spatializer, parent);
					item = enumeratorContainerItem;
					break;
				case AudioItem.AudioTypes.SwitchContainer:
					AudioSwitchContainerItem switchContainerItem = Pool<AudioSwitchContainerItem>.Create(AudioSwitchContainerItem.Default);
					switchContainerItem.Initialize((AudioSwitchContainerSettings)settings, spatializer, parent);
					item = switchContainerItem;
					break;
				case AudioItem.AudioTypes.SequenceContainer:
					AudioSequenceContainerItem sequenceContainerItem = Pool<AudioSequenceContainerItem>.Create(AudioSequenceContainerItem.Default);
					sequenceContainerItem.Initialize((AudioSequenceContainerSettings)settings, spatializer, parent);
					item = sequenceContainerItem;
					break;
			}

			return item;
		}

		public AudioItem CreateDynamicItem(Func<AudioDynamicItem, AudioDynamicData, AudioSettingsBase> getNextSettings)
		{
			AudioSpatializer spatializer = Pool<AudioSpatializer>.Create(AudioSpatializer.Default);

			return CreateDynamicItem(getNextSettings, spatializer, null);
		}

		public AudioItem CreateDynamicItem(Func<AudioDynamicItem, AudioDynamicData, AudioSettingsBase> getNextSettings, Vector3 position)
		{
			AudioSpatializer spatializer = Pool<AudioSpatializer>.Create(AudioSpatializer.Default);
			spatializer.Initialize(position);

			return CreateDynamicItem(getNextSettings, spatializer, null);
		}

		public AudioItem CreateDynamicItem(Func<AudioDynamicItem, AudioDynamicData, AudioSettingsBase> getNextSettings, Transform follow)
		{
			AudioSpatializer spatializer = Pool<AudioSpatializer>.Create(AudioSpatializer.Default);
			spatializer.Initialize(follow);

			return CreateDynamicItem(getNextSettings, spatializer, null);
		}

		public AudioItem CreateDynamicItem(Func<AudioDynamicItem, AudioDynamicData, AudioSettingsBase> getNextSettings, Func<Vector3> getPosition)
		{
			AudioSpatializer spatializer = Pool<AudioSpatializer>.Create(AudioSpatializer.Default);
			spatializer.Initialize(getPosition);

			return CreateDynamicItem(getNextSettings, spatializer, null);
		}

		public AudioDynamicItem CreateDynamicItem(Func<AudioDynamicItem, AudioDynamicData, AudioSettingsBase> getNextSettings, AudioSpatializer spatializer, AudioItem parent)
		{
			AudioDynamicItem item = Pool<AudioDynamicItem>.Create(AudioDynamicItem.Default);
			item.Initialize(getNextSettings, spatializer, parent);

			return item;
		}

		public void StopAll()
		{
			for (int i = _toUpdate.Count; i-- > 0;)
				_toUpdate[i].StopImmediate();
		}
	}
}