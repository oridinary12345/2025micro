using DG.Tweening;
using System.Collections;
using UnityEngine;

public class CharacterVisual : MonoBehaviour
{
	[SerializeField]
	private CapsuleCollider2D ChracterCollider;

	[SerializeField]
	public Transform JumpingPivot;

	[SerializeField]
	public Transform MovingPivot;

	[SerializeField]
	private CharacterRenderer _characterRenderer;

	[SerializeField]
	private GameObject _weaponPivot;

	[SerializeField]
	private SpriteRenderer _weaponSprite;

	[SerializeField]
	private SpriteRenderer _weaponSpriteExtra;

	[SerializeField]
	private SpriteRenderer _weaponSpriteExtraAnimation;

	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private MovingPivot _movingPivot;

	private string _characterId;

	private Coroutine _setColorCR;

	private bool _isFlashing;

	private Coroutine _flashCoroutine;

	public Transform FXPivot
	{
		get;
		private set;
	}

	public CapsuleCollider2D Collider => ChracterCollider;

	public GameObject WeaponPivot => _weaponPivot;

	public static CharacterVisual Create(string characterId, Element characterElement)
	{
		return Create("Character", characterId, characterElement);
	}

	private static CharacterVisual Create(string prefabPath, string characterId, Element characterElement)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load<GameObject>(prefabPath));
		return gameObject.GetComponent<CharacterVisual>().Init(characterId, characterElement);
	}

	public CharacterVisual Init(string characterId, Element characterElement)
	{
		FXPivot = _characterRenderer.transform;
		_characterId = characterId;
		_movingPivot.Init(characterElement);
		if (characterId.StartsWith("chest"))
		{
			_movingPivot.RemoveShadows();
		}
		ApplyDefaultSkin();
		return this;
	}

	public void HideWeapon()
	{
		_weaponPivot.SetActive( false);
	}

	public void HideTarget()
	{
		_movingPivot.HideTarget();
	}

	public void SetWaitingToAttack(bool isWaiting)
	{
		_movingPivot.SetWaitingToAttack(isWaiting);
	}

	public void SetColor(Color color, float duration = 0f)
	{
		if (_setColorCR != null)
		{
			StopCoroutine(_setColorCR);
			_setColorCR = null;
		}
		if (duration == 0f)
		{
			_characterRenderer.SetColor(color);
			_weaponSprite.color = color;
			_weaponSpriteExtra.color = color;
			_weaponSpriteExtraAnimation.color = color;
			_movingPivot.SetShadowColor(color);
		}
		else
		{
			_setColorCR = StartCoroutine(SetColorCR(color, duration));
		}
	}

	private IEnumerator SetColorCR(Color endingColor, float duration)
	{
		Color startingColor = _weaponSprite.color;
		float timer = 0f;
		while (timer < duration)
		{
			yield return null;
			timer += Time.deltaTime;
			Color color = Color.Lerp(startingColor, endingColor, timer / duration);
			_characterRenderer.SetColor(color);
			_weaponSprite.color = color;
			_weaponSpriteExtra.color = color;
			_weaponSpriteExtraAnimation.color = color;
			_movingPivot.SetShadowColor(color);
		}
		SetColor(endingColor);
	}

	public Sprite GetIdleSprite()
	{
		return _characterRenderer.GetIdleSprite();
	}

	public bool IsAnimating()
	{
		return _flashCoroutine != null;
	}

	public Tweener FadeOut()
	{
		CanvasGroup canvas = null;
		UIStatusBarCharacter componentInChildren = base.transform.GetComponentInChildren<UIStatusBarCharacter>();
		if (componentInChildren != null)
		{
			canvas = componentInChildren.GetComponent<CanvasGroup>();
		}
		return DOTween.ToAlpha(() => _weaponSprite.color, delegate(Color x)
		{
			SetColor(x);
			if (canvas != null)
			{
				canvas.alpha = x.a;
			}
		}, 0f, 0.2f);
	}

	public Tweener FadeIn()
	{
		CanvasGroup canvas = null;
		UIStatusBarCharacter componentInChildren = base.transform.GetComponentInChildren<UIStatusBarCharacter>();
		if (componentInChildren != null)
		{
			canvas = componentInChildren.GetComponent<CanvasGroup>();
		}
		return DOTween.ToAlpha(() => _weaponSprite.color, delegate(Color x)
		{
			SetColor(x);
			if (canvas != null)
			{
				canvas.alpha = x.a;
			}
		}, 1f, 0.2f);
	}

	public Tweener HideUI()
	{
		_movingPivot.HideExclamationMark();
		UIStatusBarCharacter componentInChildren = base.transform.GetComponentInChildren<UIStatusBarCharacter>();
		if (componentInChildren != null)
		{
			CanvasGroup canvas = componentInChildren.GetComponent<CanvasGroup>();
			return DOTween.ToAlpha(() => new Color(1f, 1f, 1f, canvas.alpha), delegate(Color x)
			{
				canvas.alpha = x.a;
			}, 0f, 0.1f);
		}
		return null;
	}

	public Tweener ShowUI()
	{
		UIStatusBarCharacter componentInChildren = base.transform.GetComponentInChildren<UIStatusBarCharacter>();
		if (componentInChildren != null)
		{
			CanvasGroup canvas = componentInChildren.GetComponent<CanvasGroup>();
			return DOTween.ToAlpha(() => new Color(1f, 1f, 1f, canvas.alpha), delegate(Color x)
			{
				canvas.alpha = x.a;
			}, 1f, 0.1f);
		}
		return null;
	}

	public void HideExclamationMark()
	{
		_movingPivot.HideExclamationMark();
	}

	public void HideShadow()
	{
		_movingPivot.HideShadow();
	}

	public void ShowShadow()
	{
		_movingPivot.ShowShadow();
	}

	public void StartFlash()
	{
		if (_flashCoroutine == null)
		{
			_flashCoroutine = StartCoroutine(FlashCR());
		}
	}

	public void StopFlash()
	{
		_isFlashing = false;
		if (_flashCoroutine != null)
		{
			StopCoroutine(_flashCoroutine);
			_flashCoroutine = null;
		}
	}

	private IEnumerator FlashCR()
	{
		Color transparent = new Color(1f, 1f, 1f, 0f);
		_isFlashing = true;
		while (_isFlashing)
		{
			SetColor(Color.Lerp(t: Mathfx.Sinerp(0f, 1f, Mathf.PingPong(Time.time * 1.5f, 1f)), a: Color.white, b: transparent));
			yield return null;
		}
		SetColor(Color.white);
		_flashCoroutine = null;
	}

	public static string GetSkinPath(string characterId)
	{
		return "Characters/" + characterId + "Skin";
	}

	public void ApplyDefaultSkin()
	{
		string skinPath = GetSkinPath(_characterId);
		ApplySkin(skinPath);
	}

	public void ApplySkin(string skinPath)
	{
		CharacterSkin characterSkin = Resources.Load<CharacterSkin>(skinPath);
		if (characterSkin == null)
		{
			UnityEngine.Debug.LogWarning("Can't find SKIN " + skinPath);
		}
		else
		{
			_characterRenderer.ApplySkin(characterSkin);
		}
	}

	public void Play(string anim)
	{
		if (base.gameObject.activeInHierarchy)
		{
			_characterRenderer.HideAll();
			_animator.Play(anim);
		}
	}

	private WeaponPrefab GetWeaponPrefab(string weaponName)
	{
		return Resources.Load<WeaponPrefab>("Weapons/" + weaponName + "/" + weaponName);
	}

	public void SetWeapon(string weaponName, bool isRangedAttack, bool isBroken)
	{
		WeaponPrefab weaponPrefab = GetWeaponPrefab(weaponName);
		_weaponSprite.sprite = ((!isBroken) ? weaponPrefab.WeaponSprite : weaponPrefab.WeaponSpriteBroken);
		_weaponSpriteExtra.sprite = ((!isBroken) ? weaponPrefab.WeaponExtraSprite : weaponPrefab.WeaponExtraSpriteBroken);
		_weaponSpriteExtraAnimation.sprite = weaponPrefab.WeaponExtraAnimationSprite;
		_weaponSpriteExtraAnimation.enabled = false;
	}

	public GameObject GetWeaponProjectile(string weaponName)
	{
		WeaponPrefab weaponPrefab = GetWeaponPrefab(weaponName);
		if (weaponPrefab != null && weaponPrefab.Projectile != null)
		{
			return UnityEngine.Object.Instantiate(weaponPrefab.Projectile);
		}
		return null;
	}
}