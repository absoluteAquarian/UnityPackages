﻿using Unity.Netcode.Components;
using UnityEngine;

namespace AbsoluteCommons.Networking {
	[AddComponentMenu("Absolute Commons/Networking/Client Authoritative Transform")]
	public class ClientAuthoritativeTransform : NetworkTransform {
		protected override bool OnIsServerAuthoritative() => false;
	}
}
