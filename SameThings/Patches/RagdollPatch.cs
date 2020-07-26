using System;

using HarmonyLib;
namespace SameThings.Patches
{
	// Token: 0x02000009 RID: 9
	[HarmonyPatch(typeof(Ragdoll), "Start")]
	public class RagdollPatch
	{
		// Token: 0x06000028 RID: 40 RVA: 0x00002D14 File Offset: 0x00000F14
		public static void Prefix(Ragdoll __instance)
		{
			if (SameThings.RagdollAutoCleanup > 0)
			{
				State.Ragdolls.Add(__instance, State.CleanupTime + SameThings.RagdollAutoCleanup);
			}
		}
	}
}
