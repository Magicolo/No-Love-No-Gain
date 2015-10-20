using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Magicolo;
using System.Reflection;
using System;
using System.Runtime.CompilerServices;
using Magicolo.AudioTools;

namespace Magicolo.EditorTools
{
	public static class AudioCustomMenus
	{
		[MenuItem("Assets/Create/Audio Settings/Source", validate = true)]
		static bool CreateAudioSourceSettingsValid()
		{
			return Array.Exists(Selection.objects, obj => obj is AudioClip);
		}

		[MenuItem("Assets/Create/Audio Settings/Source", priority = -1)]
		static void CreateAudioSourceSettings()
		{
			for (int i = 0; i < Selection.objects.Length; i++)
			{
				AudioClip clip = Selection.objects[i] as AudioClip;

				if (clip == null)
					continue;

				AudioSourceSettings settings = CreateAudioContainerSettings<AudioSourceSettings>(clip.name, AssetDatabase.GetAssetPath(clip));
				settings.name = clip.name;
				settings.Clip = clip;
			}
		}

		[MenuItem("Assets/Create/Audio Settings/Container/Mix", priority = 0)]
		static void CreateAudioMixContainerSettings()
		{
			CreateAudioContainerSettings<AudioMixContainerSettings>("Mix Container");
		}

		[MenuItem("Assets/Create/Audio Settings/Container/Random", priority = 1)]
		static void CreateAudioRandomContainerSettings()
		{
			CreateAudioContainerSettings<AudioRandomContainerSettings>("Random Container");
		}

		[MenuItem("Assets/Create/Audio Settings/Container/Enumerator", priority = 2)]
		static void CreateAudioEnumeratorContainerSettings()
		{
			CreateAudioContainerSettings<AudioEnumeratorContainerSettings>("Enumerator Container");
		}

		[MenuItem("Assets/Create/Audio Settings/Container/Switch", priority = 3)]
		static void CreateAudioSwitchContainerSettings()
		{
			CreateAudioContainerSettings<AudioSwitchContainerSettings>("Switch Container");
		}

		[MenuItem("Assets/Create/Audio Settings/Container/Sequence", priority = 4)]
		static void CreateAudioSequenceContainerSettings()
		{
			CreateAudioContainerSettings<AudioSequenceContainerSettings>("Sequence Container");
		}

		static T CreateAudioContainerSettings<T>(string name, string settingsPath = "") where T : AudioSettingsBase
		{
			string assetDirectory;

			if (!string.IsNullOrEmpty(settingsPath))
				assetDirectory = Path.GetDirectoryName(settingsPath);
			if (Selection.activeObject == null)
				assetDirectory = "Assets";
			else if (Selection.activeObject is DefaultAsset)
				assetDirectory = AssetDatabase.GetAssetPath(Selection.activeObject);
			else
				assetDirectory = Path.GetDirectoryName(AssetDatabase.GetAssetPath(Selection.activeObject));

			T settings = ScriptableObject.CreateInstance<T>();
			string path = AssetDatabase.GenerateUniqueAssetPath(assetDirectory + "/" + name + ".asset");
			AssetDatabase.CreateAsset(settings, path);

			return settings;
		}
	}
}
