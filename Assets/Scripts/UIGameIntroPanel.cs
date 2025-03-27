using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGameIntroPanel : MonoBehaviour
{
	[SerializeField]
	private UIGameHUD _gameTopHud;

	[SerializeField]
	private RectTransform _topText;

	[SerializeField]
	private RectTransform _bottomText;

	[SerializeField]
	private RectTransform _waveCountBox;

	[SerializeField]
	private RectTransform _keyUnlockingChestAnchor;

	[SerializeField]
	private RectTransform _key;

	[SerializeField]
	private Image _keyOverlay;

	[SerializeField]
	private Animator _chestAnimator;

	[SerializeField]
	private TextMeshProUGUI _waveCountText;

	private bool _isAnimOver;

	private Sequence _introTween;

	private void Awake()
	{
		_introTween = DOTween.Sequence();
		_topText.GetComponent<TextMeshProUGUI>().enabled = false;
		_bottomText.GetComponent<TextMeshProUGUI>().enabled = false;
		_waveCountBox.gameObject.SetActive( false);
		_key.gameObject.SetActive( false);
	}

	public void SetWaveCount(int waveCount)
	{
		_waveCountText.text = waveCount + " WAVES";
	}

	public void StartAnim(bool canUnlockChest)
	{
		_topText.GetComponent<TextMeshProUGUI>().enabled = true;
		_bottomText.GetComponent<TextMeshProUGUI>().enabled = true;
		_waveCountBox.gameObject.SetActive( true);
		Vector2 anchoredPosition = _topText.anchoredPosition;
		float defaulTopTextX = anchoredPosition.x;
		Vector2 anchoredPosition2 = _bottomText.anchoredPosition;
		float defaulBottomTextX = anchoredPosition2.x;
		Vector2 anchoredPosition3 = _waveCountBox.anchoredPosition;
		float defaulWaveCountX = anchoredPosition3.x;
		float topTextW = _topText.rect.width * 2f;
		float bottomTextW = _bottomText.rect.width * 2f;
		float waveBoxW = _waveCountBox.rect.width * 2f;
		_topText.DOAnchorPosX(0f - topTextW, 0f).Complete( true);
		_bottomText.DOAnchorPosX(0f - bottomTextW, 0f).Complete( true);
		_waveCountBox.DOAnchorPosX(waveBoxW, 0f).Complete( true);
		App.Instance.MenuEvents.OnTokenTextEntering();
		Action onHUDShown = delegate
		{
			Action onIntroReady = delegate
			{
				_introTween.Insert(0f, _topText.DOAnchorPosX(defaulTopTextX, 0.3f));
				_introTween.Insert(0f, _bottomText.DOAnchorPosX(defaulBottomTextX, 0.3f));
				_introTween.Insert(0f, _waveCountBox.DOAnchorPosX(defaulWaveCountX, 0.3f));
				_introTween.Insert(3f, _topText.DOAnchorPosX(topTextW, 0.3f).SetDelay(1.25f).OnStart(App.Instance.MenuEvents.OnTokenTextExiting));
				_introTween.Insert(3f, _bottomText.DOAnchorPosX(bottomTextW, 0.3f).SetDelay(1.25f));
				_introTween.Insert(3f, _waveCountBox.DOAnchorPosX(0f - waveBoxW, 0.3f).SetDelay(1.25f).OnComplete(delegate
				{
					_isAnimOver = true;
				}));
			};
			if (canUnlockChest)
			{
				TweenCallback onKeyReachedChest = delegate
				{
					App.Instance.MenuEvents.OnKeyShown();
					this.Execute(0.25f, delegate
					{
						this.Execute(0.25f, delegate
						{
							App.Instance.MenuEvents.OnChestUnlocked();
						});
						_chestAnimator.Play("chestUnlock");
						_key.GetComponent<Image>().DOColor(new Color(1f, 1f, 1f, 0f), 0.25f);
						this.Execute(1f, onIntroReady);
					});
				};
				TweenCallback action = delegate
				{
					Vector3[] array = new Vector3[2];
					ref Vector3 reference = ref array[0];
					Vector3 position = _key.position;
					float x = position.x;
					Vector3 position2 = _keyUnlockingChestAnchor.position;
					float x2 = Mathf.Lerp(x, position2.x, 0.4f);
					Vector3 position3 = _keyUnlockingChestAnchor.position;
					reference = new Vector3(x2, position3.y);
					array[1] = _keyUnlockingChestAnchor.position;
					Vector3[] path = array;
					Ease ease = Ease.Linear;
					_key.DOPath(path, 0.45f, PathType.CatmullRom, PathMode.Sidescroller2D).SetDelay(0.45f).SetEase(ease)
						.OnComplete(onKeyReachedChest);
					_key.DOScale(_key.transform.localScale * 0.5f, 0.45f).SetDelay(0.45f).SetEase(ease);
				};
				_key.gameObject.SetActive( true);
				_keyOverlay.GetComponent<Image>().DOColor(new Color(1f, 1f, 1f, 0f), 0.35f).SetEase(Ease.InQuad)
					.SetDelay(0.15f);
				Vector2 anchoredPosition4 = _key.anchoredPosition;
				float y = anchoredPosition4.y;
				_key.DOAnchorPosY(y + _key.rect.height * 0.5f, 0.45f).OnComplete(action).SetDelay(0.15f);
			}
			else
			{
				onIntroReady();
			}
		};
		_gameTopHud.ShowHUD(onHUDShown);
	}

	public bool IsAnimOver()
	{
		return _isAnimOver;
	}
}