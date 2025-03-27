using TMPro;
using UnityEngine;

public class UIWelcomeBackPopup : UIMenuPopup
{
	[SerializeField]
	private UIGameButton _closeButton;

	[SerializeField]
	private TextMeshProUGUI _cashEarnedText;

	public static UIWelcomeBackPopup Create(LootProfile lootEarned)
	{
		UIWelcomeBackPopup uIWelcomeBackPopup = Object.Instantiate(Resources.Load<UIWelcomeBackPopup>("UI/WelcomeBackPopup"));
		uIWelcomeBackPopup.GetComponent<RectTransform>().SetParent(Object.FindObjectOfType<UIAppCanvas>().transform, false);
		uIWelcomeBackPopup.Init(lootEarned);
		return uIWelcomeBackPopup;
	}

	protected override void Awake()
	{
		base.Awake();
		_closeButton.OnClick(base.Hide);
		_closeButton.ActivateOnBackKey();
	}

	private void Init(LootProfile lootEarned)
	{
		_cashEarnedText.text = "+" + lootEarned.ToString();
	}
}