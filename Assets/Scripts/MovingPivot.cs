using UnityEngine;

public class MovingPivot : MonoBehaviour
{
	[SerializeField]
	private GameObject _target;

	[SerializeField]
	private GameObject _targetWeak;

	[SerializeField]
	private GameObject _targetCritical;

	[SerializeField]
	private GameObject _exclamationMark;

	[SerializeField]
	private SpriteRenderer _shadow;

	[SerializeField]
	private SpriteRenderer _shadowSelect;

	private bool _isWaitingToAttack;

	private Animator _exclamationAnimator;

	private Element _characterElement;

	private GameObject _lastWeaponCollision;

	private void Awake()
	{
		HideTargetSign();
		_exclamationMark.SetActive( false);
		_exclamationAnimator = _exclamationMark.GetComponent<Animator>();
	}

	public void Init(Element characterElement)
	{
		_characterElement = characterElement;
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.tag == "HeroWeapon" && base.gameObject.tag != "Hero")
		{
			WeaponShape component = collider.gameObject.GetComponent<WeaponShape>();
			if (component != null && component.IsActive)
			{
				_lastWeaponCollision = collider.gameObject;
				ShowTargetSign(component.Config);
				_exclamationMark.SetActive( false);
				_shadow.enabled = false;
				_shadowSelect.enabled = true;
			}
		}
	}

	private void OnTriggerStay2D(Collider2D collider)
	{
		if (collider.gameObject.tag == "HeroWeapon" && base.gameObject.tag != "Hero")
		{
			WeaponShape component = collider.gameObject.GetComponent<WeaponShape>();
			if (component != null && component.IsActive && _lastWeaponCollision == null)
			{
				_lastWeaponCollision = collider.gameObject;
				OnTriggerEnter2D(collider);
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collider)
	{
		if (collider.gameObject.tag == "HeroWeapon" && base.gameObject.tag != "Hero")
		{
			_lastWeaponCollision = null;
			WeaponShape component = collider.gameObject.GetComponent<WeaponShape>();
			if (component != null && component.IsActive)
			{
				HideTargetSign();
				_exclamationMark.SetActive(_isWaitingToAttack);
				_shadow.enabled = true;
				_shadowSelect.enabled = false;
			}
		}
	}

	public void RemoveShadows()
	{
		_shadow.gameObject.SetActive( false);
	}

	public void SetShadowColor(Color color)
	{
		_shadow.color = color;
	}

	public void HideShadow()
	{
		_shadow.gameObject.SetActive( false);
	}

	public void ShowShadow()
	{
		_shadow.gameObject.SetActive( true);
	}

	public void HideExclamationMark()
	{
		_exclamationMark.SetActive( false);
	}

	public void HideTarget()
	{
		HideTargetSign();
		_exclamationMark.SetActive(_isWaitingToAttack);
		_shadow.enabled = true;
		_shadowSelect.enabled = false;
	}

	public void SetWaitingToAttack(bool isWaiting)
	{
		_isWaitingToAttack = isWaiting;
		_exclamationMark.SetActive(isWaiting);
		MaybePlayExclamationMarkAnim();
	}

	private void MaybePlayExclamationMarkAnim()
	{
		if (_isWaitingToAttack)
		{
			_exclamationAnimator.Play("ExclamationMark");
		}
	}

	private void ShowTargetSign(WeaponConfig weaponConfig)
	{
		_targetCritical.SetActive(weaponConfig.ElementCritical == _characterElement);
		_targetWeak.SetActive(weaponConfig.ElementWeak == _characterElement);
		_target.SetActive(!_targetCritical.activeSelf && !_targetWeak.activeSelf);
	}

	private void HideTargetSign()
	{
		_target.SetActive( false);
		_targetCritical.SetActive( false);
		_targetWeak.SetActive( false);
	}
}