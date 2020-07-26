using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Interfaces;

namespace SameThings
{

		public sealed class Config : IConfig
		{
			public bool IsEnabled { get; set; } = true;
			public float WindowHealth { get; set; } = 0f;
			public int ForceRestart { get; set; } = 0;
			public bool WarheadCleanup { get; set; } = false;
			public int ItemAutoCleanup { get; set; } = 0;
			public List<int> ItemCleanupIgnore { get; private set; } = new List<int>() { };
			public int RagdollAutoCleanup { get; set; } = 0;
			public List<string> NicknameFilter { get; private set; } = new List<string>() { };
			public string NicknameFilterReason { get; set; } = "You have been kicked from the server";
			public List<int> TeslaTriggerableTeam { get; private set; } = new List<int>() { };
			public bool ReduceAmmo { get; set; } = true;
			public bool UnlimitedRadio { get; set; } = false;
			public float LiftMoveDuration { get; set; } = -1f;
			public int AutoWarheadStart { get; set; } = 0;
			public bool AutoWarheadLock { get; set; } = false;
			public string AutoWarheadStartText { get; set; } = "";
			public float DecontaminationTime { get; set; } = 0f;
			public int GeneratorDuration { get; set; } = 0;
			public List<string> GeneratorKeycardPerm { get; private set; } = new List<string>() { };
			public List<int> GeneratorInsertTeams { get; private set; } = new List<int>() { };
			public List<int> GeneratorEjectTeams { get; private set; } = new List<int>() { };
			public List<int> GeneratorUnlockTeams { get; private set; } = new List<int>() { };
			public Dictionary<int, int> MaxHealth { get; private set; } = new Dictionary<int, int>()
		{
		};
			public Dictionary<int, int> SelfHealingDuration { get; private set; } = new Dictionary<int, int>()
			{
			};
			public Dictionary<int, int> SelfHealingAmount { get; private set; } = new Dictionary<int, int>()
			{
			};
			public int Scp106LureAmount { get; set; } = -1;
			public List<int> Scp106LureTeam { get; private set; } = new List<int>() { };
			public float Scp106LureReload { get; set; } = 0f;
			public List<int> NoInfAmmoTeam { get; private set; } = new List<int>() { };
			
		}
		

		
}
