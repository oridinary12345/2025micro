using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UILevelWavesBar : MonoBehaviour
{
	[SerializeField]
	private Slider _progressSlider;

	[SerializeField]
	private Image _backgroundImage;

	[SerializeField]
	private Image _progressSliderBackgroundImage;

	[SerializeField]
	private Image _progressSliderForegroundImage;

	[SerializeField]
	private Image _chestImage;

	private LevelData _level;

	public void Init(LevelData level)
	{
		_level = level;
		UpdateProgress();
		level.Events.WaveStartedEvent += OnWaveStarted;
		level.Events.WaveCompletedEvent += OnWaveCompleted;
	}

	private void UpdateProgress()
	{
		float previousValue = _progressSlider.value;
		float progress = _level.GetProgress01();
		_progressSlider.DOValue(progress, 0.3f).OnComplete(delegate
		{
			OnUpdateProgressDone(previousValue < 1f && _progressSlider.value >= 1f);
		});
	}

	private void OnUpdateProgressDone(bool beenFilledUp)
	{
		if (beenFilledUp)
		{
			Color colorStart = _progressSliderForegroundImage.color;
			_progressSliderForegroundImage.DOColor(Color.white, 0.5f).SetEase(Ease.Flash, 4f).OnComplete(delegate
			{
				_progressSliderForegroundImage.color = colorStart;
			});
			_chestImage.transform.DOPunchScale(Vector3.one * 0.75f, 0.25f).SetEase(Ease.InQuart);
		}
	}

	private void OnWaveStarted()
	{
		UpdateProgress();
	}

	private void OnWaveUpdated()
	{
	}

	private void OnWaveCompleted()
	{
	}
}