using System.Collections.Generic;

public static class InlineSprites
{
	public const int KingToken = 0;

	public const int Heart = 1;

	public const int Ruby = 2;

	public const int Coin = 3;

	public const int Video = 4;

	public const int Key = 5;

	private static readonly Dictionary<string, int> _inlineSprites = new Dictionary<string, int>
	{
		{
			"lootCoin",
			3
		},
		{
			"lootLife",
			1
		},
		{
			"lootRuby",
			2
		},
		{
			"lootKey",
			5
		}
	};

	public static string GetLootInlineSprite(string lootId)
	{
		if (!_inlineSprites.ContainsKey(lootId))
		{
			return string.Empty;
		}
		return GetInlineSprite(_inlineSprites[lootId]);
	}

	public static string GetInlineSprite(int id)
	{
		return $"<sprite={id}>";
	}
}