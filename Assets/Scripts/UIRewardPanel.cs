using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIRewardPanel : UIMenu
{
	[SerializeField]
	private Text _titleText;

	[SerializeField]
	private Text _itemText;

	[SerializeField]
	private Text _amountText;

	[SerializeField]
	private Image _itemImage;

	[SerializeField]
	private UIGameButton _buttonClose;

	[SerializeField]
	private GameObject _tapToContinueLabel;

	[SerializeField]
	private RectTransform _rayImage;

	[SerializeField]
	private RectTransform _fxPivot;

	[SerializeField]
	private ParticleSystem _particles;

	[SerializeField]
	private Transform _rewardBoxTransform;

	[SerializeField]
	private Transform _rewardBoxHiddenTransform;

	[SerializeField]
	private Image _fullScreenImage;

	[SerializeField]
	private UIGradient _cardBackgroundGradient;

	[SerializeField]
	private UIGradient _backgroundGradient;

	private int _cardCountTarget;

	private Tweener _rewardHiddenBoxTweenPart1;

	private Tweener _rewardHiddenBoxTweenPart2;

	private Tweener _rayTweener;

	public static UIRewardPanel Create()
	{
		UIRewardPanel uIRewardPanel = UnityEngine.Object.Instantiate(Resources.Load<UIRewardPanel>("UI/RewardsPanel"));
		uIRewardPanel.GetComponent<RectTransform>().SetParent(UnityEngine.Object.FindObjectOfType<UIAppCanvas>().transform, false);
		return uIRewardPanel;
	}

	protected override void Awake()
	{
		base.Awake();
		_buttonClose.OnClick(OnCloseButtonClicked);
		_buttonClose.ActivateOnBackKey();
		_fullScreenImage = GameObject.Find("FullScreenEffects").GetComponent<Image>();
	}

	public void Show(Reward reward, Action<Color, Color> setColor = null)
	{
		if (reward.RewardType == RewardType.list)
		{
			UnityEngine.Debug.LogWarning("RewardList should has been handled before we try to display them!");
		}
		if (reward.HasUnlockedSomething)
		{
			_backgroundGradient.m_color1 = "#00AC9EFF".ToColor();
			_backgroundGradient.m_color2 = "#08C8FFFF".ToColor();
		}
		else
		{
			_backgroundGradient.m_color1 = "#E99100FF".ToColor();
			_backgroundGradient.m_color2 = "#FBD123FF".ToColor();
		}
		setColor?.Invoke(_backgroundGradient.m_color1, _backgroundGradient.m_color2);
		_itemText.text = string.Empty;
		string text = string.Empty;
		Color color = "#c38e6c".ToColor();
		_amountText.text = string.Empty;
		_cardCountTarget = 0;
		_cardBackgroundGradient.m_color1 = "#c38f6d".ToColor();
		_cardBackgroundGradient.m_color2 = "#f4d4b1".ToColor();
		RewardLoot rewardLoot = reward as RewardLoot;
		if (rewardLoot != null)
		{
			_itemImage.sprite = Resources.Load<Sprite>("UI/UI_reward_" + rewardLoot.LootId);
			_itemText.text = rewardLoot.ToString();
			LootConfig config = MonoSingleton<LootConfigs>.Instance.GetConfig(rewardLoot.LootId);
			text = ((config == null) ? string.Empty : config.Title);
		}
		RewardFreeCoins rewardFreeCoins = reward as RewardFreeCoins;
		if (rewardFreeCoins != null)
		{
			_itemImage.sprite = Resources.Load<Sprite>("UI/UI_reward_lootCoin");
			_itemText.text = rewardFreeCoins.ToString();
			text = MonoSingleton<LootConfigs>.Instance.GetConfig("lootCoin").Title;
		}
		RewardWeapon rewardWeapon = reward as RewardWeapon;
		if (rewardWeapon != null)
		{
			_itemImage.sprite = Resources.Load<Sprite>("UI/UI_reward_" + rewardWeapon.WeaponId);
			WeaponConfig config2 = MonoSingleton<WeaponConfigs>.Instance.GetConfig(rewardWeapon.WeaponId);
			_itemText.text = config2.RangeType + " Range";
			text = config2.Title;
			if (config2.RangeType == WeaponRangeType.Short)
			{
				_cardBackgroundGradient.m_color1 = "#de742d".ToColor();
				_cardBackgroundGradient.m_color2 = "#f7c740".ToColor();
				color = "#df762d".ToColor();
			}
			else if (config2.RangeType == WeaponRangeType.Medium)
			{
				_cardBackgroundGradient.m_color1 = "#df335c".ToColor();
				_cardBackgroundGradient.m_color2 = "#f0865a".ToColor();
				color = "#df345c".ToColor();
			}
			else if (config2.RangeType == WeaponRangeType.Long)
			{
				_cardBackgroundGradient.m_color1 = "#499a7f".ToColor();
				_cardBackgroundGradient.m_color2 = "#6fc3ce".ToColor();
				color = "#499a7f".ToColor();
			}
			_cardCountTarget = rewardWeapon.CardCount;
		}
		RewardHero rewardHero = reward as RewardHero;
		if (rewardHero != null)
		{
			_cardBackgroundGradient.m_color1 = "#be5d88".ToColor();
			_cardBackgroundGradient.m_color2 = "#ef91ad".ToColor();
			color = "#be5d88".ToColor();
			_itemImage.sprite = Resources.Load<Sprite>("UI/UI_reward_" + rewardHero.HeroId);
			HeroConfig config3 = MonoSingleton<HeroConfigs>.Instance.GetConfig(rewardHero.HeroId);
			_itemText.text = "Hero";
			text = config3.Name;
			_cardCountTarget = rewardHero.CardCount;
		}
		if (_titleText != null)
		{
			_titleText.text = text.ToUpper();
			Text[] texts = _titleText.transform.GetComponentsInChildren<Text>();
			foreach (Text textComponent in texts)
			{
				if (textComponent != null)
				{
					textComponent.text = text.ToUpper();
					if (_titleText != textComponent)
					{
						textComponent.color = color;
					}
				}
			}
		}
		if (!string.IsNullOrEmpty(_itemText.text))
		{
			Show();
		}
		else
		{
			UnityEngine.Debug.LogWarning("Can't handle reward type: " + reward);
		}
		_particles.Stop();
		_particles.gameObject.SetActive( false);
		_titleText.gameObject.SetActive( false);
		_rewardBoxTransform.gameObject.SetActive( false);
		_tapToContinueLabel.SetActive( false);
		_rewardHiddenBoxTweenPart1 = null;
		_rewardHiddenBoxTweenPart2 = null;
		Action onShown = delegate
		{
			Action onHidden = delegate
			{
				Reveal();
				_rewardBoxHiddenTransform.gameObject.SetActive( false);
			};
			_rewardHiddenBoxTweenPart2 = _rewardBoxHiddenTransform.transform.DOScale(Vector3.one * 0.2f, 0.5f).OnComplete(delegate
			{
				onHidden();
			}).SetDelay(0.5f)
				.OnStart(App.Instance.MenuEvents.OnWeaponUnlockTranslated);
		};
		App.Instance.MenuEvents.OnWeaponUnlockTranslated();
		_rewardBoxHiddenTransform.gameObject.SetActive( false);
		_rewardBoxHiddenTransform.transform.localScale = Vector3.one * 0.05f;
		_rewardHiddenBoxTweenPart1 = _rewardBoxHiddenTransform.transform.DOScale(Vector3.one * 1.25f, 0.4f).SetEase(Ease.OutElastic, 1.7f, 0.5f).SetDelay(0.25f)
			.OnStart(delegate
			{
				_rewardBoxHiddenTransform.gameObject.SetActive( true);
			})
			.OnComplete(delegate
			{
				onShown();
			});
	}

	private void OnCloseButtonClicked()
	{
		if (!_tapToContinueLabel.activeSelf)
		{
			if (_rewardHiddenBoxTweenPart1 != null)
			{
				_rewardHiddenBoxTweenPart1.Complete();
				_rewardHiddenBoxTweenPart1 = null;
			}
			if (_rewardHiddenBoxTweenPart2 != null)
			{
				_rewardHiddenBoxTweenPart2.Complete();
				_rewardHiddenBoxTweenPart2 = null;
			}
		}
		else
		{
			if (_rayTweener != null)
			{
				_rayTweener.Kill( true);
				_rayTweener = null;
			}
			_rayImage.gameObject.SetActive( false);
			_particles.gameObject.SetActive( false);
			_particles.Stop();
			Hide();
		}
	}

	private void Reveal()
	{
		_titleText.gameObject.SetActive( true);
		_fullScreenImage.enabled = true;
		_fullScreenImage.DOFade(0f, 0.08f).SetDelay(0.05f).OnComplete(delegate
		{
			_fullScreenImage.enabled = false;
			_fullScreenImage.color = Color.white;
		});
		_rewardBoxTransform.localScale = Vector3.one * 2f;
		_rewardBoxTransform.gameObject.SetActive( true);
		_rewardBoxTransform.DOScale(Vector3.one * 1.5f, 0.32f).SetEase(Ease.OutElastic);
		_tapToContinueLabel.SetActive( true);
		App.Instance.MenuEvents.OnWeaponUnlockRevealed(_fxPivot.transform);
		_rayImage.gameObject.SetActive( true);
		_particles.gameObject.SetActive( true);
		_particles.Play();
		StartCardCountAnimation();
		if (_rayTweener == null)
		{
			_rayTweener = _rayImage.DORotate(new Vector3(0f, 0f, -360f), 3f).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
			_rayTweener.OnKill(delegate
			{
				_rayTweener = null;
			});
		}
	}

	private void StartCardCountAnimation()
	{
		if (_cardCountTarget > 0)
		{
			_amountText.text = string.Empty;
			int amount = 1;
			Tweener t = DOTween.To(() => amount, delegate(int x)
			{
				amount = x;
			}, _cardCountTarget, 0.3f).OnUpdate(delegate
			{
				_amountText.text = "×" + amount.ToString();
			});
			t.SetDelay(0.25f);
			_amountText.rectTransform.DOPunchScale(Vector3.one * 0.6f, 0.2f).SetDelay(0.5f).OnComplete(delegate
			{
				_amountText.rectTransform.localScale = Vector3.one;
			});
		}
	}
}