using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIShopPanelBox : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI _priceText;

	[SerializeField]
	private TextMeshProUGUI _descriptionText;

	[SerializeField]
	private TextMeshProUGUI _descriptionColoredText;

	[SerializeField]
	private Image _productImage;

	[SerializeField]
	private GameObject _bonusBadge;

	[SerializeField]
	private TextMeshProUGUI _bonusText;

	private Dictionary<object, Color> _originalColor = new Dictionary<object, Color>();

	public UIShopPanelBox Init(ShopProductConfig product)
	{
		if (_originalColor.Count == 0)
		{
			TextMeshProUGUI[] componentsInChildren = _descriptionText.GetComponentsInChildren<TextMeshProUGUI>();
			foreach (TextMeshProUGUI textMeshProUGUI in componentsInChildren)
			{
				_originalColor[textMeshProUGUI] = textMeshProUGUI.color;
			}
			_originalColor[_productImage] = _productImage.color;
			_originalColor[_priceText] = _priceText.color;
			_originalColor[_descriptionColoredText] = _descriptionColoredText.color;
		}
		return this;
	}

	private void OnEnable()
	{
		App.Instance.Player.LootManager.Events.LootUpdatedEvent += OnLootUpdated;
	}

	private void OnDisable()
	{
		if (App.IsCreated())
		{
			App.Instance.Player.LootManager.Events.LootUpdatedEvent -= OnLootUpdated;
		}
	}

	private void OnLootUpdated(LootProfile loot, int delta, CurrencyReason reason)
	{
		string lootId = loot.LootId;
		if (!(lootId == "lootCoin"))
		{
		}
	}

	public void SetPriceFont(string fontPath, string matPath)
	{
		_priceText.font = (Resources.Load(fontPath, typeof(TMP_FontAsset)) as TMP_FontAsset);
		_priceText.fontMaterial = (Resources.Load(matPath, typeof(Material)) as Material);
	}

	public void UpdateProduct(string price, string description, string imagePath, Color color, string bonus)
	{
		for (int num = price.Length - 1; num >= 0; num--)
		{
			if (!_priceText.font.HasCharacter(price[num]))
			{
				price = price.Remove(num, 1);
			}
		}
		_priceText.text = price;
		TextMeshProUGUI[] componentsInChildren = _descriptionText.GetComponentsInChildren<TextMeshProUGUI>();
		foreach (TextMeshProUGUI textMeshProUGUI in componentsInChildren)
		{
			textMeshProUGUI.text = description;
		}
		_descriptionColoredText.color = color;
		_productImage.sprite = Resources.Load<Sprite>(imagePath);
		_bonusText.text = bonus;
		_bonusBadge.SetActive(!string.IsNullOrEmpty(bonus));
	}

	public void SetActive(bool isActive)
	{
		UIGameButton component = GetComponent<UIGameButton>();
		component.interactable = isActive;
		component.SetDisabledExplanation("Please come back tomorrow!");
		if (!isActive)
		{
			Color color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
			TextMeshProUGUI[] componentsInChildren = _descriptionText.GetComponentsInChildren<TextMeshProUGUI>();
			foreach (TextMeshProUGUI textMeshProUGUI in componentsInChildren)
			{
				textMeshProUGUI.color = color;
			}
			_productImage.color = color;
			_priceText.color = color;
			_descriptionColoredText.color = color;
			return;
		}
		TextMeshProUGUI[] componentsInChildren2 = _descriptionText.GetComponentsInChildren<TextMeshProUGUI>();
		foreach (TextMeshProUGUI textMeshProUGUI2 in componentsInChildren2)
		{
			if (_originalColor.ContainsKey(textMeshProUGUI2))
			{
				textMeshProUGUI2.color = _originalColor[textMeshProUGUI2];
			}
		}
		if (_originalColor.ContainsKey(_productImage))
		{
			_productImage.color = _originalColor[_productImage];
		}
		if (_originalColor.ContainsKey(_priceText))
		{
			_priceText.color = _originalColor[_priceText];
		}
		if (_originalColor.ContainsKey(_descriptionColoredText))
		{
			_descriptionColoredText.color = _originalColor[_descriptionColoredText];
		}
	}
}