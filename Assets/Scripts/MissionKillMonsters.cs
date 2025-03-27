public class MissionKillMonsters : Mission
{
	private string _weaponId;

	public string MonsterId
	{
		get;
		private set;
	}

	public MissionKillMonsters(MissionProfile profile, MissionConfig config)
		: base(profile)
	{
		MonsterId = config.ParamStr1;
		_weaponId = config.ParamStr2;
	}

	public override string GetDescription()
	{
		if (!string.IsNullOrEmpty(_weaponId))
		{
			string title = MonoSingleton<WeaponConfigs>.Instance.GetConfig(_weaponId).Title;
			string name = MonoSingleton<MonsterConfigs>.Instance.GetConfig(MonsterId).Name;
			return $"Beat {base.Objective} {name} with the {title}";
		}
		return $"Beat {base.Objective} monsters";
	}

	public override string GetDescriptionCompact()
	{
		if (!string.IsNullOrEmpty(_weaponId))
		{
			string title = MonoSingleton<WeaponConfigs>.Instance.GetConfig(_weaponId).Title;
			return $"Beat <color=#FF4364>{base.Objective}</color> with the {title}";
		}
		return $"Beat {base.Objective} monsters";
	}

	public override void Register(GameEvents gameEvents)
	{
		gameEvents.MonsterKilledEvent += OnMonsterKilled;
	}

	public override void UnRegister(GameEvents gameEvents)
	{
		gameEvents.MonsterKilledEvent -= OnMonsterKilled;
	}

	private void OnMonsterKilled(string monsterId, string killedWithWeaponId)
	{
		if (!monsterId.StartsWith("chest") && (string.IsNullOrEmpty(_weaponId) || !(_weaponId != killedWithWeaponId)) && (string.IsNullOrEmpty(MonsterId) || !(MonsterId != monsterId)))
		{
			AddProgress(1f);
		}
	}

	public override string ToString()
	{
		return string.Format("[MissionKillMonsters] " + GetDescription() + ". Progress: " + GetProgressionText());
	}
}