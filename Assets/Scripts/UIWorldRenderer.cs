using UnityEngine;
using UnityEngine.UI;

public class UIWorldRenderer : MonoBehaviour
{
	[SerializeField]
	private Image Background;

	[SerializeField]
	private Image OverlayTop;

	[SerializeField]
	private Image OverlayBottom;

	[SerializeField]
	private Transform FXHolder;

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
		OverlayTop.enabled = (OverlayTop.sprite != null);
		OverlayBottom.enabled = (OverlayBottom.sprite != null);
		RectTransform component = Background.GetComponent<RectTransform>();
		if (Camera.main.aspect < 0.5294118f)
		{
			component.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, worldSkin.BackgroundUILarge.width);
			component.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, worldSkin.BackgroundUILarge.height);
			Vector3 localPosition = component.localPosition;
			localPosition.y = worldSkin.BackgroundUILarge.yMin;
			component.localPosition = localPosition;
			RectTransform component2 = OverlayTop.GetComponent<RectTransform>();
			component2.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, worldSkin.UILargeOverlayTopSize.x);
			component2.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, worldSkin.UILargeOverlayTopSize.y);
			localPosition = component2.localPosition;
			localPosition.y = worldSkin.UILargeOverlayTopPosY;
			component2.localPosition = localPosition;
			RectTransform component3 = OverlayBottom.GetComponent<RectTransform>();
			component3.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, worldSkin.UILargeOverlayBottomSize.x);
			component3.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, worldSkin.UILargeOverlayBottomSize.y);
			localPosition = component3.localPosition;
			localPosition.y = worldSkin.UILargeOverlayBottomPosY;
			component3.localPosition = localPosition;
		}
		else
		{
			RectTransform component4 = OverlayTop.GetComponent<RectTransform>();
			component4.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, worldSkin.UIOverlayTopSize.x);
			component4.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, worldSkin.UIOverlayTopSize.y);
			Vector3 localPosition2 = component4.localPosition;
			localPosition2.y = worldSkin.UIOverlayTopPosY;
			component4.localPosition = localPosition2;
			RectTransform component5 = OverlayBottom.GetComponent<RectTransform>();
			component5.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, worldSkin.UIOverlayBottomSize.x);
			component5.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, worldSkin.UIOverlayBottomSize.y);
			localPosition2 = component5.localPosition;
			localPosition2.y = worldSkin.UIOverlayBottomPosY;
			component5.localPosition = localPosition2;
		}
		if (worldSkin.UIFXs != null)
		{
			foreach (WorldFX uIFX in worldSkin.UIFXs)
			{
				if (uIFX.FX != null)
				{
					Transform transform = UnityEngine.Object.Instantiate(uIFX.FX);
					if (Camera.main.aspect < 0.5294118f)
					{
						transform.localPosition = uIFX.LargePosition;
					}
					else
					{
						transform.localPosition = uIFX.Position;
					}
					transform.gameObject.SetSortingOrder(101);
					RectTransform component6 = transform.gameObject.GetComponent<RectTransform>();
					component6.SetParent(FXHolder, true);
				}
			}
		}
	}
}