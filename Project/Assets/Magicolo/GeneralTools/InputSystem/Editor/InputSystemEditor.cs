using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Magicolo.EditorTools;

namespace Magicolo.GeneralTools {
	[CustomEditor(typeof(InputSystem)), CanEditMultipleObjects]
	public class InputSystemEditor : CustomEditorBase {
		
		InputSystem inputSystem;
		
		SerializedProperty keyboardInfosProperty;
		KeyboardInfo currentKeyboardInfo;
		SerializedProperty currentKeyboardInfoProperty;
		SerializedProperty keyboardButtonsProperty;
		SerializedProperty currentKeyboardButtonProperty;
		SerializedProperty keyboardAxesProperty;
		SerializedProperty currentKeyboardAxisProperty;
		SerializedProperty keyboardListenersProperty;
		SerializedProperty currentKeyboardListenerProperty;
		
		SerializedProperty joystickInfosProperty;
		JoystickInfo currentJoystickInfo;
		SerializedProperty currentJoystickInfoProperty;
		SerializedProperty joystickButtonsProperty;
		SerializedProperty currentJoystickButtonProperty;
		SerializedProperty joystickAxesProperty;
		SerializedProperty currentJoystickAxisProperty;
		SerializedProperty joystickListenersProperty;
		SerializedProperty currentJoystickListenerProperty;
		
		KeyCode[] keyboardKeys;
		string[] keyboardKeyNames;
		string[] keyboardAxes;
		
		public override void OnEnable() {
			base.OnEnable();
			
			inputSystem = (InputSystem)target;
			keyboardKeys = InputSystem.GetNonJoystickKeys();
			keyboardKeyNames = keyboardKeyNames ?? keyboardKeys.ToStringArray();
			keyboardAxes = InputSystemUtility.GetKeyboardAxes();
		}

		public override void OnInspectorGUI() {
			Begin();
			
			ShowKeyboardInfos();
			ShowJoystickInfos();
			
			Separator();
			
			End();
		}

		void ShowKeyboardInfos() {
			keyboardInfosProperty = serializedObject.FindProperty("keyboardInfos");
			
			if (AddFoldOut(keyboardInfosProperty, "Keyboards".ToGUIContent())) {
				KeyboardInfo[] keyboardInfos = inputSystem.GetKeyboardInfos();
				KeyboardInfo keyboardInfo = keyboardInfos.Last();
				
				keyboardInfo.SetUniqueName("default", "", keyboardInfos);
				
				serializedObject.Update();
			}
			
			if (keyboardInfosProperty.isExpanded) {
				EditorGUI.indentLevel += 1;
				
				for (int i = 0; i < keyboardInfosProperty.arraySize; i++) {
					currentKeyboardInfo = inputSystem.GetKeyboardInfos()[i];
					currentKeyboardInfoProperty = keyboardInfosProperty.GetArrayElementAtIndex(i);
				
					BeginBox();
					
					
					if (DeleteFoldOut(keyboardInfosProperty, i, currentKeyboardInfo.Name.ToGUIContent(), CustomEditorStyles.BoldFoldout)) {
						break;
					}
				
					ShowKeyboardInfo();
					
					EndBox();
				}
				
				Separator();
				EditorGUI.indentLevel -= 1;
			}
		}
		
		void ShowKeyboardInfo() {
			keyboardButtonsProperty = currentKeyboardInfoProperty.FindPropertyRelative("buttons");
			keyboardAxesProperty = currentKeyboardInfoProperty.FindPropertyRelative("axes");
			
			if (currentKeyboardInfoProperty.isExpanded) {
				EditorGUI.indentLevel += 1;
				
				UniqueNameField(currentKeyboardInfo, inputSystem.GetKeyboardInfos());
				
				Separator();
			
				ShowKeyboardButtons();
				ShowKeyboardAxes();
				
				Separator();
				
				ShowKeyboardListeners();
				
				Separator();
				EditorGUI.indentLevel -= 1;
			}
		}

