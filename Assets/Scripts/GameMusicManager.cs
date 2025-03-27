using EazyTools.SoundManager;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameMusicManager : MonoSingleton<GameMusicManager>
{
	private enum GameMusic
	{
		None,
		Battle,
		BattleBoss,
		BattleWin,
		BattleLost
	}

	private readonly Dictionary<string, AudioClip> _musics = new Dictionary<string, AudioClip>();

	private GameMusic _currentGameMusic;

	private int _lastAudioIdPlayed;

	protected override void Init()
	{
		AudioClip[] array = Resources.LoadAll<AudioClip>("Music");
		Array.ForEach(array, delegate(AudioClip clip)
		{
			_musics.Add(clip.name, clip);
		});
	}

	public void StopMusic()
	{
		_currentGameMusic = GameMusic.None;
		SoundManager.StopAll();
	}

	public void PauseMusic()
	{
		SoundManager.PauseAllMusic();
	}

	public void ResumeMusic()
	{
		SoundManager.ResumeAllMusic();
	}

	public void PlayMusicMenu()
	{
		_currentGameMusic = GameMusic.None;
		List<string> list = new List<string>();
		list.Add("musicMenu");
		List<string> list2 = list;
		PlayMusic(list2[UnityEngine.Random.Range(0, list2.Count)]);
	}

	public void PlayMusicBattle()
	{
		PlayGameMusic(GameMusic.Battle);
	}

	public void PlayMusicBattleBoss()
	{
		PlayGameMusic(GameMusic.BattleBoss);
	}

	public void PlayBattleWin()
	{
		PlayGameMusic(GameMusic.BattleWin);
	}

	public void PlayBattleLost()
	{
		PlayGameMusic(GameMusic.BattleLost);
	}

	private void PlayGameMusic(GameMusic music)
	{
		if (_currentGameMusic != music)
		{
			switch (music)
			{
			case GameMusic.None:
				SoundManager.StopAll();
				break;
			case GameMusic.Battle:
				PlayMusic("musicBattle");
				break;
			case GameMusic.BattleBoss:
				PlayMusic("musicBattleBoss");
				break;
			case GameMusic.BattleWin:
				PlayJingle("musicWinBattle");
				break;
			case GameMusic.BattleLost:
				PlayJingle("musicBattleLost");
				break;
			}
			_currentGameMusic = music;
		}
	}

	private void PlayJingle(string assetName)
	{
 AudioClip value;		if (IsMusicOn() && _musics.TryGetValue(assetName, out value))
		{
			SoundManager.GetMusicAudio(_lastAudioIdPlayed)?.Stop();
			SoundManager.PlaySound(value);
		}
	}

	private void PlayMusic(string assetName, bool looping = true)
	{
		if (IsMusicOn())
		{
 AudioClip value;			if (_musics.TryGetValue(assetName, out value))
			{
				_lastAudioIdPlayed = SoundManager.PlayMusic(value, 1f, looping, true, 0.5f, 0.5f);
			}
			else
			{
				UnityEngine.Debug.LogWarning("Can't play music with filename of: " + assetName);
			}
		}
	}

	private bool IsMusicOn()
	{
		return App.Instance.Player.SettingsManager.MusicEnabled;
	}

	private void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			PauseMusic();
		}
		else
		{
			ResumeMusic();
		}
	}
}