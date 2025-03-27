using System.Collections.Generic;

public static class ElementExtension
{
	private static readonly Dictionary<Element, Element> _strong = new Dictionary<Element, Element>();

	private static readonly Dictionary<Element, Element> _weak = new Dictionary<Element, Element>();

	public static float GetDamageFactor(this Element current, Element testedAgainst)
	{
		if (current != 0 && testedAgainst != 0)
		{
			if (_strong[current] == testedAgainst)
			{
				return 2f;
			}
			if (_weak[current] == testedAgainst)
			{
				return 0.5f;
			}
		}
		return 1f;
	}

	public static string GetImagePath(this Element element)
	{
		return "Elements/element_" + element;
	}
}