		void ShowKeyboardButtons() {
			BeginBox();
			
			if (AddFoldOut(keyboardButtonsProperty)) {
				keyboardButtonsProperty.Last().FindPropertyRelative("name").SetValue("default");
			}
				
			if (keyboardButtonsProperty.isExpanded) {
				EditorGUI.indentLevel += 1;

				for (int i = 0; i < keyboardButtonsProperty.arraySize; i++) {
					currentKeyboardButtonProperty = keyboardButtonsProperty.GetArrayElementAtIndex(i);
						
					EditorGUILayout.BeginHorizontal();
					EditorGUI.BeginChangeCheck();
					
					KeyboardButton currentKeyboardKey = currentKeyboardInfo.GetButtons()[i];
					int index = System.Array.IndexOf(keyboardKeys, currentKeyboardKey.Key);
					
					index = EditorGUILayout.Popup(index, keyboardKeyNames);
					
					if (EditorGUI.EndChangeCheck()) {
						currentKeyboardKey.Key = keyboardKeys[Mathf.Clamp(index, 0, Mathf.Max(keyboardKeys.Length - 1, 0))];
						serializedObject.Update();
					}
						
					int indent = EditorGUI.indentLevel;
					EditorGUI.indentLevel = 0;
						
					EditorGUILayout.PropertyField(currentKeyboardButtonProperty.FindPropertyRelative("name"), GUIContent.none);
						
					EditorGUI.indentLevel = indent;
						
					if (DeleteButton(keyboardButtonsProperty, i, ButtonAlign.Right)) {
						break;
					}
						
					EditorGUILayout.EndHorizontal();
					
					Reorderable(keyboardButtonsProperty, i, true);
				}
					
				Separator();
				EditorGUI.indentLevel -= 1;
				
			}
			
			EndBox();
		}

		void ShowKeyboardAxes() {
			BeginBox();
				
			if (AddFoldOut(keyboardAxesProperty)) {
				keyboardAxesProperty.Last().FindPropertyRelative("name").SetValue("default");
			}
			
			if (keyboardAxesProperty.isExpanded) {
				EditorGUI.indentLevel += 1;
				
				for (int i = 0; i < keyboardAxesProperty.arraySize; i++) {
					currentKeyboardAxisProperty = keyboardAxesProperty.GetArrayElementAtIndex(i);
				
					EditorGUILayout.BeginHorizontal();
				
					SerializedProperty axisProperty = currentKeyboardAxisProperty.FindPropertyRelative("axis");
					int index = System.Array.IndexOf(keyboardAxes, axisProperty.GetValue<string>());
					
					index = EditorGUILayout.Popup(index, keyboardAxes);
					
					axisProperty.SetValue(keyboardAxes[Mathf.Clamp(index, 0, Mathf.Max(keyboardAxes.Length - 1, 0))]);
				
					int indent = EditorGUI.indentLevel;
					EditorGUI.indentLevel = 0;
						
					EditorGUILayout.PropertyField(currentKeyboardAxisProperty.FindPropertyRelative("name"), GUIContent.none);
						
					EditorGUI.indentLevel = indent;
					
					if (DeleteButton(keyboardAxesProperty, i, ButtonAlign.Right)) {
						break;
					}
					
					EditorGUILayout.EndHorizontal();
					
					Reorderable(keyboardAxesProperty, i, true);
				}
				
				Separator();
				EditorGUI.indentLevel -= 1;
			}
			
			EndBox();
		}
		
		void ShowKeyboardListeners() {
			keyboardListenersProperty = currentKeyboardInfoProperty.FindPropertyRelative("listenerReferences");
			
			BeginBox();
			
			AddFoldOut(keyboardListenersProperty, "Listeners".ToGUIContent());

			if (keyboardListenersProperty.isExpanded) {
				EditorGUI.indentLevel += 1;
				
				
				for (int i = 0; i < keyboardListenersProperty.arraySize; i++) {
					currentKeyboardListenerProperty = keyboardListenersProperty.GetArrayElementAtIndex(i);
					
					EditorGUI.BeginDisabledGroup(Application.isPlaying);
					EditorGUILayout.BeginHorizontal();
					
					ShowAddKeyboardListenerPopup();
					
					if (DeleteButton(keyboardListenersProperty, i)) {
						break;
					}
					
					EditorGUILayout.EndHorizontal();
					EditorGUI.EndDisabledGroup();
					
					Reorderable(keyboardListenersProperty, i, true);
				}
					
				Separator();
				EditorGUI.indentLevel -= 1;
			}
			
			EndBox();
		}
		
