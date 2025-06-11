﻿using AbsoluteCommons.Runtime.Attributes;
using UnityEngine;

namespace AbsoluteCommons.Runtime.Objects {
	public class PooledObject : MonoBehaviour {
		public DynamicObjectPool Pool {
			get => _pool;
			private set => _pool = value;
		}

		public int Index {
			get => _index;
			private set => _index = value;
		}

		[SerializeField, ReadOnly] private DynamicObjectPool _pool;
		[SerializeField, ReadOnly] private int _index = -1;

		public static void EnsureConnection(GameObject obj, DynamicObjectPool pool, int index) {
			if (!obj.TryGetComponent(out PooledObject component))
				component = obj.AddComponent<PooledObject>();

			component._pool = pool;
			component._index = index;
		}

		public void ReturnToPool() {
			Pool.Return(gameObject);
			_index = -1;
		}

		private void OnDestroy() {
			_pool = null;
			_index = -1;
		}
	}
}
