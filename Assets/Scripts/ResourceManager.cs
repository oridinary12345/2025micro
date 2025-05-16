using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 资源类型枚举
/// </summary>
public enum ResourceType
{
    Coin,
    Gem,
    Energy,
    Key,
    Heart,
    Star,
    // 添加更多资源类型...
}

/// <summary>
/// 资源管理器，用于管理游戏中的资源图标
/// </summary>
public class ResourceManager : MonoBehaviour
{
    private static ResourceManager _instance;
    public static ResourceManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ResourceManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("ResourceManager");
                    _instance = go.AddComponent<ResourceManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }

    [System.Serializable]
    public class ResourceIconMapping
    {
        public ResourceType resourceType;
        public Sprite icon;
    }

    [Tooltip("资源类型与图标的映射")]
    [SerializeField] private List<ResourceIconMapping> resourceIcons = new List<ResourceIconMapping>();

    // 缓存资源图标的字典，提高查询效率
    private Dictionary<ResourceType, Sprite> iconDictionary;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        // 初始化图标字典
        InitializeIconDictionary();
    }

    /// <summary>
    /// 初始化图标字典
    /// </summary>
    private void InitializeIconDictionary()
    {
        iconDictionary = new Dictionary<ResourceType, Sprite>();
        foreach (var mapping in resourceIcons)
        {
            if (!iconDictionary.ContainsKey(mapping.resourceType))
            {
                iconDictionary.Add(mapping.resourceType, mapping.icon);
            }
        }
    }

    /// <summary>
    /// 获取资源图标
    /// </summary>
    /// <param name="resourceType">资源类型</param>
    /// <returns>资源图标</returns>
    public Sprite GetResourceIcon(ResourceType resourceType)
    {
        if (iconDictionary == null)
        {
            InitializeIconDictionary();
        }

        if (iconDictionary.TryGetValue(resourceType, out Sprite icon))
        {
            return icon;
        }

        Debug.LogWarning($"未找到资源类型 {resourceType} 的图标");
        return null;
    }

    /// <summary>
    /// 通过索引获取资源图标（兼容旧的sprite索引方式）
    /// </summary>
    /// <param name="iconIndex">图标索引</param>
    /// <returns>资源图标</returns>
    public Sprite GetResourceIconByIndex(int iconIndex)
    {
        if (iconIndex >= 0 && iconIndex < resourceIcons.Count)
        {
            return resourceIcons[iconIndex].icon;
        }

        Debug.LogWarning($"未找到索引为 {iconIndex} 的图标");
        return null;
    }
    
    /// <summary>
    /// 将TextMeshPro的sprite索引映射到ResourceType
    /// </summary>
    /// <param name="spriteIndex">TextMeshPro中的sprite索引</param>
    /// <returns>对应的ResourceType</returns>
    public ResourceType MapSpriteIndexToResourceType(int spriteIndex)
    {
        switch (spriteIndex)
        {
            case 0: return ResourceType.Coin;
            case 1: return ResourceType.Heart; // 假设在TextMeshPro中索引1是心形
            case 2: return ResourceType.Gem;
            case 3: return ResourceType.Energy;
            case 4: return ResourceType.Key;
            case 5: return ResourceType.Star;
            default: return ResourceType.Coin;
        }
    }
    
    /// <summary>
    /// 通过TextMeshPro的sprite索引获取资源图标
    /// </summary>
    /// <param name="spriteIndex">TextMeshPro中的sprite索引</param>
    /// <returns>对应的资源图标</returns>
    public Sprite GetResourceIconBySpriteIndex(int spriteIndex)
    {
        ResourceType type = MapSpriteIndexToResourceType(spriteIndex);
        return GetResourceIcon(type);
    }
}
