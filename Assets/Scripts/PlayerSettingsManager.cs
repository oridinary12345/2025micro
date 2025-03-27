using I2.Loc;
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
			LocalizationManager.CurrentLanguage = _profile.Language;
		}
	}

	public PlayerSettingsManager(SettingsProfile profile)
	{
		_profile = profile;
		if (string.IsNullOrEmpty(Language))
		{
			string language = Application.systemLanguage.ToString();
			if (LocalizationManager.HasLanguage(language))
			{
				Language = language;
			}
			else
			{
				Language = "English";
			}
		}
	}
}