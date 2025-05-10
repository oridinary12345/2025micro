using UnityEngine;
using UnityEngine.UI;

public class HeroBoxCard : MonoBehaviour
{
	[SerializeField]
	private Text _heroHPText;

	[SerializeField]
	private Text _heroLevelText;

	private HeroData _hero;

	public HeroData HeroData => _hero;

	public HeroBoxCard Init(HeroData heroData)
	{
		_hero = heroData;
		if (_hero != null)
		{
			_hero.Events.HeroLevelUpEvent += OnHeroLevelUp;
		}
		UpdateHeroStats();
		return this;
	}

	private void OnDestroy()
	{
		if (_hero != null)
		{
			_hero.Events.HeroLevelUpEvent -= OnHeroLevelUp;
		}
	}

	private void OnHeroLevelUp(HeroData hero)
	{
		UpdateHeroStats();
	}

	private void UpdateHeroStats()
	{
		if (_hero == null || _hero.HeroConfig == null || _hero.Profile == null)
		{
			Debug.LogWarning("UpdateHeroStats: Missing hero data");
			return;
		}

		try
		{
			SetHP(_hero.HeroConfig.GetHPMax(_hero.Profile.Level));
			SetLevel(_hero.Profile.Level);
		}
		catch (System.Exception ex)
		{
			Debug.LogError($"Error updating hero stats: {ex.Message}");
		}
	}

	private void SetHP(int hp)
	{
		if (_heroHPText == null)
		{
			Debug.LogWarning("SetHP: Missing HP text component");
			return;
		}

		_heroHPText.text = "<sprite=1>" + hp;
	}

	private void SetLevel(int level)
	{
		if (_heroLevelText == null)
		{
			Debug.LogWarning("SetLevel: Missing level text component");
			return;
		}

		_heroLevelText.text = "Lv." + level;
	}
}