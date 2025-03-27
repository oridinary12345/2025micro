using EazyTools.SoundManager;
using UnityEngine;
using UnityEngine.UI;

public class EazySoundManagerDemo : MonoBehaviour
{
	public EazySoundDemoAudioControls[] AudioControls;

	public Slider globalVolSlider;

	public Slider globalMusicVolSlider;

	public Slider globalSoundVolSlider;

	private void Start()
	{
	}

	private void Update()
	{
		for (int i = 0; i < AudioControls.Length; i++)
		{
			EazySoundDemoAudioControls eazySoundDemoAudioControls = AudioControls[i];
			if (eazySoundDemoAudioControls.audio != null && eazySoundDemoAudioControls.audio.playing)
			{
				if (eazySoundDemoAudioControls.pauseButton != null)
				{
					eazySoundDemoAudioControls.playButton.interactable = false;
					eazySoundDemoAudioControls.pauseButton.interactable = true;
					eazySoundDemoAudioControls.stopButton.interactable = true;
					eazySoundDemoAudioControls.pausedStatusTxt.enabled = false;
				}
			}
			else if (eazySoundDemoAudioControls.audio != null && eazySoundDemoAudioControls.audio.paused)
			{
				if (eazySoundDemoAudioControls.pauseButton != null)
				{
					eazySoundDemoAudioControls.playButton.interactable = true;
					eazySoundDemoAudioControls.pauseButton.interactable = false;
					eazySoundDemoAudioControls.stopButton.interactable = false;
					eazySoundDemoAudioControls.pausedStatusTxt.enabled = true;
				}
			}
			else if (eazySoundDemoAudioControls.pauseButton != null)
			{
				eazySoundDemoAudioControls.playButton.interactable = true;
				eazySoundDemoAudioControls.pauseButton.interactable = false;
				eazySoundDemoAudioControls.stopButton.interactable = false;
				eazySoundDemoAudioControls.pausedStatusTxt.enabled = false;
			}
		}
	}

	public void PlayMusic1()
	{
		EazySoundDemoAudioControls eazySoundDemoAudioControls = AudioControls[0];
		if (eazySoundDemoAudioControls.audio != null && eazySoundDemoAudioControls.audio.paused)
		{
			eazySoundDemoAudioControls.audio.Resume();
			return;
		}
		int audioID = SoundManager.PlayMusic(eazySoundDemoAudioControls.audioclip, eazySoundDemoAudioControls.volumeSlider.value, true, false);
		AudioControls[0].audio = SoundManager.GetAudio(audioID);
	}

	public void PlayMusic2()
	{
		EazySoundDemoAudioControls eazySoundDemoAudioControls = AudioControls[1];
		if (eazySoundDemoAudioControls.audio != null && eazySoundDemoAudioControls.audio.paused)
		{
			eazySoundDemoAudioControls.audio.Resume();
			return;
		}
		int audioID = SoundManager.PlayMusic(eazySoundDemoAudioControls.audioclip, eazySoundDemoAudioControls.volumeSlider.value, false, false);
		AudioControls[1].audio = SoundManager.GetAudio(audioID);
	}

	public void PlaySound1()
	{
		EazySoundDemoAudioControls eazySoundDemoAudioControls = AudioControls[2];
		int audioID = SoundManager.PlaySound(eazySoundDemoAudioControls.audioclip, eazySoundDemoAudioControls.volumeSlider.value);
		AudioControls[2].audio = SoundManager.GetAudio(audioID);
	}

	public void PlaySound2()
	{
		EazySoundDemoAudioControls eazySoundDemoAudioControls = AudioControls[3];
		int audioID = SoundManager.PlaySound(eazySoundDemoAudioControls.audioclip, eazySoundDemoAudioControls.volumeSlider.value);
		AudioControls[3].audio = SoundManager.GetAudio(audioID);
	}

	public void Pause(string audioControlIDStr)
	{
		int num = int.Parse(audioControlIDStr);
		EazySoundDemoAudioControls eazySoundDemoAudioControls = AudioControls[num];
		eazySoundDemoAudioControls.audio.Pause();
	}

	public void Stop(string audioControlIDStr)
	{
		int num = int.Parse(audioControlIDStr);
		EazySoundDemoAudioControls eazySoundDemoAudioControls = AudioControls[num];
		eazySoundDemoAudioControls.audio.Stop();
	}

	public void AudioVolumeChanged(string audioControlIDStr)
	{
		int num = int.Parse(audioControlIDStr);
		EazySoundDemoAudioControls eazySoundDemoAudioControls = AudioControls[num];
		if (eazySoundDemoAudioControls.audio != null)
		{
			eazySoundDemoAudioControls.audio.SetVolume(eazySoundDemoAudioControls.volumeSlider.value, 0f);
		}
	}

	public void GlobalVolumeChanged()
	{
		SoundManager.globalVolume = globalVolSlider.value;
	}

	public void GlobalMusicVolumeChanged()
	{
		SoundManager.globalMusicVolume = globalMusicVolSlider.value;
	}

	public void GlobalSoundVolumeChanged()
	{
		SoundManager.globalSoundsVolume = globalSoundVolSlider.value;
	}
}