using AbsoluteCommons.Runtime.Attributes;
using UnityEngine;

namespace AbsoluteCommons.Runtime.Data {
	[System.Serializable]
	public class Optional<T> {
		[SerializeField] private bool hasValue;
		[SerializeField, ReadOnlyIfTrue(nameof(Disabled))] private T value;

		public bool Disabled => !hasValue;

		public T Value => hasValue ? value : default;

		public Optional() {
			hasValue = false;
			value = default;
		}

		public Optional(T value) {
			hasValue = true;
			this.value = value;
		}
	}
}
