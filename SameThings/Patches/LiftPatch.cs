using System;

using HarmonyLib;

namespace SameThings.Patches
{
	// Token: 0x02000007 RID: 7
	[HarmonyPatch(typeof(Lift), "UseLift")]
	public class LiftPatch
	{
		// Token: 0x06000024 RID: 36 RVA: 0x00002CF0 File Offset: 0x00000EF0
		public static void Prefix(Lift __instance)
		{
			if (SameThings.LiftMoveDuration == -1f)
			{
				return;
			}
			__instance.movingSpeed = SameThings.LiftMoveDuration;
			
		}
	}
}
