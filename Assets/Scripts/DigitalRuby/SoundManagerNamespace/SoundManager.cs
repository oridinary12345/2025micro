using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DigitalRuby.SoundManagerNamespace
{
	public class SoundManager : MonoBehaviour
	{
		private static int persistTag = 0;

		private static bool needsInitialize = true;

		private static GameObject root;

		private static SoundManager instance;

		private static readonly List<LoopingAudioSource> music = new List<LoopingAudioSource>();

		private static readonly List<AudioSource> musicOneShot = new List<AudioSource>();

		private static readonly List<LoopingAudioSource> sounds = new List<LoopingAudioSource>();

		private static readonly HashSet<LoopingAudioSource> persistedSounds = new HashSet<LoopingAudioSource>();

		private static readonly Dictionary<AudioClip, List<float>> soundsOneShot = new Dictionary<AudioClip, List<float>>();

		private static float soundVolume = 1f;

		private static float musicVolume = 1f;

		private static bool updated;

		private static bool pauseSoundsOnApplicationPause = true;

		public static int MaxDuplicateAudioClips = 4;

		public static float MusicVolume
		{
			get
			{
				return musicVolume;
			}
			set
			{
				if (value != musicVolume)
				{
					musicVolume = value;
					UpdateMusic();
				}
			}
		}

		public static float SoundVolume
		{
			get
			{
				return soundVolume;
			}
			set
			{
				if (value != soundVolume)
				{
					soundVolume = value;
					UpdateSounds();
				}
			}
		}

		public static bool PauseSoundsOnApplicationPause
		{
			get
			{
				return pauseSoundsOnApplicationPause;
			}
			set
			{
				pauseSoundsOnApplicationPause = value;
			}
		}

		private static void EnsureCreated()
		{
			if (needsInitialize)
			{
				needsInitialize = false;
				root = new GameObject();
				root.name = "DigitalRubySoundManager";
				root.hideFlags = HideFlags.HideAndDontSave;
				instance = root.AddComponent<SoundManager>();
				Object.DontDestroyOnLoad(root);
			}
		}

		private void StopLoopingListOnLevelLoad(IList<LoopingAudioSource> list)
		{
			for (int num = list.Count - 1; num >= 0; num--)
			{
				if (!list[num].Persist || !list[num].AudioSource.isPlaying)
				{
					list.RemoveAt(num);
				}
			}
		}

		private void ClearPersistedSounds()
		{
			foreach (LoopingAudioSource persistedSound in persistedSounds)
			{
				if (!persistedSound.AudioSource.isPlaying)
				{
					UnityEngine.Object.Destroy(persistedSound.AudioSource.gameObject);
				}
			}
			persistedSounds.Clear();
		}

		private void SceneManagerSceneLoaded(Scene s, LoadSceneMode m)
		{
			if (updated)
			{
				persistTag++;
				updated = false;
				StopNonLoopingSounds();
				StopLoopingListOnLevelLoad(sounds);
				soundsOneShot.Clear();
				ClearPersistedSounds();
			}
		}

		private void Start()
		{
			SceneManager.sceneLoaded += SceneManagerSceneLoaded;
		}

		private void Update()
		{
			updated = true;
			for (int num = sounds.Count - 1; num >= 0; num--)
			{
				if (sounds[num].Update())
				{
					sounds.RemoveAt(num);
				}
			}
			for (int num2 = music.Count - 1; num2 >= 0; num2--)
			{
				bool flag = music[num2] == null || music[num2].AudioSource == null;
				if (flag || music[num2].Update())
				{
					if (!flag && music[num2].Tag != persistTag)
					{
						UnityEngine.Debug.LogWarning("Destroying persisted audio from previous scene: " + music[num2].AudioSource.gameObject.name);
						UnityEngine.Object.Destroy(music[num2].AudioSource.gameObject);
					}
					music.RemoveAt(num2);
				}
			}
			for (int num3 = musicOneShot.Count - 1; num3 >= 0; num3--)
			{
				if (!musicOneShot[num3].isPlaying)
				{
					musicOneShot.RemoveAt(num3);
				}
			}
		}

		private void OnApplicationFocus(bool paused)
		{
			if (PauseSoundsOnApplicationPause)
			{
				if (paused)
				{
					ResumeAll();
				}
				else
				{
					PauseAll();
				}
			}
		}

		private static void UpdateSounds()
		{
			foreach (LoopingAudioSource sound in sounds)
			{
				sound.TargetVolume = sound.OriginalTargetVolume * soundVolume;
			}
		}

		private static void UpdateMusic()
		{
			foreach (LoopingAudioSource item in music)
			{
				if (!item.Stopping)
				{
					item.TargetVolume = item.OriginalTargetVolume * musicVolume;
				}
			}
			foreach (AudioSource item2 in musicOneShot)
			{
				item2.volume = musicVolume;
			}
		}

		private static IEnumerator RemoveVolumeFromClip(AudioClip clip, float volume)
		{
			yield return new WaitForSeconds(clip.length);
 List<float> volumes;			if (soundsOneShot.TryGetValue(clip, out volumes))
			{
				volumes.Remove(volume);
			}
		}

		private static void PlayLooping(AudioSource source, List<LoopingAudioSource> sources, float volumeScale, float fadeSeconds, bool persist, bool stopAll)
		{
			if (source == null)
			{
				return;
			}
			EnsureCreated();
			for (int num = sources.Count - 1; num >= 0; num--)
			{
				LoopingAudioSource loopingAudioSource = sources[num];
				if (loopingAudioSource.AudioSource == source)
				{
					sources.RemoveAt(num);
				}
				if (stopAll)
				{
					loopingAudioSource.Stop();
				}
			}
			source.gameObject.SetActive( true);
			LoopingAudioSource loopingAudioSource2 = new LoopingAudioSource(source, fadeSeconds, fadeSeconds, persist);
			loopingAudioSource2.Play(volumeScale, true);
			loopingAudioSource2.Tag = persistTag;
			sources.Add(loopingAudioSource2);
			if (persist)
			{
				if (!source.gameObject.name.StartsWith("PersistedBySoundManager-"))
				{
					source.gameObject.name = "PersistedBySoundManager-" + source.gameObject.name + "-" + source.gameObject.GetInstanceID();
				}
				source.gameObject.transform.parent = null;
				Object.DontDestroyOnLoad(source.gameObject);
				persistedSounds.Add(loopingAudioSource2);
			}
		}

		private static void StopLooping(AudioSource source, List<LoopingAudioSource> sources)
		{
			foreach (LoopingAudioSource source2 in sources)
			{
				if (source2.AudioSource == source)
				{
					source2.Stop();
					source = null;
					break;
				}
			}
			if (source != null)
			{
				source.Stop();
			}
		}

		public static void PlayOneShotSound(AudioSource source, AudioClip clip)
		{
			PlayOneShotSound(source, clip, 1f);
		}

		public static void PlayOneShotSound(AudioSource source, AudioClip clip, float volumeScale)
		{
			EnsureCreated();
 List<float> value;			if (!soundsOneShot.TryGetValue(clip, out value))
			{
				value = new List<float>();
				soundsOneShot[clip] = value;
			}
			else if (value.Count == MaxDuplicateAudioClips)
			{
				return;
			}
			float num = float.MaxValue;
			float num2 = float.MinValue;
			foreach (float item in value)
			{
				float b = item;
				num = Mathf.Min(num, b);
				num2 = Mathf.Max(num2, b);
			}
			float num3 = volumeScale * soundVolume;
			if (num2 > 0.5f)
			{
				num3 = (num + num2) / (float)(value.Count + 2);
			}
			value.Add(num3);
			source.PlayOneShot(clip, num3);
			instance.StartCoroutine(RemoveVolumeFromClip(clip, num3));
		}

		public static void PlayLoopingSound(AudioSource source)
		{
			PlayLoopingSound(source, 1f, 1f);
		}

		public static void PlayLoopingSound(AudioSource source, float volumeScale, float fadeSeconds)
		{
			PlayLooping(source, sounds, volumeScale, fadeSeconds, false, false);
			UpdateSounds();
		}

		public static void PlayOneShotMusic(AudioSource source, AudioClip clip)
		{
			PlayOneShotMusic(source, clip, 1f);
		}

		public static void PlayOneShotMusic(AudioSource source, AudioClip clip, float volumeScale)
		{
			EnsureCreated();
			int num = musicOneShot.IndexOf(source);
			if (num >= 0)
			{
				musicOneShot.RemoveAt(num);
			}
			source.PlayOneShot(clip, volumeScale * musicVolume);
			musicOneShot.Add(source);
		}

		public static void PlayLoopingMusic(AudioSource source)
		{
			PlayLoopingMusic(source, 1f, 1f, false);
		}

		public static void PlayLoopingMusic(AudioSource source, float volumeScale, float fadeSeconds, bool persist)
		{
			PlayLooping(source, music, volumeScale, fadeSeconds, persist, true);
			UpdateMusic();
		}

		public static void StopLoopingSound(AudioSource source)
		{
			StopLooping(source, sounds);
		}

		public static void StopLoopingMusic(AudioSource source)
		{
			StopLooping(source, music);
		}

		public static void StopAll()
		{
			StopAllLoopingSounds();
			StopNonLoopingSounds();
		}

		public static void StopAllLoopingSounds()
		{
			foreach (LoopingAudioSource sound in sounds)
			{
				sound.Stop();
			}
			foreach (LoopingAudioSource item in music)
			{
				item.Stop();
			}
		}

		public static void StopNonLoopingSounds()
		{
			foreach (AudioSource item in musicOneShot)
			{
				item.Stop();
			}
		}

		public static void PauseAll()
		{
			foreach (LoopingAudioSource sound in sounds)
			{
				sound.Pause();
			}
			foreach (LoopingAudioSource item in music)
			{
				item.Pause();
			}
		}

		public static void ResumeAll()
		{
			foreach (LoopingAudioSource sound in sounds)
			{
				sound.Resume();
			}
			foreach (LoopingAudioSource item in music)
			{
				item.Resume();
			}
		}
	}
}