		void ShowAddKeyboardListenerPopup() {
			List<MonoBehaviour> listeners = new List<MonoBehaviour>();
			List<string> options = new List<string>{ " " };
			MonoBehaviour currentListener = currentKeyboardListenerProperty.GetValue<MonoBehaviour>();
			
			foreach (MonoBehaviour listener in inputSystem.GetComponents<MonoBehaviour>()) {
				if (listener is IInputListener && (currentListener == listener || !keyboardListenersProperty.Contains(listener))) {
					listeners.Add(listener);
					options.Add(InputSystemUtility.FormatListener(listener));
				}
			}
			
			foreach (MonoBehaviour listener in FindObjectsOfType<MonoBehaviour>()) {
				if (listener is IInputListener && (currentListener == listener || !keyboardListenersProperty.Contains(listener))) {
					listeners.Add(listener);
					options.Add(InputSystemUtility.FormatListener(listener));
				}
			}
			
			int index = listeners.IndexOf(currentKeyboardListenerProperty.GetValue<MonoBehaviour>()) + 1;
			
			index = EditorGUILayout.Popup(index, options.ToArray()) - 1;
			
			currentKeyboardListenerProperty.SetValue(index == -1 ? null : listeners[index]);
		}
		
		void ShowJoystickInfos() {
			joystickInfosProperty = serializedObject.FindProperty("joystickInfos");
			
			if (AddFoldOut(joystickInfosProperty, "Joysticks".ToGUIContent())) {
				JoystickInfo[] joystickInfos = inputSystem.GetJoystickInfos();
				JoystickInfo joystickInfo = joystickInfos.Last();
				
				joystickInfo.SetUniqueName("default", "", joystickInfos);
				
				serializedObject.Update();
			}
			
			if (joystickInfosProperty.isExpanded) {
				EditorGUI.indentLevel += 1;
				
				for (int i = 0; i < joystickInfosProperty.arraySize; i++) {
					currentJoystickInfo = inputSystem.GetJoystickInfos()[i];
					currentJoystickInfoProperty = joystickInfosProperty.GetArrayElementAtIndex(i);
				
					BeginBox();
					
					string joystickName = currentJoystickInfoProperty.FindPropertyRelative("joystick").GetValue<Joysticks>().ToString();
					
					if (DeleteFoldOut(joystickInfosProperty, i, string.Format("{0} ({1})", currentJoystickInfo.Name, joystickName).ToGUIContent(), CustomEditorStyles.BoldFoldout)) {
						break;
					}
				
					ShowJoystickInfo();
					
					EndBox();
				}
				
				Separator();
				EditorGUI.indentLevel -= 1;
			}
		}
		
		void ShowJoystickInfo() {
			joystickButtonsProperty = currentJoystickInfoProperty.FindPropertyRelative("buttons");
			joystickAxesProperty = currentJoystickInfoProperty.FindPropertyRelative("axes");
			
			if (currentJoystickInfoProperty.isExpanded) {
				EditorGUI.indentLevel += 1;
				
				UniqueNameField(currentJoystickInfo, inputSystem.GetJoystickInfos());
				EditorGUILayout.PropertyField(currentJoystickInfoProperty.FindPropertyRelative("joystick"), "Input".ToGUIContent());
				
				Separator();
			
				ShowJoystickButtons();
				ShowJoystickAxes();
				
				Separator();
				
				ShowJoystickListeners();
				
				Separator();
				
				EditorGUI.indentLevel -= 1;
			}
		}

		void ShowJoystickButtons() {
			BeginBox();
			
			if (AddFoldOut(joystickButtonsProperty)) {
				joystickButtonsProperty.Last().FindPropertyRelative("name").SetValue("default");
				
				JoystickButton button = currentJoystickInfo.GetButtons().Last();
				
				button.Joystick = currentJoystickInfo.Joystick;
				button.Button = JoystickButtons.Cross_A;
				
				serializedObject.Update();
			}
				
			if (joystickButtonsProperty.isExpanded) {
				EditorGUI.indentLevel += 1;

				for (int i = 0; i < joystickButtonsProperty.arraySize; i++) {
					currentJoystickButtonProperty = joystickButtonsProperty.GetArrayElementAtIndex(i);
						
					EditorGUILayout.BeginHorizontal();
					
					EditorGUILayout.PropertyField(currentJoystickButtonProperty.FindPropertyRelative("button"), GUIContent.none);
						
					int indent = EditorGUI.indentLevel;
					EditorGUI.indentLevel = 0;
						
					EditorGUILayout.PropertyField(currentJoystickButtonProperty.FindPropertyRelative("name"), GUIContent.none);
						
					EditorGUI.indentLevel = indent;
						
					if (DeleteButton(joystickButtonsProperty, i, ButtonAlign.Right)) {
						break;
					}
						
					EditorGUILayout.EndHorizontal();
					
					Reorderable(joystickButtonsProperty, i, true);
				}
					
				Separator();
				EditorGUI.indentLevel -= 1;
				
			}
			
			EndBox();
		}

