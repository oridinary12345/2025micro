using System.Collections.Generic;
using UnityEngine;

public class CharacterAttackingPhaseWeapon : CharacterAttackingPhase
{
	public CharacterAttackingPhaseWeapon Init(Character character, WeaponShape weaponShape)
	{
		Init(character);
		if (character.PendingTargets.Count != 0)
		{
			UnityEngine.Debug.LogWarning(character + " is already attacking monsters...");
		}
		List<GameObject> lastTouchedObjects = weaponShape.GetLastTouchedObjects("Monster");
		List<Character> list = new List<Character>();
		foreach (GameObject item in lastTouchedObjects)
		{
			Transform transform = item.transform;
			Character character2 = null;
			do
			{
				character2 = transform.GetComponent<Character>();
				transform = transform.parent;
			}
			while (character2 == null && transform != null);
			if (character2 != null)
			{
				list.Add(character2);
			}
		}
		list.Sort((Character c1, Character c2) => Vector2.Distance(character.GetPositionXY(), c1.GetPositionXY()).CompareTo(Vector2.Distance(character.GetPositionXY(), c2.GetPositionXY())));
		character.SetAttackTargets(list);
		if (list.Count > 0)
		{
			character.StartAttack();
		}
		return this;
	}
}