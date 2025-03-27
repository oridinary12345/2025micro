public class Chest : Character
{
	private ChestData _chestData;

	public override string Id => _chestData.Config.Id;

	public override string Name => _chestData.Config.Name;

	public override float AttackMovementDuration => 0f;

	public override float RunningSpeed => 0f;

	public override float MissRate => 0f;

	public static Chest Create(ChestData chestData, CharacterEvents characterEvents)
	{
		CharacterVisual characterVisual = CharacterVisual.Create(chestData.Config.PrefabSkinName, Element.None);
		Chest chest = characterVisual.gameObject.AddComponent<Chest>();
		return chest.Init(chestData, characterVisual, characterEvents);
	}

	private Chest Init(ChestData chestData, CharacterVisual characterVisual, CharacterEvents characterEvents)
	{
		Init(characterVisual, characterEvents);
		_chestData = chestData;
		return this;
	}

	protected override int ComputeDamage()
	{
		return 0;
	}

	public override Element GetCharacterElement()
	{
		return Element.None;
	}

	public override Element GetWeaponElementWeak()
	{
		return Element.None;
	}

	public override Element GetWeaponElementCritical()
	{
		return Element.None;
	}

	public override Element GetWeaponElementMiss()
	{
		return Element.None;
	}
}