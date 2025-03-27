using UnityEngine;

public class CharacterRenderer : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer _attack1;

	[SerializeField]
	private SpriteRenderer _attack2;

	[SerializeField]
	private SpriteRenderer _death0;

	[SerializeField]
	private SpriteRenderer _death1;

	[SerializeField]
	private SpriteRenderer _death2;

	[SerializeField]
	private SpriteRenderer _death3;

	[SerializeField]
	private SpriteRenderer _damaged1;

	[SerializeField]
	private SpriteRenderer _damaged2;

	[SerializeField]
	private SpriteRenderer _idle;

	[SerializeField]
	private SpriteRenderer _jump;

	[SerializeField]
	private SpriteRenderer _run1;

	[SerializeField]
	private SpriteRenderer _run2;

	[SerializeField]
	private SpriteRenderer _win;

	[SerializeField]
	private SpriteRenderer _dodge;

	[SerializeField]
	private SpriteRenderer _surprised;

	[SerializeField]
	private SpriteRenderer _weaponFound;

	[SerializeField]
	private SpriteRenderer _opened;

	[SerializeField]
	private SpriteRenderer _sleep;

	public void ApplySkin(CharacterSkin skin)
	{
		if (skin == null)
		{
			UnityEngine.Debug.LogWarning("Can't ApplySkin. Null value.");
			return;
		}
		_attack1.sprite = skin.Attack1;
		_attack2.sprite = skin.Attack2;
		_death0.sprite = skin.Death0;
		_death1.sprite = skin.Death1;
		_death2.sprite = skin.Death2;
		_death3.sprite = skin.Death3;
		_damaged1.sprite = skin.Damage1;
		_damaged2.sprite = skin.Damage2;
		_idle.sprite = skin.Idle;
		_jump.sprite = skin.Jump;
		_run1.sprite = skin.Run1;
		_run2.sprite = skin.Run2;
		_win.sprite = skin.Win;
		_dodge.sprite = skin.Dodge;
		_surprised.sprite = skin.Suprised;
		_weaponFound.sprite = skin.WeaponFound;
		_opened.sprite = skin.Opened;
		_sleep.sprite = skin.Sleep;
	}

	public void HideAll()
	{
		_attack1.enabled = false;
		_attack2.enabled = false;
		_death0.enabled = false;
		_death1.enabled = false;
		_death2.enabled = false;
		_death3.enabled = false;
		_damaged1.enabled = false;
		_damaged2.enabled = false;
		_idle.enabled = false;
		_jump.enabled = false;
		_run1.enabled = false;
		_run2.enabled = false;
		_win.enabled = false;
		_dodge.enabled = false;
		_surprised.enabled = false;
		_weaponFound.enabled = false;
		_opened.enabled = false;
		_sleep.enabled = false;
	}

	public void SetColor(Color color)
	{
		_attack1.color = color;
		_attack2.color = color;
		_death0.color = color;
		_death1.color = color;
		_death2.color = color;
		_death3.color = color;
		_damaged1.color = color;
		_damaged2.color = color;
		_idle.color = color;
		_jump.color = color;
		_run1.color = color;
		_run2.color = color;
		_win.color = color;
		_dodge.color = color;
		_surprised.color = color;
		_weaponFound.color = color;
		_opened.color = color;
		_sleep.color = color;
	}

	public Sprite GetIdleSprite()
	{
		return _idle.sprite;
	}
}