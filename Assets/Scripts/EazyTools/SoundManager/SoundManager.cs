using System.Collections.Generic;
using UnityEngine;

namespace EazyTools.SoundManager
{
	public class SoundManager : MonoBehaviour
	{
		private static SoundManager _instance = null;

		private static float vol = 1f;

		private static float musicVol = 1f;

		private static float soundsVol = 1f;

		private static float UISoundsVol = 1f;

		private static Dictionary<int, Audio> musicAudio = new Dictionary<int, Audio>();

		private static Dictionary<int, Audio> soundsAudio = new Dictionary<int, Audio>();

		private static Dictionary<int, Audio> UISoundsAudio = new Dictionary<int, Audio>();

		private static bool initialized = false;

		private List<int> _keysToDelete = new List<int>();

		private static SoundManager instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = (SoundManager)UnityEngine.Object.FindObjectOfType(typeof(SoundManager));
					if (_instance == null)
					{
						_instance = new GameObject("EazySoundManager").AddComponent<SoundManager>();
					}
				}
				return _instance;
			}
		}

		public static GameObject gameobject => instance.gameObject;

		public static bool ignoreDuplicateMusic
		{
			get;
			set;
		}

		public static bool ignoreDuplicateSounds
		{
			get;
			set;
		}

		public static bool ignoreDuplicateUISounds
		{
			get;
			set;
		}

		public static float globalVolume
		{
			get
			{
				return vol;
			}
			set
			{
				vol = value;
			}
		}

		public static float globalMusicVolume
		{
			get
			{
				return musicVol;
			}
			set
			{
				musicVol = value;
			}
		}

		public static float globalSoundsVolume
		{
			get
			{
				return soundsVol;
			}
			set
			{
				soundsVol = value;
			}
		}

		public static float globalUISoundsVolume
		{
			get
			{
				return UISoundsVol;
			}
			set
			{
				UISoundsVol = value;
			}
		}

		private void Awake()
		{
			instance.Init();
		}

		private void Update()
		{
			foreach (KeyValuePair<int, Audio> item in musicAudio)
			{
				Audio value = item.Value;
				value.Update();
				if (!value.playing && !value.paused)
				{
					_keysToDelete.Add(item.Key);
				}
			}
			foreach (int item2 in _keysToDelete)
			{
				UnityEngine.Object.Destroy(musicAudio[item2].audioSource);
				musicAudio.Remove(item2);
			}
			_keysToDelete.Clear();
			foreach (KeyValuePair<int, Audio> item3 in soundsAudio)
			{
				Audio value2 = item3.Value;
				value2.Update();
				if (!value2.playing && !value2.paused)
				{
					_keysToDelete.Add(item3.Key);
				}
			}
			foreach (int item4 in _keysToDelete)
			{
				UnityEngine.Object.Destroy(soundsAudio[item4].audioSource);
				soundsAudio.Remove(item4);
			}
			_keysToDelete.Clear();
			foreach (KeyValuePair<int, Audio> item5 in UISoundsAudio)
			{
				Audio value3 = item5.Value;
				value3.Update();
				if (!value3.playing && !value3.paused)
				{
					_keysToDelete.Add(item5.Key);
				}
			}
			foreach (int item6 in _keysToDelete)
			{
				UnityEngine.Object.Destroy(UISoundsAudio[item6].audioSource);
				UISoundsAudio.Remove(item6);
			}
			_keysToDelete.Clear();
		}

		private void Init()
		{
			if (!initialized)
			{
				ignoreDuplicateMusic = false;
				ignoreDuplicateSounds = false;
				ignoreDuplicateUISounds = false;
				initialized = true;
				Object.DontDestroyOnLoad(this);
			}
		}

		public static Audio GetAudio(int audioID)
		{
			Audio audio = GetMusicAudio(audioID);
			if (audio != null)
			{
				return audio;
			}
			audio = GetSoundAudio(audioID);
			if (audio != null)
			{
				return audio;
			}
			audio = GetUISoundAudio(audioID);
			if (audio != null)
			{
				return audio;
			}
			return null;
		}

		public static Audio GetAudio(AudioClip audioClip)
		{
			Audio audio = GetMusicAudio(audioClip);
			if (audio != null)
			{
				return audio;
			}
			audio = GetSoundAudio(audioClip);
			if (audio != null)
			{
				return audio;
			}
			audio = GetUISoundAudio(audioClip);
			if (audio != null)
			{
				return audio;
			}
			return null;
		}

		public static Audio GetMusicAudio(int audioID)
		{
			List<int> list = new List<int>(musicAudio.Keys);
			foreach (int item in list)
			{
				if (audioID == item)
				{
					return musicAudio[item];
				}
			}
			return null;
		}

		public static Audio GetMusicAudio(AudioClip audioClip)
		{
			List<int> list = new List<int>(musicAudio.Keys);
			foreach (int item in list)
			{
				Audio audio = musicAudio[item];
				if (audio.clip == audioClip)
				{
					return audio;
				}
			}
			return null;
		}

		public static Audio GetSoundAudio(int audioID)
		{
			List<int> list = new List<int>(soundsAudio.Keys);
			foreach (int item in list)
			{
				if (audioID == item)
				{
					return soundsAudio[item];
				}
			}
			return null;
		}

		public static Audio GetSoundAudio(AudioClip audioClip)
		{
			List<int> list = new List<int>(soundsAudio.Keys);
			foreach (int item in list)
			{
				Audio audio = soundsAudio[item];
				if (audio.clip == audioClip)
				{
					return audio;
				}
			}
			return null;
		}

		public static Audio GetUISoundAudio(int audioID)
		{
			List<int> list = new List<int>(UISoundsAudio.Keys);
			foreach (int item in list)
			{
				if (audioID == item)
				{
					return UISoundsAudio[item];
				}
			}
			return null;
		}

		public static Audio GetUISoundAudio(AudioClip audioClip)
		{
			List<int> list = new List<int>(UISoundsAudio.Keys);
			foreach (int item in list)
			{
				Audio audio = UISoundsAudio[item];
				if (audio.clip == audioClip)
				{
					return audio;
				}
			}
			return null;
		}

		public static int PlayMusic(AudioClip clip)
		{
			return PlayMusic(clip, 1f, false, false, 1f, 1f, -1f, null);
		}

		public static int PlayMusic(AudioClip clip, float volume)
		{
			return PlayMusic(clip, volume, false, false, 1f, 1f, -1f, null);
		}

		public static int PlayMusic(AudioClip clip, float volume, bool loop, bool persist)
		{
			return PlayMusic(clip, volume, loop, persist, 1f, 1f, -1f, null);
		}

		public static int PlayMusic(AudioClip clip, float volume, bool loop, bool persist, float fadeInSeconds, float fadeOutSeconds)
		{
			return PlayMusic(clip, volume, loop, persist, fadeInSeconds, fadeOutSeconds, -1f, null);
		}

		public static int PlayMusic(AudioClip clip, float volume, bool loop, bool persist, float fadeInSeconds, float fadeOutSeconds, float currentMusicfadeOutSeconds, Transform sourceTransform)
		{
			if (clip == null)
			{
				UnityEngine.Debug.LogError("Sound Manager: Audio clip is null, cannot play music", clip);
			}
			if (ignoreDuplicateMusic)
			{
				List<int> list = new List<int>(musicAudio.Keys);
				foreach (int item in list)
				{
					if (musicAudio[item].audioSource.clip == clip)
					{
						return musicAudio[item].audioID;
					}
				}
			}
			instance.Init();
			StopAllMusic(currentMusicfadeOutSeconds);
			Audio audio = new Audio(Audio.AudioType.Music, clip, loop, persist, volume, fadeInSeconds, fadeOutSeconds, sourceTransform);
			musicAudio.Add(audio.audioID, audio);
			return audio.audioID;
		}

		public static int PlaySound(AudioClip clip)
		{
			return PlaySound(clip, 1f, false, null);
		}

		public static int PlaySound(AudioClip clip, float volume)
		{
			return PlaySound(clip, volume, false, null);
		}

		public static int PlaySound(AudioClip clip, bool loop)
		{
			return PlaySound(clip, 1f, loop, null);
		}

		public static int PlaySound(AudioClip clip, float volume, bool loop, Transform sourceTransform)
		{
			if (clip == null)
			{
				UnityEngine.Debug.LogError("Sound Manager: Audio clip is null, cannot play music", clip);
			}
			if (ignoreDuplicateSounds)
			{
				List<int> list = new List<int>(soundsAudio.Keys);
				foreach (int item in list)
				{
					if (soundsAudio[item].audioSource.clip == clip)
					{
						return soundsAudio[item].audioID;
					}
				}
			}
			instance.Init();
			Audio audio = new Audio(Audio.AudioType.Sound, clip, loop, false, volume, 0f, 0f, sourceTransform);
			soundsAudio.Add(audio.audioID, audio);
			return audio.audioID;
		}

		public static int PlayUISound(AudioClip clip)
		{
			return PlayUISound(clip, 1f);
		}

		public static int PlayUISound(AudioClip clip, float volume)
		{
			if (clip == null)
			{
				UnityEngine.Debug.LogError("Sound Manager: Audio clip is null, cannot play music", clip);
			}
			if (ignoreDuplicateUISounds)
			{
				List<int> list = new List<int>(UISoundsAudio.Keys);
				foreach (int item in list)
				{
					if (UISoundsAudio[item].audioSource.clip == clip)
					{
						return UISoundsAudio[item].audioID;
					}
				}
			}
			instance.Init();
			Audio audio = new Audio(Audio.AudioType.UISound, clip, false, false, volume, 0f, 0f, null);
			UISoundsAudio.Add(audio.audioID, audio);
			return audio.audioID;
		}

		public static void StopAll()
		{
			StopAll(-1f);
		}

		public static void StopAll(float fadeOutSeconds)
		{
			StopAllMusic(fadeOutSeconds);
			StopAllSounds();
			StopAllUISounds();
		}

		public static void StopAllMusic()
		{
			StopAllMusic(-1f);
		}

		public static void StopAllMusic(float fadeOutSeconds)
		{
			List<int> list = new List<int>(musicAudio.Keys);
			foreach (int item in list)
			{
				Audio audio = musicAudio[item];
				if (fadeOutSeconds > 0f)
				{
					audio.fadeOutSeconds = fadeOutSeconds;
				}
				audio.Stop();
			}
		}

		public static void StopAllSounds()
		{
			List<int> list = new List<int>(soundsAudio.Keys);
			foreach (int item in list)
			{
				Audio audio = soundsAudio[item];
				audio.Stop();
			}
		}

		public static void StopAllUISounds()
		{
			List<int> list = new List<int>(UISoundsAudio.Keys);
			foreach (int item in list)
			{
				Audio audio = UISoundsAudio[item];
				audio.Stop();
			}
		}

		public static void PauseAll()
		{
			PauseAllMusic();
			PauseAllSounds();
			PauseAllUISounds();
		}

		public static void PauseAllMusic()
		{
			List<int> list = new List<int>(musicAudio.Keys);
			foreach (int item in list)
			{
				Audio audio = musicAudio[item];
				audio.Pause();
			}
		}

		public static void PauseAllSounds()
		{
			List<int> list = new List<int>(soundsAudio.Keys);
			foreach (int item in list)
			{
				Audio audio = soundsAudio[item];
				audio.Pause();
			}
		}

		public static void PauseAllUISounds()
		{
			List<int> list = new List<int>(UISoundsAudio.Keys);
			foreach (int item in list)
			{
				Audio audio = UISoundsAudio[item];
				audio.Pause();
			}
		}

		public static void ResumeAll()
		{
			ResumeAllMusic();
			ResumeAllSounds();
			ResumeAllUISounds();
		}

		public static void ResumeAllMusic()
		{
			if (musicAudio != null)
			{
				List<int> list = new List<int>(musicAudio.Keys);
				foreach (int item in list)
				{
					Audio audio = musicAudio[item];
					audio.Resume();
				}
			}
		}

		public static void ResumeAllSounds()
		{
			if (soundsAudio != null)
			{
				List<int> list = new List<int>(soundsAudio.Keys);
				foreach (int item in list)
				{
					Audio audio = soundsAudio[item];
					audio.Resume();
				}
			}
		}

		public static void ResumeAllUISounds()
		{
			if (UISoundsAudio != null)
			{
				List<int> list = new List<int>(UISoundsAudio.Keys);
				foreach (int item in list)
				{
					Audio audio = UISoundsAudio[item];
					audio.Resume();
				}
			}
		}
	}
}