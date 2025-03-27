using UnityEngine;

public class CommonFXManager : MonoBehaviour
{
	public CommonFXManager Setup(Player player)
	{
		player.WeaponManager.Events.RepairedEvent += OnWeaponRepaired;
		player.WeaponManager.Events.LevelUpEvent += OnWeaponLevelUp;
		player.WeaponManager.Events.BrokenEvent += OnWeaponBroken;
		player.HeroManager.Events.HeroLevelUpEvent += OnHeroLevelUp;
		player.HeroManager.Events.HeroHealedEvent += OnHeroHealed;
		player.LootManager.Events.LootUpdatedEvent += OnLootUpdated;
		return this;
	}

	private void OnLootUpdated(LootProfile loot, int delta, CurrencyReason reason)
	{
		if (loot.LootId == "lootCoin" && delta > 0)
		{
			PlaySoundFX("coinsAdded");
		}
		if (loot.LootId == "lootKey" && delta < 0)
		{
			PlaySoundFX("key");
		}
	}

	private void OnWeaponRepaired(string weaponId, int repairedAmount, bool skipFX)
	{
		if (skipFX)
		{
		}
	}

	private void OnWeaponLevelUp(string weaponId)
	{
		PlaySoundFX("levelUp");
	}

	private void OnWeaponBroken(string weaponId)
	{
		PlaySoundFX("heroAttackBroken");
	}

	private void OnHeroLevelUp(HeroData heroData)
	{
		PlaySoundFX("levelUp");
	}

	private void OnHeroHealed(HeroData hero, int healAmount, bool skipFx)
	{
		if (skipFx)
		{
		}
	}

	public void PlayParticleFX(string name, Vector3 position)
	{
		ParticleSystem particleSystem = UnityEngine.Object.Instantiate(Resources.Load<ParticleSystem>("GameFX/" + name));
		if (particleSystem != null)
		{
			particleSystem.transform.position = position;
			particleSystem.Play();
		}
	}

	public AudioClip PlaySoundFX(string name, float pitch = 1f)
	{
		if (!App.Instance.Player.SettingsManager.SoundEnabled)
		{
			return null;
		}
		AudioClip clip = Resources.Load<AudioClip>(name);
		return PlaySoundFX(clip, pitch);
	}

	public AudioClip PlaySoundFX(AudioClip clip, float pitch = 1f)
	{
		if (!App.Instance.Player.SettingsManager.SoundEnabled)
		{
			return null;
		}
		if (clip == null)
		{
			return null;
		}
		MonoSingleton<AudioManager>.Instance.PlaySound(clip, pitch);
		return clip;
	}

	public AudioClip PlaySoundFX(AudioSource source, string filename)
	{
		if (!App.Instance.Player.SettingsManager.SoundEnabled)
		{
			return null;
		}
		AudioClip audioClip = Resources.Load<AudioClip>(filename);
		if (audioClip != null)
		{
			source.clip = audioClip;
			source.Play();
		}
		return audioClip;
	}
}