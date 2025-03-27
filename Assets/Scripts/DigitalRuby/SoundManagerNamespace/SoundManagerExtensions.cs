using UnityEngine;

namespace DigitalRuby.SoundManagerNamespace
{
	public static class SoundManagerExtensions
	{
		public static void PlayOneShotSoundManaged(this AudioSource source, AudioClip clip)
		{
			SoundManager.PlayOneShotSound(source, clip, 1f);
		}

		public static void PlayOneShotSoundManaged(this AudioSource source, AudioClip clip, float volumeScale)
		{
			SoundManager.PlayOneShotSound(source, clip, volumeScale);
		}

		public static void PlayOneShotMusicManaged(this AudioSource source, AudioClip clip)
		{
			SoundManager.PlayOneShotMusic(source, clip, 1f);
		}

		public static void PlayOneShotMusicManaged(this AudioSource source, AudioClip clip, float volumeScale)
		{
			SoundManager.PlayOneShotMusic(source, clip, volumeScale);
		}

		public static void PlayLoopingSoundManaged(this AudioSource source)
		{
			SoundManager.PlayLoopingSound(source, 1f, 1f);
		}

		public static void PlayLoopingSoundManaged(this AudioSource source, float volumeScale, float fadeSeconds)
		{
			SoundManager.PlayLoopingSound(source, volumeScale, fadeSeconds);
		}

		public static void PlayLoopingMusicManaged(this AudioSource source)
		{
			SoundManager.PlayLoopingMusic(source, 1f, 1f, false);
		}

		public static void PlayLoopingMusicManaged(this AudioSource source, float volumeScale, float fadeSeconds, bool persist)
		{
			SoundManager.PlayLoopingMusic(source, volumeScale, fadeSeconds, persist);
		}

		public static void StopLoopingSoundManaged(this AudioSource source)
		{
			SoundManager.StopLoopingSound(source);
		}

		public static void StopLoopingMusicManaged(this AudioSource source)
		{
			SoundManager.StopLoopingMusic(source);
		}
	}
}