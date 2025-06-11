using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AbsoluteCommons.Mutual.Utility {
	partial class TypeExtensions {
		public static GameObject FindChildRecursively(this GameObject parent, string name) {
			Queue<GameObject> scanQueue = new();
			scanQueue.Enqueue(parent);

			// Traverse the hierarchy
			while (scanQueue.TryDequeue(out GameObject current)) {
				if (current.name == name)
					return current;

				foreach (Transform child in current.transform)
					scanQueue.Enqueue(child.gameObject);
			}

			return null;
		}

		public static GameObject GetChild(this GameObject parent, string path) {
			string[] pathParts = path.Split('/');
			Transform current = parent.transform;

			// Traverse the path
			foreach (string part in pathParts) {
				current = current.Find(part);
				if (current == null)
					return null;
			}

			return current.gameObject;
		}

		public static T GetChildComponent<T>(this GameObject parent, string path) where T : Component {
			GameObject child = parent.GetChild(path);
			return child == null ? null : child.GetComponent<T>();
		}

		public static bool IsObjectOrParentOfObject(this GameObject searcher, GameObject target) {
			if (!searcher)
				return false;

			Transform check = target.transform;
			
			while (check != null) {
				if (check.gameObject == searcher)
					return true;

				check = check.parent;
			}

			return false;
		}

		public static bool TryGetComponentInParent<T>(this GameObject obj, out T component) where T : Component {
			Transform current = obj.transform;

			while (current) {
				if (current.TryGetComponent(out component))
					return true;

				current = current.parent;
			}

			component = null;
			return false;
		}

		public static string GetHierarchyPath(this GameObject obj) {
			StringBuilder sb = new StringBuilder(obj.name);
			Transform current = obj.transform;

			while (current.parent) {
				current = current.parent;
				sb.Insert(0, '/').Insert(0, current.name);
			}

			return sb.ToString();
		}
	}
}
