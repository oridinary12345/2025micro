using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIMenuGameOverContinue : UIMenuPopup
{
	[SerializeField]
	private UIGameButton _purchaseContinueButton;

	[SerializeField]
	private UIGameButton _skipContinueButton;

	[SerializeField]
	private Slider _progressBar;

	[SerializeField]
	private Text _priceText;

	[SerializeField]
	private Text _descriptionText;

	private Tweener _progressBarTween;

	private GameOverContinueManager _gameOverManager;

	protected override void Awake()
	{
		base.Awake();
		_purchaseContinueButton.OnClick(OnPurchaseContinueButton);
		_skipContinueButton.OnClick(OnSkipContinueButton);
	}

	public void Init(GameOverContinueManager gameOverManager)
	{
		_gameOverManager = gameOverManager;
	}

	private void OnPurchaseContinueButton()
	{
		if (_progressBarTween != null)
		{
			_gameOverManager.AcceptOffer();
		}
	}

	private void OnSkipContinueButton()
	{
		if (_progressBarTween != null)
		{
			_gameOverManager.DeclineOffer();
		}
	}

	public override void OnPop()
	{
		_gameOverManager.OfferAcceptedEvent -= OnOfferAccepted;
		_gameOverManager.OfferDeclinedEvent -= OnOfferDeclined;
		EndTween();
		base.OnPop();
	}

	public override void OnPush()
	{
		_gameOverManager.StartOffer();
		_progressBar.value = 1f;
		_progressBarTween = _progressBar.DOValue(0f, 10f);
		_gameOverManager.OfferAcceptedEvent += OnOfferAccepted;
		_gameOverManager.OfferDeclinedEvent += OnOfferDeclined;
		LootProfile continuePrice = _gameOverManager.GetContinuePrice();
		string lootInlineSprite = InlineSprites.GetLootInlineSprite(continuePrice.LootId);
		if (_priceText != null)
		{
			_priceText.text = $"{continuePrice.Amount}{lootInlineSprite} CONTINUE";
		}
		if (_descriptionText != null)
		{
			_descriptionText.text = $"Hero HP: +{25}%";
		}
		bool interactable = App.Instance.Player.LootManager.CanAfford(continuePrice.LootId, continuePrice.Amount);
		_purchaseContinueButton.interactable = interactable;
		_purchaseContinueButton.SetDisabledExplanation("You don't have enough rubies");
		base.OnPush();
	}

	private void OnOfferAccepted()
	{
		Pop();
	}

	private void OnOfferDeclined()
	{
		Pop();
	}

	private void OnDestroy()
	{
		EndTween();
	}

	private void EndTween()
	{
		if (_progressBarTween != null)
		{
			_progressBarTween.Kill( true);
			_progressBarTween = null;
		}
	}
}