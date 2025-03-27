using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroGameFlow : MonoBehaviour, IGameAnimationFlow
{
	private bool _isFinished;

	private GameController _gameController;

	public bool Finished => _isFinished;

	public static IGameAnimationFlow Create(GameController gameController, Transform parent)
	{
		IntroGameFlow introGameFlow = new GameObject(typeof(IntroGameFlow).ToString()).AddComponent<IntroGameFlow>();
		introGameFlow.transform.parent = parent;
		introGameFlow._gameController = gameController;
		return introGameFlow;
	}

	public void StartFlow(List<Monster> monsters)
	{
		StartCoroutine(FlowCR());
	}

	private IEnumerator FlowCR()
	{
		yield return new WaitForSeconds(0.25f);
		if (_gameController.State.IsObjectiveWaveBased())
		{
			UIGameIntroPanel introBox = GameObject.FindWithTag("GameIntroBox").GetComponent<UIGameIntroPanel>();
			introBox.StartAnim(_gameController.State.ChestUnlocked);
			while (!introBox.IsAnimOver())
			{
				yield return null;
			}
		}
		else
		{
			UnityEngine.Debug.LogWarning("This mode is missing a intro text...");
		}
		_isFinished = true;
	}
}