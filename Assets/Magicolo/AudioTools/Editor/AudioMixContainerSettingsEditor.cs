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
	[CustomEditor(typeof(AudioMixContainerSettings)), CanEditMultipleObjects]
	public class AudioMixContainerSettingsEditor : AudioContainerSettingsEditor
	{
		SerializedProperty _delaysProperty;

		public override void OnInspectorGUI()
		{
			_delaysProperty = serializedObject.FindProperty("Delays");

			base.OnInspectorGUI();
		}

		public override void ShowSource(SerializedProperty arrayProperty, int index, SerializedProperty sourceProperty)
		{
			base.ShowSource(arrayProperty, index, sourceProperty);

			_delaysProperty.arraySize = arrayProperty.arraySize;

			if (sourceProperty.isExpanded)
			{
				EditorGUI.indentLevel++;

				EditorGUILayout.PropertyField(_sourceSettingsProperty);
				EditorGUILayout.PropertyField(_delaysProperty.GetArrayElementAtIndex(index), "Delay".ToGUIContent());
				ArrayFoldout(sourceProperty.FindPropertyRelative("Options"), disableOnPlay: false);

				EditorGUI.indentLevel--;
			}
		}

		public override void OnSourceDeleted(SerializedProperty arrayProperty, int index)
		{
			base.OnSourceDeleted(arrayProperty, index);

			DeleteFromArray(_delaysProperty, index);
		}

		public override void OnSourceReordered(SerializedProperty arrayProperty, int sourceIndex, int targetIndex)
		{
			base.OnSourceReordered(arrayProperty, sourceIndex, targetIndex);

			ReorderArray(_delaysProperty, sourceIndex, targetIndex);
		}
	}
}