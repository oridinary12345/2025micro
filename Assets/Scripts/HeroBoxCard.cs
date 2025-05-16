using UnityEngine;
using UnityEngine.UI;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
using TMPro;
#endif

public class HeroBoxCard : MonoBehaviour
{
	[SerializeField]
	private ResourceDisplay _heroHPDisplay;
	
	[SerializeField]
	private Sprite _hpIcon; // 在Inspector中手动设置的心形图标
	
	[Tooltip("指定心形图标的名称，如heart_full或heart_ico")]
	[SerializeField]
	private string _hpIconName = "heart_full"; // 默认使用heart_full图标

	[SerializeField]
	private Text _heroLevelText;

	private HeroData _hero;

	public HeroData HeroData => _hero;

	public HeroBoxCard Init(HeroData heroData)
	{
		_hero = heroData;
		if (_hero != null)
		{
			_hero.Events.HeroLevelUpEvent += OnHeroLevelUp;
		}
		UpdateHeroStats();
		return this;
	}

	private void OnDestroy()
	{
		if (_hero != null)
		{
			_hero.Events.HeroLevelUpEvent -= OnHeroLevelUp;
		}
	}

	private void OnHeroLevelUp(HeroData hero)
	{
		UpdateHeroStats();
	}

	private void UpdateHeroStats()
	{
		if (_hero == null || _hero.HeroConfig == null || _hero.Profile == null)
		{
			Debug.LogWarning("UpdateHeroStats: Missing hero data");
			return;
		}

		try
		{
			SetHP(_hero.HeroConfig.GetHPMax(_hero.Profile.Level));
			SetLevel(_hero.Profile.Level);
		}
		catch (System.Exception ex)
		{
			Debug.LogError($"Error updating hero stats: {ex.Message}");
		}
	}

	private void SetHP(int hp)
	{
		if (_heroHPDisplay == null)
		{
			Debug.LogWarning("SetHP: Missing HP display component");
			return;
		}

		// 尝试从多个来源获取图标
		Sprite hpIcon = null;
		
		// 1. 尝试从SpriteAssetManager获取TextMeshPro图集中的图标
		if (SpriteAssetManager.Instance != null)
		{
			// 原来的富文本标签是<sprite=1>，所以使用索引1
			hpIcon = SpriteAssetManager.Instance.GetSprite(1);
			
			if (hpIcon != null)
			{
				Debug.Log($"成功从SpriteAssetManager获取心形图标: {hpIcon.name}");
			}
			else
			{
				Debug.LogWarning("SpriteAssetManager中的心形图标为null");
				
				// 如果SpriteAssetManager失败，尝试使用ResourceManager
				if (ResourceManager.Instance != null)
				{
					hpIcon = ResourceManager.Instance.GetResourceIconBySpriteIndex(1);
					
					if (hpIcon != null)
					{
						Debug.Log($"成功使用ResourceManager获取心形图标: {hpIcon.name}");
					}
					else
					{
						Debug.LogWarning("ResourceManager中的心形图标也为null");
					}
				}
			}
		}
		else if (ResourceManager.Instance != null)
		{
			// 如果SpriteAssetManager不存在，尝试使用ResourceManager
			hpIcon = ResourceManager.Instance.GetResourceIconBySpriteIndex(1);
			
			if (hpIcon != null)
			{
				Debug.Log($"成功使用ResourceManager获取心形图标: {hpIcon.name}");
			}
			else
			{
				Debug.LogWarning("ResourceManager中的心形图标为null");
			}
		}
		else
		{
			Debug.LogWarning("ResourceManager实例不存在");
		}
		
		// 2. 如果从ResourceManager获取失败，使用备用图标
		if (hpIcon == null)
		{
			hpIcon = _hpIcon;
			if (hpIcon == null)
			{
				Debug.LogWarning("_hpIcon也为null，请在Inspector中设置");
				
				// 3. 尝试在运行时查找图标
				StartCoroutine(FindHPIconAndUpdate(hp));
				return;
			}
		}
		
		// 设置图标和数值
		_heroHPDisplay.SetValue(hpIcon, hp);
	}
	
	// 尝试在运行时查找图标并更新UI
	private IEnumerator FindHPIconAndUpdate(int hp)
	{
		// 尝试查找项目中的指定心形图标
		Sprite foundSprite = null;
		
		// 首先尝试从SpriteAssetManager获取
		if (SpriteAssetManager.Instance != null)
		{
			foundSprite = SpriteAssetManager.Instance.GetSprite(1); // 原来的富文本标签是<sprite=1>
			if (foundSprite != null)
			{
				Debug.Log($"从SpriteAssetManager获取到心形图标: {foundSprite.name}");
			}
		}
		
		// 如果还是找不到，尝试在编辑器中查找
		#if UNITY_EDITOR
		if (foundSprite == null)
		{
			// 尝试直接加载图标资源
			Object[] sprites = AssetDatabase.LoadAllAssetsAtPath("Assets/Resources/sprite assets/InlineSpriteAtlas.png");
			foreach (Object obj in sprites)
			{
				if (obj is Sprite sprite)
				{
					if (sprite.name.Contains("InlineSpriteAtlas_1") || sprite.name.ToLower().Contains("heart"))
					{
						foundSprite = sprite;
						Debug.Log($"从InlineSpriteAtlas直接加载到心形图标: {foundSprite.name}");
						break;
					}
				}
			}
			
			// 如果还是找不到，尝试查找指定的心形图标
			if (foundSprite == null)
			{
				string[] guids = AssetDatabase.FindAssets(_hpIconName + " t:Sprite");
				if (guids.Length > 0)
				{
					string path = AssetDatabase.GUIDToAssetPath(guids[0]);
					foundSprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
					Debug.Log($"找到指定的心形图标: {foundSprite.name} at {path}");
				}
				else
				{
					// 如果找不到指定的图标，尝试查找任何心形图标
					guids = AssetDatabase.FindAssets("heart t:Sprite");
					foreach (string guid in guids)
					{
						string path = AssetDatabase.GUIDToAssetPath(guid);
						Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
						if (sprite != null)
						{
							foundSprite = sprite;
							Debug.Log($"找到备用心形图标: {sprite.name} at {path}");
							break;
						}
					}
				}
			}
		}
		#endif
		
		// 如果还是找不到，尝试直接从资源加载
		if (foundSprite == null)
		{
			// 尝试加载InlineSpriteAtlas中的图标
			Object spriteAssetObj = Resources.Load("sprite assets/InlineSpriteAtlas");
			if (spriteAssetObj != null)
			{
				Debug.Log($"加载到InlineSpriteAtlas: {spriteAssetObj.name}");
			}
			
			// 如果还是找不到，尝试加载单独的图标
			foundSprite = Resources.Load<Sprite>("Sprites/" + _hpIconName);
			if (foundSprite != null)
			{
				Debug.Log($"从资源加载心形图标: {foundSprite.name}");
			}
		}
		
		// 如果找到了图标，使用它并保存到_hpIcon
		if (foundSprite != null)
		{
			_hpIcon = foundSprite;
			_heroHPDisplay.SetValue(foundSprite, hp);
			Debug.Log("成功找到并设置了HP图标");
		}
		else
		{
			// 如果还是找不到，只显示数字
			Debug.LogError("无法找到HP图标，只显示数字");
			_heroHPDisplay.SetValueOnly(hp);
		}
		
		yield return null;
	}

	private void SetLevel(int level)
	{
		if (_heroLevelText == null)
		{
			Debug.LogWarning("SetLevel: Missing level text component");
			return;
		}

		_heroLevelText.text = "Lv." + level;
	}
}