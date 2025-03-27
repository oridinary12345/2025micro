using DG.Tweening;
using System.Collections;
using UnityEngine;

public class UIMenuHudNotification : MonoBehaviour
{
	[SerializeField]
	private RectTransform _mainPanel;

	[SerializeField]
	private UINotificationNewWeapon _newWeapon;

	[SerializeField]
	private UINotificationQuestCompleted _questCompleted;

	private MonsterMissionManager _questManager;

	private float _widgetHeight;

	private float _defaultY;

	private Sequence _sequence;

	public void Init(MonsterMissionManager questManager)
	{
		_questManager = questManager;
		UnityCanvasContainer componentInChildren = base.transform.GetComponentInChildren<UnityCanvasContainer>();
		if (componentInChildren != null)
		{
			componentInChildren.Init();
		}
		Vector2 anchoredPosition = _mainPanel.anchoredPosition;
		_defaultY = anchoredPosition.y;
		_widgetHeight = _mainPanel.rect.height;
		questManager.Events.MonsterMissionCompletedEvent += OnMonsterMissionCompleted;
	}

	private void OnDestroy()
	{
		if (_questManager != null)
		{
			_questManager.Events.MonsterMissionCompletedEvent -= OnMonsterMissionCompleted;
		}
	}

	private void OnMonsterMissionCompleted(string monsterId)
	{
		_questCompleted.Init(monsterId);
		ShowNotification(_questCompleted);
	}

	private void ShowNotification(UINotificationPanel panel)
	{
		StartCoroutine(Animate(panel));
	}

	private IEnumerator Animate(UINotificationPanel panel)
	{
		while (_sequence != null)
		{
			yield return null;
		}
		_mainPanel.DOAnchorPosY(_widgetHeight, 0f);
		_mainPanel.gameObject.SetActive( true);
		panel.gameObject.SetActive( true);
		_sequence = DOTween.Sequence();
		_sequence.Append(_mainPanel.DOAnchorPosY(_defaultY, 0.25f).OnComplete(panel.OnShown));
		_sequence.Append(_mainPanel.DOAnchorPosY(_widgetHeight, 0.25f).SetDelay(2f));
		yield return _sequence.WaitForCompletion();
		_mainPanel.gameObject.SetActive( false);
		panel.gameObject.SetActive( false);
		_sequence = null;
	}
}