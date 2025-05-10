using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShopPanelBox : MonoBehaviour
{
	[SerializeField]
	private Text _priceText;

	[SerializeField]
	private Text _descriptionText;

	[SerializeField]
	private Text _descriptionColoredText;

	[SerializeField]
	private Image _productImage;

	[SerializeField]
	private GameObject _bonusBadge;

	[SerializeField]
	private Text _bonusText;

	private Dictionary<object, Color> _originalColor = new Dictionary<object, Color>();

	public UIShopPanelBox Init(ShopProductConfig product)
	{
		if (product == null)
		{
			Debug.LogWarning("Product is null in UIShopPanelBox.Init");
			return this;
		}

		if (_originalColor.Count == 0)
		{
			if (_descriptionText != null)
			{
				Text[] componentsInChildren = _descriptionText.GetComponentsInChildren<Text>();
				if (componentsInChildren != null)
				{
					foreach (Text item in componentsInChildren)
					{
						if (item != null)
						{
							_originalColor[item] = item.color;
						}
					}
				}
			}

			if (_productImage != null)
			{
				_originalColor[_productImage] = _productImage.color;
			}

			if (_priceText != null)
			{
				_originalColor[_priceText] = _priceText.color;
			}

			if (_descriptionColoredText != null)
			{
				_originalColor[_descriptionColoredText] = _descriptionColoredText.color;
			}
		}

		if (_priceText != null)
		{
			_priceText.text = product.PriceLootAmount.ToString();
		}

		if (_descriptionText != null)
		{
			_descriptionText.text = product.Description;
		}

		if (_descriptionColoredText != null)
		{
			_descriptionColoredText.text = product.Description;
		}

		if (_productImage != null)
		{
			Sprite sprite = Resources.Load<Sprite>(product.ImagePath);
			if (sprite != null)
			{
				_productImage.sprite = sprite;
			}
			else
			{
				Debug.LogWarning($"Failed to load sprite from path: {product.ImagePath}");
			}
		}

		if (_bonusBadge != null)
		{
			_bonusBadge.SetActive(!string.IsNullOrEmpty(product.Bonus));
		}

		if (_bonusText != null)
		{
			_bonusText.text = product.Bonus;
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
		if (_priceText != null)
		{
			_priceText.font = Resources.Load<Font>(fontPath);
			_priceText.material = Resources.Load<Material>(matPath);
		}
	}

	public void UpdateProduct(string price, string description, string imagePath, Color color, string bonus)
	{
		if (_priceText != null)
		{
			_priceText.text = price;
		}

		if (_descriptionText != null)
		{
			Text[] texts = _descriptionText.GetComponentsInChildren<Text>();
			if (texts != null)
			{
				foreach (Text text in texts)
				{
					if (text != null)
					{
						text.text = description;
					}
				}
			}
		}

		if (_descriptionColoredText != null)
		{
			_descriptionColoredText.color = color;
		}

		if (_productImage != null)
		{
			Sprite sprite = Resources.Load<Sprite>(imagePath);
			if (sprite != null)
			{
				_productImage.sprite = sprite;
			}
			else
			{
				Debug.LogWarning($"Failed to load sprite from path: {imagePath}");
			}
		}

		if (_bonusText != null)
		{
			_bonusText.text = bonus;
		}

		if (_bonusBadge != null)
		{
			_bonusBadge.SetActive(!string.IsNullOrEmpty(bonus));
		}
	}

	public void SetActive(bool isActive, Color? color = null)
	{
		Color targetColor = color ?? new Color(0.5f, 0.5f, 0.5f, 0.5f);

		UIGameButton component = GetComponent<UIGameButton>();
		if (component != null)
		{
			component.interactable = isActive;
			component.SetDisabledExplanation("Please come back tomorrow!");
		}

		if (_descriptionText != null)
		{
			Text[] texts = _descriptionText.GetComponentsInChildren<Text>();
			if (texts != null)
			{
				foreach (Text text in texts)
				{
					if (text != null)
					{
						text.color = isActive && _originalColor.ContainsKey(text) ? _originalColor[text] : targetColor;
					}
				}
			}
		}

		if (_productImage != null)
		{
			_productImage.color = isActive && _originalColor.ContainsKey(_productImage) ? _originalColor[_productImage] : targetColor;
		}

		if (_priceText != null)
		{
			_priceText.color = isActive && _originalColor.ContainsKey(_priceText) ? _originalColor[_priceText] : targetColor;
		}

		if (_descriptionColoredText != null)
		{
			_descriptionColoredText.color = isActive && _originalColor.ContainsKey(_descriptionColoredText) ? _originalColor[_descriptionColoredText] : targetColor;
		}

		if (_descriptionColoredText != null && _originalColor.ContainsKey(_descriptionColoredText))
		{
			_descriptionColoredText.color = _originalColor[_descriptionColoredText];
		}
	}
}