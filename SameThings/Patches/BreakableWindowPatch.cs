using System;

using HarmonyLib;

namespace SameThings.Patches
{
	// Token: 0x02000006 RID: 6
	[HarmonyPatch(typeof(BreakableWindow), "ServerDamageWindow")]
	public class BreakableWindowPatch
	{
		// Token: 0x06000022 RID: 34 RVA: 0x00002C98 File Offset: 0x00000E98
		public static void Prefix(BreakableWindow __instance)
		{
			if (SameThings.WindowHealth < 1f || State.BreakableWindows.Contains(__instance))
			{
				return;
			}
			__instance.health = ((SameThings.WindowHealth == 0f) ? 9999999f : SameThings.WindowHealth);
			State.BreakableWindows.Add(__instance);
		}
	}
}
