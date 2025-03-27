using System;
using System.Collections.Generic;
using UnityEngine;

public class GridCircle
{
	public const float CircleSize = 0.45f;

	public const int CircleCount = 7;

	public Color CircleColor = new Color(0f, 0f, 0f, 0.1f);

	public const int FirstLayer = 0;

	public const int LastLayer = 5;

	public const int LayerCount = 6;

	public const int ColumnCount = 24;

	public const float ColumnAngle = 15f;

	public const float CenterPaddingSize = 0.35f;

	public const float CenterRadius = 0.799999952f;

	public const float Radius = 3.5f;

	private readonly Cell[][] _cells;

	public event Action<Cell> CharacterEnteredCellEvent;

	public GridCircle()
	{
		_cells = new Cell[6][];
		for (int i = 0; i < 6; i++)
		{
			_cells[i] = new Cell[24];
			for (int j = 0; j < 24; j++)
			{
				_cells[i][j] = new Cell(this, i, j);
			}
		}
	}

	public void OnCharacterEnteredCell(Cell cell)
	{
		if (this.CharacterEnteredCellEvent != null)
		{
			this.CharacterEnteredCellEvent(cell);
		}
	}

	public Cell GetCell(int layer, int column)
	{
		if (layer >= 0 && layer < 6 && column >= 0 && column < 24)
		{
			return _cells[layer][column];
		}
		return null;
	}

	public Cell GetCellFrom(Cell fromCell, MoveDirection direction)
	{
		if (fromCell != null)
		{
			switch (direction)
			{
			case MoveDirection.Pause:
			case MoveDirection.Skip:
				return fromCell;
			case MoveDirection.Center:
				return fromCell.Center();
			case MoveDirection.Back:
				return fromCell.Back();
			case MoveDirection.Left:
				return fromCell.Left();
			case MoveDirection.Right:
				return fromCell.Right();
			}
		}
		return null;
	}

	public Cell FindCell(Cell fromCell, List<MoveDirection> directions)
	{
		Cell cell = fromCell;
		foreach (MoveDirection direction in directions)
		{
			if (direction == MoveDirection.Pause || direction == MoveDirection.Skip)
			{
				return cell;
			}
			Cell cellFrom = GetCellFrom(cell, direction);
			if (cellFrom == null)
			{
				return cell;
			}
			cell = cellFrom;
		}
		if (cell == fromCell)
		{
			UnityEngine.Debug.LogWarning("Directions is invalid. FindCell will return the same origin cell.");
		}
		return cell;
	}

	public Cell GetCell(Vector2 mapPos)
	{
		float num = Mathf.Atan2(mapPos.x, mapPos.y) / (float)Math.PI * 180f;
		if (num < 0f)
		{
			num = 360f + num;
		}
		float magnitude = mapPos.magnitude;
		int column = Mathf.FloorToInt(num / 15f);
		int layer = Mathf.FloorToInt((magnitude - 0.799999952f) / 0.45f);
		return GetCell(layer, column);
	}

	public Vector3 ToMapPos(Cell cell)
	{
		float d = GetCirclePos(cell.Layer + 1) - 0.225f;
		return RotatePointAroundPivot(Vector3.up * d, Vector3.zero, Vector3.back * (15f * (float)cell.Column + 7.5f));
	}

	private float GetCirclePos(int layerPos)
	{
		float num = 0f;
		if (layerPos == 0)
		{
			return 0.799999952f;
		}
		return (float)layerPos * 0.45f + 0.799999952f;
	}

	public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
	{
		return Quaternion.Euler(angles) * (point - pivot) + pivot;
	}

	public void OnDrawGizmos()
	{
	}
}