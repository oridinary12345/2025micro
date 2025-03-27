using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStatusBarCharacter : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI _characterNameText;

	[SerializeField]
	private Slider _hpSlider;

	[SerializeField]
	private TextMeshProUGUI _hpText;

	[SerializeField]
	private Transform _lifeIcon;

	[SerializeField]
	private Image _elementImage;

	[SerializeField]
	private TextMeshProUGUI _levelText;

	[SerializeField]
	private TextMeshProUGUI _remainingTurnText;

	private CharacterEvents _characterEvents;

	private Character _character;

	private bool _showMaxHP;

	private bool _isAnimating;

	public void Init(Character character, CharacterEvents characterEvents, bool showMaxHP, bool showName)
	{
		_character = character;
		_characterEvents = characterEvents;
		_showMaxHP = showMaxHP;
		_characterNameText.text = ((!showName) ? string.Empty : character.Name);
		UpdateHP( false);
		UnregisterEvents();
		_characterEvents.DamagedEvent += OnDamaged;
		_characterEvents.HealedEvent += OnHealed;
		if (_elementImage != null)
		{
			_elementImage.gameObject.SetActive( false);
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
		_remainingTurnText.enabled = (turnCount > 0);
		_remainingTurnText.transform.parent.GetComponent<Image>().enabled = (turnCount > 0);
		_remainingTurnText.text = turnCount.ToString();
	}

	private void UpdateHP(bool animate = true)
	{
		_hpSlider.DOValue(_character.HP01, (!animate) ? 0f : 0.2f);
		_hpText.text = _character.HP + ((!_showMaxHP) ? string.Empty : ("/" + _character.HPMax));
		if (_character.IsHero())
		{
			_hpText.SwapMaterial((_character.HP > 5) ? "FiraSansExtraCondensed-ExtraBold SDF - Default" : "FiraSansExtraCondensed-ExtraBold SDF - Negative");
		}
		if (animate && !_isAnimating && _lifeIcon != null)
		{
			_isAnimating = true;
			_lifeIcon.DOPunchScale(Vector3.one * 0.5f, 0.2f).OnComplete(delegate
			{
				_lifeIcon.localScale = Vector3.one;
				_isAnimating = false;
			});
		}
	}
}