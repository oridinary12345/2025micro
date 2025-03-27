using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameInputController : MonoBehaviour
{
	private const float DelayBetweenMouseDownSec = 0.1f;

	private GameEvents _gameEvents;

	private bool _paused;

	private GameController _gameController;

	public GameInputController Init(GameController gameController)
	{
		_gameController = gameController;
		_gameEvents = _gameController.Events;
		_gameEvents.GamePausedEvent += OnGamePaused;
		_gameEvents.GameResumedEvent += OnGameResumed;
		return this;
	}

	private void OnGamePaused()
	{
		_paused = true;
	}

	private void OnGameResumed()
	{
		_paused = false;
	}

	private void Update()
	{
		if (UnityEngine.Input.touchCount > 0 && UnityEngine.Input.GetTouch(0).phase == TouchPhase.Began && !_paused && !IsPointerOverUIObject())
		{
			_gameController.OnTouch();
		}
	}

	private bool IsPointerOverUIObject()
	{
		PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
		PointerEventData pointerEventData2 = pointerEventData;
		Vector3 mousePosition = UnityEngine.Input.mousePosition;
		float x = mousePosition.x;
		Vector3 mousePosition2 = UnityEngine.Input.mousePosition;
		pointerEventData2.position = new Vector2(x, mousePosition2.y);
		List<RaycastResult> list = new List<RaycastResult>();
		EventSystem.current.RaycastAll(pointerEventData, list);
		foreach (RaycastResult item in list)
		{
			if (item.gameObject.tag == "GameTouchZone")
			{
				return false;
			}
		}
		return list.Count > 0;
	}
}