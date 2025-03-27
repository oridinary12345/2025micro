using UnityEngine;

public class FloatingTextManager : MonoBehaviour
{
	private GameEvents _gameEvents;

	private CharacterEvents _characterEvents;

	private GameState _gameState;

	private float _lastHealingEndTime;

	public void Init(GameEvents gameEvents, CharacterEvents characterEvents, GameState gameState)
	{
		_gameEvents = gameEvents;
		_characterEvents = characterEvents;
		_gameState = gameState;
		_gameEvents.LevelFinishedEvent += OnLevelFinished;
		_gameEvents.WaveStartedEvent += OnWaveStarted;
		_characterEvents.DamagedEvent += OnDamaged;
		_characterEvents.DodgedEvent += OnDodged;
		_characterEvents.HealedEvent += OnHealed;
	}

	private void OnDestroy()
	{
		if (!App.IsCreated())
		{
		}
	}

	private void OnDamaged(Character attacker, Character defender, WeaponData weapon, int damage)
	{
		if (!(attacker == null))
		{
			Vector3 position = defender.GetPosition();
			Vector3 vector = new Vector3(position.x, position.y + 1.5f, position.z - 3f);
			Vector3 endPosition = vector;
			endPosition.x += 0.8f;
			endPosition.y += 0.6f;
			FloatingText.Create(damage.ToString(), "FloatingTextDamage", vector, endPosition);
		}
	}

	private void OnHealed(Character character, int healAmount)
	{
		if (healAmount != 0)
		{
			float num = 0f;
			if (Time.realtimeSinceStartup < _lastHealingEndTime)
			{
				num = _lastHealingEndTime - Time.realtimeSinceStartup;
			}
			else if (Time.realtimeSinceStartup - _lastHealingEndTime < 0.3f)
			{
				num = Time.realtimeSinceStartup - _lastHealingEndTime;
			}
			Vector3 position = character.GetPosition();
			Vector3 pos = new Vector3(position.x, position.y + 1f, position.z - 3f);
			Vector3 endPos = pos;
			endPos.y += 0.3f;
			this.Execute(num, delegate
			{
				FloatingText.Create("+" + healAmount + $"<sprite={1}>", "FloatingTextHeal", pos, endPos);
			});
			num += 0.3f;
			_lastHealingEndTime = Time.realtimeSinceStartup + num;
		}
	}

	private void OnDodged(Character attacker, Character defender, Vector3 defaultPos)
	{
	}

	private void OnLevelFinished(LevelData levelData)
	{
	}

	private void OnWaveStarted()
	{
		if (_gameState.IsObjectiveWaveBased() && _gameState.GetWaveIndex() > 0)
		{
			FloatingText.CreateScaleUp(startPosition: new Vector3(0f, 3f, -8f), text: $"{_gameState.GetWaveRemainingCount()} waves remaining!", startColor: Color.white);
		}
	}
}