using UnityEngine.Purchasing;

public class ShopProductConfig
{
	public string Id;

	public string IAPId;

	public string Title;

	public string Description;

	public string RewardId;

	public string ImagePath;

	public string PriceLootId;

	public int PriceLootAmount;

	public string Bonus;

	public ProductType PurchaseType;

	public ShopProductType IAPType;

	public int Amount;

	public ShopProductConfig(string productId, string iapId, string title, string description, ShopProductType type, ProductType purchaseType, int amount, string bonus, string lootPriceId = "", int lootPriceAmount = 0)
	{
		Id = productId;
		if (string.IsNullOrEmpty(iapId))
		{
			IAPId = string.Empty;
		}
		else
		{
			IAPId = "com.dhb.microrpg." + iapId;
		}
		Title = title;
		Description = description;
		IAPType = type;
		PurchaseType = purchaseType;
		PriceLootId = lootPriceId;
		PriceLootAmount = lootPriceAmount;
		Bonus = bonus;
		RewardId = "reward" + productId.ToUpperFirst();
		ImagePath = "UI/shop" + productId.ToUpperFirst();
		Amount = amount;
	}

	public bool IsInApp()
	{
		return !string.IsNullOrEmpty(IAPId);
	}
}