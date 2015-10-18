﻿using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace Magicolo.EditorTools
{
	[CustomPropertyDrawer(typeof(ButtonAttribute))]
	public class ButtonDrawer : CustomAttributePropertyDrawerBase
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			Begin(position, property, label);

			if (property.propertyType == SerializedPropertyType.Boolean)
			{
				string buttonLabel = string.IsNullOrEmpty(((ButtonAttribute)attribute).label) ? label.text : ((ButtonAttribute)attribute).label;
				string buttonPressedMethodName = string.IsNullOrEmpty(((ButtonAttribute)attribute).methodName) ? label.text.Replace(" ", "").Replace("_", "").Capitalized() : ((ButtonAttribute)attribute).methodName;
				string buttonIndexVariableName = ((ButtonAttribute)attribute).indexVariableName;
				GUIStyle buttonStyle = ((ButtonAttribute)attribute).style;
				_currentPosition = AttributeUtility.BeginIndentation(_currentPosition);

				if (noFieldLabel) buttonLabel = "";

				bool pressed;
				if (buttonStyle != null)
				{
					pressed = GUI.Button(_currentPosition, buttonLabel, buttonStyle);
				}
				else
				{
					pressed = GUI.Button(_currentPosition, buttonLabel);
				}

				AttributeUtility.EndIndentation();

				if (pressed)
				{
					if (!string.IsNullOrEmpty(buttonIndexVariableName))
					{
						property.serializedObject.FindProperty(buttonIndexVariableName).intValue = _index;
					}

					if (!string.IsNullOrEmpty(buttonPressedMethodName))
					{
						(property.serializedObject.targetObject as MonoBehaviour).Invoke(buttonPressedMethodName, 0);
					}

					EditorUtility.SetDirty(property.serializedObject.targetObject);
				}
				property.boolValue = pressed;
			}
			else
			{
				EditorGUI.LabelField(_currentPosition, "Button variable must be of type boolean.");
			}

			End();
		}
	}
}
