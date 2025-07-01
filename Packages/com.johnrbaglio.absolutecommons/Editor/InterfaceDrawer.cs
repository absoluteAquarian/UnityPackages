using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AbsoluteCommons.Editor {
	public abstract class InterfaceDrawer<T> : PropertyDrawer {
		private static Dictionary<string, Type> _typesByName;

		private static void LoadDerivingTypes() {
			if (_typesByName is not null)
				return;

			_typesByName = AppDomain.CurrentDomain
				.GetAssemblies()
				.SelectMany(static assembly => assembly.GetTypes())
				.Where(static type => typeof(T).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
				.ToDictionary(static type => type.Name, static type => type);
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			return EditorGUI.GetPropertyHeight(property, label, true);
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			LoadDerivingTypes();

			EditorGUI.BeginProperty(position, label, property);

			Rect dropdown = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
			Rect content = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2, position.width, position.height - EditorGUIUtility.singleLineHeight - 2);

			object obj = property.managedReferenceValue;
			string name = obj?.GetType().FullName ?? string.Empty;

			// Handle the dropdown
			var names = _typesByName.Keys.ToList();
			int currentIndex = Mathf.Max(0, names.IndexOf(name));
			int newIndex = EditorGUI.Popup(dropdown, "Type", currentIndex, names.Select(static n => n.Split('.').Last()).ToArray());

			if (newIndex != currentIndex || obj is null) {
				Type newType = _typesByName[names[newIndex]];
				property.managedReferenceValue = Activator.CreateInstance(newType);
				property.serializedObject.ApplyModifiedProperties();
			}

			if (property.managedReferenceValue is not null) {
				EditorGUI.indentLevel++;
				EditorGUI.PropertyField(content, property, true);
				EditorGUI.indentLevel--;
			}

			EditorGUI.EndProperty();
		}
	}
}
