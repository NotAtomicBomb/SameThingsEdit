using System;

using HarmonyLib;
namespace SameThings.Patches
{
	// Token: 0x02000008 RID: 8
	[HarmonyPatch(typeof(Radio), "UseBattery")]
	public class RadioPatch
	{
		// Token: 0x06000026 RID: 38 RVA: 0x00002D0A File Offset: 0x00000F0A
		public static bool Prefix()
		{
			return !SameThings.UnlimitedRadio;
		}
	}
}
