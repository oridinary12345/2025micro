using UnityEngine;
using UnityEngine.UI;

public class UIWelcomeBackPopup : UIMenuPopup
{
	[SerializeField]
	private UIGameButton _closeButton;

	[SerializeField]
	private Text _cashEarnedText;

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
		if (_cashEarnedText != null)
		{
			_cashEarnedText.text = "+" + lootEarned.ToString();
		}
		else
		{
			Debug.LogWarning("Init: Missing cash earned text component");
		}
	}
}