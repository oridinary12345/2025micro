using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameHUD : MonoBehaviour
{
	[SerializeField]
	private UIStatusBarCharacter _hero;

	[SerializeField]
	private UIGameWeaponSelector _weaponSelector;

	[SerializeField]
	private UIGameWeaponSelector _itemSelector;

	[SerializeField]
	private UILevelWavesBar _waveBar;

	[SerializeField]
	private Text _comboText;

	[SerializeField]
	private Text _levelStateText;

	[SerializeField]
	private UIGameButton _pauseButton;

	[SerializeField]
	private UIMenuPausePanel _pausePanel;

	[SerializeField]
	private UIWeaponStatusPanel _weaponStatusPanel;

	[SerializeField]
	private UIGameIntroPanel _introPanel;

	[SerializeField]
	private DebugHUD _debugHUD;

	[SerializeField]
	private Image _chestIcon;

	private RectTransform _weaponRectTransform;

	private RectTransform _itemRectTransform;

	private CharacterEvents _characterEvents;

	private LevelData _levelData;

	private float _pauseButtonTargetY;

	private float _levelBarTargetY;

	public const float TransitionDuration = 0.25f;

	private Tweener _weaponTweener;

	public void Init(Character hero, GameEvents gameEvents, CharacterEvents characterEvents, GameStats gameStats, GameState gameState, LevelData level)
	{
		if (characterEvents == null)
		{
			UnityEngine.Debug.LogWarning("UIGameHUD.Init() characterEvents == null!");
		}
		if (level == null)
		{
			UnityEngine.Debug.LogWarning("UIGameHUD.Init() level == null!");
		}
		if (_hero == null)
		{
			UnityEngine.Debug.LogWarning("UIGameHUD.Init() _hero == null!");
		}
		if (_pausePanel == null)
		{
			UnityEngine.Debug.LogWarning("UIGameHUD.Init() _pausePanel == null!");
		}
		if (_weaponSelector == null)
		{
			UnityEngine.Debug.LogWarning("UIGameHUD.Init() _weaponSelector == null!");
		}
		if (gameEvents == null)
		{
			UnityEngine.Debug.LogWarning("UIGameHUD.Init() gameEvents == null!");
		}
		_characterEvents = characterEvents;
		_levelData = level;
		_hero.Init(hero, _characterEvents, true, true);
		_pausePanel.Init(gameEvents, gameStats, gameState);
		_weaponSelector.Init(hero, gameState, gameEvents, hero.GetWeapons());
		_weaponRectTransform = _weaponSelector.GetComponent<RectTransform>();
		if (_weaponRectTransform == null)
		{
			UnityEngine.Debug.LogWarning("UIGameHUD.Init() _weaponRectTransform == null!");
		}
		if (_levelData.IsObjectiveWaveBased)
		{
			_waveBar.Init(level);
		}
		else
		{
			_waveBar.gameObject.SetActive( false);
		}
		_introPanel.SetWaveCount(level.GetWaveMaxCount());
		gameEvents.ComboUpdatedEvent += OnComboUpdated;
		gameEvents.RoundEndedEvent += OnRoundEnded;
		_weaponStatusPanel.RegisterEvents();
		HideWeapons( true);
		_pauseButton.OnClick(delegate
		{
			if (MonoSingleton<UIMenuStack>.Instance.Peek() != _pausePanel)
			{
				gameEvents.OnGamePaused();
				_pausePanel.Show();
			}
		});
		_pauseButton.ActivateOnBackKey();
		if (level == null || level.Config == null || level.Config.Waves == null || level.Config.Waves.Count == 0)
		{
			UnityEngine.Debug.LogWarning("Debug text could cause a crash...");
		}
		if (UIMenuCheat.ShowDebugHUD)
		{
			object[] obj = new object[7]
			{
				level.LevelTitle,
				"\nMax monster: ",
				null,
				null,
				null,
				null,
				null
			};
			WaveConfig waveConfig = level.Config.Waves[0];
			obj[2] = waveConfig.MaxMonster;
			obj[3] = "\nLayer Min: ";
			WaveConfig waveConfig2 = level.Config.Waves[0];
			obj[4] = waveConfig2.SpawnLayerMin + 1;
			obj[5] = "\nLayer Max: ";
			WaveConfig waveConfig3 = level.Config.Waves[0];
			obj[6] = waveConfig3.SpawnLayerMax + 1;
			string text = string.Concat(obj);
			_debugHUD.SetText(text);
		}
	}

	public void PrepareTransition()
	{
		RectTransform component = _waveBar.GetComponent<RectTransform>();
		Vector2 anchoredPosition = component.anchoredPosition;
		_levelBarTargetY = anchoredPosition.y;
		component.DOAnchorPosY(_levelBarTargetY + component.rect.height * 2f, 0f).Complete();
		RectTransform component2 = _pauseButton.GetComponent<RectTransform>();
		Vector2 anchoredPosition2 = component2.anchoredPosition;
		_pauseButtonTargetY = anchoredPosition2.y;
		component2.DOAnchorPosY(_pauseButtonTargetY + component.rect.height, 0f).Complete();
	}

	public void ShowHUD(Action onHUDShown)
	{
		RectTransform component = _pauseButton.GetComponent<RectTransform>();
		component.DOAnchorPosY(_pauseButtonTargetY, 0.25f);
		RectTransform component2 = _waveBar.GetComponent<RectTransform>();
		component2.DOAnchorPosY(_levelBarTargetY, 0.25f).OnComplete(delegate
		{
			onHUDShown();
		});
	}

	private void OnDestroy()
	{
		if (_weaponTweener != null)
		{
			_weaponTweener.Kill();
			_weaponTweener = null;
		}
		if (_weaponStatusPanel != null)
		{
			_weaponStatusPanel.UnregisterEvents();
		}
	}

	public void UpdateWeapons(List<WeaponData> equippedWeapons)
	{
		_weaponSelector.UpdateWeapons(equippedWeapons);
	}

	public void UpdateItems(List<WeaponData> equippedItems)
	{
	}

	public void UpdateHero(Character hero, GameState gameState, GameEvents gameEvents)
	{
		_hero.Init(hero, _characterEvents, true, true);
		_weaponSelector.Init(hero, gameState, gameEvents, hero.GetWeapons());
	}

	public void HideWeapons(bool instantHide = false)
	{
		if (_weaponTweener != null)
		{
			_weaponTweener.Complete();
		}
		_weaponTweener = _weaponRectTransform.DOPivotY(1.8f, (!instantHide) ? 0.2f : 0f);
		if (instantHide)
		{
			_weaponTweener.Complete();
		}
	}

	public void ShowWeapons()
	{
		if (_weaponTweener != null)
		{
			_weaponTweener.Complete();
		}
		_weaponTweener = _weaponRectTransform.DOPivotY(0f, 0.2f);
	}

	public void OnScreenTapWhileWeaponRotating()
	{
		_weaponSelector.OnScreenTapWhileWeaponRotating();
	}

	public void StartGameLevelStartedAnim()
	{
	}

	private void OnComboUpdated(int comboCount)
	{
		if (_comboText != null)
		{
			if (comboCount > 1)
			{
				_comboText.text = "Combo x" + comboCount;
				_comboText.gameObject.SetActive(true);
			}
			else
			{
				_comboText.gameObject.SetActive(false);
			}
		}
	}

	private void OnRoundEnded()
	{
		if (_weaponSelector != null)
		{
			_weaponSelector.OnRoundEnded();
		}
	}
}