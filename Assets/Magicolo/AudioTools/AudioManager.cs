using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Magicolo;
using Magicolo.GeneralTools;
using Magicolo.AudioTools;

namespace Magicolo
{
	// TODO Remove all removable memory allocations
	// TODO Uniformize RTPCValues and SwitchValues
	// TODO Find a clean way to limit instances of multiple Settings together
	// TODO AudioSettings editors should all have unique colors/icons
	// TODO Add random selection types in AudioRandomContainerSettings
	// TODO Documentation for everything
	// FIXME Minor editor issue: when scrollbar is visible, AudioOption and AudioRTPC are partially under it
	public class AudioManager : Singleton<AudioManager>
	{
		[SerializeField]
		AudioSource _reference;
		AudioItemManager _itemManager = new AudioItemManager();

		Dictionary<string, AudioValue<int>> _switchValues = new Dictionary<string, AudioValue<int>>();

		[Tooltip("If you use custom curves in the Reference AudioSource, set this to true.")]
		public bool UseCustomCurves = true;

		public AudioSource Reference
		{
			get
			{
				if (_reference == null)
					InitializeReference();

				return _reference;
			}
		}
		public AudioItemManager ItemManager { get { return _itemManager; } }

		protected override void Awake()
		{
			base.Awake();

			InitializeReference();
		}

		void Reset()
		{
			InitializeReference();
		}

		void Update()
		{
			_itemManager.Update();
		}

		void InitializeReference()
		{
			_reference = gameObject.FindOrAddChild("Reference").GetOrAddComponent<AudioSource>();
			_reference.playOnAwake = false;
			_reference.spatialBlend = 1f;
		}

		public AudioItem CreateItem(AudioSettingsBase settings)
		{
			return _itemManager.CreateItem(settings);
		}

		public AudioItem CreateItem(AudioSettingsBase settings, Vector3 position)
		{
			return _itemManager.CreateItem(settings, position);
		}

		public AudioItem CreateItem(AudioSettingsBase settings, Transform follow)
		{
			return _itemManager.CreateItem(settings, follow);
		}

		public AudioItem CreateItem(AudioSettingsBase settings, Func<Vector3> getPosition)
		{
			return _itemManager.CreateItem(settings, getPosition);
		}

		public AudioItem CreateDynamicItem(Func<AudioDynamicItem, AudioDynamicData, AudioSettingsBase> getNextSettings)
		{
			return ItemManager.CreateDynamicItem(getNextSettings);
		}

		public AudioItem CreateDynamicItem(Func<AudioDynamicItem, AudioDynamicData, AudioSettingsBase> getNextSettings, Vector3 position)
		{
			return ItemManager.CreateDynamicItem(getNextSettings, position);
		}

		public AudioItem CreateDynamicItem(Func<AudioDynamicItem, AudioDynamicData, AudioSettingsBase> getNextSettings, Transform follow)
		{
			return ItemManager.CreateDynamicItem(getNextSettings, follow);
		}

		public AudioItem CreateDynamicItem(Func<AudioDynamicItem, AudioDynamicData, AudioSettingsBase> getNextSettings, Func<Vector3> getPosition)
		{
			return ItemManager.CreateDynamicItem(getNextSettings, getPosition);
		}

		public void StopAll()
		{
			ItemManager.StopAll();
		}

		public AudioValue<int> GetSwitchValue(string name)
		{
			AudioValue<int> value;

			if (!_switchValues.ContainsKey(name))
			{
				value = Pool<AudioValue<int>>.Create();
				_switchValues[name] = value;
			}
			else
				value = _switchValues[name];

			return value;
		}

		public void SetSwitchValue(string name, int value)
		{
			GetSwitchValue(name).Value = value;
		}
	}
}