using AbsoluteCommons.Runtime.Utility;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using AbsoluteCommons.Runtime.Attributes;

namespace AbsoluteCommons.Runtime.Objects {
	[AddComponentMenu("Absolute Commons/Objects/Dynamic Object Pool")]
	public class DynamicObjectPool : MonoBehaviour {
		[SerializeField, ReadOnly] private GameObject _prefab;
		[SerializeField] private int _initialCapacity = 10;
		[SerializeField] private bool _showPoolInHierarchy = false;

		[SerializeField, ReadOnly] private List<GameObject> _pool;
		private BitArray _dirty;
		[SerializeField, ReadOnly] private GameObject _container;
		private int _index;

		private static GameObject _visibleObjectContainer;

		public int Count => _pool.Count;

		private void Awake() {
			_pool = new List<GameObject>(_initialCapacity);
			_dirty = new BitArray(_initialCapacity, true);

			_container = new GameObject("Dynamic Pool");
			_container.hideFlags = _showPoolInHierarchy ? HideFlags.None : HideFlags.HideInHierarchy;
			_container.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
			_container.transform.SetParent(transform, false);

			Debug.Log($"[DynamicObjectPool] [Awake] Initialized _container for \"{gameObject.GetHierarchyPath()}\"");

			// Make a global pool for objects in the world
			// This is just so that they don't clutter the scene list
			if (!_visibleObjectContainer) {
				_visibleObjectContainer = new GameObject("Visible Dynamic Pool Objects");
				_visibleObjectContainer.hideFlags = _showPoolInHierarchy ? HideFlags.None : HideFlags.HideInHierarchy;
				_visibleObjectContainer.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

				Debug.Log($"[DynamicObjectPool] [Awake] Initialized _visibleObjectContainer from \"{gameObject.GetHierarchyPath()}\"");
			}
		}

		private void OnDestroy() {
			// Force all objects to despawn
			for (int i = 0; i < _pool.Count; i++) {
				var obj = _pool[i];

				if (obj) {
					obj.SetActive(false);
					Object.Destroy(obj);
					_pool[i] = null;
				}
			}
		}

		public void SetPrefab(GameObject prefab) {
			// Dirty flags do not need to be set if the prefab is the same
			if (_prefab == prefab)
				return;

			_prefab = prefab;
			// Keep any objects that were already present, they'll get replaced when they are "deactivated" anyway
		//	_dirty.SetAll(true);
		}

		public void SetPrefab<T>(T prefab) where T : Component => SetPrefab(prefab.gameObject);

		public GameObject Get() {
			if (_prefab == null) {
				Debug.LogError("[DynamicObjectPool] No prefab set");
				return null;
			}

			if (!_container)
				_container = gameObject.FindChildRecursively("Dynamic Pool");

			if (!_visibleObjectContainer)
				Debug.LogError("[DynamicObjectPool] [Get] _visibleObjectContainer is null");

			for (int i = 0; i < _pool.Count; i++) {
				_index = (_index + 1) % _pool.Count;

				GameObject obj = _pool[_index];
				if (!obj || !obj.activeInHierarchy)
					return PrepareObject(obj);
			}

			// Reached capacity, create a new object
			_index = _pool.Count;
			GameObject spawned = PrepareObject(AddNewObject());

			Debug.Log($"[DynamicObjectPool] [Get] Initialized object \"{spawned.GetHierarchyPath()}\" for requesting pool \"{gameObject.GetHierarchyPath()}\"");

			return spawned;
		}

		private GameObject PrepareObject(GameObject obj) {
			bool hasNewObject = false;

			if (!obj || _dirty[_index]) {
				// A new object is needed since the prefab has changed or the object was destroyed
				if (obj)
					TypeExtensions.DestroyAndSetNull(ref obj);

				obj = Create();

				_dirty[_index] = false;

				hasNewObject = true;
			}

			obj.SetActive(true);
			obj.transform.SetParent(Application.isEditor ? _visibleObjectContainer?.transform : null, true);

			if (hasNewObject) {
				int index = _index;

				_pool[index] = obj;

				if (_dirty.Length <= index)
					_dirty.Length = index + 1;

				_dirty[index] = false;
			}

			PooledObject.EnsureConnection(obj, this, _index);

			return obj;
		}

		private GameObject AddNewObject() {
			GameObject newObj = Create();
			_pool.Add(newObj);
			_dirty.Length++;
			_dirty[_index] = false;
			return newObj;
		}

		private GameObject Create() {
			GameObject obj = Instantiate(_prefab, _container.transform, false);

			Debug.Log($"[DynamicObjectPool] [Create] Created new object \"{obj.GetHierarchyPath()}\" for pool \"{gameObject.GetHierarchyPath()}\"");

			return obj;
		}

		private void ResetObjectState(GameObject obj) {
			if (!obj)
				return;

			obj.SetActive(false);
			obj.transform.SetParent(_container.transform, true);
		}

		public T Get<T>() where T : Component {
			GameObject obj = Get();
			return obj ? obj.GetComponent<T>() : null;
		}

		public void Return(GameObject obj) {
			ResetObjectState(obj);
		}

		public void Return<T>(T component) where T : Component => Return(component.gameObject);

		public Transform[] ExtractObjectTransforms() {
			Transform[] transforms = new Transform[_pool.Count];

			for (int i = 0; i < _pool.Count; i++) {
				GameObject obj = _pool[i];
				if (obj)
					transforms[i] = obj.transform;
			}

			return transforms;
		}

		public IEnumerable<Transform> ExtractObjectTransforms(int start) {
			for (int i = start; i < _pool.Count; i++) {
				GameObject obj = _pool[i];
				yield return obj ? obj.transform : null;
			}
		}
	}
}
