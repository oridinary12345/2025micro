using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoSingleton<AudioManager>
{
	private const int AudioSourceMax = 10;

	private const float LowPitchRange = 0.95f;

	private const float HightPitchRange = 1.05f;

	private List<AudioSource> _audioSources;

	private int _lastAssignSourceIndex;

	public void PlaySound(AudioClip clip, float pitch = 1f)
	{
		if (!(clip == null) && App.Instance.Player.SettingsManager.SoundEnabled)
		{
			AudioSource audioSource = GetAudioSource();
			audioSource.clip = clip;
			audioSource.loop = false;
			audioSource.volume = 1f;
			audioSource.time = 0f;
			audioSource.playOnAwake = false;
			audioSource.pitch = pitch;
			audioSource.Play();
		}
	}

	public void StopAllSounds()
	{
		StartCoroutine(StopAllSoundsCR());
	}

	private IEnumerator StopAllSoundsCR()
	{
		float timer = 0f;
		while (timer < 1.5f)
		{
			for (int i = 0; i < _audioSources.Count; i++)
			{
				_audioSources[i].volume = Mathfx.Lerp(1f, 0f, timer / 1.5f);
			}
			timer += Time.deltaTime;
			yield return null;
		}
		for (int j = 0; j < _audioSources.Count; j++)
		{
			_audioSources[j].Stop();
		}
	}

	protected override void Init()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		_audioSources = new List<AudioSource>(10);
		for (int i = 0; i < 10; i++)
		{
			GameObject gameObject = new GameObject();
			gameObject.transform.parent = base.transform;
			_audioSources.Add(gameObject.AddComponent<AudioSource>());
		}
	}

	public AudioSource GetAudioSource()
	{
		int num = 10;
		AudioSource audioSource;
		do
		{
			num--;
			int lastAssignSourceIndex = _lastAssignSourceIndex;
			_lastAssignSourceIndex++;
			_lastAssignSourceIndex %= 10;
			audioSource = _audioSources[lastAssignSourceIndex];
		}
		while (audioSource.isPlaying && num > 0);
		return audioSource;
	}
}