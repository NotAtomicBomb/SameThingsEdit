using System;
using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.Events;
using HarmonyLib;

namespace SameThings
{
	// Token: 0x02000003 RID: 3
	public class SameThings : Plugin<Config>
	{
		internal static bool PluginOn;

		internal static float WindowHealth;
		internal static int ForceRestart;

		internal static bool WarheadCleanup;
		internal static int ItemAutoCleanup;
		internal static List<int> ItemCleanupIgnore;
		internal static int RagdollAutoCleanup;
		internal static List<string> NicknameFilter;
		internal static string NicknameFilterReason;
		internal static List<int> TeslaTriggerableTeam;
		internal static bool ReduceAmmo;
		internal static bool UnlimitedRadio;
		internal static float LiftMoveDuration;

		internal static int AutoWarheadStart;
		internal static bool AutoWarheadLock;
		internal static string AutoWarheadStartText;

		internal static float DecontaminationTime;

		internal static int GeneratorDuration;
		internal static List<string> GeneratorKeycardPerm;
		internal static List<int> GeneratorInsertTeams;
		internal static List<int> GeneratorEjectTeams;
		internal static List<int> GeneratorUnlockTeams;

		internal static Dictionary<int, int> MaxHealth;
		internal static Dictionary<int, int> SelfHealingDuration;
		internal static Dictionary<int, int> SelfHealingAmount;

		internal static int Scp106LureAmount;
		internal static float Scp106LureReload;
		internal static List<int> Scp106LureTeam;
		internal static List<int> NoInfAmmoTeam;
		public override string Name
		{
			get
			{
				return "SameThings";
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x0000286C File Offset: 0x00000A6C
		public override void OnEnabled()
		{
			OnReloaded();
			if (!PluginOn)
			{
				Log.Info("Plugin v2020.0406.0 disabled. Bye;");
				return;
			}
			if (this._instance == null)
			{
				this._instance = new Harmony("me.nekonyx.same-things");
			}
			this._instance.PatchAll();
			State.Refresh();
			EventHandlers.Subscribe();
			Log.Info("Plugin v2020.0406.0 initialied");
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000028F3 File Offset: 0x00000AF3
		public override void OnDisabled()
		{
			if (!PluginOn)
			{
				return;
			}
			this._instance.UnpatchAll(null);
			State.Refresh();
			EventHandlers.Unsubscribe();
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002913 File Offset: 0x00000B13
		public override void OnReloaded()
		{
			PluginOn = Config.IsEnabled;
			WindowHealth = Config.WindowHealth;
			ForceRestart = Config.ForceRestart;
			WarheadCleanup = Config.WarheadCleanup;
			ItemAutoCleanup = Config.ItemAutoCleanup;
			ItemCleanupIgnore = Config.ItemCleanupIgnore;
			RagdollAutoCleanup = Config.RagdollAutoCleanup;
			NicknameFilter = Config.NicknameFilter;
			NicknameFilterReason = Config.NicknameFilterReason;
			TeslaTriggerableTeam = Config.TeslaTriggerableTeam;
			ReduceAmmo = Config.ReduceAmmo;
			UnlimitedRadio = Config.UnlimitedRadio;
			LiftMoveDuration = Config.LiftMoveDuration;
			AutoWarheadStart = Config.AutoWarheadStart;
			AutoWarheadLock = Config.AutoWarheadLock;
			AutoWarheadStartText = Config.AutoWarheadStartText;
			DecontaminationTime = Config.DecontaminationTime;
			GeneratorDuration = Config.GeneratorDuration;
			GeneratorKeycardPerm = Config.GeneratorKeycardPerm;
			GeneratorEjectTeams = Config.GeneratorEjectTeams;
			GeneratorInsertTeams = Config.GeneratorInsertTeams;
			GeneratorUnlockTeams = Config.GeneratorUnlockTeams;
			MaxHealth = Config.MaxHealth;
			SelfHealingAmount = Config.SelfHealingAmount;
			SelfHealingDuration = Config.SelfHealingDuration;
			Scp106LureAmount = Config.Scp106LureAmount;
			Scp106LureReload = Config.Scp106LureReload;
			Scp106LureTeam = Config.Scp106LureTeam;
			NoInfAmmoTeam = Config.NoInfAmmoTeam;

		}

		// Token: 0x04000001 RID: 1
		

		// Token: 0x04000002 RID: 2
		private Harmony _instance;
	}
}
