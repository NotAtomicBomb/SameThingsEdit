using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Exiled.API.Features;
using Exiled.Events.Commands.Reload;
using Exiled.API.Extensions;
using Exiled.Events.EventArgs;
using MEC;
using Mirror;
using UnityEngine;
using Object = UnityEngine.Object;
using Config = SameThings.Config;
using Exiled.API.Enums;

namespace SameThings
{
	// Token: 0x02000002 RID: 2
	internal static class EventHandlers
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002048 File Offset: 0x00000248
		internal static void Subscribe()
		{
			Exiled.Events.Handlers.Server.RoundStarted += EventHandlers.HandleRoundStart;
            Exiled.Events.Handlers.Server.RoundEnded += EventHandlers.HandleRoundEnd;
			Exiled.Events.Handlers.Player.Joined += EventHandlers.HandlePlayerJoin;
			Exiled.Events.Handlers.Player.TriggeringTesla += EventHandlers.HandleTeslaTrigger;
			Exiled.Events.Handlers.Player.ReloadingWeapon += EventHandlers.HandleWeaponReload;
			Exiled.Events.Handlers.Player.ChangingRole += EventHandlers.HandleSetClass;
			Exiled.Events.Handlers.Player.ItemDropped += EventHandlers.HandleDroppedItem;
			Exiled.Events.Handlers.Player.EjectingGeneratorTablet += EventHandlers.HandleGeneratorEject;
			Exiled.Events.Handlers.Player.InsertingGeneratorTablet += EventHandlers.HandleGeneratorInsert;
			Exiled.Events.Handlers.Player.UnlockingGenerator += EventHandlers.HandleGeneratorUnlock;
			Exiled.Events.Handlers.Player.EnteringFemurBreaker += EventHandlers.HandleFemurEnter;
			Exiled.Events.Handlers.Player.Left += EventHandlers.HandlePlayerLeave;
			Exiled.Events.Handlers.Warhead.Detonated += EventHandlers.HandleWarheadDetonation;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002134 File Offset: 0x00000334
		internal static void Unsubscribe()
		{
			Exiled.Events.Handlers.Server.RoundStarted -= EventHandlers.HandleRoundStart;
			Exiled.Events.Handlers.Server.RoundEnded -= EventHandlers.HandleRoundEnd;
			Exiled.Events.Handlers.Player.Joined -= EventHandlers.HandlePlayerJoin;
			Exiled.Events.Handlers.Player.TriggeringTesla -= EventHandlers.HandleTeslaTrigger;
			Exiled.Events.Handlers.Player.ReloadingWeapon -= EventHandlers.HandleWeaponReload;
			Exiled.Events.Handlers.Player.ChangingRole -= EventHandlers.HandleSetClass;
			Exiled.Events.Handlers.Player.ItemDropped -= EventHandlers.HandleDroppedItem;
			Exiled.Events.Handlers.Player.EjectingGeneratorTablet -= EventHandlers.HandleGeneratorEject;
			Exiled.Events.Handlers.Player.InsertingGeneratorTablet -= EventHandlers.HandleGeneratorInsert;
			Exiled.Events.Handlers.Player.UnlockingGenerator -= EventHandlers.HandleGeneratorUnlock;
			Exiled.Events.Handlers.Player.EnteringFemurBreaker -= EventHandlers.HandleFemurEnter;
			Exiled.Events.Handlers.Player.Left -= EventHandlers.HandlePlayerLeave;
			Exiled.Events.Handlers.Warhead.Detonated -= EventHandlers.HandleWarheadDetonation;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002220 File Offset: 0x00000420
		private static void HandleRoundStart()
		{
			if (SameThings.AutoWarheadLock)
			{
				Warhead.IsWarheadLocked = false;
			}
			if (SameThings.ForceRestart > 0)
			{
				State.RunCoroutine(EventHandlers.RunForceRestart());
			}
			if (SameThings.AutoWarheadStart > 0)
			{
				State.RunCoroutine(EventHandlers.RunAutoWarhead());
			}
			if (SameThings.RagdollAutoCleanup > 0 || SameThings.ItemAutoCleanup > 0)
			{
				State.RunCoroutine(EventHandlers.RunAutoCleanup());
			}
			if (SameThings.DecontaminationTime > 0f)
			{	
				//PlayerManager.localPlayer.GetComponent<DecontaminationLCZ>() = (float)((11.7399997711182 - (double)SameThings.DecontaminationTime) * 60.0);
				
			}
			if (SameThings.GeneratorDuration > 0)
			{
				foreach (Generator079 generator in Generator079.Generators)
				{
					generator.startDuration = (float)SameThings.GeneratorDuration;
					generator.SetTime((float)SameThings.GeneratorDuration);
					
				}
			}
			if (SameThings.SelfHealingDuration.Count > 0)
			{
				State.RunCoroutine(EventHandlers.RunSelfHealing());
			}
			if (SameThings.Scp106LureAmount == 0)
			{
				Object.FindObjectOfType<LureSubjectContainer>().SetState(true);
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002334 File Offset: 0x00000534
		private static void HandleRoundEnd(RoundEndedEventArgs ev)
		{
			State.Refresh();
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000233C File Offset: 0x0000053C
		private static void HandlePlayerJoin(JoinedEventArgs ev)
		{
			State.AfkTime[ev.Player.ReferenceHub] = 0;
			State.PrevPos[ev.Player.ReferenceHub] = Vector3.zero;
			
			if (!ev.Player.ReferenceHub.serverRoles.Staff && SameThings.NicknameFilter.Any((string s) => ev.Player.Nickname.Contains(s)))
			{
				ServerConsole.Disconnect(ev.Player.GameObject, SameThings.NicknameFilterReason);
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000023D0 File Offset: 0x000005D0
		private static void HandlePlayerLeave(LeftEventArgs ev)
		{
			if (State.AfkTime.ContainsKey(ev.Player.ReferenceHub))
			{
				State.AfkTime.Remove(ev.Player.ReferenceHub);
			}
			if (State.PrevPos.ContainsKey(ev.Player.ReferenceHub))
			{
				State.PrevPos.Remove(ev.Player.ReferenceHub);
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002424 File Offset: 0x00000624
		private static void HandleWarheadDetonation()
		{
			if (!SameThings.WarheadCleanup)
			{
				return;
			}
			foreach (Pickup pickup2 in from pickup in Object.FindObjectsOfType<Pickup>()
			where pickup.transform.position.y < 5f
			select pickup)
			{
				NetworkServer.Destroy(pickup2.gameObject);
			}
			foreach (Ragdoll ragdoll2 in from ragdoll in Object.FindObjectsOfType<Ragdoll>()
			where ragdoll.transform.position.y < 5f
			select ragdoll)
			{
				NetworkServer.Destroy(ragdoll2.gameObject);
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002500 File Offset: 0x00000700
		private static void HandleTeslaTrigger(TriggeringTeslaEventArgs ev)
		{
			if (SameThings.TeslaTriggerableTeam.Count == 0)
			{
				return;
			}
			if (!SameThings.TeslaTriggerableTeam.Contains((int)ev.Player.Team))
			{
				ev.IsTriggerable = false;
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x0000252F File Offset: 0x0000072F
		private static void HandleWeaponReload(ReloadingWeaponEventArgs ev)
		{	
			if (SameThings.ReduceAmmo)
			{
				return;
			}
			if (SameThings.NoInfAmmoTeam.Contains((int)ev.Player.Team))
			{
				return;
			}
			
			//ev.Player.ReferenceHub.ammoBox.Networkamount = "101:101:101";
			/*ev.Player.SetAmmo(AmmoType.Nato556, 101);
			ev.Player.SetAmmo(AmmoType.Nato762, 101);
			ev.Player.SetAmmo(AmmoType.Nato9, 101);*/
	
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002550 File Offset: 0x00000750
		private static void HandleSetClass(ChangingRoleEventArgs ev)
		{
			ReferenceHub player = ev.Player.ReferenceHub;
			State.PrevPos[player] = Vector3.zero;
			State.AfkTime[player] = 0;
			int maxHp;
			if (SameThings.MaxHealth.TryGetValue((int)ev.NewRole, out maxHp))
			{
				State.RunCoroutine(EventHandlers.RunRestoreMaxHp(player, maxHp));
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000025A0 File Offset: 0x000007A0
		private static void HandleDroppedItem(ItemDroppedEventArgs ev)
		{
			/*if (SameThings.ItemAutoCleanup <= 0 || SameThings.ItemCleanupIgnore.Contains((int)ev.Pickup.ItemId))
			{
				return;
			}
			State.Pickups.Add(ev.Pickup, State.CleanupTime + SameThings.ItemAutoCleanup);*/
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000025E0 File Offset: 0x000007E0
		private static void HandleGeneratorUnlock(UnlockingGeneratorEventArgs ev)
		{
			if (SameThings.GeneratorKeycardPerm.Count == 0)
			{
				return;
			}
			ev.IsAllowed = false;
			if (!SameThings.GeneratorUnlockTeams.Contains((int)ev.Player.Team))
			{
				return;
			}
			ItemType curr = ev.Player.Inventory.curItem;
			global::Item item = ev.Player.Inventory.availableItems.FirstOrDefault((global::Item i) => i.id == curr);
			if (item == null)
			{
				return;
			}
			foreach (string item2 in item.permissions)
			{
				if (SameThings.GeneratorKeycardPerm.Contains(item2))
				{
					ev.IsAllowed = true;
					return;
				}
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002690 File Offset: 0x00000890
		private static void HandleGeneratorInsert(InsertingGeneratorTabletEventArgs ev)
		{
			if (SameThings.GeneratorInsertTeams.Count == 0)
			{
				return;
			}
			ev.IsAllowed = SameThings.GeneratorInsertTeams.Contains((int)ev.Player.Team);
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000026BC File Offset: 0x000008BC
		private static void HandleGeneratorEject(EjectingGeneratorTabletEventArgs ev)
		{
			if (SameThings.GeneratorEjectTeams.Count == 0)
			{
				return;
			}
			ev.IsAllowed = SameThings.GeneratorEjectTeams.Contains((int)ev.Player.Team);
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000026E8 File Offset: 0x000008E8
		private static void HandleFemurEnter(EnteringFemurBreakerEventArgs ev)
		{
			if (SameThings.Scp106LureAmount == -1)
			{
				return;
			}
			if (!SameThings.Scp106LureTeam.Contains((int)ev.Player.Team))
			{
				ev.IsAllowed = false;
				return;
			}
			if (++State.LuresCount < SameThings.Scp106LureAmount)
			{
				State.RunCoroutine(EventHandlers.RunLureReload());
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x0000273B File Offset: 0x0000093B
		private static IEnumerator<float> RunLureReload()
		{
			yield return Timing.WaitForSeconds(SameThings.Scp106LureReload);
			Object.FindObjectOfType<LureSubjectContainer>().NetworkallowContain = false;
			yield break;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002743 File Offset: 0x00000943
		private static IEnumerator<float> RunForceRestart()
		{
			yield return Timing.WaitForSeconds((float)SameThings.ForceRestart);
			Log.Info("Restarting round");
			PlayerManager.localPlayer.GetComponent<PlayerStats>().Roundrestart();
			
			
			yield break;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0000274B File Offset: 0x0000094B
		private static IEnumerator<float> RunAutoWarhead()
		{
			yield return Timing.WaitForSeconds((float)SameThings.AutoWarheadStart);
			if (SameThings.AutoWarheadLock)
			{
				Warhead.IsWarheadLocked = true;
			}
			if (AlphaWarheadController.Host.detonated || AlphaWarheadController.Host.NetworkinProgress)
			{
				Log.Info("Alpha Warhead is detonated or detonation is in progress");
				yield break;
			}
			Log.Info("Activating Alpha Warhead");
			AlphaWarheadController.Host.StartDetonation();
			if (!string.IsNullOrEmpty(SameThings.AutoWarheadStartText))
			{
				
				Map.Broadcast(10,SameThings.AutoWarheadStartText);
			}
			yield break;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002753 File Offset: 0x00000953
		private static IEnumerator<float> RunAutoCleanup()
		{
			for (;;)
			{
				State.CleanupTime++;
				if (SameThings.ItemAutoCleanup > 0)
				{
					foreach (Pickup pickup in State.Pickups.Keys.ToArray<Pickup>())
					{
						if (pickup == null)
						{
							State.Pickups.Remove(pickup);
						}
						else if (State.Pickups[pickup] <= State.CleanupTime)
						{
							NetworkServer.Destroy(pickup.gameObject);
						}
					}
				}
				yield return float.NegativeInfinity;
				if (SameThings.RagdollAutoCleanup > 0)
				{
					foreach (Ragdoll ragdoll in State.Ragdolls.Keys.ToArray<Ragdoll>())
					{
						if (ragdoll == null)
						{
							State.Ragdolls.Remove(ragdoll);
						}
						else if (State.Ragdolls[ragdoll] <= State.CleanupTime)
						{
							NetworkServer.Destroy(ragdoll.gameObject);
						}
					}
				}
				yield return Timing.WaitForSeconds(1f);
			}
			yield break;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000275B File Offset: 0x0000095B
		private static IEnumerator<float> RunRestoreMaxHp(ReferenceHub player, int maxHp)
		{
			yield return Timing.WaitForSeconds(0.1f);
			player.playerStats.maxHP = maxHp;
			//player.SetHealth((float)maxHp);
			player.playerStats.SetHPAmount(maxHp);
			yield break;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002771 File Offset: 0x00000971
		private static IEnumerator<float> RunSelfHealing()
		{
			for (;;)
			{
				foreach (ReferenceHub hub in ReferenceHub.GetAllHubs().Values)
				{
					try
					{
						EventHandlers.DoSelfHealing(hub);
					}
					catch (Exception arg)
					{
						Log.Error(string.Format("Please report this error to Nekonyx#2752: {0}", arg));
					}
					yield return float.NegativeInfinity;
				}
				Dictionary<GameObject, ReferenceHub>.ValueCollection.Enumerator enumerator = default(Dictionary<GameObject, ReferenceHub>.ValueCollection.Enumerator);
				yield return Timing.WaitForSeconds(1f);
			}
			yield break;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0000277C File Offset: 0x0000097C
		private static void DoSelfHealing(ReferenceHub hub)
		{
			int role = (int)hub.characterClassManager.CurClass;
			int num;
			int num2;
			if (hub.characterClassManager.IsHost || !SameThings.SelfHealingAmount.TryGetValue(role, out num) || !SameThings.SelfHealingDuration.TryGetValue(role, out num2))
			{
				return;
			}
			Vector3 position = hub.PlayerCameraReference.position;
			State.AfkTime[hub] = ((State.PrevPos[hub] == position) ? (State.AfkTime[hub] + 1) : 0);
			State.PrevPos[hub] = position;
			if (State.AfkTime[hub] <= num2)
			{
				return;
			}
			int maxHp = EventHandlers.GetMaxHp(hub);
			float num3 = hub.playerStats.Health + (float)num;
			hub.playerStats.SetHPAmount((int)((num3 >= (float)maxHp) ? ((float)maxHp) : num3));
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002834 File Offset: 0x00000A34
		private static int GetMaxHp(ReferenceHub player)
		{
			int result;
			if (!SameThings.MaxHealth.TryGetValue((int)player.characterClassManager.CurClass, out result))
			{
				return player.playerStats.maxHP;
				
			}
			return result;
		}
	}
}
