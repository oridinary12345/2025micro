using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class UIShopPanel : UIMenuPage
{
    [SerializeField]
    private UIGameButton _closeButton;

    [SerializeField]
    private RectTransform _viewportContent;

    [SerializeField]
    private RectTransform _chestContent;

    [SerializeField]
    private RectTransform _rubiesContent;

    [SerializeField]
    private RectTransform _coinsContent;

    private UINoAdsAvailablePanel _noAdsPopup;

    private UIRewardsController _menuRewardController;

    private UIShopPanelBox _freeRubiesShopBox;

    private PlayerChestManager _chestManager;

    private FreeRubiesProfile _freeRubiesProfile;

    private UIShopPanelBoxChest _chestAds;

    private UIShopPanelBoxChest _chestPremium1;

    private UIShopPanelBoxChest _chestPremium2;

    private readonly Dictionary<string, UIShopPanelBox> _productBoxes = new Dictionary<string, UIShopPanelBox>();

    private const string FreeRubiesDateKey = "FreeRubiesKey";

    public void Init(PlayerChestManager chestManager)
    {
        _chestManager = chestManager;
        _closeButton.OnClick(OnCloseButtonClicked);
        _closeButton.ActivateOnBackKey();
        _noAdsPopup = UINoAdsAvailablePanel.Create();
        _menuRewardController = UIRewardsController.Create();
        _freeRubiesProfile = new FreeRubiesProfile();
        _freeRubiesProfile.ConsumedDate.Time = DateTime.UtcNow;
        LoadFreeRubiesProfile();
        _chestAds = UnityEngine.Object.Instantiate(Resources.Load<UIShopPanelBoxChest>("UI/ShopChestBox"));
        _chestAds.Init(chestManager.GetChestData("chestTimedFree"), "UI/UI_shop_chest01");
        _chestAds.Button.OnClick(OnFreeBoxButtonClicked);
        _chestAds.GetComponent<RectTransform>().SetParent(_chestContent, false);
        _chestPremium1 = UnityEngine.Object.Instantiate(Resources.Load<UIShopPanelBoxChest>("UI/ShopChestBox"));
        _chestPremium1.Init(chestManager.GetChestData("chestPremium1"), "UI/UI_shop_chest02");
        _chestPremium1.Button.OnClick(OnPremium1ButtonClicked);
        _chestPremium1.GetComponent<RectTransform>().SetParent(_chestContent, false);
        _chestPremium2 = UnityEngine.Object.Instantiate(Resources.Load<UIShopPanelBoxChest>("UI/ShopChestBox"));
        _chestPremium2.Init(chestManager.GetChestData("chestPremium2"), "UI/UI_shop_chest03");
        _chestPremium2.Button.OnClick(OnPremium2ButtonClicked);
        _chestPremium2.GetComponent<RectTransform>().SetParent(_chestContent, false);
        List<ShopProductConfig> productConfigs = ShopProductCatalog.GetProductConfigs(ShopProductType.Rubies);
        foreach (ShopProductConfig product in productConfigs)
        {
            UIShopPanelBox uIShopPanelBox = CreateProductBox(product);
            uIShopPanelBox.GetComponent<RectTransform>().SetParent(_rubiesContent, false);
            uIShopPanelBox.GetComponent<UIGameButton>().OnClick(delegate
            {
                OnProductClicked(product);
            });
            if (product.Id == "productRubiesVideo")
            {
                _freeRubiesShopBox = uIShopPanelBox;
            }
            _productBoxes[product.Id] = uIShopPanelBox;
        }
        List<ShopProductConfig> productConfigs2 = ShopProductCatalog.GetProductConfigs(ShopProductType.Coins);
        foreach (ShopProductConfig product2 in productConfigs2)
        {
            UIShopPanelBox uIShopPanelBox2 = CreateProductBox(product2);
            uIShopPanelBox2.GetComponent<RectTransform>().SetParent(_coinsContent, false);
            uIShopPanelBox2.GetComponent<UIGameButton>().OnClick(delegate
            {
                OnProductClicked(product2);
            });
            _productBoxes[product2.Id] = uIShopPanelBox2;
        }
        UpdateContent();
        RefreshProducts();
    }

    private void OnEnable()
    {
        RegisterEvents();
    }

    private void OnDisable()
    {
        UnRegisterEvents();
    }

    public override void OnPush()
    {
        RefreshProducts();
        RefreshFreeRubiesButton();
        UpdateContent();
        base.OnPush();
    }

    private void UpdateContent()
    {
        _chestAds.UpdateContent();
        _chestPremium1.UpdateContent();
        _chestPremium2.UpdateContent();
    }

    private void RegisterEvents()
    {
        UnRegisterEvents();
        MonoSingleton<GameAdsController>.Instance.Events.VideoPlayFailedEvent += OnVideoPlayFailed;
        MonoSingleton<GameAdsController>.Instance.Events.VideoRewardCompletedEvent += OnVideoRewardCompleted;
        MonoSingleton<GameAdsController>.Instance.Events.VideoAvailabilityChangedEvent += OnVideoAvailabilityChanged;
    }

    private void UnRegisterEvents()
    {
        if (MonoSingleton<GameAdsController>.IsCreated())
        {
            MonoSingleton<GameAdsController>.Instance.Events.VideoPlayFailedEvent -= OnVideoPlayFailed;
            MonoSingleton<GameAdsController>.Instance.Events.VideoRewardCompletedEvent -= OnVideoRewardCompleted;
            MonoSingleton<GameAdsController>.Instance.Events.VideoAvailabilityChangedEvent -= OnVideoAvailabilityChanged;
        }
    }

    private void RefreshProducts()
    {
        RefreshRubies();
        RefreshCoins();
    }

    private void RefreshRubies()
    {
        List<ShopProductConfig> productConfigs = ShopProductCatalog.GetProductConfigs(ShopProductType.Rubies);
        List<Product> products = MonoSingleton<InAppManager>.Instance.GetProducts(productConfigs.ConvertAll((ShopProductConfig c) => c.IAPId));
        foreach (ShopProductConfig item in productConfigs)
        {
            string iapId = item.IAPId;
            Product product = products.Find((Product p) => p.definition.id == iapId);
            if (_productBoxes.ContainsKey(item.Id))
            {
                string price = (product == null) ? string.Empty : product.metadata.localizedPriceString;
                string description = item.Amount.ToString("### ### ###").Trim();
                string imagePath = item.ImagePath;
                Color color = "#FF3346FF".ToColor();
                if (item.Id == "productRubiesVideo")
                {
                    _productBoxes[item.Id].SetPriceFont("Fonts & Materials/FiraSansExtraCondensed-ExtraBold SDF", "Fonts & Materials/FiraSansExtraCondensed-ExtraBold SDF - NoOutline");
                    price = item.Description;
                }
                _productBoxes[item.Id].UpdateProduct(price, description, imagePath, color, item.Bonus);
            }
        }
    }

    private void RefreshCoins()
    {
        List<ShopProductConfig> productConfigs = ShopProductCatalog.GetProductConfigs(ShopProductType.Coins);
        foreach (ShopProductConfig item in productConfigs)
        {
            if (_productBoxes.ContainsKey(item.Id))
            {
                string price = LootProfile.Create(item.PriceLootId, item.PriceLootAmount).ToString();
                string description = GetCoinsPackRewardAmount(item).ToString("### ### ###").Trim();
                string imagePath = item.ImagePath;
                Color color = "#FF8E02FF".ToColor();
                _productBoxes[item.Id].UpdateProduct(price, description, imagePath, color, item.Bonus);
            }
        }
    }

    private void RefreshFreeRubiesButton()
    {
        if (_freeRubiesProfile.ConsumedDate.Time.AddDays(1.0) < DateTime.UtcNow)
        {
            _freeRubiesProfile.Consumed = false;
        }
        _freeRubiesShopBox.SetActive(!_freeRubiesProfile.Consumed);
    }

    private void SaveFreeRubiesProfile()
    {
        string value = JsonConvert.SerializeObject(_freeRubiesProfile);
        PlayerPrefs.SetString("FreeRubiesKey", value);
    }

    private void LoadFreeRubiesProfile()
    {
        if (PlayerPrefs.HasKey("FreeRubiesKey"))
        {
            JsonConvert.PopulateObject(PlayerPrefs.GetString("FreeRubiesKey"), _freeRubiesProfile);
        }
    }

    private int GetCoinsPackRewardAmount(ShopProductConfig config)
    {
        return config.Amount;
    }

    private void OnProductClicked(ShopProductConfig productConfig)
    {
        //if (!productConfig.IsInApp())
        {
            if (productConfig.Id == "productRubiesVideo")
            {
                MonoSingleton<GameAdsController>.Instance.PlayRewardedVideo(GameAdsPlacement.FreeRubies);
            }
            else if (App.Instance.Player.LootManager.TryExpense(productConfig.PriceLootId, productConfig.PriceLootAmount, CurrencyReason.shop))
            {
                Reward reward = App.Instance.RewardFactory.Create(productConfig.RewardId);
                App.Instance.Player.RewardManager.Redeem(reward, CurrencyReason.shop);
                _menuRewardController.Show(new List<Reward>
                {
                    reward
                });
            }
            else
            {
                App.Instance.MenuEvents.OnTextOverlayRequested("You don't have enough rubies", new Vector3(0f, 300f, 0f));
            }
        }
    }

    private UIShopPanelBox CreateProductBox(ShopProductConfig product)
    {
        UIShopPanelBox uIShopPanelBox = UnityEngine.Object.Instantiate(Resources.Load<UIShopPanelBox>("UI/ShopBox"));
        return uIShopPanelBox.Init(product);
    }

    private void OnCloseButtonClicked()
    {
        if (MonoSingleton<UIMenuStack>.Instance.Peek() == this)
        {
            Hide();
        }
    }

    private void OnPremium1ButtonClicked()
    {
        Debug.LogError("1111111111111111111");
        ChestData chestData = _chestManager.GetChestData("chestPremium1");
        if (_chestManager.RedeemChest(chestData))
        {
            LootProfile price = chestData.GetPrice();
            if (App.Instance.Player.LootManager.TryExpense(price.LootId, price.Amount, CurrencyReason.chestPremium))
            {
                List<Reward> list = new List<Reward>();
                foreach (string rewardId in chestData.Config.RewardIds)
                {
                    Reward reward = App.Instance.RewardFactory.Create(rewardId);
                    if (reward != null)
                    {
                        App.Instance.Player.RewardManager.Redeem(reward, CurrencyReason.chestPremium);
                        list.Add(reward);
                    }
                }
                _menuRewardController.Show(list);
            }
        }
    }

    private void OnPremium2ButtonClicked()
    {
        ChestData chestData = _chestManager.GetChestData("chestPremium2");
        if (_chestManager.RedeemChest(chestData))
        {
            LootProfile price = chestData.GetPrice();
            if (App.Instance.Player.LootManager.TryExpense(price.LootId, price.Amount, CurrencyReason.chestPremium))
            {
                List<Reward> list = new List<Reward>();
                foreach (string rewardId in chestData.Config.RewardIds)
                {
                    Reward reward = App.Instance.RewardFactory.Create(rewardId);
                    if (reward != null)
                    {
                        App.Instance.Player.RewardManager.Redeem(reward, CurrencyReason.chestPremium);
                        list.Add(reward);
                    }
                }
                _menuRewardController.Show(list);
            }
        }
    }

    private void OnFreeBoxButtonClicked()
    {
        ChestData chestData = _chestManager.GetChestData("chestTimedFree");
        if (_chestManager.RedeemChest(chestData))
        {
            List<Reward> list = new List<Reward>();
            foreach (string rewardId in chestData.Config.RewardIds)
            {
                Reward reward = App.Instance.RewardFactory.Create(rewardId);
                if (reward != null)
                {
                    App.Instance.Player.RewardManager.Redeem(reward, CurrencyReason.chestFree);
                    list.Add(reward);
                }
            }
            _menuRewardController.Show(list);
        }
    }

    private void OnVideoAvailabilityChanged(bool isAvailable)
    {
    }

    private void OnVideoPlayFailed()
    {
        _noAdsPopup.Show();
    }

    private void OnVideoRewardCompleted(string placementId, List<Reward> rewards)
    {
        if (placementId == GameAdsPlacement.FreeRubies.ToString())
        {
            _freeRubiesProfile.Consumed = true;
            _freeRubiesProfile.ConsumedDate.Time = DateTime.UtcNow.Date;
            SaveFreeRubiesProfile();
            if (_menuRewardController.IsShowingRewards())
            {
                UnityEngine.Debug.LogWarning("UIShopPanel.OnVideoRewardCompleted() was about show rewards while we already showing some");
                return;
            }
            _menuRewardController.Show(rewards);
            RefreshFreeRubiesButton();
        }
    }
}