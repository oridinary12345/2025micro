using System;
using System.Collections;
using UnityEngine;

public class GameOverContinueManager : MonoBehaviour
{
	private Coroutine _timerCR;

	private GameEvents _gameEvents;

	private Hero _hero;

	private int _continueCount;

	public event Action OfferAcceptedEvent;

	public event Action OfferDeclinedEvent;

	public GameOverContinueManager Init(GameEvents gameEvents, Hero hero)
	{
		_gameEvents = gameEvents;
		_hero = hero;
		return this;
	}

	public void StartOffer()
	{
		if (_timerCR == null)
		{
			_timerCR = StartCoroutine(TimerCR());
		}
		else
		{
			UnityEngine.Debug.LogWarning("GameOverContinueManager can't start the timer twice!!");
		}
	}

	private void EndOffer()
	{
		if (_timerCR != null)
		{
			StopCoroutine(_timerCR);
			_timerCR = null;
		}
	}

	public void AcceptOffer()
	{
		if (_timerCR == null)
		{
			return;
		}
		LootProfile continuePrice = GetContinuePrice();
		if (App.Instance.Player.LootManager.TryExpense(continuePrice.LootId, continuePrice.Amount, CurrencyReason.gameOverContinue))
		{
			_continueCount++;
			int healAmount = Mathf.RoundToInt((float)_hero.HPMax * 0.25f);
			App.Instance.Player.HeroManager.Heal(healAmount);
			_gameEvents.OnGameStateMessage(new GameOverContinueAcceptedMessage());
			if (this.OfferAcceptedEvent != null)
			{
				this.OfferAcceptedEvent();
			}
			EndOffer();
		}
	}

	public void DeclineOffer()
	{
		if (_timerCR != null)
		{
			_gameEvents.OnGameStateMessage(new GameOverContinueDeclinedMessage());
			if (this.OfferDeclinedEvent != null)
			{
				this.OfferDeclinedEvent();
			}
			EndOffer();
		}
	}

	public bool CanOfferContinue()
	{
		return _continueCount < 100;
	}

	private IEnumerator TimerCR()
	{
		yield return new WaitForSeconds(10f);
		DeclineOffer();
	}

	public LootProfile GetContinuePrice()
	{
		int amount = 10 * (_continueCount + 1);
		return LootProfile.Create("lootRuby", amount);
	}
}