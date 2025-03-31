using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;

public class InAppManager : MonoSingleton<InAppManager>, IStoreListener
{
	private IStoreController _storeController;

	private IExtensionProvider _storeExtensionProvider;

	private string _purchaseIdInProgress;

	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		InitPurchasing();
	}

	private void InitPurchasing()
	{
		ConfigurationBuilder configurationBuilder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
		foreach (ShopProductConfig allConfig in ShopProductCatalog.GetAllConfigs())
		{
			if (allConfig.IsInApp())
			{
				configurationBuilder.AddProduct(allConfig.IAPId, allConfig.PurchaseType);
			}
		}
		UnityPurchasing.Initialize(this, configurationBuilder);
	}

	private bool IsInitialized()
	{
		return _storeController != null && _storeExtensionProvider != null;
	}

	public void BuyProduct(string productId)
	{
		if (!string.IsNullOrEmpty(_purchaseIdInProgress))
		{
			UnityEngine.Debug.Log("Please wait, purchase in progress: " + _purchaseIdInProgress);
			return;
		}
		if (!IsInitialized())
		{
			UnityEngine.Debug.LogWarning("BuyProduct FAIL. Not initialized.");
			return;
		}
		if (_storeController.products == null)
		{
			UnityEngine.Debug.LogWarning("BuyProduct FAIL. product list is empty.");
			return;
		}
		Product product = _storeController.products.WithID(productId);
		if (product != null && product.availableToPurchase)
		{
			UnityEngine.Debug.Log($"Purchasing product asychronously: '{product.definition.id}'");
			_purchaseIdInProgress = productId;
			_storeController.InitiatePurchase(product);
		}
		else
		{
			UnityEngine.Debug.LogWarning("BuyProduct: FAIL. Not purchasing product, either is not found or is not available for purchase");
		}
	}

	public void RestorePurchases()
	{
		if (!IsInitialized())
		{
			UnityEngine.Debug.LogWarning("RestorePurchases FAIL. Not initialized.");
		}
		else if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
		{
			UnityEngine.Debug.Log("RestorePurchases started ...");
			IAppleExtensions extension = _storeExtensionProvider.GetExtension<IAppleExtensions>();
			extension.RestoreTransactions(delegate(bool result)
			{
				UnityEngine.Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
			});
		}
		else
		{
			UnityEngine.Debug.LogWarning("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
		}
	}

	public List<Product> GetProducts(List<string> ids)
	{
		List<Product> list = new List<Product>();
		if (_storeController != null && _storeController.products != null)
		{
			Product[] all = _storeController.products.all;
			foreach (Product product in all)
			{
				if (ids.Contains(product.definition.id))
				{
					list.Add(product);
				}
			}
		}
		return list;
	}

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
		_storeController = controller;
		_storeExtensionProvider = extensions;
	}

	public void OnInitializeFailed(InitializationFailureReason error)
	{
		UnityEngine.Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
	}

	public void OnInitializeFailed(InitializationFailureReason error, string message)
	{
		UnityEngine.Debug.Log($"OnInitializeFailed InitializationFailureReason: {error}, Message: {message}");
	}

	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
		_purchaseIdInProgress = string.Empty;
		UnityEngine.Debug.Log($"OnPurchaseFailed: FAIL. Product: '{product.definition.storeSpecificId}', PurchaseFailureReason: {failureReason}");
	}

	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
	{
		UnityEngine.Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
		string id = args.purchasedProduct.definition.id;
		ShopProductConfig inAppConfig = ShopProductCatalog.GetInAppConfig(id);
		if (inAppConfig != null)
		{
			Reward reward = App.Instance.RewardFactory.Create(inAppConfig.RewardId);
			App.Instance.Player.RewardManager.Redeem(reward, CurrencyReason.iap);
			App.Instance.Events.OnInAppPurchaseDone(reward, args.purchasedProduct);
		}
		else
		{
			UnityEngine.Debug.LogWarning("IN-APP Product not found!");
		}
		_purchaseIdInProgress = string.Empty;
		return PurchaseProcessingResult.Complete;
	}
}