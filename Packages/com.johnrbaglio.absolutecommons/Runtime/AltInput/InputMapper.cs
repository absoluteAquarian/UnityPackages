using System;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace AbsoluteCommons.Runtime.AltInput {
	public static class InputMapper {
		private static InputMap _map;
		private static bool _pendingUpdate;

		public static void AddControls(MappedInputDatabase database) {
			_map ??= new InputMap();

			if (!database)
				return;

			foreach (var control in database.GetControls())
				control.Register(_map);

			_pendingUpdate = true;
		}

		internal static void RequestUpdate() => _pendingUpdate = true;

		private static bool ValidateInput() {
			if (_map is null) {
				Debug.LogError($"[InputMapper] {nameof(MappedInputDatabase)} resource was not found. Please add it via \"Create/Absolute Commons/Mapped Input Database\" either in the scene or in a Resources folder.");
				return false;
			}

			if (_pendingUpdate) {
				_map.Update();
				_pendingUpdate = false;
			}

			return true;
		}

		public static float GetRaw(string name) => ValidateInput() ? _map.GetRaw(name) : 0;

		public static float GetRaw(KeyCode key) => ValidateInput() ? _map.GetRaw(key) : 0;

		public static bool IsInactive(string name) => ValidateInput() && _map.IsInactive(name);

		public static bool IsInactive(KeyCode key) => ValidateInput() && _map.IsInactive(key);

		public static bool IsTriggered(string name) => ValidateInput() && _map.IsTriggered(name);

		public static bool IsTriggered(KeyCode key) => ValidateInput() && _map.IsTriggered(key);

		public static bool IsPressed(string name) => ValidateInput() && _map.IsPressed(name);

		public static bool IsPressed(KeyCode key) => ValidateInput() && _map.IsPressed(key);

		public static bool IsReleased(string name) => ValidateInput() && _map.IsReleased(name);

		public static bool IsReleased(KeyCode key) => ValidateInput() && _map.IsReleased(key);

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void FindDatabaseResources() {
			var resources = Resources.FindObjectsOfTypeAll<MappedInputDatabase>();

			foreach (var resource in resources)
				AddControls(resource);

			LateUpdateHook.InjectIntoUpdateLoop();
		}
	}

	internal static class LateUpdateHook {
		internal static void InjectIntoUpdateLoop() {
			var loop = PlayerLoop.GetCurrentPlayerLoop();

			// LateUpdate is part of the PreLateUpdate loop's system list
			InjectAfter<PreLateUpdate>(ref loop, typeof(InputMapper), InputMapper.RequestUpdate);
		}

		private static void InjectAfter<T>(ref PlayerLoopSystem rootLoop, Type hookType, PlayerLoopSystem.UpdateFunction callback) where T : struct {
			ref var systems = ref rootLoop.subSystemList;

			for (int i = 0; i < systems.Length; i++) {
				ref var system = ref systems[i];

				if (system.type == typeof(T)) {
					var newSystem = new PlayerLoopSystem() {
						type = hookType,
						updateDelegate = callback
					};

					ref var systemList = ref system.subSystemList;

					var extendedList = new PlayerLoopSystem[systemList.Length + 1];
					Array.Copy(systemList, extendedList, systemList.Length);
					extendedList[systemList.Length] = newSystem;

					systemList = extendedList;
				}
			}
		}
	}
}
