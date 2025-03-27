using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class UIWeaponUnlockedPanel : UIMenu
{
	[SerializeField]
	private UIWeaponsPanelBox _weaponBox;

	[SerializeField]
	private TextMeshProUGUI _titleText;

	[SerializeField]
	private TextMeshProUGUI _weaponRangeText;

	[SerializeField]
	private UIGameButton _buttonClose;

	[SerializeField]
	private GameObject _tapToContinueLabel;

	[SerializeField]
	private RectTransform _rayImage;

	[SerializeField]
	private RectTransform _fxPivot;

	[SerializeField]
	private ParticleSystem _particles;

	private Action _onContinue;

	private Tweener _rayTweener;

	protected override void Awake()
	{
		base.Awake();
		Vector2 pivot = _weaponBox.GetComponent<RectTransform>().pivot;
		if (!Mathf.Approximately(pivot.y, 0.5f))
		{
			UnityEngine.Debug.LogWarning("Weapon box Y pivot isn't properly set!");
		}
		_buttonClose.OnClick(OnCloseButtonClicked);
		_buttonClose.ActivateOnBackKey();
	}

	public UIWeaponUnlockedPanel Init(WeaponData lockedWeaponData, Action onContinue)
	{
		UIWeaponsPanelBox uIWeaponsPanelBox = UnityEngine.Object.Instantiate(_weaponBox);
		uIWeaponsPanelBox.Init(lockedWeaponData.Config, lockedWeaponData, 0);
		uIWeaponsPanelBox.GetComponent<RectTransform>().pivot = Vector2.one * 0.5f;
		uIWeaponsPanelBox.GetComponent<RectTransform>().sizeDelta = _weaponBox.GetComponent<RectTransform>().sizeDelta;
		uIWeaponsPanelBox.GetComponent<RectTransform>().SetParent(_weaponBox.transform.parent, false);
		uIWeaponsPanelBox.transform.localScale = Vector3.one * 1.25f;
		uIWeaponsPanelBox.transform.position = _weaponBox.transform.position;
		uIWeaponsPanelBox.HideStatusIcons();
		return _Init(uIWeaponsPanelBox, onContinue);
	}

	public UIWeaponUnlockedPanel Init(UIWeaponsPanelBox weaponBox, Action onContinue = null)
	{
		UIWeaponsPanelBox uIWeaponsPanelBox = UnityEngine.Object.Instantiate(weaponBox);
		uIWeaponsPanelBox.Init(weaponBox.WeaponConfig, weaponBox.WeaponData, 0);
		uIWeaponsPanelBox.GetComponent<RectTransform>().pivot = Vector2.one * 0.5f;
		uIWeaponsPanelBox.GetComponent<RectTransform>().sizeDelta = _weaponBox.GetComponent<RectTransform>().sizeDelta;
		uIWeaponsPanelBox.GetComponent<RectTransform>().SetParent(_weaponBox.transform.parent, false);
		uIWeaponsPanelBox.transform.localScale = Vector3.one * 1.25f;
		uIWeaponsPanelBox.transform.position = _weaponBox.transform.position;
		uIWeaponsPanelBox.HideStatusIcons();
		return _Init(uIWeaponsPanelBox, onContinue);
	}

	private UIWeaponUnlockedPanel _Init(UIWeaponsPanelBox copyWeaponBox, Action onContinue)
	{
		_particles.Stop();
		_particles.gameObject.SetActive( false);
		_onContinue = onContinue;
		ApplyWeapon(copyWeaponBox.WeaponConfig, copyWeaponBox.WeaponData);
		_titleText.text = copyWeaponBox.WeaponConfig.Title.ToUpper();
		_weaponRangeText.text = copyWeaponBox.WeaponConfig.RangeType + " range";
		_titleText.gameObject.SetActive( false);
		_weaponRangeText.gameObject.SetActive( false);
		_rayImage.gameObject.SetActive( false);
		_weaponBox.gameObject.SetActive( false);
		_buttonClose.interactable = false;
		_tapToContinueLabel.SetActive( false);
		Action onShown = delegate
		{
			Action onHidden = delegate
			{
				Reveal();
				copyWeaponBox.gameObject.SetActive( false);
				UnityEngine.Object.Destroy(copyWeaponBox.gameObject);
			};
			copyWeaponBox.transform.DOScale(Vector3.one * 0.2f, 0.5f).OnComplete(delegate
			{
				onHidden();
			}).SetDelay(1f);
		};
		App.Instance.MenuEvents.OnWeaponUnlockTranslated();
		copyWeaponBox.gameObject.SetActive( false);
		copyWeaponBox.transform.localScale = Vector3.one * 0.05f;
		copyWeaponBox.transform.DOScale(Vector3.one * 1.25f, 0.4f).SetEase(Ease.OutElastic, 1.7f, 0.5f).SetDelay(0.25f)
			.OnStart(delegate
			{
				copyWeaponBox.gameObject.SetActive( true);
			})
			.OnComplete(delegate
			{
				onShown();
			});
		return this;
	}

	private void ApplyWeapon(WeaponConfig weaponConfig, WeaponData weaponData)
	{
		_weaponBox.Init(weaponConfig, weaponData, 0);
		_weaponBox.HideStatusIcons();
	}

	private void OnCloseButtonClicked()
	{
		if (_rayTweener != null)
		{
			DOTween.Kill(_rayTweener);
		}
		Hide();
		if (_onContinue != null)
		{
			_onContinue();
		}
	}

	private void Reveal()
	{
		_titleText.gameObject.SetActive( true);
		_weaponRangeText.gameObject.SetActive( true);
		_weaponBox.transform.localScale = Vector3.one * 2f;
		_weaponBox.gameObject.SetActive( true);
		_weaponBox.transform.DOScale(Vector3.one * 1.5f, 0.32f).SetEase(Ease.OutElastic);
		_tapToContinueLabel.SetActive( true);
		_buttonClose.interactable = true;
		App.Instance.MenuEvents.OnWeaponUnlockRevealed(_fxPivot.transform);
		_rayImage.gameObject.SetActive( true);
		_particles.gameObject.SetActive( true);
		_particles.Play();
		if (_rayTweener == null)
		{
			_rayTweener = _rayImage.DORotate(new Vector3(0f, 0f, -360f), 3f).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
			_rayTweener.OnKill(delegate
			{
				_rayTweener = null;
			});
		}
	}
}