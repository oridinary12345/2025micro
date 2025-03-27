using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DigitalRuby.SoundManagerNamespace
{
	public class SoundManagerDemo : MonoBehaviour
	{
		public Slider SoundSlider;

		public Slider MusicSlider;

		public InputField SoundCountTextBox;

		public Toggle PersistToggle;

		public AudioSource[] SoundAudioSources;

		public AudioSource[] MusicAudioSources;

		private void PlaySound(int index)
		{
 int result;			if (!int.TryParse(SoundCountTextBox.text, out result))
			{
				result = 1;
			}
			while (result-- > 0)
			{
				SoundAudioSources[index].PlayOneShotSoundManaged(SoundAudioSources[index].clip);
			}
		}

		private void PlayMusic(int index)
		{
			MusicAudioSources[index].PlayLoopingMusicManaged(1f, 1f, PersistToggle.isOn);
		}

		private void CheckPlayKey()
		{
			if (SoundCountTextBox.isFocused)
			{
				return;
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha1))
			{
				PlaySound(0);
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha2))
			{
				PlaySound(1);
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha3))
			{
				PlaySound(2);
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha4))
			{
				PlaySound(3);
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha5))
			{
				PlaySound(4);
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha6))
			{
				PlaySound(5);
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha7))
			{
				PlaySound(6);
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha8))
			{
				PlayMusic(0);
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha9))
			{
				PlayMusic(1);
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha0))
			{
				PlayMusic(2);
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.A))
			{
				PlayMusic(3);
			}
			if (UnityEngine.Input.GetKeyDown(KeyCode.R))
			{
				UnityEngine.Debug.LogWarning("Reloading level");
				if (!PersistToggle.isOn)
				{
					SoundManager.StopAll();
				}
				SceneManager.LoadScene(0, LoadSceneMode.Single);
			}
		}

		private void Start()
		{
		}

		private void Update()
		{
			CheckPlayKey();
		}

		public void SoundVolumeChanged()
		{
			SoundManager.SoundVolume = SoundSlider.value;
		}

		public void MusicVolumeChanged()
		{
			SoundManager.MusicVolume = MusicSlider.value;
		}
	}
}