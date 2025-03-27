using UnityEngine;

namespace EazyTools.SoundManager
{
	public class Audio
	{
		public enum AudioType
		{
			Music,
			Sound,
			UISound
		}

		private static int audioCounter;

		private float volume;

		private float targetVolume;

		private float initTargetVolume;

		private float tempFadeSeconds;

		private float fadeInterpolater;

		private float onFadeStartVolume;

		private AudioType audioType;

		private AudioClip initClip;

		private Transform sourceTransform;

		public int audioID
		{
			get;
			private set;
		}

		public AudioSource audioSource
		{
			get;
			private set;
		}

		public AudioClip clip => (!(audioSource == null)) ? audioSource.clip : initClip;

		public bool loop
		{
			get;
			set;
		}

		public bool persist
		{
			get;
			set;
		}

		public float fadeInSeconds
		{
			get;
			set;
		}

		public float fadeOutSeconds
		{
			get;
			set;
		}

		public bool playing
		{
			get;
			set;
		}

		public bool paused
		{
			get;
			private set;
		}

		public bool stopping
		{
			get;
			private set;
		}

		public bool activated
		{
			get;
			private set;
		}

		public Audio(AudioType audioType, AudioClip clip, bool loop, bool persist, float volume, float fadeInValue, float fadeOutValue, Transform sourceTransform)
		{
			if (sourceTransform == null)
			{
				this.sourceTransform = SoundManager.gameobject.transform;
			}
			else
			{
				this.sourceTransform = sourceTransform;
			}
			audioID = audioCounter;
			audioCounter++;
			this.audioType = audioType;
			initClip = clip;
			this.loop = loop;
			this.persist = persist;
			targetVolume = volume;
			initTargetVolume = volume;
			tempFadeSeconds = -1f;
			this.volume = 0f;
			fadeInSeconds = fadeInValue;
			fadeOutSeconds = fadeOutValue;
			playing = false;
			paused = false;
			activated = false;
			CreateAudiosource(clip, loop);
			Play();
		}

		private void CreateAudiosource(AudioClip clip, bool loop)
		{
			audioSource = sourceTransform.gameObject.AddComponent<AudioSource>();
			audioSource.clip = clip;
			audioSource.loop = loop;
			audioSource.volume = 0f;
			if (sourceTransform != SoundManager.gameobject.transform)
			{
				audioSource.spatialBlend = 1f;
			}
		}

		public void Play()
		{
			Play(initTargetVolume);
		}

		public void Play(float volume)
		{
			if (audioSource == null)
			{
				CreateAudiosource(initClip, loop);
			}
			audioSource.Play();
			playing = true;
			fadeInterpolater = 0f;
			onFadeStartVolume = this.volume;
			targetVolume = volume;
		}

		public void Stop()
		{
			fadeInterpolater = 0f;
			onFadeStartVolume = volume;
			targetVolume = 0f;
			stopping = true;
		}

		public void Pause()
		{
			audioSource.Pause();
			paused = true;
		}

		public void Resume()
		{
			audioSource.UnPause();
			paused = false;
		}

		public void SetVolume(float volume)
		{
			if (volume > targetVolume)
			{
				SetVolume(volume, fadeOutSeconds);
			}
			else
			{
				SetVolume(volume, fadeInSeconds);
			}
		}

		public void SetVolume(float volume, float fadeSeconds)
		{
			SetVolume(volume, fadeSeconds, this.volume);
		}

		public void SetVolume(float volume, float fadeSeconds, float startVolume)
		{
			targetVolume = Mathf.Clamp01(volume);
			fadeInterpolater = 0f;
			onFadeStartVolume = startVolume;
			tempFadeSeconds = fadeSeconds;
		}

		public void Set3DMaxDistance(float max)
		{
			audioSource.maxDistance = max;
		}

		public void Set3DMinDistance(float min)
		{
			audioSource.minDistance = min;
		}

		public void Set3DDistances(float min, float max)
		{
			Set3DMinDistance(min);
			Set3DMaxDistance(max);
		}

		public void Update()
		{
			if (!(audioSource == null))
			{
				activated = true;
				if (volume != targetVolume)
				{
					fadeInterpolater += Time.deltaTime;
					float num = (!(volume > targetVolume)) ? ((tempFadeSeconds == -1f) ? fadeInSeconds : tempFadeSeconds) : ((tempFadeSeconds == -1f) ? fadeOutSeconds : tempFadeSeconds);
					volume = Mathf.Lerp(onFadeStartVolume, targetVolume, fadeInterpolater / num);
				}
				else if (tempFadeSeconds != -1f)
				{
					tempFadeSeconds = -1f;
				}
				switch (audioType)
				{
				case AudioType.Music:
					audioSource.volume = volume * SoundManager.globalMusicVolume * SoundManager.globalVolume;
					break;
				case AudioType.Sound:
					audioSource.volume = volume * SoundManager.globalSoundsVolume * SoundManager.globalVolume;
					break;
				case AudioType.UISound:
					audioSource.volume = volume * SoundManager.globalUISoundsVolume * SoundManager.globalVolume;
					break;
				}
				if (volume == 0f && stopping)
				{
					audioSource.Stop();
					stopping = false;
					playing = false;
					paused = false;
				}
				if (audioSource.isPlaying != playing)
				{
					playing = audioSource.isPlaying;
				}
			}
		}
	}
}