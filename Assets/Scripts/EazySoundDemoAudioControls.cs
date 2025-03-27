using EazyTools.SoundManager;
using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct EazySoundDemoAudioControls
{
	public AudioClip audioclip;

	public Audio audio;

	public Button playButton;

	public Button pauseButton;

	public Button stopButton;

	public Slider volumeSlider;

	public Text pausedStatusTxt;
}