using UnityEngine;

[RequireComponent(typeof(UIGameButton))]
public class UIGameButtonStyle : MonoBehaviour
{
	public Sprite EnabledSprite;

	public Sprite DisabledSprite;

	private UIGameButton _button;

	private void Awake()
	{
		_button = GetComponent<UIGameButton>();
		if (EnabledSprite == null && _button.image != null)
		{
			EnabledSprite = _button.image.sprite;
		}
	}

	public Sprite GetStateSprite(bool isDisabled)
	{
		if (isDisabled)
		{
			return DisabledSprite;
		}
		return EnabledSprite;
	}
}