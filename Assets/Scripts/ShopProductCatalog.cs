using System.Collections.Generic;
using UnityEngine.Purchasing;

public static class ShopProductCatalog
{
    public const string WatchVideoRubiesProductId = "productRubiesVideo";

    private static Dictionary<string, ShopProductConfig> _products = new Dictionary<string, ShopProductConfig>
    {
        {
            "productRubies1",
            new ShopProductConfig("productRubies1", "rubies1a", "Pack 1", string.Empty, ShopProductType.Rubies, ProductType.Consumable, 900, string.Empty, "lootCoin")
        },
        {
            "productRubies2",
            new ShopProductConfig("productRubies2", "rubies2", "Pack 2", string.Empty, ShopProductType.Rubies, ProductType.Consumable, 2000, "+27%","lootCoin")
        },
        {
            "productRubies3",
            new ShopProductConfig("productRubies3", "rubies3", "Pack 3", string.Empty, ShopProductType.Rubies, ProductType.Consumable, 5000, "+58%", "lootCoin")
        },
        {
            "productRubies4",
            new ShopProductConfig("productRubies4", "rubies4", "Pack 4", string.Empty, ShopProductType.Rubies, ProductType.Consumable, 12000, "+90%", "lootCoin")
        },
        {
            "productRubies5",
            new ShopProductConfig("productRubies5", "rubies5", "Pack 5", string.Empty, ShopProductType.Rubies, ProductType.Consumable, 35000, "+120%", "lootCoin")
        },
        {
            "productRubiesVideo",
            new ShopProductConfig("productRubiesVideo", string.Empty, "video", "Watch a video", ShopProductType.Rubies, ProductType.Consumable, 10, string.Empty, "lootCoin")
        }
    };

    public static List<ShopProductConfig> GetProductConfigs(ShopProductType iapType)
    {
        List<ShopProductConfig> list = new List<ShopProductConfig>();
        foreach (KeyValuePair<string, ShopProductConfig> product in _products)
        {
            if (product.Value.IAPType == iapType)
            {
                list.Add(product.Value);
            }
        }
        return list;
    }

    public static List<ShopProductConfig> GetAllConfigs()
    {
        return new List<ShopProductConfig>(_products.Values);
    }

    public static ShopProductConfig GetInAppConfig(string inAppId)
    {
        foreach (KeyValuePair<string, ShopProductConfig> product in _products)
        {
            if (product.Value.IAPId == inAppId)
            {
                return product.Value;
            }
        }
        return null;
    }
}