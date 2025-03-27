using UnityEngine;

public class CharacterTest : MonoBehaviour
{
	private Character _hero;

	private CharacterEvents _characterEvents;

	private void Start()
	{
		_characterEvents = new CharacterEvents();
		HeroData currentHeroData = App.Instance.Player.HeroManager.GetCurrentHeroData();
		_hero = Hero.Create(currentHeroData, App.Instance.Player.WeaponManager.Events, _characterEvents);
		_hero.transform.position = Vector3.zero;
		_hero.SetPosition(_hero.transform.position);
		_hero.SetTag("Hero");
		_hero.Play(AnimationState.Idle);
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha1))
		{
			_hero.Play(AnimationState.Idle);
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha2))
		{
			_hero.Play(AnimationState.IdleRanged);
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha3))
		{
			_hero.Play(AnimationState.Run);
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha4))
		{
			_hero.Play(AnimationState.Jump);
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha5))
		{
			_hero.Play(AnimationState.AttackMelee);
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha6))
		{
			_hero.Play(AnimationState.AttackRanged);
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha7))
		{
			_hero.Play(AnimationState.JumpRanged);
		}
	}
}