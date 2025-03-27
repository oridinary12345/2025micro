using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIWorldPage : MonoBehaviour
{
	[SerializeField]
	private UINextWorldPopup _nextWorldPopup;

	[SerializeField]
	private UIGameButton _nextWorldButton;

	[SerializeField]
	private UIGameButton _questButton;

	[SerializeField]
	private Image _questImage;

	[SerializeField]
	private UIBadge _questBadge;

	[SerializeField]
	private TextMeshProUGUI _nextWorldUnlockCompletionText;

	[SerializeField]
	private UIBadge _nextWorldBadge;

	[SerializeField]
	private UIChestsPanel _chestPanel;

	[SerializeField]
	private UIWorldRenderer _worldRenderer;

	[SerializeField]
	private Transform _target;

	private WorldData _worldData;

	private UIQuestPopup _questPopup;

	private Action _nextWorldButtonClicked;

	public UIGameButton NextButton => _nextWorldButton;

	public string WorldId => _worldData.Config.Id;

	public float TargetOffsetY
	{
		get
		{
			Vector2 anchoredPosition = _target.GetComponent<RectTransform>().anchoredPosition;
			return anchoredPosition.y;
		}
	}

	public UIWorldPage Init(WorldData worldData, Action nextWorldButtonClicked)
	{
		_worldData = worldData;
		_nextWorldButtonClicked = nextWorldButtonClicked;
		string str = MonoSingleton<AdventureLevelConfigs>.Instance.GetMonsterIds(worldData.Config.Id)[0];
		_questImage.sprite = Resources.Load<Sprite>("Monsters/" + str);
		if (_questPopup == null)
		{
			_questPopup = UIQuestPopup.Create(_worldData.Config.Id);
		}
		if (!IsUnlocked())
		{
			_nextWorldPopup.Init(_worldData);
		}
		_nextWorldPopup.gameObject.SetActive(!IsUnlocked());
		_questButton.gameObject.SetActive(IsUnlocked());
		NextButton.OnClick(OnNextWorldButtonClicked);
		_questButton.OnClick(OnQuestButtonClicked);
		UpdateNextWorldButtonState();
		SetupBackground();
		_chestPanel.Init(App.Instance.Player.ChestManager);
		_chestPanel.gameObject.SetActive( false);
		UpdateNextWorldBadge();
		UpdateQuestBadge();
		App.Instance.Player.MonsterMissions.Events.MonsterRewardClaimedEvent += OnMonsterRewardClaimed;
		return this;
	}

	private void OnNextWorldButtonClicked()
	{
		_nextWorldBadge.SetVisible( false, false);
		if (_nextWorldButtonClicked != null)
		{
			_nextWorldButtonClicked();
		}
	}

	private void OnQuestButtonClicked()
	{
		_questBadge.SetVisible( false, false);
		_questPopup.Show();
	}

	private void UpdateQuestBadge()
	{
		bool isVisible = App.Instance.Player.MonsterMissions.CanClaimQuests(_worldData.Config.Id);
		_questBadge.SetVisible(isVisible, true);
	}

	public void OnPageEnter()
	{
		if (IsUnlocked())
		{
			_chestPanel.gameObject.SetActive( true);
		}
	}

	public void OnPageExit()
	{
		_chestPanel.gameObject.SetActive( false);
	}

	private void UpdateWorld(string worldId)
	{
	}

	private void UpdateNextWorldBadge()
	{
		_nextWorldBadge.SetVisible(CanShowNextWorldBadge(), true);
	}

	private bool CanShowNextWorldBadge()
	{
		return IsNextWorldLocked() && IsNextWorldReady();
	}

	private bool IsNextWorldReady()
	{
		WorldData nextLockedWorldData = App.Instance.Player.LevelManager.GetNextLockedWorldData();
		return nextLockedWorldData.Config.IsValid() && nextLockedWorldData.AreMissionsCompleted();
	}

	private void SetupBackground()
	{
		_worldRenderer.Init(_worldData.Config.Id);
	}

	public bool IsNextWorldLocked()
	{
		WorldData nextLockedWorldData = App.Instance.Player.LevelManager.GetNextLockedWorldData();
		string lastUnlockedWorldId = App.Instance.Player.LevelManager.LastUnlockedWorldId;
		return nextLockedWorldData.Config.IsValid() && _worldData.Config.Id == lastUnlockedWorldId;
	}

	public bool IsUnlocked()
	{
		return _worldData.IsUnlocked;
	}

	private void OnDestroy()
	{
		if (App.IsCreated())
		{
			App.Instance.Player.MonsterMissions.Events.MonsterRewardClaimedEvent -= OnMonsterRewardClaimed;
		}
	}

	private void OnMonsterRewardClaimed(MonsterMissionData data, List<Reward> rewards)
	{
		UpdateQuestBadge();
	}

	private void ShowNextWorldButton()
	{
		UpdateNextWorldButtonText();
		_nextWorldButton.gameObject.SetActive( true);
	}

	private void HideNextWorldButton()
	{
		_nextWorldButton.gameObject.SetActive( false);
	}

	private void UpdateNextWorldButtonText()
	{
		WorldData nextLockedWorldData = App.Instance.Player.LevelManager.GetNextLockedWorldData();
		if (nextLockedWorldData.Config.IsValid())
		{
			_nextWorldUnlockCompletionText.text = Mathf.FloorToInt(nextLockedWorldData.GetUnlockCompletion01() * 100f) + "%";
		}
	}

	private void UpdateNextWorldButtonState()
	{
		if (IsNextWorldLocked())
		{
			ShowNextWorldButton();
		}
		else
		{
			HideNextWorldButton();
		}
	}

	public void OnWorldUnlocked()
	{
		UpdateNextWorldButtonState();
		_nextWorldPopup.gameObject.SetActive( false);
		_chestPanel.gameObject.SetActive( true);
		_questButton.gameObject.SetActive( true);
	}

	public void UpdateNextWorldPanel()
	{
		_nextWorldPopup.UpdateMainBox();
	}
}