using System;
using UnityEngine;

public class WeaponData
{
	private readonly WeaponConfig _config;

	private readonly WeaponProfile _profile;

	public WeaponConfig Config => _config;

	public WeaponProfile Profile => _profile;

	public string Id => _config.Id;

	public int HP
	{
		get
		{
			return _profile.HP;
		}
		private set
		{
			_profile.HP = value;
		}
	}

	public float HP01 => (float)HP / (float)HPMax;

	public int HPMax => GetHPMax(_profile.Level);

	public int CardCollectedCount => _config.GetCurrentCardsCount(Level, _profile.CardCollectedCount);

	public int CardAmountNeededBase => _config.GetCardAmountNeeded(Level);

	public float NextLevelObjective01 => Mathf.Clamp01((float)CardCollectedCount / (float)CardAmountNeededBase);

	public bool CardObjectiveReached => CardCollectedCount >= CardAmountNeededBase;

	public string NextLevelProgressString => CardCollectedCount + "/" + CardAmountNeededBase;

	public bool CanHeal => HP < HPMax;

	public bool HasReachMaxLevel => Level >= 20;

	public bool Broken => HP <= 0;

	public int Level => _profile.Level;

	public bool Unlocked => _profile.CardCollectedCount > 0;

	public WeaponSpeedType SpinSpeed => WeaponSpeedType.Normal;

	public float AttackPrecision => 1f;

	public int PushBackForce => _config.PushForce;

	public event Action DamageTakenEvent;

	public event Action<string> BrokenEvent;

	public WeaponData(WeaponConfig config, WeaponProfile profile)
	{
		_config = config;
		_profile = profile;
	}

	public int GetMinDamage(bool ignoreBroken = false)
	{
		return GetMinDamage(_profile.Level, ignoreBroken);
	}

	private int GetMinDamage(int level, bool ignoreBroken = false)
	{
		int num = _config.DamageMin + _config.DamageLevelRatio * (level - 1);
		return Mathf.RoundToInt((float)num * ((!Broken || ignoreBroken) ? 1f : 0.5f));
	}

	private int GetHPMax(int level)
	{
		return _config.GetHPMax(level);
	}

	public int GetDamage()
	{
		int min = _config.DamageMin + _config.DamageLevelRatio * (_profile.Level - 1);
		int num = _config.DamageMax + _config.DamageLevelRatio * (_profile.Level - 1);
		int num2 = UnityEngine.Random.Range(min, num + 1);
		return Mathf.RoundToInt((float)num2 * ((!Broken) ? 1f : 0.5f));
	}

	public int GetNextLevelHPBonus()
	{
		return GetHPMax(_profile.Level + 1) - HPMax;
	}

	public int GetNextLevelDamageBonus()
	{
		return GetMinDamage(_profile.Level + 1, true) - GetMinDamage( true);
	}

	public int GetLevelUpPriceAmount()
	{
		return _config.GetLevelUpPriceAmount(_profile.Level);
	}

	public int GetRepairPriceAmount()
	{
		return _config.GetRepairPriceAmount(_profile.Level, _profile.HP);
	}

	public bool IsMeleeAttack()
	{
		return _config.WeaponType == WeaponType.Melee;
	}

	public bool IsRangedAttack()
	{
		return _config.WeaponType == WeaponType.Ranged;
	}

	public bool IsBombAttack()
	{
		return _config.WeaponType == WeaponType.Bomb;
	}

	public void TakeDamage(int damage)
	{
		if (HP > 0)
		{
			HP = Mathf.Max(0, HP - damage);
			if (this.DamageTakenEvent != null)
			{
				this.DamageTakenEvent();
			}
			if (Broken && this.BrokenEvent != null)
			{
				this.BrokenEvent(Config.Id);
			}
		}
	}

	public string GetSpriteName()
	{
		return Id;
	}
}