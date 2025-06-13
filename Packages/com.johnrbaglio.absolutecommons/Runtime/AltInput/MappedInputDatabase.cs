using System.Collections.Generic;
using UnityEngine;

namespace AbsoluteCommons.Runtime.AltInput {
	[CreateAssetMenu(fileName = nameof(MappedInputDatabase), menuName = "Absolute Commons/Mapped Input Database", order = 1)]
	public class MappedInputDatabase : ScriptableObject {
		[SerializeField] private List<KeyCode> _keys = new();
		[SerializeField] private List<string> _axes = new();

		public IEnumerable<IInputControl> GetControls() {
			foreach (var key in _keys)
				yield return new InputKey(key);

			foreach (var axis in _axes)
				yield return new InputAxis(axis);
		}

		private void Awake() {
			InputMapper.AddControls(this);
			Destroy(this);
		}
	}
}
