using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
	public Transform MovingPivot;

	private readonly List<Cell> _affectedCells = new List<Cell>();

	private int _aliveRoundRemaining;

	public void OnDrop(Cell cell1, Cell cell2)
	{
		bool flag = (cell1.Column + 1) % 24 < cell2.Column;
		_affectedCells.Add(cell1);
		_affectedCells.Add(cell1.Back());
		_affectedCells.Add(cell1.Back().Left());
		_affectedCells.Add(cell1.Left());
		if (flag)
		{
			_affectedCells.Add(cell1.Back().Left().Left());
			_affectedCells.Add(cell1.Left().Left());
		}
		_aliveRoundRemaining = 4;
	}

	public bool IsReadyToExplode()
	{
		return _aliveRoundRemaining <= 0;
	}

	public void OnRoundStarted()
	{
		_aliveRoundRemaining--;
	}

	public List<Cell> GetAffectedCells()
	{
		return _affectedCells;
	}
}