using System.Collections.Generic;

public class PlayerItemInventory
{
	public PlayerItemInventory(PlayerLootManager lootManager)
	{
	}

	public List<LootProfile> GetEquippedItems()
	{
		List<LootProfile> list = new List<LootProfile>();
		list.Add(LootProfile.Create("lootItemBomb", 10));
		return list;
	}

	public List<WeaponData> GetEquippedWeapons()
	{
		List<WeaponData> list = new List<WeaponData>();
		WeaponData weapon = App.Instance.Player.WeaponManager.GetWeapon("weapon04Bomb");
		if (weapon != null)
		{
			list.Add(weapon);
		}
		return list;
	}
}