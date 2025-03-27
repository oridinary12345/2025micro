using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIQuestPopup : UIMenuPopup
{
	[SerializeField]
	private TextMeshProUGUI _newQuestTimer;

	[SerializeField]
	private UIGameButton _buttonClose;

	[SerializeField]
	private RectTransform _content;

	private UIRewardsController _menuRewardController;

	private Dictionary<string, UIQuestBox> _boxes = new Dictionary<string, UIQuestBox>();

	private string _worldId;

	[SerializeField]
	private TextMeshProUGUI _coinAmountText;

	[SerializeField]
	private TextMeshProUGUI _cardsAmountText;

	[SerializeField]
	private UIGameButton _rewardInfoButton;

	[SerializeField]
	private RectTransform _rewardInfo;

	private Coroutine _rewardPanelCR;

	private Coroutine _timerCR;

	private readonly WaitForSeconds wait = new WaitForSeconds(0.4f);

	public static UIQuestPopup Create(string worldId)
	{
		UIQuestPopup uIQuestPopup = Object.Instantiate(Resources.Load<UIQuestPopup>("UI/UIQuestPopup"));
		uIQuestPopup.GetComponent<RectTransform>().SetParent(Object.FindObjectOfType<UIAppCanvas>().transform, false);
		uIQuestPopup.Init(worldId);
		return uIQuestPopup;
	}

	protected override void Awake()
	{
		base.Awake();
		_buttonClose.OnClick(OnCloseButtonClicked);
		_buttonClose.ActivateOnBackKey();
		_buttonClose.Animate = false;
		_menuRewardController = UIRewardsController.Create();
		_rewardInfoButton.OnClick(HideRewardInfo);
		_newQuestTimer.text = "-";
		MonsterMisionEvents events = App.Instance.Player.MonsterMissions.Events;
		events.MonsterRewardClaimedEvent += OnMonsterRewardClaimed;
		events.MonsterMissionRefreshedEvent += OnMonsterMissionRefreshed;
	}

	protected void OnDestroy()
	{
		if (App.IsCreated())
		{
			MonsterMisionEvents events = App.Instance.Player.MonsterMissions.Events;
			events.MonsterRewardClaimedEvent -= OnMonsterRewardClaimed;
			events.MonsterMissionRefreshedEvent -= OnMonsterMissionRefreshed;
		}
	}

	public void Init(string worldId)
	{
		_worldId = worldId;
		List<MonsterMissionData> worldMonsterData = App.Instance.Player.MonsterMissions.GetWorldMonsterData(worldId);
		MonsterMisionEvents events = App.Instance.Player.MonsterMissions.Events;
		foreach (MonsterMissionData item in worldMonsterData)
		{
			UIQuestBox uIQuestBox = UIQuestBox.Create(events, item, OnChestClicked);
			uIQuestBox.GetComponent<RectTransform>().SetParent(_content, false);
			_boxes[item.MonsterId] = uIQuestBox;
		}
	}

	private void OnChestClicked(RectTransform pivot, string monsterId)
	{
		bool flag = !_rewardInfo.gameObject.activeSelf;
		_rewardInfo.gameObject.SetActive(flag);
		if (flag)
		{
			_rewardInfo.SetParent(pivot, true);
			_rewardInfo.transform.localScale = Vector3.one;
			_rewardInfo.transform.localPosition = Vector3.zero;
			_coinAmountText.text = App.Instance.Player.MonsterMissions.GetRewardCoinsAmount(monsterId).ToString();
			_cardsAmountText.text = "×1";
			if (_rewardPanelCR != null)
			{
				StopCoroutine(_rewardPanelCR);
			}
			_rewardPanelCR = StartCoroutine(RewardPanelCR());
		}
	}

	private IEnumerator RewardPanelCR()
	{
		yield return new WaitForSeconds(2f);
		HideRewardInfo();
	}

	private void HideRewardInfo()
	{
		if (_rewardPanelCR != null)
		{
			StopCoroutine(_rewardPanelCR);
		}
		_rewardInfo.gameObject.SetActive( false);
	}

	private void OnMonsterRewardClaimed(MonsterMissionData data, List<Reward> rewards)
	{
		if (!(data.WorldId != _worldId))
		{
			foreach (Reward reward in rewards)
			{
				if (reward != null)
				{
					App.Instance.Player.RewardManager.Redeem(reward, CurrencyReason.quest);
				}
			}
			_menuRewardController.Show(rewards);
		}
	}

	private void OnMonsterMissionRefreshed()
	{
		List<MonsterMissionData> worldMonsterData = App.Instance.Player.MonsterMissions.GetWorldMonsterData(_worldId);
		foreach (MonsterMissionData item in worldMonsterData)
		{
			if (_boxes.ContainsKey(item.MonsterId))
			{
				_boxes[item.MonsterId].UpdateData(item);
			}
			else
			{
				UnityEngine.Debug.LogWarning("We are missing a quest box for monster " + item.MonsterId);
			}
		}
	}

	public override void OnFocusGained()
	{
		base.OnFocusGained();
		if (_timerCR == null)
		{
			_timerCR = StartCoroutine(UpdateTimerCR());
		}
	}

	public override void OnFocusLost()
	{
		if (_timerCR != null)
		{
			StopCoroutine(_timerCR);
			_timerCR = null;
		}
		base.OnFocusLost();
	}

	private void OnCloseButtonClicked()
	{
		if (MonoSingleton<UIMenuStack>.Instance.Peek() == this)
		{
			Hide();
		}
	}

	private IEnumerator UpdateTimerCR()
	{
		while (true)
		{
			_newQuestTimer.text = App.Instance.Player.MonsterMissions.GetRemainingTimeBeforeNextQuests();
			yield return wait;
		}
	}
}