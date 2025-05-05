using System;
using System.Collections.Generic;
using TMPro;
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
	private TextMeshProUGUI _questDescriptionText;

	[SerializeField]
	private GameObject _statusBox;

	[SerializeField]
	private Image _monsterImage;

	[SerializeField]
	private TextMeshProUGUI _rewardTitle;

	[SerializeField]
	private Image _rewardImage;

	[SerializeField]
	private TextMeshProUGUI _missionProgressText;

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
		_questStatusBoxLocked.SetActive(!IsUnlocked());
		_questStatusBoxDone.SetActive(IsRewardClaimed());
		_questStatusBoxDescription.SetActive(IsUnlocked() && !IsRewardClaimed());
		bool flag = IsMissionCompleted() && !IsRewardClaimed();
		_claimButton.gameObject.SetActive(flag);
		_statusBox.SetActive(!flag);
		_missionSlider.gameObject.SetActive(!IsRewardClaimed() && IsUnlocked() && !flag);
		UpdateMissionSlider();
		_questDescriptionText.text = _data.MissionDescription;
		_rewardImage.gameObject.SetActive(!IsRewardClaimed());
		_rewardTitle.gameObject.SetActive(!IsRewardClaimed());
	}

	private void OnClaimButtonClicked()
	{
		App.Instance.Player.MonsterMissions.ClaimMission(_data);
	}

	private bool IsUnlocked()
	{
		return App.Instance.Player.StatsManager.GetMonsterKillCount(_data.MonsterId) > 0;
	}

	private bool IsMissionCompleted()
	{
		return _data.Mission != null && _data.Mission.Completed;
	}

	private bool IsRewardClaimed()
	{
		return _data.Mission == null && _data.Profile.MissionsCompletedCount > 0;
	}

	private void UpdateMissionSlider()
	{
		_missionSlider.value = _data.MissionObjective01;
		_missionProgressText.text = _data.MissionProgress;
	}

	public void UpdateData(MonsterMissionData data)
	{
		_data = data;
		RefreshUI();
	}
}