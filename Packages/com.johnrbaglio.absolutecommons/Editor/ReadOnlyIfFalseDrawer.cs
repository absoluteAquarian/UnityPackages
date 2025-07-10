using AbsoluteCommons.Runtime.Attributes;
using UnityEditor;
using UnityEngine;

namespace AbsoluteCommons.Editor {
	[CustomPropertyDrawer(typeof(ReadOnlyIfFalseAttribute))]
	public class ReadOnlyIfFalseDrawer : PropertyDrawer {
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			return EditorGUI.GetPropertyHeight(property, label, true);
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			string requiredProperty = ((ReadOnlyIfFalseAttribute)attribute).ConditionProperty;

			SerializedProperty conditionProperty = property.serializedObject.FindProperty(requiredProperty);

			if (conditionProperty == null) {
				EditorGUI.LabelField(position, label.text, "Condition property was missing: " + requiredProperty);
				return;
			}

			if (conditionProperty.propertyType != SerializedPropertyType.Boolean) {
				EditorGUI.LabelField(position, label.text, "Property must be a boolean: " + requiredProperty);
				return;
			}

			bool enabled = !conditionProperty.boolValue;

			using var scope = new EditorGUI.DisabledGroupScope(!enabled);
			EditorGUI.PropertyField(position, property, label, true);
		}
	}
}
