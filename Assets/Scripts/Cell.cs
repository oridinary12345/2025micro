using System.Collections.Generic;
using UnityEngine;

public class Cell
{
	private GridCircle _grid;

	public int Layer
	{
		get;
		private set;
	}

	public int Column
	{
		get;
		private set;
	}

	public Character IncomingCharacter
	{
		get;
		private set;
	}

	public Character OccupiedBy
	{
		get;
		private set;
	}

	public Cell(GridCircle grid, int layer, int column)
	{
		_grid = grid;
		Layer = layer;
		Column = column;
	}

	public bool IsOccupied()
	{
		return OccupiedBy != null || IncomingCharacter != null;
	}

	public bool IsLastLayer()
	{
		return Layer == 5;
	}

	public void AddIncoming(Character character)
	{
		if (IncomingCharacter != null && IncomingCharacter != character)
		{
			UnityEngine.Debug.LogWarning("There's already an incoming character on " + ToString() + ", " + character.name + " can't replace " + IncomingCharacter.name);
		}
		IncomingCharacter = character;
		character.SetIncomingCell(this);
	}

	public void ClearIncoming()
	{
		if (IncomingCharacter != null)
		{
			IncomingCharacter.SetIncomingCell(null);
		}
		IncomingCharacter = null;
	}

	public void AddCharacter(Character character)
	{
		if (IncomingCharacter != null && IncomingCharacter != character)
		{
			UnityEngine.Debug.LogWarning(IncomingCharacter.name + " was incoming on " + this + ", but " + character.name + ", has stolen his place");
		}
		ClearIncoming();
		if (IsOccupied())
		{
		}
		OccupiedBy = character;
		_grid.OnCharacterEnteredCell(this);
	}

	public void RemoveCharacter(Character character)
	{
		if (OccupiedBy != character)
		{
		}
		ClearIncoming();
		OccupiedBy = null;
	}

	public Cell GetCell(int layerOffset)
	{
		int layer = Mathf.Clamp(Layer + layerOffset, 0, 5);
		return _grid.GetCell(layer, Column);
	}

	public Cell GetCell(int layer, int column)
	{
		return _grid.GetCell(layer, column);
	}

	public Cell Back()
	{
		return _grid.GetCell(Layer + 1, Column);
	}

	public Cell Center()
	{
		return _grid.GetCell(Layer - 1, Column);
	}

	public Cell Left()
	{
		int num = (Column + 1) % 24;
		Cell cell = _grid.GetCell(Layer, num);
		if (cell == null)
		{
			UnityEngine.Debug.LogWarning("Left() can't find a valid cell for layer " + Layer + " and column " + num);
		}
		return cell;
	}

	public Cell Right()
	{
		int num = ((Column != 0) ? (Column - 1) : 23) % 24;
		Cell cell = _grid.GetCell(Layer, num);
		if (cell == null)
		{
			UnityEngine.Debug.LogWarning("Right() can't find a valid cell for layer " + Layer + " and column " + num);
		}
		return cell;
	}

	public Cell FindCell(List<MoveDirection> directions)
	{
		return _grid.FindCell(this, directions);
	}

	public Cell GetCellFrom(MoveDirection direction)
	{
		return _grid.GetCellFrom(this, direction);
	}

	public Vector3 ToMapPos()
	{
		return _grid.ToMapPos(this);
	}

	public override string ToString()
	{
		return $"[Cell: Layer={Layer}, Column={Column}, MapPos={ToMapPos()}]";
	}
}