		void ShowJoystickAxes() {
			BeginBox();
				
			if (AddFoldOut(joystickAxesProperty)) {
				joystickAxesProperty.Last().FindPropertyRelative("name").SetValue("default");
				
				JoystickAxis axis = currentJoystickInfo.GetAxes().Last();
				
				axis.Joystick = currentJoystickInfo.Joystick;
				axis.Axis = JoystickAxes.LeftStickX;
				
				serializedObject.Update();
			}
			
			if (joystickAxesProperty.isExpanded) {
				EditorGUI.indentLevel += 1;
				
				for (int i = 0; i < joystickAxesProperty.arraySize; i++) {
					currentJoystickAxisProperty = joystickAxesProperty.GetArrayElementAtIndex(i);
				
					EditorGUILayout.BeginHorizontal();
				
					EditorGUILayout.PropertyField(currentJoystickAxisProperty.FindPropertyRelative("axis"), GUIContent.none);
				
					int indent = EditorGUI.indentLevel;
					EditorGUI.indentLevel = 0;
						
					EditorGUILayout.PropertyField(currentJoystickAxisProperty.FindPropertyRelative("name"), GUIContent.none);
						
					EditorGUI.indentLevel = indent;
					
					if (DeleteButton(joystickAxesProperty, i, ButtonAlign.Right)) {
						break;
					}
					
					EditorGUILayout.EndHorizontal();
					
					Reorderable(joystickAxesProperty, i, true);
				}
				
				Separator();
				EditorGUI.indentLevel -= 1;
			}
			
			EndBox();
		}
		
		void ShowJoystickListeners() {
			joystickListenersProperty = currentJoystickInfoProperty.FindPropertyRelative("listenerReferences");
			
			BeginBox();
			
			AddFoldOut(joystickListenersProperty, "Listeners".ToGUIContent());

			if (joystickListenersProperty.isExpanded) {
				EditorGUI.indentLevel += 1;
				
				
				for (int i = 0; i < joystickListenersProperty.arraySize; i++) {
					currentJoystickListenerProperty = joystickListenersProperty.GetArrayElementAtIndex(i);
					
					EditorGUI.BeginDisabledGroup(Application.isPlaying);
					EditorGUILayout.BeginHorizontal();
					
					ShowAddJoystickListenerPopup();
					
					if (DeleteButton(joystickListenersProperty, i)) {
						break;
					}
					
					EditorGUILayout.EndHorizontal();
					EditorGUI.EndDisabledGroup();
					
					Reorderable(joystickListenersProperty, i, true);
				}
					
				Separator();
				EditorGUI.indentLevel -= 1;
			}
			
			EndBox();
		}
		
		void ShowAddJoystickListenerPopup() {
			List<MonoBehaviour> listeners = new List<MonoBehaviour>();
			List<string> options = new List<string>{ " " };
			MonoBehaviour currentListener = currentJoystickListenerProperty.GetValue<MonoBehaviour>();
			
			foreach (MonoBehaviour listener in inputSystem.GetComponents<MonoBehaviour>()) {
				if (listener is IInputListener && (currentListener == listener || !joystickListenersProperty.Contains(listener))) {
					listeners.Add(listener);
					options.Add(InputSystemUtility.FormatListener(listener));
				}
			}
			
			foreach (MonoBehaviour listener in FindObjectsOfType<MonoBehaviour>()) {
				if (listener is IInputListener && (currentListener == listener || !joystickListenersProperty.Contains(listener))) {
					listeners.Add(listener);
					options.Add(InputSystemUtility.FormatListener(listener));
				}
			}
			
			int index = listeners.IndexOf(currentJoystickListenerProperty.GetValue<MonoBehaviour>()) + 1;
			
			index = EditorGUILayout.Popup(index, options.ToArray()) - 1;
			
			currentJoystickListenerProperty.SetValue(index == -1 ? null : listeners[index]);
		}
	}
}
