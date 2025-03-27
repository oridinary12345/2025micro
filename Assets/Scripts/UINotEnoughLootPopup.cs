using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class UINotEnoughLootPopup : UIMenuPopup
{
	[SerializeField]
	private UIGameButton _closeButton;

	[SerializeField]
	private UIGameButton _shopButton;

	[SerializeField]
	private TextMeshProUGUI _titleText;

	[SerializeField]
	private TextMeshProUGUI _messageText;

	private Action _continue;

	public static UINotEnoughLootPopup Create(string lootId, Action onContinue)
	{
		UINotEnoughLootPopup uINotEnoughLootPopup = UnityEngine.Object.Instantiate(Resources.Load<UINotEnoughLootPopup>("UI/NotEnoughLootPopup"));
		uINotEnoughLootPopup.GetComponent<RectTransform>().SetParent(UnityEngine.Object.FindObjectOfType<UIAppCanvas>().transform, false);
		uINotEnoughLootPopup.Init(lootId, onContinue);
		return uINotEnoughLootPopup;
	}

	protected override void Awake()
	{
		base.Awake();
		_closeButton.OnClick(OnCloseButtonClicked);
		_closeButton.ActivateOnBackKey();
	}

	public void Init(string lootId, Action onContinue)
	{
		_continue = onContinue;
		_titleText.text = string.Format("MISSING {0}".ToUpper(), LootIdToString(lootId));
		_messageText.text = $"Want more {LootIdToString(lootId)}?\nVisit the shop!";
		_shopButton.OnClick(base.Hide);
	}

	private void OnCloseButtonClicked()
	{
		_continue = null;
		Hide();
	}

	private string LootIdToString(string lootId)
	{
		return MonoSingleton<LootConfigs>.Instance.GetConfig(lootId).Title;
	}

	public override IEnumerator PopAnimationCR()
	{
		if (_continue != null)
		{
			_continue();
		}
		return base.PopAnimationCR();
	}
}