using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// 精灵图集管理器，用于获取TextMeshPro的Sprite Asset中的图标
/// </summary>
public class SpriteAssetManager : MonoBehaviour
{
    private static SpriteAssetManager _instance;
    public static SpriteAssetManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SpriteAssetManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("SpriteAssetManager");
                    _instance = go.AddComponent<SpriteAssetManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }

    [SerializeField] private Object _spriteAsset;
    [SerializeField] private List<Sprite> _spriteList = new List<Sprite>();
    private bool _initialized = false;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
        
        // 初始化
        Initialize();
    }

    /// <summary>
    /// 初始化精灵图集
    /// </summary>
    public void Initialize()
    {
        if (_initialized)
            return;

        if (_spriteList.Count == 0)
        {
            // 尝试直接加载图标
            LoadSprites();
            
            if (_spriteList.Count == 0)
            {
                Debug.LogError("无法加载图标，请确保图标资源存在");
                return;
            }
        }

        _initialized = true;
        Debug.Log($"成功初始化SpriteAssetManager，加载了{_spriteList.Count}个图标");
    }

    /// <summary>
    /// 获取精灵图标
    /// </summary>
    /// <param name="spriteIndex">图标索引</param>
    /// <returns>图标精灵</returns>
    public Sprite GetSprite(int spriteIndex)
    {
        if (!_initialized)
            Initialize();

        if (_spriteList.Count == 0)
        {
            Debug.LogError("图标列表为空");
            return null;
        }

        // 查找指定索引的图标
        // 注意：这里的索引可能与原来的sprite索引不完全一致
        // 我们假设索引1对应的是第二个图标（索引1）
        if (spriteIndex == 1 && _spriteList.Count > 0)
        {
            // 尝试找到名称中包含InlineSpriteAtlas_1的图标
            foreach (Sprite sprite in _spriteList)
            {
                if (sprite.name.Contains("InlineSpriteAtlas_1") || 
                    sprite.name.ToLower().Contains("heart"))
                {
                    return sprite;
                }
            }
            
            // 如果找不到特定名称的图标，尝试返回第二个图标（如果存在）
            if (_spriteList.Count > 1)
            {
                return _spriteList[1];
            }
            // 如果只有一个图标，返回第一个
            return _spriteList[0];
        }
        
        // 如果是其他索引，尝试直接根据索引返回
        if (spriteIndex >= 0 && spriteIndex < _spriteList.Count)
        {
            return _spriteList[spriteIndex];
        }

        Debug.LogWarning($"未找到索引为{spriteIndex}的图标");
        return null;
    }

    /// <summary>
    /// 获取精灵图标
    /// </summary>
    /// <param name="spriteName">图标名称</param>
    /// <returns>图标精灵</returns>
    public Sprite GetSprite(string spriteName)
    {
        if (!_initialized)
            Initialize();

        if (_spriteList.Count == 0)
        {
            Debug.LogError("图标列表为空");
            return null;
        }

        // 查找指定名称的图标
        foreach (Sprite sprite in _spriteList)
        {
            if (sprite.name.Contains(spriteName))
            {
                return sprite;
            }
        }

        Debug.LogWarning($"未找到名称为{spriteName}的图标");
        return null;
    }
    
    /// <summary>
    /// 获取爱心图标
    /// </summary>
    /// <returns>爱心图标</returns>
    public Sprite GetHeartSprite()
    {
        // 直接获取索引1的图标，对应InlineSpriteAtlas_1
        return GetSprite(1);
    }
    
    /// <summary>
    /// 加载图标
    /// </summary>
    private void LoadSprites()
    {
        _spriteList.Clear();
        
        // 尝试加载图标
        #if UNITY_EDITOR
        // 在编辑器中加载
        Object[] sprites = AssetDatabase.LoadAllAssetsAtPath("Assets/Resources/sprite assets/InlineSpriteAtlas.png");
        foreach (Object obj in sprites)
        {
            if (obj is Sprite sprite)
            {
                _spriteList.Add(sprite);
                Debug.Log($"加载图标: {sprite.name}");
            }
        }
        #else
        // 在运行时加载
        Sprite[] sprites = Resources.LoadAll<Sprite>("sprite assets/InlineSpriteAtlas");
        if (sprites != null && sprites.Length > 0)
        {
            _spriteList.AddRange(sprites);
            Debug.Log($"从资源加载了{sprites.Length}个图标");
        }
        else
        {
            // 如果无法直接加载图集，尝试加载单独的图标
            Sprite heartSprite = Resources.Load<Sprite>("Sprites/heart_full");
            if (heartSprite != null)
            {
                _spriteList.Add(heartSprite);
                Debug.Log($"加载心形图标: {heartSprite.name}");
            }
        }
        #endif
    }
}
