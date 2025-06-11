using AbsoluteCommons.Runtime.Animation;
using UnityEngine;

namespace AbsoluteCommons.Runtime.Animation {
	[AddComponentMenu("Absolute Commons/Animation/Animator Data Caching")]
	public class TrackedAnimator : MonoBehaviour {
		[SerializeField] private Animator _animator;

		private void OnEnable() {
			AnimatorTracker.Track(_animator);
		}

		private void OnDisable() {
			AnimatorTracker.Untrack(_animator);
		}
	}
}
