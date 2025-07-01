using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace AbsoluteCommons.Editor {
	public abstract class InterfaceDrawer<T> : PropertyDrawer {
		private static Dictionary<string, Type> _typesByName;
		private static List<string> _fullNames;
		private static List<string> _names;

		private static void LoadDerivingTypes() {
			if (_typesByName is not null)
				return;

			_typesByName = AppDomain.CurrentDomain
				.GetAssemblies()
				.SelectMany(static assembly => assembly.GetTypes())
				.Where(static type => typeof(T).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
				.ToDictionary(static type => type.Name, static type => type);

			_fullNames = _typesByName.Keys.ToList();
			_names = _fullNames.Select(static name => name.Split('.').Last()).ToList();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => property.managedReferenceValue is null ? EditorGUIUtility.singleLineHeight : EditorGUI.GetPropertyHeight(property, true);

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			LoadDerivingTypes();

			EditorGUI.BeginProperty(position, label, property);

			Rect dropdown = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
			Rect content = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2, position.width, position.height - EditorGUIUtility.singleLineHeight);

			// If there is a paired Unity Object for the field, it will need to be updated
			var parent = property.serializedObject.targetObject;
			var pairedFieldInfo = parent.GetType().GetField(property.name + "Object", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			var pairedField = pairedFieldInfo?.GetValue(parent) as UnityEngine.Object;

			object obj = property.managedReferenceValue;
			string name = obj?.GetType().FullName ?? (pairedField ? pairedField.GetType().FullName : string.Empty);

			// Handle the dropdown
			int currentIndex = Mathf.Max(0, _fullNames.IndexOf(name));
			int newIndex = EditorGUI.Popup(dropdown, property.displayName, currentIndex, _names.ToArray());

			Type type = _typesByName[_fullNames[newIndex]];
			bool unityObject = typeof(UnityEngine.Object).IsAssignableFrom(type);

			if (unityObject) {
				// Clear the original field
				if (obj is not null) {
					property.managedReferenceValue = null;
					property.serializedObject.ApplyModifiedProperties();
				}

				// Show a Unity Object
				var newPairedField = EditorGUI.ObjectField(position, label.text, pairedField, type, true);
				if (newPairedField != pairedField) {
					pairedFieldInfo?.SetValue(parent, newPairedField);
					EditorUtility.SetDirty(parent);
				}
			} else {
				// Clear the paired field
				if (pairedField != null) {
					pairedFieldInfo?.SetValue(parent, null);
					EditorUtility.SetDirty(parent);
				}

				// Show a general object
				if (obj is null || obj.GetType() != type) {
					property.managedReferenceValue = Activator.CreateInstance(type);
					property.serializedObject.ApplyModifiedProperties();
				}

				EditorGUI.indentLevel++;
				EditorGUI.PropertyField(new Rect(position.x, content.y + EditorGUIUtility.singleLineHeight + 2, position.width, EditorGUI.GetPropertyHeight(property, true)), property, true);
				EditorGUI.indentLevel--;
			}

			EditorGUI.EndProperty();
		}
	}
}
