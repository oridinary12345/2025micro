using DG.Tweening;
using System.Collections;
using UnityEngine;

public class WorldRenderer : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer Background;

	[SerializeField]
	private SpriteRenderer OverlayTop;

	[SerializeField]
	private SpriteRenderer OverlayBottom;

	[SerializeField]
	private Transform FXHolder;

	[SerializeField]
	private SpriteRenderer TargetSprite;

	public void Init(string skinName)
	{
		WorldSkin worldSkin = Resources.Load<WorldSkin>("World/" + skinName);
		if (worldSkin == null)
		{
			UnityEngine.Debug.LogWarning("Can't load world' skin: " + skinName);
			return;
		}
		Background.sprite = worldSkin.Background;
		OverlayTop.sprite = worldSkin.OverlayTop;
		OverlayBottom.sprite = worldSkin.OverlayBottom;
		if (OverlayTop != null)
		{
			Transform transform = OverlayTop.transform;
			Vector3 localPosition = OverlayTop.transform.localPosition;
			float x = localPosition.x;
			float overlayTopPosY = worldSkin.OverlayTopPosY;
			Vector3 localPosition2 = OverlayTop.transform.localPosition;
			transform.localPosition = new Vector3(x, overlayTopPosY, localPosition2.z);
		}
		for (int num = FXHolder.transform.childCount - 1; num >= 0; num--)
		{
			Transform child = FXHolder.GetChild(num);
			child.parent = null;
			UnityEngine.Object.Destroy(child.gameObject);
		}
		if (worldSkin.FXs != null)
		{
			foreach (WorldFX fX in worldSkin.FXs)
			{
				if (fX.FX != null)
				{
					Transform transform2 = UnityEngine.Object.Instantiate(fX.FX);
					transform2.parent = FXHolder;
					if (Camera.main.aspect < 0.5294118f)
					{
						transform2.localPosition = fX.LargePosition;
					}
					else
					{
						transform2.localPosition = fX.Position;
					}
				}
			}
		}
		if (Camera.main.aspect < 0.5294118f)
		{
			Background.transform.localScale = Vector3.one * 1.3f;
			OverlayTop.transform.localScale = Vector3.one * 1.3f;
			OverlayBottom.transform.localScale = Vector3.one * 1.3f;
			Vector3 localPosition3 = OverlayTop.transform.localPosition;
			localPosition3.y = worldSkin.OverlayTopPosYLarge;
			OverlayTop.transform.localPosition = localPosition3;
			Vector3 localPosition4 = OverlayBottom.transform.localPosition;
			localPosition4.y = worldSkin.OverlayBottomPosYLarge;
			OverlayBottom.transform.localPosition = localPosition4;
		}
	}

	public void ShowTarget()
	{
		StartCoroutine(ShowTargetCR());
	}

	private IEnumerator ShowTargetCR()
	{
		yield return null;
		TargetSprite.color = new Color(1f, 1f, 1f, 0f);
		TargetSprite.enabled = true;
		TargetSprite.DOFade(1f, 0.5f);
	}
}