using UnityEngine;

namespace DigitalRuby.SoundManagerNamespace
{
	public class LoopingAudioSource
	{
		private float startVolume;

		private float startMultiplier;

		private float stopMultiplier;

		private float currentMultiplier;

		private float timestamp;

		private bool paused;

		public AudioSource AudioSource
		{
			get;
			private set;
		}

		public float TargetVolume
		{
			get;
			set;
		}

		public float OriginalTargetVolume
		{
			get;
			private set;
		}

		public bool Stopping
		{
			get;
			private set;
		}

		public bool Persist
		{
			get;
			private set;
		}

		public int Tag
		{
			get;
			set;
		}

		public LoopingAudioSource(AudioSource audioSource, float startMultiplier, float stopMultiplier, bool persist)
		{
			AudioSource = audioSource;
			if (audioSource != null)
			{
				AudioSource.loop = true;
				AudioSource.volume = 0f;
				AudioSource.Stop();
			}
			this.startMultiplier = (currentMultiplier = startMultiplier);
			this.stopMultiplier = stopMultiplier;
			Persist = persist;
		}

		public void Play(bool isMusic)
		{
			Play(1f, isMusic);
		}

		public bool Play(float targetVolume, bool isMusic)
		{
			if (AudioSource != null)
			{
				AudioSource.volume = (startVolume = ((!AudioSource.isPlaying) ? 0f : AudioSource.volume));
				AudioSource.loop = true;
				currentMultiplier = startMultiplier;
				OriginalTargetVolume = targetVolume;
				TargetVolume = targetVolume;
				Stopping = false;
				timestamp = 0f;
				if (!AudioSource.isPlaying)
				{
					AudioSource.Play();
					return true;
				}
			}
			return false;
		}

		public void Stop()
		{
			if (AudioSource != null && AudioSource.isPlaying && !Stopping)
			{
				startVolume = AudioSource.volume;
				TargetVolume = 0f;
				currentMultiplier = stopMultiplier;
				Stopping = true;
				timestamp = 0f;
			}
		}

		public void Pause()
		{
			if (AudioSource != null && !paused && AudioSource.isPlaying)
			{
				paused = true;
				AudioSource.Pause();
			}
		}

		public void Resume()
		{
			if (AudioSource != null && paused)
			{
				paused = false;
				AudioSource.UnPause();
			}
		}

		public bool Update()
		{
			if (AudioSource != null && AudioSource.isPlaying)
			{
				float num = Mathf.Lerp(startVolume, TargetVolume, (timestamp += Time.deltaTime) / currentMultiplier);
				AudioSource.volume = num;
				if (num == 0f && Stopping)
				{
					AudioSource.Stop();
					Stopping = false;
					return true;
				}
				return false;
			}
			return !paused;
		}
	}
}