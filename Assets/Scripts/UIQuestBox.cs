using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQuestBox : MonoBehaviour
{
	[SerializeField]
	private GameObject _questStatusBoxDone;

	[SerializeField]
	private GameObject _questStatusBoxLocked;

	[SerializeField]
	private GameObject _questStatusBoxDescription;

	[SerializeField]
	private Text _questDescriptionText;

	[SerializeField]
	private GameObject _statusBox;

	[SerializeField]
	private Image _monsterImage;

	[SerializeField]
	private Text _rewardTitle;

	[SerializeField]
	private Image _rewardImage;

	[SerializeField]
	private Text _missionProgressText;

	[SerializeField]
	private Slider _missionSlider;

	[SerializeField]
	private UIGameButton _claimButton;

	[SerializeField]
	private UIGameButton _chestButton;

	[SerializeField]
	private RectTransform _rewardInfoPivot;

	private MonsterMisionEvents _events;

	private MonsterMissionData _data;

	public RectTransform RewardInfoPivot => _rewardInfoPivot;

	public static UIQuestBox Create(MonsterMisionEvents events, MonsterMissionData data, Action<RectTransform, string> onChestClicked)
	{
		UIQuestBox uIQuestBox = UnityEngine.Object.Instantiate(Resources.Load<UIQuestBox>("UI/MonsterQuestBox"));
		return uIQuestBox.Init(events, data, onChestClicked);
	}

	private UIQuestBox Init(MonsterMisionEvents events, MonsterMissionData data, Action<RectTransform, string> onChestClicked)
	{
		if (data == null)
		{
			Debug.LogError("MonsterMissionData is null in UIQuestBox.Init");
			return this;
		}

		_data = data;
		_events = events;

		if (_monsterImage == null)
		{
			Debug.LogError("_monsterImage is null in UIQuestBox");
			return this;
		}

		string monsterId = data.MonsterId;
		if (string.IsNullOrEmpty(monsterId))
		{
			Debug.LogError("MonsterId is null or empty in MonsterMissionData");
			return this;
		}

		Sprite monsterSprite = Resources.Load<Sprite>("Monsters/" + monsterId);
		if (monsterSprite == null)
		{
			Debug.LogError($"Could not load sprite for monster: Monsters/{monsterId}");
			return this;
		}

		_monsterImage.sprite = monsterSprite;
		_monsterImage.color = ((!IsUnlocked()) ? new Color(0f, 0f, 0f, 0.5f) : Color.white);

		if (_rewardTitle != null) _rewardTitle.enabled = IsUnlocked();
		if (_rewardImage != null) _rewardImage.enabled = IsUnlocked();
		if (_chestButton != null && _rewardInfoPivot != null)
		{
			_chestButton.OnClick(delegate
			{
				onChestClicked(_rewardInfoPivot, monsterId);
			});
		}

		RefreshUI();
		if (_claimButton != null) _claimButton.OnClick(OnClaimButtonClicked);
		if (_events != null) _events.MonsterRewardClaimedEvent += MonsterRewardClaimedEvent;

		return this;
	}

	private void OnDestroy()
	{
		if (_events != null)
		{
			_events.MonsterRewardClaimedEvent -= MonsterRewardClaimedEvent;
		}
	}

	private void MonsterRewardClaimedEvent(MonsterMissionData data, List<Reward> rewards)
	{
		if (data == _data)
		{
			RefreshUI();
		}
	}

	private void RefreshUI()
	{
		if (_data == null)
		{
			Debug.LogWarning("Data is null in UIQuestBox.RefreshUI");
			return;
		}

		if (_questStatusBoxLocked != null)
			_questStatusBoxLocked.SetActive(!IsUnlocked());

		if (_questStatusBoxDone != null)
			_questStatusBoxDone.SetActive(IsRewardClaimed());

		if (_questStatusBoxDescription != null)
			_questStatusBoxDescription.SetActive(IsUnlocked() && !IsRewardClaimed());

		bool flag = IsMissionCompleted() && !IsRewardClaimed();

		if (_claimButton != null && _claimButton.gameObject != null)
			_claimButton.gameObject.SetActive(flag);

		if (_statusBox != null)
			_statusBox.SetActive(!flag);

		if (_missionSlider != null && _missionSlider.gameObject != null)
			_missionSlider.gameObject.SetActive(!IsRewardClaimed() && IsUnlocked() && !flag);

		UpdateMissionSlider();

		if (_questDescriptionText != null)
			_questDescriptionText.text = _data.MissionDescription;

		if (_rewardImage != null && _rewardImage.gameObject != null)
			_rewardImage.gameObject.SetActive(!IsRewardClaimed());

		if (_rewardTitle != null && _rewardTitle.gameObject != null)
			_rewardTitle.gameObject.SetActive(!IsRewardClaimed());
	}

	private void OnClaimButtonClicked()
	{
		if (App.Instance?.Player?.MonsterMissions != null && _data != null)
		{
			App.Instance.Player.MonsterMissions.ClaimMission(_data);
		}
		else
		{
			Debug.LogWarning("Cannot claim mission: App.Instance, Player, MonsterMissions, or _data is null");
		}
	}

	private bool IsUnlocked()
	{
		if (App.Instance?.Player?.StatsManager != null && _data != null)
		{
			return App.Instance.Player.StatsManager.GetMonsterKillCount(_data.MonsterId) > 0;
		}
		Debug.LogWarning("Cannot check if unlocked: App.Instance, Player, StatsManager, or _data is null");
		return false;
	}

	private bool IsMissionCompleted()
	{
		if (_data?.Mission != null)
		{
			return _data.Mission.Completed;
		}
		Debug.LogWarning("Cannot check if mission is completed: _data or Mission is null");
		return false;
	}

	private bool IsRewardClaimed()
	{
		if (_data?.Profile != null)
		{
			return _data.Mission == null && _data.Profile.MissionsCompletedCount > 0;
		}
		Debug.LogWarning("Cannot check if reward is claimed: _data or Profile is null");
		return false;
	}

	private void UpdateMissionSlider()
	{
		if (_data == null)
		{
			Debug.LogWarning("Cannot update mission slider: _data is null");
			return;
		}

		if (_missionSlider != null)
		{
			_missionSlider.value = _data.MissionObjective01;
		}

		if (_missionProgressText != null)
		{
			if (!string.IsNullOrEmpty(_data.MissionProgress))
			{
				_missionProgressText.text = _data.MissionProgress;
			}
			else
			{
				_missionProgressText.text = "0/0";
				Debug.LogWarning("Mission progress text is null or empty");
			}
		}
	}

	public void UpdateData(MonsterMissionData data)
	{
		if (data == null)
		{
			Debug.LogWarning("Cannot update data: new data is null");
			return;
		}

		_data = data;
		RefreshUI();
	}
}