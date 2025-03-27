using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WorldSkin : MonoBehaviour
{
	public Sprite Background;

	public Sprite OverlayTop;

	public Sprite OverlayBottom;

	public List<WorldFX> FXs;

	public List<WorldFX> UIFXs;

	public float OverlayTopPosY;

	public float OverlayTopPosYLarge;

	public float OverlayBottomPosYLarge;

	public Rect BackgroundUI;

	public Rect BackgroundUILarge;

	public Vector2 UIOverlayTopSize;

	public float UIOverlayTopPosY;

	public Vector2 UIOverlayBottomSize;

	public float UIOverlayBottomPosY;

	public Vector2 UILargeOverlayTopSize;

	public float UILargeOverlayTopPosY;

	public Vector2 UILargeOverlayBottomSize;

	public float UILargeOverlayBottomPosY;
}