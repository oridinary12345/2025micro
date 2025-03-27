using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firebreath : MonoBehaviour
{
	private Character _attacker;

	private Character _defender;

	private int _damage;

	public static Firebreath Create(Character attacker, int damage, Character target)
	{
		Firebreath firebreath = new GameObject("Firebreath").AddComponent<Firebreath>();
		return firebreath.Init(attacker, damage, target);
	}

	private Firebreath Init(Character attacker, int damage, Character target)
	{
		_attacker = attacker;
		_defender = target;
		_damage = damage;
		StartCoroutine(BreathfireCR());
		return this;
	}

	private IEnumerator BreathfireCR()
	{
		Cell previousCell2 = _attacker.CurrentCell;
		Cell flameCell = previousCell2;
		Transform flameTemplate = Resources.Load<Transform>("GameFX/FXFlames");
		List<Sequence> animationSequences = new List<Sequence>();
		for (int i = 0; i < 3; i++)
		{
			previousCell2 = flameCell;
			flameCell = flameCell.Center();
			Transform flame = UnityEngine.Object.Instantiate(flameTemplate);
			Vector3 flamePos = Vector3.zero;
			if (flameCell != null)
			{
				if (flameCell.OccupiedBy != null)
				{
					flameCell.OccupiedBy.TakeDamage(_attacker, null, _damage, _attacker.GetLastJumpPosition());
				}
				flamePos = flameCell.ToMapPos();
				flamePos.y -= 0.05f;
			}
			else
			{
				Vector3 b = previousCell2.ToMapPos() - previousCell2.Back().ToMapPos();
				flamePos = previousCell2.ToMapPos() + b;
				if (_defender != null)
				{
					if (!_defender.IsHero())
					{
						UnityEngine.Debug.LogWarning("Firebreath should be targetting the hero at this point. This should not happen.");
					}
					Sequence sequence = DOTween.Sequence();
					animationSequences.Add(sequence);
					Vector3 position = _defender.GetPosition();
					position.y -= 0.1f;
					Transform flameHit = UnityEngine.Object.Instantiate(flameTemplate);
					flameHit.parent = base.transform;
					flameHit.position = new Vector3(position.x, position.y, position.y * 2f - 3f);
					flameHit.localScale = Vector3.one * 0.5f;
					sequence.Append(flameHit.DOScale(1.5f, 0.12f).SetDelay(0.1f));
					sequence.Append(flameHit.DOScale(0.5f, 0.12f).SetDelay(0.8f - 0.07f * ((i != 0) ? 1f : 0f)).OnComplete(delegate
					{
						flameHit.gameObject.SetActive( false);
					}));
					_defender.TakeDamage(_attacker, null, _damage, _attacker.GetLastJumpPosition());
				}
			}
			Sequence seq2 = DOTween.Sequence();
			animationSequences.Add(seq2);
			flame.parent = base.transform;
			flame.position = new Vector3(flamePos.x, flamePos.y, flamePos.y * 2f - 3f);
			flame.localScale = Vector3.one * 0.5f;
			seq2.Append(flame.DOScale(1.5f, 0.12f));
			seq2.Append(flame.DOScale(0.5f, 0.12f).SetDelay(0.7f - 0.07f * ((i != 0) ? 1f : 0f)).OnComplete(delegate
			{
				flame.gameObject.SetActive( false);
			}));
			yield return new WaitForSeconds(0.1f);
			if (flameCell == null)
			{
				break;
			}
		}
		yield return new WaitForSeconds(1f);
		bool animationDone = false;
		while (!animationDone)
		{
			animationDone = true;
			yield return new WaitForSeconds(0.1f);
			foreach (Sequence item in animationSequences)
			{
				animationDone &= item.IsComplete();
			}
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}
}