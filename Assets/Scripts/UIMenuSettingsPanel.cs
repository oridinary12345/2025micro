using System;
using TMPro;
using UnityEngine;

public class UIMenuSettingsPanel : UIMenuPopup
{
	[SerializeField]
	private UIGameButton _buttonC;

	[SerializeField]
	private UIGameButton _buttonClose;

	[SerializeField]
	private UIGameButton _buttonContactUs;

	[SerializeField]
	private UIGameButton _twitterButton1;

	[SerializeField]
	private UIGameButton _twitterButton2;

	[SerializeField]
	private UIGameButton _buttonReset;

	[SerializeField]
	private UIGameButton _promoCodeButton;

	[SerializeField]
	private UIGameButton _languageButton;

	[SerializeField]
	private UIGameToggle _batterySavingToggle;

	[SerializeField]
	private UIGameToggle _musicToggle;

	[SerializeField]
	private UIGameToggle _soundFxToggle;

	[SerializeField]
	private TextMeshProUGUI _versionText;

	[SerializeField]
	private TextMeshProUGUI _idText;

	[SerializeField]
	private UIMenuCheat _menuCheat;

	[SerializeField]
	private UIInputPopup _inputPopup;

	[SerializeField]
	private UIMenuChooseLanguagePanel _chooseLanguagePopup;

	private int _cheatClickCount;

	protected override void Awake()
	{
		base.Awake();
		_buttonClose.OnClick(OnCloseButtonClicked);
		_buttonClose.ActivateOnBackKey();
		_buttonReset.OnClick(OnResetButtonClicked);
		_twitterButton1.OnClick(OnTwitterButton1Clicked);
		_twitterButton2.OnClick(OnTwitterButton2Clicked);
		_batterySavingToggle.onValueChanged.AddListener(OnBatterySavingToggled);
		_musicToggle.onValueChanged.AddListener(OnMusicToggled);
		_soundFxToggle.onValueChanged.AddListener(OnSoundFXToggled);
		_buttonContactUs.OnClick(OnContactUsClicked);
		_promoCodeButton.OnClick(OnPromoCodeClicked);
		_languageButton.OnClick(OnChooseLanguageClicked);
		_batterySavingToggle.isOn = !App.Instance.Player.SettingsManager.BatterySavingEnabled;
		_musicToggle.isOn = !App.Instance.Player.SettingsManager.MusicEnabled;
		_soundFxToggle.isOn = !App.Instance.Player.SettingsManager.SoundEnabled;
		_versionText.text = "Version: " + Application.version + "-" + App.Instance.Configs.Version;
		_idText.text = "ID: " + GetUID();
		_buttonC.OnClick(OnCheatButtonClicked);
		_buttonC.RemoveSound();
	}

	public override void OnFocusGained()
	{
		_cheatClickCount = 0;
		base.OnFocusGained();
	}

	private void OnResetButtonClicked()
	{
		Action<string> onContinue = delegate(string inputText)
		{
			if (inputText.ToUpper() == "RESET")
			{
				Universe.Instance.ResetSave();
			}
		};
		_inputPopup.Show();
		_inputPopup.Init("RESET GAME", "Please enter the word <color=red>RESET</color> to reset your progress.", "RESET", onContinue, true, false);
	}

	private void OnPromoCodeClicked()
	{
		Action<string> onContinue = delegate(string inputText)
		{
			_inputPopup.Hide();
			App.Instance.Player.RemoteRewardManager.ValidateCode(inputText);
		};
		_inputPopup.Show();
		_inputPopup.Init("CODE", "Enter your code:", "DONE", onContinue, false, true);
	}

	private void OnChooseLanguageClicked()
	{
		_chooseLanguagePopup.Show();
	}

	private void OnCloseButtonClicked()
	{
		if (MonoSingleton<UIMenuStack>.Instance.Peek() == this)
		{
			Hide();
		}
	}

	private void OnCheatButtonClicked()
	{
		_cheatClickCount++;
		if (_cheatClickCount >= 10)
		{
			_menuCheat.Show();
			_cheatClickCount = 0;
		}
	}

	private void OnTwitterButton1Clicked()
	{
		float timeSinceLevelLoad = Time.timeSinceLevelLoad;
		Application.OpenURL("twitter://user?screen_name=MagicFred");
		if (Time.timeSinceLevelLoad - timeSinceLevelLoad <= 1f)
		{
			Application.OpenURL("https://twitter.com/MagicFred");
		}
	}

	private void OnTwitterButton2Clicked()
	{
		float timeSinceLevelLoad = Time.timeSinceLevelLoad;
		Application.OpenURL("twitter://user?screen_name=dohamelin");
		if (Time.timeSinceLevelLoad - timeSinceLevelLoad <= 1f)
		{
			Application.OpenURL("https://twitter.com/dohamelin");
		}
	}

	private void OnBatterySavingToggled(bool isActive)
	{
		App.Instance.Player.SettingsManager.BatterySavingEnabled = !isActive;
		Application.targetFrameRate = ((!isActive) ? 30 : 60);
		if (!isActive)
		{
			Vector3 inputPosition = new Vector3(0f, (float)Screen.height * 0.5f, 0f);
			App.Instance.MenuEvents.OnTextOverlayRequested("Performance reduced!", inputPosition);
		}
	}

	private void OnMusicToggled(bool isActive)
	{
		AudioClip clip = Resources.Load<AudioClip>("buttonClicked");
		if (isActive)
		{
			MonoSingleton<AudioManager>.Instance.PlaySound(clip);
		}
		App.Instance.Player.SettingsManager.MusicEnabled = !isActive;
		if (!isActive)
		{
			MonoSingleton<AudioManager>.Instance.PlaySound(clip);
		}
		if (isActive)
		{
			MonoSingleton<GameMusicManager>.Instance.StopMusic();
		}
		else
		{
			MonoSingleton<GameMusicManager>.Instance.PlayMusicMenu();
		}
	}

	private void OnSoundFXToggled(bool isActive)
	{
		AudioClip clip = Resources.Load<AudioClip>("buttonClicked");
		if (isActive)
		{
			MonoSingleton<AudioManager>.Instance.PlaySound(clip);
		}
		App.Instance.Player.SettingsManager.SoundEnabled = !isActive;
		if (!isActive)
		{
			MonoSingleton<AudioManager>.Instance.PlaySound(clip);
		}
	}

	private string GetUID()
	{
		CloudUser currentUser = MonoSingleton<Cloud>.Instance.CurrentUser;
		return (currentUser == null) ? App.Instance.Player.UID : currentUser.PlayfabId;
	}

	private void OnContactUsClicked()
	{
		string str = "%0D%0A%0D%0A%0D%0AVersion: " + Application.version + "%0D%0AID: " + GetUID() + "%0D%0ADevice: " + SystemInfo.deviceModel + "%0D%0AOS: " + SystemInfo.operatingSystem;
		string text = "?subject=Micro RPG Support&body=" + str;
		string url = "mailto:dhamelin@gmail.com" + text.Replace(" ", "%20");
		Application.OpenURL(url);
	}
}