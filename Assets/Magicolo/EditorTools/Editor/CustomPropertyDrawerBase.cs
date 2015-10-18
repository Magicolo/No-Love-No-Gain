﻿using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Magicolo;
using System;
using System.Reflection;


namespace Magicolo.EditorTools
{
	public class CustomPropertyDrawerBase : PropertyDrawer
	{

		public UnityEngine.Object _target;
		public UnityEngine.Object[] _targets;
		public SerializedProperty _currentProperty;
		public SerializedObject _serializedObject;
		public Rect _currentPosition;
		public float _lineHeight;
		public bool _isArray;
		public int _index;
		public float _scrollbarThreshold;
		public GUIContent _currentLabel = GUIContent.none;
		public Rect _initPosition;
		public SerializedProperty _arrayProperty;

		static MethodInfo _getPropertyDrawerMethod;
		public static MethodInfo GetPropertyDrawerMethod
		{
			get
			{
				if (_getPropertyDrawerMethod == null)
				{
					foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
					{
						foreach (Type type in assembly.GetTypes())
						{
							if (type.Name == "ScriptAttributeUtility")
							{
								_getPropertyDrawerMethod = type.GetMethod("GetDrawerTypeForType", ObjectExtensions.AllFlags);
							}
						}
					}
				}
				return _getPropertyDrawerMethod;
			}
		}

		bool _initialized;

		public virtual void Initialize(SerializedProperty property, GUIContent label)
		{
			_initialized = true;
			_isArray = typeof(IList).IsAssignableFrom(fieldInfo.FieldType);
			_lineHeight = EditorGUIUtility.singleLineHeight;

			if (_isArray)
			{
				_index = AttributeUtility.GetIndexFromLabel(label);
				_arrayProperty = property.GetParent();
			}
		}

		public virtual void Begin(Rect position, SerializedProperty property, GUIContent label)
		{
			_initPosition = position;
			_currentPosition = position;
			_currentProperty = property;
			_currentLabel = label;
			_serializedObject = property.serializedObject;
			_target = _serializedObject.targetObject;
			_targets = _serializedObject.targetObjects;
			_scrollbarThreshold = Screen.width - position.width > 19 ? 298 : 313;

			EditorGUI.BeginChangeCheck();
		}

		public virtual void End()
		{
			_serializedObject.ApplyModifiedProperties();

			if (EditorGUI.EndChangeCheck())
				EditorUtility.SetDirty(_serializedObject.targetObject);
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			if (!_initialized)
				Initialize(property, label);

			return EditorGUI.GetPropertyHeight(property, label, true);
		}

		public void ToggleButton(SerializedProperty boolProperty, GUIContent trueLabel, GUIContent falseLabel)
		{
			Rect indentedPosition = EditorGUI.IndentedRect(_currentPosition);
			//Rect labelPosition = new Rect(indentedPosition.x - EditorGUI.indentLevel * 8f, indentedPosition.y, indentedPosition.width, indentedPosition.height);

			//boolProperty.SetValue(EditorGUI.Toggle(indentedPosition, boolProperty.GetValue<bool>(), new GUIStyle("button")));
			boolProperty.SetValue(ToggleButton(indentedPosition, boolProperty.GetValue<bool>(), trueLabel, falseLabel));

			//GUIStyle style = new GUIStyle("label");
			//style.clipping = TextClipping.Overflow;
			//style.alignment = TextAnchor.MiddleCenter;

			//if (boolProperty.GetValue<bool>())
			//	EditorGUI.LabelField(labelPosition, trueLabel, style);
			//else
			//	EditorGUI.LabelField(labelPosition, falseLabel, style);

			_currentPosition.y += _currentPosition.height + 2;
		}

		public void PropertyField(SerializedProperty property, GUIContent label, bool includeChildren)
		{
			_currentPosition.height = EditorGUI.GetPropertyHeight(property, label, includeChildren);

			EditorGUI.PropertyField(_currentPosition, property, label, includeChildren);

			_currentPosition.y += _currentPosition.height + 2f;
		}

		public void PropertyField(SerializedProperty property, GUIContent label)
		{
			PropertyField(property, label, true);
		}

		public void PropertyField(SerializedProperty property)
		{
			PropertyField(property, property.displayName.ToGUIContent(), true);
		}

		public PropertyDrawer GetPropertyDrawer(Type propertyAttributeType, params object[] arguments)
		{
			Type propertyDrawerType = GetPropertyDrawerMethod.Invoke(null, new object[] { propertyAttributeType }) as Type;

			if (propertyDrawerType != null)
			{
				PropertyAttribute propertyAttribute = Activator.CreateInstance(propertyAttributeType, arguments) as PropertyAttribute;
				PropertyDrawer propertyDrawer = Activator.CreateInstance(propertyDrawerType) as PropertyDrawer;
				propertyDrawer.SetValueToMember("m_Attribute", propertyAttribute);
				propertyDrawer.SetValueToMember("m_FieldInfo", fieldInfo);
				return propertyDrawer;
			}

			return null;
		}

		public PropertyDrawer GetPropertyDrawer(Attribute propertyAttribute, params object[] arguments)
		{
			return GetPropertyDrawer(propertyAttribute.GetType(), arguments);
		}

		public static bool ToggleButton(Rect position, bool value, GUIContent trueLabel, GUIContent falseLabel)
		{
			Rect labelPosition = new Rect(position.x - EditorGUI.indentLevel * 8f, position.y, position.width, position.height);

			value = EditorGUI.Toggle(position, value, new GUIStyle("button"));

			GUIStyle style = new GUIStyle("label");
			style.clipping = TextClipping.Overflow;
			style.alignment = TextAnchor.MiddleCenter;
			style.contentOffset = new Vector2(-1f, 0f);

			if (value)
				EditorGUI.LabelField(labelPosition, trueLabel, style);
			else
				EditorGUI.LabelField(labelPosition, falseLabel, style);

			return value;
		}
	}
}
