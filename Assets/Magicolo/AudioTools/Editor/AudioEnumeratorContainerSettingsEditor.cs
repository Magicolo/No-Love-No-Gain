using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Magicolo;
using UnityEditor;
using Magicolo.EditorTools;

namespace Magicolo.AudioTools
{
	[CustomEditor(typeof(AudioEnumeratorContainerSettings)), CanEditMultipleObjects]
	public class AudioEnumeratorContainerSettingsEditor : AudioContainerSettingsEditor
	{
		SerializedProperty _repeats;

		public override void OnInspectorGUI()
		{
			_repeats = serializedObject.FindProperty("Repeats");

			base.OnInspectorGUI();
		}

		public override void ShowSource(SerializedProperty arrayProperty, int index, SerializedProperty sourceProperty)
		{
			base.ShowSource(arrayProperty, index, sourceProperty);

			_repeats.arraySize = arrayProperty.arraySize;

			if (sourceProperty.isExpanded)
			{
				EditorGUI.indentLevel++;

				EditorGUILayout.PropertyField(_sourceSettingsProperty);
				EditorGUILayout.PropertyField(_repeats.GetArrayElementAtIndex(index), "Repeat".ToGUIContent());
				ArrayFoldout(sourceProperty.FindPropertyRelative("Options"), disableOnPlay: false);

				EditorGUI.indentLevel--;
			}
		}

		public override void OnSourceDeleted(SerializedProperty arrayProperty, int index)
		{
			base.OnSourceDeleted(arrayProperty, index);

			DeleteFromArray(_repeats, index);
		}

		public override void OnSourceReordered(SerializedProperty arrayProperty, int sourceIndex, int targetIndex)
		{
			base.OnSourceReordered(arrayProperty, sourceIndex, targetIndex);

			ReorderArray(_repeats, sourceIndex, targetIndex);
		}
	}
}