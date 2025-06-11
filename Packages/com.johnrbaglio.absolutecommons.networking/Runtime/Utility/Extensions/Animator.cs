using AbsoluteCommons.Runtime.Utility;
using Unity.Netcode.Components;
using UnityEngine;

namespace AbsoluteCommons.Networking.Utility {
	partial class TypeExtensions {
		public static void NetForceTrigger(this Animator animator, string triggerName) {
			if (animator.gameObject.TryGetComponent(out NetworkAnimator networkSelf))
				networkSelf.ForceTrigger(triggerName);
			else
				animator.ForceTrigger(triggerName);
		}

		public static void NetForceTrigger(this Animator animator, int triggerHash) {
			if (animator.gameObject.TryGetComponent(out NetworkAnimator networkSelf))
				networkSelf.ForceTrigger(triggerHash);
			else
				animator.ForceTrigger(triggerHash);
		}

		public static void NetSetTriggerSafely(this Animator animator, string parameterName) {
			if (animator.gameObject.TryGetComponent(out NetworkAnimator networkSelf))
				networkSelf.SetTriggerSafely(parameterName);
			else
				animator.SetTriggerSafely(parameterName);
		}

		public static void NetSetTriggerSafely(this Animator animator, int parameterHash) {
			if (animator.gameObject.TryGetComponent(out NetworkAnimator networkSelf))
				networkSelf.SetTriggerSafely(parameterHash);
			else
				animator.SetTriggerSafely(parameterHash);
		}

		public static void NetResetTriggerSafely(this Animator animator, string parameterName) {
			if (animator.gameObject.TryGetComponent(out NetworkAnimator networkSelf))
				networkSelf.ResetTriggerSafely(parameterName);
			else
				animator.ResetTriggerSafely(parameterName);
		}

		public static void NetResetTriggerSafely(this Animator animator, int parameterHash) {
			if (animator.gameObject.TryGetComponent(out NetworkAnimator networkSelf))
				networkSelf.ResetTriggerSafely(parameterHash);
			else
				animator.ResetTriggerSafely(parameterHash);
		}
	}
}
