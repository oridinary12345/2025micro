using TMPro;
using UnityEngine;

public class DebugHUD : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI _label;

	private void Awake()
	{
		base.gameObject.SetActive(UIMenuCheat.ShowDebugHUD);
	}

	public void SetText(string text)
	{
		_label.text = text;
	}
}