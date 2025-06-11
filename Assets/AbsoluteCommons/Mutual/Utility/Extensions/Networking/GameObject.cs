using AbsoluteCommons.Objects;
using Unity.Netcode;
using UnityEngine;

namespace AbsoluteCommons.Mutual.Utility {
	partial class TypeExtensions {
		public static void DestroyOrDespawnAndSetNull(ref GameObject obj) {
			if (obj) {
				if (obj.TryGetComponent(out NetworkObject netObj))
					netObj.SmartDespawn(true);
				else
					Object.Destroy(obj);

				obj = null;
			}
		}

		public static void DestroyDespawnOrReturnToPoolAndSetNull(ref GameObject obj) {
			if (obj) {
				if (obj.TryGetComponent(out PooledObject pooled))
					pooled.ReturnToPool();
				else if (obj.TryGetComponent(out NetworkObject netObj))
					netObj.SmartDespawn(true);
				else
					Object.Destroy(obj);

				obj = null;
			}
		}
	}
}
