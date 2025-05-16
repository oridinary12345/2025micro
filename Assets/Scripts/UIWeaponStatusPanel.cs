using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIWeaponStatusPanel : MonoBehaviour
{
	[SerializeField]
	private Text _statusText;
	
	[SerializeField]
	private ResourceDisplay _statusResourceDisplay; // 用于显示状态图标和文本

	[SerializeField]
	private Image _overlayImage;

	[SerializeField]
	private Image _weaponImage;

	[SerializeField]
	private Image _fullScreenImage;

	private float _defaultImagePosX;

	private float _defaultTextPosX;

	private Sequence _animationSequence;

	private void Awake()
	{
		Vector2 anchoredPosition = _statusText.GetComponent<RectTransform>().anchoredPosition;
		_defaultTextPosX = anchoredPosition.x;
		Vector2 anchoredPosition2 = _weaponImage.GetComponent<RectTransform>().anchoredPosition;
		_defaultImagePosX = anchoredPosition2.x;
	}

	public void RegisterEvents()
	{
		UnregisterEvents();
		WeaponEvents events = App.Instance.Player.WeaponManager.Events;
		events.RepairedEvent += OnWeaponRepaired;
		events.BrokenEvent += OnWeaponBroken;
	}

	public void UnregisterEvents()
	{
		if (App.IsCreated())
		{
			WeaponEvents events = App.Instance.Player.WeaponManager.Events;
			events.RepairedEvent -= OnWeaponRepaired;
			events.BrokenEvent -= OnWeaponBroken;
		}
	}

	private void OnWeaponRepaired(string weaponId, int repairedAmount, bool skipFX)
	{
		AnimateWeaponRepaired(weaponId);
	}

	private void OnWeaponBroken(string weaponId)
	{
		AnimateWeaponBroken(weaponId);
	}

	private void AnimateWeaponBroken(string weaponID)
	{
		_statusText.text = "BROKEN!";
		SetWeaponImage(weaponID, true);
		StartCoroutine(AnimateCR( true));
	}

	private void AnimateWeaponRepaired(string weaponID)
	{
		_statusText.text = "REPAIRED!";
		SetWeaponImage(weaponID, false);
		StartCoroutine(AnimateCR( false));
	}

	private void SetWeaponImage(string weaponId, bool isBroken)
	{
		_weaponImage.sprite = Resources.Load<Sprite>("Weapons/" + weaponId + "/UI_w_" + weaponId + ((!isBroken) ? string.Empty : "_broken"));
	}

	private IEnumerator AnimateCR(bool isFlashing)
	{
		if (_animationSequence != null)
		{
			_animationSequence.Complete();
		}
		_animationSequence = DOTween.Sequence();
		_statusText.gameObject.SetActive( true);
		_weaponImage.gameObject.SetActive( true);
		_overlayImage.gameObject.SetActive( true);
		RectTransform textRect = _statusText.GetComponent<RectTransform>();
		textRect.DOAnchorPosX(textRect.rect.width, 0f).Complete( true);
		RectTransform imageRect = _weaponImage.GetComponent<RectTransform>();
		imageRect.DOAnchorPosX(0f - imageRect.rect.width, 0f).Complete( true);
		_overlayImage.color = new Color(0f, 0f, 0f, 0f);
		if (isFlashing)
		{
			_fullScreenImage.enabled = true;
			yield return new WaitForSeconds(0.08f);
			_fullScreenImage.enabled = false;
		}
		_animationSequence.Join(textRect.DOAnchorPosX(_defaultTextPosX, 0.25f).SetEase(Ease.OutBack));
		_animationSequence.Join(imageRect.DOAnchorPosX(_defaultImagePosX, 0.25f).SetEase(Ease.OutBack));
		_animationSequence.Join(_overlayImage.DOFade(13f / 51f, 0.1875f));
		yield return new WaitForSeconds(1.2f);
		_animationSequence.Join(textRect.DOAnchorPosX(0f - textRect.rect.width, 0.25f).SetEase(Ease.InBack));
		_animationSequence.Join(imageRect.DOAnchorPosX(imageRect.rect.width, 0.25f).SetEase(Ease.InBack));
		_animationSequence.Join(_overlayImage.DOFade(0f, 0.225f));
	}
}