using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttackingPhaseBomb : CharacterAttackingPhase
{
	private bool _areBombHandled;

	private GameController _gameController;

	private List<Bomb> _bombs = new List<Bomb>();

	public CharacterAttackingPhaseBomb Init(GameController gameController, Character character, WeaponShape weaponShape, GridCircle grid)
	{
		Init(character);
		_gameController = gameController;
		_areBombHandled = false;
		character.WeaponTakingDamage(1);
		List<Vector3> collidersCenter = weaponShape.GetCollidersCenter();
		List<Vector3> extraPivotPos = weaponShape.GetExtraPivotPos();
		int num = 0;
		float num2 = 0f;
		foreach (Vector3 item in collidersCenter)
		{
			Vector3 vector = item + new Vector3(0f, -0.25f, 0f);
			Bomb bomb = Object.Instantiate(Resources.Load<Bomb>("MapObjects/BombObject"));
			bomb.transform.position = new Vector3(vector.x, vector.y, vector.y * 2f - 3f);
			bomb.MovingPivot.localPosition = new Vector3(0f, 1.35f, 0f);
			bomb.gameObject.SetActive( false);
			_bombs.Add(bomb);
			Cell cell = grid.GetCell(extraPivotPos[num++]);
			Cell cell2 = grid.GetCell(extraPivotPos[num++]);
			bomb.OnDrop(cell, cell2);
			GameObject bombShadow = Object.Instantiate(Resources.Load<GameObject>("MapObjects/BombShadow"));
			bombShadow.SetActive( false);
			bombShadow.transform.parent = bomb.transform;
			bombShadow.transform.localPosition = new Vector3(0f, 0f, 0.2f);
			bomb.MovingPivot.DOLocalMoveY(0f, 0.22f).OnStart(delegate
			{
				bomb.gameObject.SetActive( true);
				bombShadow.SetActive( true);
				gameController.AddBomb(bomb);
			}).SetEase(Ease.InQuad)
				.SetDelay(num2)
				.OnComplete(delegate
				{
					OnBombLanding(bomb);
				});
			num2 += 0.42f;
		}
		MonoExtensions.Execute(num2, delegate
		{
			_areBombHandled = true;
		});
		return this;
	}

	private void OnBombLanding(Bomb bomb)
	{
		_gameController.Events.OnBombDrop();
		Animator componentInChildren = bomb.GetComponentInChildren<Animator>( true);
		componentInChildren.Play("BombLanding");
		_gameController.CheckBombCollision();
	}

	public override bool IsOver()
	{
		return base.IsOver() && _areBombHandled;
	}
}