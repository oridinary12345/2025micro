using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UINotificationQuestCompleted : UINotificationPanel
{
	[SerializeField]
	private Image _monsterImage;

	[SerializeField]
	private Image _missionSliderFillImage;

	[SerializeField]
	private TextMeshProUGUI _text;

	[SerializeField]
	private Slider _slider;

	public void Init(string monsterId)
	{
		_text.gameObject.SetActive( false);
		_monsterImage.sprite = Resources.Load<Sprite>("Monsters/" + monsterId);
		_slider.value = 0.5f;
	}

	public override void OnShown()
	{
		_slider.DOValue(1f, 0.3f).SetDelay(0.4f).OnComplete(delegate
		{
			UINotificationQuestCompleted uINotificationQuestCompleted = this;
			_text.gameObject.SetActive( true);
			Color colorStart = _missionSliderFillImage.color;
			_missionSliderFillImage.DOColor(Color.white, 0.3f).SetEase(Ease.Flash, 6f).OnComplete(delegate
			{
				uINotificationQuestCompleted._missionSliderFillImage.color = colorStart;
			});
		});
	}
}