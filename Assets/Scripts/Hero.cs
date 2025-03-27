using System.Collections.Generic;
using UnityEngine;

public class Hero : Character
{
	private HeroConfig _config;

	private HeroProfile _profile;

	private HeroEvents _heroEvents;

	private WeaponEvents _weaponEvents;

	private List<WeaponData> _weapons;

	private WeaponData _currentWeapon;

	public override string Id => _config.Id;

	public override string Name => _config.Name;

	public override int HP
	{
		get
		{
			return _profile.HP;
		}
		protected set
		{
			_profile.HP = value;
		}
	}

	public override int HPMax => GetHPMax(_profile.Level);

	public override float AttackPrecision => _currentWeapon.AttackPrecision;

	public override bool CanJump => true;

	public override bool IsRangedAttack => _currentWeapon.IsRangedAttack();

	public override bool IsMeleeAttack => _currentWeapon.IsMeleeAttack();

	public HeroProfile Profile => _profile;

	public override float AttackMovementDuration => 0.2f;

	public override float RunningSpeed => 7f;

	public override float MissRate => _config.MissRate;

	public int Level => _profile.Level;

	public bool CanHeal => HP < HPMax;

	public HeroEvents HeroEvents => _heroEvents;

	public static Hero Create(HeroData heroData, WeaponEvents weaponEvents, CharacterEvents characterEvents)
	{
		CharacterVisual characterVisual = CharacterVisual.Create(heroData.HeroConfig.Id, Element.None);
		Hero hero = characterVisual.gameObject.AddComponent<Hero>();
		return hero.Init(heroData, weaponEvents, characterVisual, characterEvents);
	}

	private Hero Init(HeroData heroData, WeaponEvents weaponEvents, CharacterVisual visual, CharacterEvents characterEvents)
	{
		_config = heroData.HeroConfig;
		_profile = heroData.Profile;
		_weapons = heroData.Weapons;
		_heroEvents = heroData.Events;
		_weaponEvents = weaponEvents;
		_currentWeapon = _weapons[0];
		_weaponEvents.RepairedEvent += base.OnWeaponRepaired;
		_weaponEvents.LevelUpEvent += base.OnLevelUp;
		_heroEvents.HeroHealedEvent += base.OnHeroHealed;
		RegisterWeaponEvent();
		Init(visual, characterEvents);
		return this;
	}

	protected override void OnDestroy()
	{
		UnregisterWeaponEvent();
		_weaponEvents.RepairedEvent -= base.OnWeaponRepaired;
		_weaponEvents.LevelUpEvent -= base.OnLevelUp;
		_heroEvents.HeroHealedEvent -= base.OnHeroHealed;
		base.OnDestroy();
	}

	private void UnregisterWeaponEvent()
	{
		if (_currentWeapon != null)
		{
			_currentWeapon.BrokenEvent -= OnWeaponBroken;
		}
	}

	private void RegisterWeaponEvent()
	{
		_currentWeapon.BrokenEvent += OnWeaponBroken;
	}

	protected override void OnWeaponBroken(string weaponId)
	{
		base.OnWeaponBroken(weaponId);
		_weaponEvents.OnBroken(weaponId);
	}

	protected override void UpdateWeapons(List<WeaponData> weapons)
	{
		_weapons = weapons;
		if (_weapons.Find((WeaponData w) => w.Id == _currentWeapon.Id) == null)
		{
			UnregisterWeaponEvent();
			_currentWeapon = weapons[0];
			RegisterWeaponEvent();
		}
	}

	private int GetHPMax(int level)
	{
		return _config.GetHPMax(level);
	}

	public int GetNextLevelHPBonus()
	{
		return GetHPMax(_profile.Level + 1) - HPMax;
	}

	public override Element GetCharacterElement()
	{
		return Element.None;
	}

	public override Element GetWeaponElementWeak()
	{
		return _currentWeapon.Config.ElementWeak;
	}

	public override Element GetWeaponElementCritical()
	{
		return _currentWeapon.Config.ElementCritical;
	}

	public override Element GetWeaponElementMiss()
	{
		return _currentWeapon.Config.ElementMiss;
	}

	protected override void OnAttack(int damageToWeapon)
	{
		_currentWeapon.TakeDamage(damageToWeapon);
	}

	public override List<WeaponData> GetWeapons()
	{
		return _weapons;
	}

	public override List<WeaponData> GetWeaponItems()
	{
		return App.Instance.Player.ItemInventory.GetEquippedWeapons();
	}

	public override WeaponData GetWeapon(string weaponId)
	{
		return _weapons.Find((WeaponData w) => w.Id == weaponId);
	}

	public override WeaponData GetWeaponItem(string weaponId)
	{
		List<WeaponData> weaponItems = GetWeaponItems();
		return weaponItems.Find((WeaponData w) => w.Id == weaponId);
	}

	protected override int ComputeDamage()
	{
		int damage = _currentWeapon.GetDamage();
		return Mathf.RoundToInt(damage);
	}

	protected override string GetWeaponSpriteName()
	{
		return _currentWeapon.GetSpriteName();
	}

	public override WeaponData GetCurrentWeapon()
	{
		return _currentWeapon;
	}

	protected override void OnWeaponChangedImpl(WeaponData data)
	{
		if (_currentWeapon != null)
		{
			UnregisterWeaponEvent();
			_weaponEvents.RepairedEvent -= base.OnWeaponRepaired;
			_weaponEvents.LevelUpEvent -= base.OnLevelUp;
		}
		_currentWeapon = data;
		RegisterWeaponEvent();
		_weaponEvents.RepairedEvent += base.OnWeaponRepaired;
		_weaponEvents.LevelUpEvent += base.OnLevelUp;
	}
}