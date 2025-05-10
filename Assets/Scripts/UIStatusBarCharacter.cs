using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIStatusBarCharacter : MonoBehaviour
{
	[SerializeField]
	private Text _characterNameText;

	[SerializeField]
	private Slider _hpSlider;

	[SerializeField]
	private Text _hpText;

	[SerializeField]
	private Transform _lifeIcon;

	[SerializeField]
	private Image _elementImage;

	[SerializeField]
	private Text _levelText;

	[SerializeField]
	private Text _remainingTurnText;

	private CharacterEvents _characterEvents;

	private Character _character;

	private bool _showMaxHP;

	private bool _isAnimating;

	public void Init(Character character, CharacterEvents characterEvents, bool showMaxHP, bool showName)
	{
		if (character == null)
		{
			Debug.LogWarning("Character is null in UIStatusBarCharacter.Init");
			return;
		}

		if (characterEvents == null)
		{
			Debug.LogWarning("CharacterEvents is null in UIStatusBarCharacter.Init");
			return;
		}

		_character = character;
		_characterEvents = characterEvents;
		_showMaxHP = showMaxHP;

		if (_characterNameText != null)
		{
			_characterNameText.text = ((!showName) ? string.Empty : character.Name);
		}

		UpdateHP(false);
		UnregisterEvents();
		_characterEvents.DamagedEvent += OnDamaged;
		_characterEvents.HealedEvent += OnHealed;

		if (_elementImage != null)
		{
			_elementImage.gameObject.SetActive(false);
		}

		if (_levelText != null && character is Hero)
		{
			_levelText.text = $"Lv. {((Hero)character).Level}";
		}
	}

	private void OnDestroy()
	{
		UnregisterEvents();
	}

	private void UnregisterEvents()
	{
		if (_characterEvents != null)
		{
			_characterEvents.DamagedEvent -= OnDamaged;
			_characterEvents.HealedEvent -= OnHealed;
		}
	}

	private void OnDamaged(Character attacker, Character defender, WeaponData weapon, int damage)
	{
		if (_character == defender)
		{
			UpdateHP();
		}
	}

	private void OnHealed(Character character, int healAmount)
	{
		if (_character.Id == character.Id)
		{
			UpdateHP();
		}
	}

	public void SetRemainingTurnText(int turnCount)
	{
		if (_remainingTurnText != null)
		{
			_remainingTurnText.enabled = (turnCount > 0);
			var parentImage = _remainingTurnText.transform.parent?.GetComponent<Image>();
			if (parentImage != null)
			{
				parentImage.enabled = (turnCount > 0);
			}
			_remainingTurnText.text = turnCount.ToString();
		}
	}

	private void UpdateHP(bool animate = true)
	{
		if (_character == null)
		{
			Debug.LogWarning("Character is null in UIStatusBarCharacter.UpdateHP");
			return;
		}

		if (_hpSlider != null)
		{
			_hpSlider.DOValue(_character.HP01, (!animate) ? 0f : 0.2f);
		}

		if (_hpText != null)
		{
			_hpText.text = _character.HP + ((!_showMaxHP) ? string.Empty : ("/" + _character.HPMax));
			if (_character.IsHero())
			{
				// 根据血量设置颜色
				_hpText.color = (_character.HP > 5) ? Color.white : Color.red;
			}
		}

		if (animate && !_isAnimating && _lifeIcon != null)
		{
			_isAnimating = true;
			_lifeIcon.DOPunchScale(Vector3.one * 0.5f, 0.2f).OnComplete(delegate
			{
				if (_lifeIcon != null)
				{
					_lifeIcon.localScale = Vector3.one;
				}
				_isAnimating = false;
			});
		}
	}
}