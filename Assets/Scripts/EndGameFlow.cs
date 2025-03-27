using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameFlow : MonoBehaviour, IGameAnimationFlow
{
	private const float KnightDuration = 3.35f;

	private bool _isFinished;

	private List<Monster> _monsters;

	private GameController _gameController;

	public bool Finished => _isFinished;

	public static IGameAnimationFlow Create(GameController gameController, Transform parent)
	{
		EndGameFlow endGameFlow = new GameObject(typeof(EndGameFlow).ToString()).AddComponent<EndGameFlow>();
		endGameFlow.transform.parent = parent;
		endGameFlow._gameController = gameController;
		return endGameFlow;
	}

	public void StartFlow(List<Monster> monsters)
	{
		_monsters = monsters;
		StartCoroutine(FlowCR());
	}

	private IEnumerator FlowCR()
	{
		yield return new WaitForSeconds(0.75f);
		if (!_gameController.State.CurrentHero.IsDead())
		{
			int monsterCount = 0;
			_monsters.ForEach(delegate(Monster m)
			{
				if (m != null && !m.IsDead() && m.CurrentCell != null)
				{
					monsterCount++;
				}
			});
			if (!_gameController.State.CurrentHero.IsPetrified())
			{
				_gameController.State.CurrentHero.Play(AnimationState.IdleWeaponFound);
			}
			Vector3 pos2 = new Vector3(0f, 3f, -8f);
			FloatingText.CreateScaleUp("LEVEL\nCOMPLETED!", pos2, Color.white);
			yield return new WaitForSeconds(1.65f);
		}
		else
		{
			Vector3 pos = new Vector3(0f, 3f, -8f);
			FloatingText.CreateScaleUp("GAME OVER", pos, Color.white);
			yield return new WaitForSeconds(2f);
		}
		_isFinished = true;
	}

	private void SetupKnight(GameObject knight, int knightIndex, int knightCount, int delayIndex)
	{
		float orthographicSize = Camera.main.orthographicSize;
		float num = orthographicSize * 2f;
		float num2 = num * (float)Screen.width / (float)Screen.height;
		float num3 = num / (float)knightCount;
		float minZoneY = (float)knightIndex * num3 - orthographicSize;
		float maxZoneY = minZoneY + num3;
		float x = num2 + knight.transform.GetWidth();
		float x2 = 0f - num2 - knight.transform.GetWidth();
		float y = 355f / (678f * (float)Math.PI) + (minZoneY + num3 * 0.5f) * 0.625f;
		Vector2 v = new Vector2(x2, y);
		Vector2 vector = new Vector2(x, y);
		knight.transform.position = v;
		knight.SetActive( true);
		float delay = UnityEngine.Random.value * Mathf.Clamp(delayIndex, 0f, 2f) * 0.45f;
		knight.transform.DOMoveX(vector.x, 3.35f).SetUpdate( true).OnUpdate(delegate
		{
			OnKnightRunning(knight.transform, minZoneY, maxZoneY);
		})
			.SetDelay(delay);
	}

	private void OnKnightRunning(Transform knight, float minPosY, float maxPosY)
	{
		foreach (Monster monster in _monsters)
		{
			if (monster != null && !monster.IsDead() && monster.CurrentCell != null)
			{
				Vector3 position = monster.GetPosition();
				if (position.y > minPosY && position.y < maxPosY)
				{
					Vector3 position2 = knight.position;
					if (position2.x > position.x - 1.5f)
					{
						monster.TakeDamage(null, null, Mathf.RoundToInt(monster.HP), new Vector2(position.x - 1f, position.y - 1f));
					}
				}
			}
		}
	}
}