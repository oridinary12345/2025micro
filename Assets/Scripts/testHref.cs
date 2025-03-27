using UnityEngine;
using UnityEngine.UI.Extensions;

public class testHref : MonoBehaviour
{
	public TextPic textPic;

	private void Awake()
	{
		textPic = GetComponent<TextPic>();
	}

	private void OnEnable()
	{
		textPic.onHrefClick.AddListener(OnHrefClick);
	}

	private void OnDisable()
	{
		textPic.onHrefClick.RemoveListener(OnHrefClick);
	}

	private void OnHrefClick(string hrefName)
	{
		UnityEngine.Debug.Log("Click on the " + hrefName);
	}
}