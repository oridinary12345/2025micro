using UnityEngine;

public class UIMenuHudPanel : MonoBehaviour
{
	[SerializeField]
	private UILootBar _coinsBar;

	[SerializeField]
	private UILootBar _rubyBar;

	private void Awake()
	{
		UnityCanvasContainer componentInChildren = base.transform.GetComponentInChildren<UnityCanvasContainer>();
		if (componentInChildren != null)
		{
			componentInChildren.Init();
		}
		_coinsBar.Init("lootCoin");
		_rubyBar.Init("lootRuby");
	}
}