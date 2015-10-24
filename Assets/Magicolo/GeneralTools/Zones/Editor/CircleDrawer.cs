using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Magicolo;
using Magicolo.EditorTools;
using UnityEditor;

namespace Magicolo.GeneralTools
{
	[CustomPropertyDrawer(typeof(Circle))]
	public class CircleDrawer : CustomPropertyDrawerBase
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			Begin(position, property, label);

			property.isExpanded = true;

			EditorGUI.BeginProperty(position, label, property);
			_currentPosition.height = EditorGUI.GetPropertyHeight(property, label, false);
			EditorGUI.LabelField(_currentPosition, label);
			_currentPosition.y += _currentPosition.height;

			EditorGUI.indentLevel++;
			float labelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 28f;

			// X
			SerializedProperty xProperty = property.FindPropertyRelative("_x");
			Rect rect = new Rect(_currentPosition);
			rect.width /= 2f;
			rect.width += 4.25f;
			rect.height = EditorGUI.GetPropertyHeight(xProperty, xProperty.ToGUIContent());
			EditorGUI.BeginProperty(rect, label, xProperty);

			EditorGUI.BeginChangeCheck();

			float x = EditorGUI.FloatField(rect, xProperty.ToGUIContent(), xProperty.GetValue<float>());

			if (EditorGUI.EndChangeCheck())
				xProperty.SetValue(x);

			EditorGUI.EndProperty();

			// Y
			SerializedProperty yProperty = property.FindPropertyRelative("_y");
			rect.x += rect.width - 13f;
			rect.width += 4.25f;
			rect.height = EditorGUI.GetPropertyHeight(yProperty, yProperty.ToGUIContent());
			EditorGUI.BeginProperty(rect, label, yProperty);

			EditorGUI.BeginChangeCheck();

			float y = EditorGUI.FloatField(rect, yProperty.ToGUIContent(), yProperty.GetValue<float>());

			if (EditorGUI.EndChangeCheck())
				yProperty.SetValue(y);

			_currentPosition.y += _currentPosition.height;
			EditorGUIUtility.labelWidth = labelWidth;
			EditorGUI.EndProperty();

			// Radius
			PropertyField(property.FindPropertyRelative("_radius"));

			EditorGUI.indentLevel--;
			EditorGUI.EndProperty();

			End();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return 48f;
		}
	}
}