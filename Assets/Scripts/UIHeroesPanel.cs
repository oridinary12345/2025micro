using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHeroesPanel : UIMenuPage
{
	[SerializeField]
	private UIGameButton _closeButton;

	[SerializeField]
	private ScrollRect _scroller;

	[SerializeField]
	private RectTransform _viewportContent;

	[SerializeField]
	private HeroBoxCard _selectedHeroBox;

	[SerializeField]
	private UIHeroDetailsPopup _detailsPopup;

	private Dictionary<string, UIHeroesPanelBox> _heroBoxes = new Dictionary<string, UIHeroesPanelBox>();

	protected override void Awake()
	{
		base.Awake();
		_closeButton.OnClick(OnCloseButtonClicked);
		_closeButton.ActivateOnBackKey();
		List<HeroConfig> configs = MonoSingleton<HeroConfigs>.Instance.GetConfigs();
		string currentHeroId = App.Instance.Player.HeroManager.GetCurrentHeroId();
		foreach (HeroConfig heroConfig in configs)
		{
			bool flag = currentHeroId == heroConfig.Id;
			UIHeroesPanelBox heroBox = CreateHeroBox(heroConfig, flag);
			heroBox.GetComponent<RectTransform>().SetParent(_viewportContent, false);
			heroBox.GetComponent<UIGameButton>().OnClick(delegate
			{
				OnHeroBoxClicked(heroBox, heroConfig);
			});
			_heroBoxes[heroConfig.Id] = heroBox;
			if (flag)
			{
				heroBox.Select();
				HeroData heroData = App.Instance.Player.HeroManager.GetHeroData(heroConfig.Id);
				_selectedHeroBox.Init(heroData);
			}
			else
			{
				heroBox.Unselect();
			}
		}
		_selectedHeroBox.GetComponent<UIGameButton>().OnClick(delegate
		{
			_detailsPopup.Show();
			_detailsPopup.Init(_selectedHeroBox.HeroData, null);
		});
	}

	private void OnEnable()
	{
		foreach (KeyValuePair<string, UIHeroesPanelBox> heroBox in _heroBoxes)
		{
			heroBox.Value.Refresh();
		}
	}

	private void OnHeroBoxClicked(UIHeroesPanelBox heroBox, HeroConfig heroConfig)
	{
		Action onEquipConfirmed = delegate
		{
			foreach (UIHeroesPanelBox value in _heroBoxes.Values)
			{
				if (heroBox != value)
				{
					value.Unselect();
				}
			}
			heroBox.Select();
			App.Instance.Player.HeroManager.SetCurrentHero(heroConfig.Id);
		};
		_detailsPopup.Show();
		_detailsPopup.Init(heroBox.HeroData, onEquipConfirmed);
	}

	private UIHeroesPanelBox CreateHeroBox(HeroConfig heroConfig, bool isSelected)
	{
		UIHeroesPanelBox uIHeroesPanelBox = UnityEngine.Object.Instantiate(Resources.Load<UIHeroesPanelBox>("UI/HeroBox"));
		return uIHeroesPanelBox.Init(App.Instance.Player.HeroManager.GetHeroData(heroConfig.Id), isSelected);
	}

	private void OnCloseButtonClicked()
	{
		Hide();
	}
}