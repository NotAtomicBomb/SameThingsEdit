using System;
using System.Collections.Generic;
using MEC;
using UnityEngine;

namespace SameThings
{
	// Token: 0x02000005 RID: 5
	internal static class State
	{
		// Token: 0x0600001F RID: 31 RVA: 0x00002C14 File Offset: 0x00000E14
		internal static void Refresh()
		{
			if (State._coroutines != null)
			{
				Timing.KillCoroutines(State._coroutines);
			}
			State._coroutines = new List<CoroutineHandle>();
			State.LuresCount = 0;
			State.CleanupTime = 0;
			State.Pickups = new Dictionary<Pickup, int>();
			State.Ragdolls = new Dictionary<Ragdoll, int>();
			State.AfkTime = new Dictionary<ReferenceHub, int>();
			State.PrevPos = new Dictionary<ReferenceHub, Vector3>();
			State.BreakableWindows = new List<BreakableWindow>();
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002C7B File Offset: 0x00000E7B
		internal static void RunCoroutine(IEnumerator<float> coroutine)
		{
			State.AddCoroutine(Timing.RunCoroutine(coroutine));
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002C88 File Offset: 0x00000E88
		internal static void AddCoroutine(CoroutineHandle handle)
		{
			State._coroutines.Add(handle);
		}

		// Token: 0x0400001F RID: 31
		internal static int LuresCount;

		// Token: 0x04000020 RID: 32
		internal static int CleanupTime;

		// Token: 0x04000021 RID: 33
		internal static List<BreakableWindow> BreakableWindows;

		// Token: 0x04000022 RID: 34
		internal static Dictionary<ReferenceHub, int> AfkTime;

		// Token: 0x04000023 RID: 35
		internal static Dictionary<ReferenceHub, Vector3> PrevPos;

		// Token: 0x04000024 RID: 36
		internal static Dictionary<Pickup, int> Pickups;

		// Token: 0x04000025 RID: 37
		internal static Dictionary<Ragdoll, int> Ragdolls;

		// Token: 0x04000026 RID: 38
		private static List<CoroutineHandle> _coroutines;
	}
}
