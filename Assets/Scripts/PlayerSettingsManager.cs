using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine;

public class PlayerSettingsManager
{
	private readonly SettingsProfile _profile;

	public bool BatterySavingEnabled
	{
		get
		{
			return _profile.BatterySavingEnabled;
		}
		set
		{
			_profile.BatterySavingEnabled = value;
		}
	}

	public bool MusicEnabled
	{
		get
		{
			return _profile.MusicEnabled;
		}
		set
		{
			_profile.MusicEnabled = value;
		}
	}

	public bool SoundEnabled
	{
		get
		{
			return _profile.SoundEnabled;
		}
		set
		{
			_profile.SoundEnabled = value;
		}
	}

	public string Language
	{
		get
		{
			return _profile.Language;
		}
		set
		{
			_profile.Language = value;
			LocalizationHelper.CurrentLanguage = _profile.Language;
		}
	}

	public PlayerSettingsManager(SettingsProfile profile)
	{
		_profile = profile;
		InitLanguage();
	}

	private void InitLanguage()
	{
		// 如果已经有保存的语言设置，使用保存的设置
		if (!string.IsNullOrEmpty(Language))
		{
			if (LocalizationHelper.HasLanguage(Language))
			{
				LocalizationHelper.CurrentLanguage = Language;
				return;
			}
		}

		// 默认使用中文
		if (LocalizationHelper.HasLanguage("Chinese (Simplified)"))
		{
			Language = "Chinese (Simplified)";
			LocalizationHelper.CurrentLanguage = "Chinese (Simplified)";
			return;
		}

		// 如果没有中文，使用英文作为后备语言
		if (LocalizationHelper.HasLanguage("English"))
		{
			Language = "English";
			LocalizationHelper.CurrentLanguage = "English";
		}
	}
}