﻿using UnityEngine;
using UnityEditor;

namespace Magicolo.EditorTools {
	[CustomPropertyDrawer(typeof(EmptyAttribute))]
	public class EmptyDrawer : CustomAttributePropertyDrawerBase {

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			drawPrefixLabel = false;
			
			Begin(position, property, label);
			
			EditorGUI.PropertyField(_currentPosition, property, label, true);
			
			End();
		}
	}
}