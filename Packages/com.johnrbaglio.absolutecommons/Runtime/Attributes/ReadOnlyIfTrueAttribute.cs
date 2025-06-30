using UnityEngine;

namespace AbsoluteCommons.Runtime.Attributes {
	/// <summary>
	/// A variant of <see cref="ReadOnlyAttribute"/> that allows the field to be conditionally read-only based on the state of another field in the same class.
	/// </summary>
	public class ReadOnlyIfTrueAttribute : PropertyAttribute {
		/// <summary>
		/// The <see langword="bool"/> property that determines if the field given this attribute is read-only.
		/// </summary>
		public string ConditionProperty { get; }

		public ReadOnlyIfTrueAttribute(string conditionProperty) {
			ConditionProperty = conditionProperty;
		}
	}
}
