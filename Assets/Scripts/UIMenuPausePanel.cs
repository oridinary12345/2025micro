using UnityEngine;
using UnityEngine.UI;

public class UIMenuPausePanel : UIMenuPopup
{
	[SerializeField]
	private UIGameButton _buttonClose;

	[SerializeField]
	private UIGameButton _buttonQuit;

	[SerializeField]
	private Toggle _musicToggle;

	[SerializeField]
	private Toggle _soundFxToggle;

	private GameEvents _gameEvents;

	private GameState _gameState;

	private GameStats _gameStats;

	public void Init(GameEvents gameEvents, GameStats gameStats, GameState gameState)
	{
		_gameEvents = gameEvents;
		_gameStats = gameStats;
		_gameState = gameState;
		_buttonClose.OnClick(OnCloseButtonClicked);
		_buttonClose.ActivateOnBackKey();
		_buttonQuit.OnClick(OnQuitButtonClicked);
		_musicToggle.onValueChanged.AddListener(OnMusicToggled);
		_soundFxToggle.onValueChanged.AddListener(OnSoundFXToggled);
		_musicToggle.isOn = !App.Instance.Player.SettingsManager.MusicEnabled;
		_soundFxToggle.isOn = !App.Instance.Player.SettingsManager.SoundEnabled;
	}

	private void OnCloseButtonClicked()
	{
		_gameEvents.OnGameResumed();
		Hide();
	}

	private void OnQuitButtonClicked()
	{
		_gameEvents.OnGameAbandoned(_gameStats, _gameState);
		App.Instance.LoadMainMenuFromGame();
	}

	private void OnMusicToggled(bool isActive)
	{
		App.Instance.Player.SettingsManager.MusicEnabled = !isActive;
		if (isActive)
		{
			MonoSingleton<GameMusicManager>.Instance.StopMusic();
		}
		else
		{
			MonoSingleton<GameMusicManager>.Instance.PlayMusicBattle();
		}
	}

	private void OnSoundFXToggled(bool isActive)
	{
		App.Instance.Player.SettingsManager.SoundEnabled = !isActive;
	}
}