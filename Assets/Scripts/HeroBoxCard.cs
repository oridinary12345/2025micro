using TMPro;
using UnityEngine;

public class HeroBoxCard : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI _heroHPText;

	[SerializeField]
	private TextMeshProUGUI _heroLevelText;

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
		SetHP(_hero.HeroConfig.GetHPMax(_hero.Profile.Level));
		SetLevel(_hero.Profile.Level);
	}

	private void SetHP(int hp)
	{
		_heroHPText.text = "<sprite=1>" + hp;
	}

	private void SetLevel(int level)
	{
		_heroLevelText.text = "Lv. " + level;
	}
}