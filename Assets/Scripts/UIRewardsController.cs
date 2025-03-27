using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRewardsController : UIMenu
{
	[SerializeField]
	private UIGradient _backgroundGradient;

	private readonly List<Reward> _rewardQueue = new List<Reward>();

	private UIRewardPanel _rewardPopup;

	private Action _onRewardShown;

	public static UIRewardsController Create()
	{
		UIRewardsController uIRewardsController = UnityEngine.Object.Instantiate(Resources.Load<UIRewardsController>("UI/RewardsController"));
		uIRewardsController.GetComponent<RectTransform>().SetParent(UnityEngine.Object.FindObjectOfType<UIAppCanvas>().transform, false);
		uIRewardsController.Init();
		return uIRewardsController;
	}

	private void Init()
	{
		_backgroundGradient.gameObject.SetActive( false);
		_rewardPopup = UIRewardPanel.Create();
	}

	public void Show(List<Reward> rewards, Action continuation = null)
	{
		_onRewardShown = continuation;
		rewards = RewardFactory.Merge(rewards);
		_rewardQueue.AddRange(rewards);
		Show();
	}

	public bool IsShowingRewards()
	{
		return _rewardQueue.Count > 0 || MonoSingleton<UIMenuStack>.Instance.Peek() == _rewardPopup;
	}

	private bool MaybeShowNextReward()
	{
		if (_rewardQueue.Count > 0)
		{
			Reward reward = _rewardQueue[0];
			_rewardQueue.RemoveAt(0);
			Show(reward);
			return true;
		}
		return false;
	}

	private void SetBackgroundColor(Color color1, Color color2)
	{
		_backgroundGradient.gameObject.SetActive( true);
		_backgroundGradient.SetColor1(color1);
		_backgroundGradient.SetColor2(color2);
	}

	public override void OnFocusGained()
	{
		base.OnFocusGained();
		StartCoroutine(CheckForRewardsCR());
	}

	private IEnumerator CheckForRewardsCR()
	{
		yield return new WaitForEndOfFrame();
		if (!MaybeShowNextReward())
		{
			Hide();
			if (_onRewardShown != null)
			{
				_onRewardShown();
			}
		}
	}

	private void Show(Reward reward)
	{
		_rewardPopup.Show(reward, SetBackgroundColor);
	}
}