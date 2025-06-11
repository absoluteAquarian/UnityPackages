using UnityEngine;

namespace AbsoluteCommons.Runtime.AltPhysics {
	[AddComponentMenu("Absolute Commons/Physics/Gravity")]
	[RequireComponent(typeof(PhysicsMetricsFinalizer), typeof(PhysicsMetricsFinalizer))]
	public class PhysicsMetrics : MonoBehaviour {
		public bool useGravityOverride = true;
		public Vector3 gravityOverride = Vector3.down * 9.81f;
		public float gravityScale = 1f;
	}
}
