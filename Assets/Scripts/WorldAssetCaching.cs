using UnityEngine;

public class WorldAssetCaching
{
	public static void PreloadAssets(string worldId)
	{
		LoadCommonAssets();
		switch (worldId)
		{
		case "w00":
			LoadWorld1();
			break;
		case "w01":
			LoadWorld2();
			break;
		case "w02":
			LoadWorld3();
			break;
		default:
			UnityEngine.Debug.LogWarning("WorldAssetCaching is missing asset preloading for world id = " + worldId);
			break;
		}
	}

	private static void LoadCommonAssets()
	{
		PreloadGameFX("FXHitImpact");
		PreloadGameFX("FXHitImpactCritical");
		PreloadGameFX("FXStars");
		PreloadGameFX("FXCoinsSparkling");
		PreloadGameFX("FXCoinsSparklingCollect");
		PreloadGameFX("FXStatusWeak");
		PreloadGameFX("FXStatusMiss");
		PreloadGameFX("FXStatusCritical");
		PreloadGameFX("FXProjectileLandWeapon03");
		PreloadGameFX("FXProjectileLandWeapon09");
	}

	private static void LoadWorld1()
	{
		PreloadMonster("mushroom");
		PreloadMonster("woodlog");
		PreloadMonster("acorn");
		PreloadMonster("strawberry");
		PreloadMonster("dragon");
		PreloadGameFX("FXFlames");
	}

	private static void LoadWorld2()
	{
		PreloadMonster("skeleton");
		PreloadMonster("snake");
		PreloadMonster("ghost");
		PreloadMonster("spider");
		PreloadMonster("eye");
		PreloadGameFX("TrailRendererLine");
		PreloadGameFX("FXAttackSpider");
		PreloadGameFX("FXPetrifyRay");
		PreloadGameFX("FXPetrifyTarget");
	}

	private static void LoadWorld3()
	{
		PreloadMonster("fishman");
		PreloadMonster("leech");
		PreloadMonster("mushroomToxic");
		PreloadMonster("salamander");
		PreloadMonster("frog");
		PreloadGameFX("FXAttackSalamander");
		PreloadGameFX("FXHealing");
		PreloadGameFX("FXDiveSplash");
		PreloadGameFX("FXToxicGaz");
		PreloadGameFX("FXUnderwater");
	}

	private static void PreloadMonster(string characterId)
	{
		if (Resources.Load<GameObject>("Characters/" + characterId + "Skin") == null)
		{
			UnityEngine.Debug.LogWarning("Failed to preload monster: " + characterId);
		}
	}

	private static void PreloadGameFX(string fxName)
	{
		if (Resources.Load<GameObject>("GameFX/" + fxName) == null)
		{
			UnityEngine.Debug.LogWarning("Failed to preload game fx: " + fxName);
		}
	}
}