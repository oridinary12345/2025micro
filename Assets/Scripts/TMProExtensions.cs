using TMPro;
using UnityEngine;

public static class TMProExtensions
{
	public static void SwapMaterial(this TextMeshProUGUI oldText, string newMaterialName)
	{
		oldText.fontSharedMaterial = (Resources.Load("Fonts & Materials/" + newMaterialName, typeof(Material)) as Material);
	}
}