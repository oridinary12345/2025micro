# 2025micro

## 项目简介
这是一个基于Unity引擎开发的游戏项目，包含角色成长、战斗系统、关卡探索和任务挑战等功能。

## 微信小游戏接入指南

### 1. 基础环境配置
- 安装微信官方提供的"微信小游戏Unity SDK"插件
- 在Build Settings中将平台切换为WebGL
- 配置WebGL构建设置，禁用相关不兼容选项
- 确保Unity版本兼容微信小游戏要求（推荐Unity 2019.4 LTS或更高版本）

### 2. 云服务与用户认证调整

#### 修改 Cloud.cs
- 替换PlayFab登录逻辑为微信登录
- 实现微信开放数据域用户信息获取
- 移除PlayFab相关依赖

```csharp
// 替换当前的PlayFab登录逻辑
private void LoginGuestAndroid() {
    // PlayFab登录代码
}

// 新代码（微信登录）
private void LoginWechatGame() {
    // 使用微信开放数据域进行用户登录
    // wx.login({...});
    // 获取用户信息：wx.getUserInfo({...});
}
```

#### 修改 StartSyncFlow() 方法
```csharp
public void StartSyncFlow() {
    if (Application.platform == RuntimePlatform.WebGLPlayer) {
        LoginWechatGame();
    } else {
        // 原有平台的登录逻辑
        LoginGuestAndroid();
    }
}
```

### 3. 数据存储方式修改

#### 修改 SaveFile.cs
- 使用微信小游戏存储API替代PlayerPrefs
- 处理存储大小限制（单个key不超过1MB，总存储不超过10MB）

```csharp
// 修改SaveFile.cs中的存储逻辑
public void Save() {
    // 微信小游戏环境检测
    if (Application.platform == RuntimePlatform.WebGLPlayer) {
        // 使用wx.setStorageSync替代PlayerPrefs
        string text = _data.toJson();
        // 注意数据大小限制
        if (text.Length > 1024 * 1024) {
            Debug.LogWarning("数据超过1MB，需要进行压缩或分割");
        }
        // wx.setStorageSync('data', text);
        // wx.setStorageSync('hash', ComputeHash(text));
    } else {
        // 原有存储逻辑
        string text = _data.toJson();
        string value = ComputeHash(text);
        PlayerPrefs.SetString("data", text);
        PlayerPrefs.SetString("hash", value);
        PlayerPrefs.SetInt("version", 1);
    }
}
```

### 4. 支付系统调整

#### 修改 InAppManager 和相关支付逻辑
- 移除现有IAP实现
- 集成微信支付API
- 处理支付回调和订单验证

```csharp
// 替换现有的支付逻辑
public void ProcessPayment(string productId, int amount) {
    if (Application.platform == RuntimePlatform.WebGLPlayer) {
        // 调用微信支付
        // wx.requestPayment({
        //     timeStamp: '',
        //     nonceStr: '',
        //     package: '',
        //     signType: 'MD5',
        //     paySign: '',
        //     success(res) { },
        //     fail(res) { }
        // });
    } else {
        // 原有平台的支付逻辑
    }
}
```

### 5. 广告系统适配

#### 修改 GameAdsController
- 集成微信小游戏广告API
- 实现Banner广告、插屏广告和激励视频广告

```csharp
// 替换广告相关代码
public void ShowBannerAd() {
    if (Application.platform == RuntimePlatform.WebGLPlayer) {
        // 创建并显示微信Banner广告
        // wx.createBannerAd({
        //     adUnitId: 'your-ad-unit-id',
        //     style: {
        //         left: 0,
        //         top: 0,
        //         width: 320
        //     }
        // }).show();
    } else {
        // 原有平台的广告逻辑
    }
}
```

### 6. 资源加载优化

#### 实现分包加载
- 创建资源分包配置
- 调整资源加载逻辑以支持按需加载

#### 资源压缩
- 压缩音频文件（转换为ogg格式）
- 优化纹理资源大小和压缩格式
- 移除不必要的资源

### 7. 性能优化

#### 渲染优化
- 降低渲染分辨率和复杂度
- 减少过度绘制
- 优化后处理效果

#### 内存优化
- 减少内存占用
- 优化对象池管理
- 实现更好的资源释放策略

#### WebGL设置优化
```
Player Settings > WebGL > Optimization:
- Enable Exceptions: None
- Data Caching: Enable
- Compression Format: Brotli
```

### 8. 平台特殊功能实现

#### 添加分享功能
```csharp
public void ShareGame() {
    if (Application.platform == RuntimePlatform.WebGLPlayer) {
        // 微信分享
        // wx.shareAppMessage({
        //     title: '分享标题',
        //     imageUrl: '分享图片URL',
        //     query: '分享参数'
        // });
    }
}
```

#### 实现开放数据域排行榜
- 创建开放数据域项目
- 实现排行榜数据同步

### 9. 代码兼容性处理

#### 添加平台判断
在关键功能处添加平台判断，确保代码在不同平台上正常运行：

```csharp
if (Application.platform == RuntimePlatform.WebGLPlayer) {
    // 微信小游戏特定代码
} else {
    // 其他平台代码
}
```

#### 处理不兼容API
- 移除或替换微信小游戏不支持的Unity API
- 处理WebGL环境下的特殊限制

### 10. 测试与发布

#### 本地测试
- 使用微信开发者工具进行本地调试
- 测试各功能模块在WebGL环境下的表现

#### 真机测试
- 在不同性能的设备上进行测试
- 检查性能和兼容性问题

#### 发布流程
- 申请微信小游戏账号和游戏资质
- 配置游戏信息和权限
- 上传代码包并提交审核

## 注意事项

1. **包体大小限制**：微信小游戏主包大小不能超过20MB，需严格控制资源大小
2. **存储限制**：本地存储总和不超过10MB，单个key不超过1MB
3. **API限制**：部分Unity API在WebGL环境下不可用，需寻找替代方案
4. **性能优化**：WebGL性能通常弱于原生平台，需特别注意性能优化
5. **跨域问题**：处理网络请求的跨域限制

## TextMesh Pro 文本系统

### 简介
本项目使用 TextMesh Pro (TMP) 作为主要的文本渲染解决方案。TextMesh Pro 是 Unity 的高级文本渲染系统，提供了更好的文本质量、性能和灵活性。

### 项目中的集成方式
- 采用传统的 Assets 文件夹导入方式（非 Package Manager）
- TextMesh Pro 资源位于 `Assets/TextMesh Pro/` 目录

### 核心组件使用

#### 1. TextMeshProUGUI
- **用途**：UI Canvas 中的文本显示
- **使用场景**：
  - 游戏界面中的所有文本元素
  - 英雄卡片信息（血量、等级、名称）
  - 商店面板（价格、描述、奖励）
  - 任务系统（任务描述、进度）
  - 提示弹窗（标题、消息）
  - 游戏设置（版本号、玩家ID）

#### 2. TextMeshPro
- **用途**：3D 世界空间中的文本
- **特点**：可直接放置在 3D 场景中，支持透视和深度

#### 3. TMP_InputField
- **用途**：文本输入框
- **功能**：支持单行和多行文本输入

#### 4. TMP_Dropdown
- **用途**：下拉菜单组件
- **功能**：提供选项列表选择

### 资源配置

#### 字体资源
```
Assets/TextMesh Pro/Resources/Fonts & Materials/
├── LiberationSans SDF.asset          # 默认 SDF 字体
├── LiberationSans SDF - Outline.mat  # 描边材质
└── LiberationSans SDF - Drop Shadow.mat  # 阴影材质
```

#### 着色器资源
```
Assets/TextMesh Pro/Resources/Shaders/
├── TMP_SDF.shader                    # 标准 SDF 着色器
├── TMP_SDF-Mobile.shader             # 移动端优化着色器
├── TMP_Bitmap.shader                 # 位图字体着色器
├── TMP_Sprite.shader                 # 精灵着色器
└── 其他变体着色器...
```

#### 精灵资源
```
Assets/TextMesh Pro/Sprites/
└── EmojiOne.png                      # Emoji 表情精灵图集
```

### 代码使用示例

```csharp
using TMPro;
using UnityEngine;

public class UIExample : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _titleText;
    
    [SerializeField]
    private TextMeshProUGUI _descriptionText;
    
    private void Start()
    {
        // 设置文本内容
        _titleText.text = "游戏标题";
        _descriptionText.text = "这是描述文字";
        
        // 设置文本颜色
        _titleText.color = Color.white;
        
        // 支持富文本标记
        _descriptionText.text = "<color=red>红色</color>文字";
    }
}
```

### 微信小游戏适配注意事项

#### WebGL 平台兼容性
- TextMesh Pro 在 WebGL 平台下表现良好
- SDF 字体在移动设备上性能优秀
- 建议使用移动端优化的着色器（TMP_SDF-Mobile.shader）

#### 字体资源优化
1. **字体图集优化**
   - 只包含实际使用的字符
   - 减小字体图集纹理尺寸
   - 使用合适的采样距离（Sampling Point Size）

2. **多语言支持**
   - 为中文创建独立的 SDF 字体资源
   - 使用字体回退（Font Fallback）机制
   - 考虑使用动态字体图集

#### 性能优化建议
```csharp
// 避免频繁修改文本导致网格重建
private TextMeshProUGUI _cachedText;
private string _lastText = "";

void UpdateText(string newText)
{
    if (_lastText != newText)
    {
        _cachedText.text = newText;
        _lastText = newText;
    }
}

// 使用对象池管理动态文本对象
// 禁用不可见的文本组件
_text.enabled = false;
```

#### 内联图标支持
项目使用了内联精灵（Inline Sprites）功能来显示图标：
```csharp
// 示例：在文本中显示货币图标
string text = InlineSprites.GetLootInlineSprite("lootRuby") + "100";
_priceText.text = text;
```

### 常见问题解决

1. **文字模糊**
   - 增加字体图集分辨率
   - 调整 Sampling Point Size
   - 使用合适的材质（SDF 而非 Bitmap）

2. **中文显示异常**
   - 确保字体图集包含所需中文字符
   - 创建中文专用字体资源
   - 检查字符集配置

3. **性能问题**
   - 使用 TMP_SDF-Mobile 着色器
   - 减少同时渲染的文本数量
   - 避免频繁的文本更新
   - 使用 Canvas 分组优化

### 开发建议

1. **统一使用 TextMesh Pro**
   - 项目中已全面采用 TMP，避免混用传统 UI Text
   - 保持文本组件使用的一致性

2. **字体资源管理**
   - 建立字体资源命名规范
   - 集中管理字体配置
   - 版本控制时注意字体资源的同步

3. **样式复用**
   - 使用 TMP 的样式表功能
   - 定义常用文本样式
   - 便于全局调整和维护

## 多语言系统（I2 Localization）

### 简介
本项目使用 **I2 Localization** 插件实现完整的多语言支持系统。I2 Localization 是 Unity 平台上功能强大的本地化解决方案，提供了丰富的多语言管理功能。

### 核心架构

#### 1. LocalizationManager（本地化管理器）
**位置**：`Assets/Scripts/I2/Loc/LocalizationManager.cs`

**主要功能**：
- 管理当前语言和语言代码
- 处理语言切换
- 提供翻译查询接口
- 支持 RTL（从右到左）语言
- 管理多个语言数据源

**关键属性**：
```csharp
// 当前语言
public static string CurrentLanguage { get; set; }

// 当前语言代码（ISO 639-1）
public static string CurrentLanguageCode { get; set; }

// 是否为从右到左语言（阿拉伯语、希伯来语等）
public static bool IsRight2Left { get; }

// 语言数据源列表
public static List<LanguageSource> Sources { get; }
```

**核心方法**：
```csharp
// 获取翻译文本
string translation = LocalizationManager.GetTranslation("Term_Key");

// 获取所有可用语言
List<string> languages = LocalizationManager.GetAllLanguages();

// 检查是否支持某个语言
bool hasLanguage = LocalizationManager.HasLanguage("Chinese (Simplified)");

// 切换语言
LocalizationManager.CurrentLanguage = "English";
```

#### 2. LanguageSource（语言数据源）
**位置**：`Assets/Scripts/I2/Loc/LanguageSource.cs`

**功能**：
- 存储术语（Terms）和翻译数据
- 管理多个语言的翻译内容
- 支持 Google Spreadsheet 集成
- 提供术语查询和管理接口

**配置文件**：`Assets/Resources/I2Languages.prefab`
- 全局语言数据源
- 包含所有翻译术语和对应的多语言文本

#### 3. Localize 组件
**位置**：`Assets/Scripts/I2/Loc/Localize.cs`

**用途**：附加到 GameObject 上，自动本地化 UI 元素

**主要属性**：
```csharp
public class Localize : MonoBehaviour
{
    // 主术语键
    public string Term;
    
    // 次术语键（用于字体、图片等资源）
    public string SecondaryTerm;
    
    // 是否在 Awake 时自动本地化
    public bool LocalizeOnAwake = true;
    
    // RTL 相关设置
    public bool IgnoreRTL;
    public int MaxCharactersInRTL;
    public bool IgnoreNumbersInRTL = true;
    
    // 术语修饰符（大写、小写等）
    public TermModification PrimaryTermModifier;
    public TermModification SecondaryTermModifier;
}
```

**使用方式**：
1. 在 UI 元素上添加 `Localize` 组件
2. 设置 `Term` 为翻译术语的键
3. 组件会自动根据当前语言更新文本

#### 4. LocalizedString（本地化字符串）
**位置**：`Assets/Scripts/LocalizedString.cs`

**功能**：用于在代码中获取本地化字符串

```csharp
[SerializeField]
private LocalizedString _localizedText;

void Start()
{
    string translatedText = _localizedText.ToString();
}
```

### 语言管理系统

#### 1. PlayerSettingsManager（玩家设置管理器）
**位置**：`Assets/Scripts/PlayerSettingsManager.cs`

**语言管理**：
```csharp
public class PlayerSettingsManager
{
    public string Language
    {
        get { return _profile.Language; }
        set
        {
            _profile.Language = value;
            LocalizationManager.CurrentLanguage = _profile.Language;
        }
    }
}
```

**初始化逻辑**：
1. 检查是否有已保存的语言设置
2. 如果没有，尝试使用设备系统语言
3. 如果系统语言不支持，默认使用英语

#### 2. UIMenuChooseLanguagePanel（语言选择面板）
**位置**：`Assets/Scripts/UIMenuChooseLanguagePanel.cs`

**功能**：提供用户界面供玩家选择语言

**当前支持的语言**：
- English（英语）
- French（法语）

**实现方式**：
```csharp
private void OnLanguageSelected(SystemLanguage language)
{
    App.Instance.Player.SettingsManager.Language = language.ToString();
    OnCloseButtonClicked();
}
```

### 高级特性

#### 1. RTL（从右到左）语言支持
**支持的 RTL 语言**：
- 阿拉伯语（ar）及其各种地区变体
- 希伯来语（he）
- 乌尔都语（ur）
- 意第绪语（ji）

**自动处理**：
- 文本方向自动调整
- 标点符号位置修正
- 数字显示优化
- UI 对齐方式自动调整

**使用方法**：
```csharp
// 系统自动检测并应用 RTL 修正
string text = LocalizationManager.GetTranslation("Term", fixForRTL: true);

// 在 Localize 组件中可以配置 RTL 行为
localize.IgnoreRTL = false; // 启用 RTL 支持
localize.MaxCharactersInRTL = 50; // 最大字符数
localize.IgnoreNumbersInRTL = true; // 忽略数字的 RTL
```

#### 2. 本地化参数（Dynamic Content）
**功能**：在翻译文本中插入动态内容

**语法**：`{[参数名]}`

**示例**：
```csharp
// 翻译术语定义：
// "Welcome_Message" = "Welcome, {[PlayerName]}!"

// 使用参数管理器提供值
LocalizationManager.ApplyLocalizationParams(ref translation);

// 或使用字典提供参数
var parameters = new Dictionary<string, object>
{
    { "PlayerName", "John" },
    { "Level", 10 }
};
LocalizationManager.ApplyLocalizationParams(ref translation, parameters);
```

**参数管理器接口**：
```csharp
public interface ILocalizationParamsManager
{
    string GetParameterValue(string ParamName);
}
```

#### 3. Google Spreadsheet 集成
**功能**：
- 从 Google Spreadsheet 导入/导出翻译
- 支持在线协作翻译
- 实时同步更新

**配置**：
在 LanguageSource 组件中设置：
- Google Spreadsheet URL
- 更新频率（Never, Always, Daily, Weekly, Monthly）
- 更新延迟时间

#### 4. Google Translation API 集成
**位置**：`Assets/Scripts/I2/Loc/GoogleTranslation.cs`

**功能**：
- 自动翻译缺失的翻译项
- 批量翻译支持
- 翻译查询缓存

**使用方法**：
```csharp
// 创建翻译查询
Dictionary<string, TranslationQuery> dict = new Dictionary<string, TranslationQuery>();
GoogleTranslation.AddQuery("Hello", "en", "fr", dict);

// 执行翻译
GoogleTranslation.Translate(dict, OnTranslationReady);

void OnTranslationReady(Dictionary<string, TranslationQuery> dict, string errorMsg)
{
    if (string.IsNullOrEmpty(errorMsg))
    {
        string translation = dict["Hello"].Results[0];
    }
}
```

### 使用指南

#### 1. 添加新的翻译术语
1. 在 Unity 中找到 `I2Languages` 预制件
2. 在 Inspector 中的 LanguageSource 组件添加新术语
3. 为每种支持的语言添加翻译文本
4. 保存预制件

#### 2. 本地化 UI 元素
```csharp
// 方法 1：使用 Localize 组件（推荐用于静态文本）
// 在 Inspector 中添加 Localize 组件并设置 Term

// 方法 2：代码中动态本地化
string localizedText = LocalizationManager.GetTranslation("Menu/Start");
_textComponent.text = localizedText;

// 方法 3：使用 LocalizedString
[SerializeField]
private LocalizedString _titleText;

void Start()
{
    _label.text = _titleText.ToString();
}
```

#### 3. 添加新语言支持
1. 在 `UIMenuChooseLanguagePanel.cs` 中添加新语言按钮：
```csharp
[SerializeField]
private UIGameButton _chineseButton;

protected override void Awake()
{
    base.Awake();
    _chineseButton.OnClick(delegate
    {
        OnLanguageSelected(SystemLanguage.Chinese);
    });
}
```

2. 在 `I2Languages` 数据源中添加新语言列
3. 为所有术语添加该语言的翻译

#### 4. 在代码中切换语言
```csharp
// 直接设置语言
LocalizationManager.CurrentLanguage = "Chinese (Simplified)";

// 通过语言代码设置
LocalizationManager.CurrentLanguageCode = "zh-CN";

// 使用玩家设置管理器
App.Instance.Player.SettingsManager.Language = "Chinese (Simplified)";
```

### 资源本地化

#### 支持的资源类型
- 字体（Font/TMP_FontAsset）
- 图片（Sprite/Texture）
- 音频（AudioClip）
- 预制件（Prefab）
- 任意 Unity Object

#### 使用方法
```csharp
// 在 Localize 组件中：
// 1. 设置 SecondaryTerm 为资源的术语键
// 2. 在 TranslatedObjects 列表中添加本地化资源

// 代码中获取本地化资源：
TMP_FontAsset localizedFont = LocalizationManager.GetTranslatedObject<TMP_FontAsset>("Fonts/Chinese");
```

### 性能优化

#### 1. 语言数据懒加载
```csharp
// LanguageSource 支持延迟加载语言数据
public void LoadLanguage(int languageIndex, bool UnloadOtherLanguages = true);
public void UnloadLanguage(int languageIndex);
```

#### 2. 翻译缓存
- LocalizationManager 自动缓存翻译结果
- 避免重复查询相同术语

#### 3. 批量本地化
```csharp
// 避免频繁调用 OnLocalize
LocalizationManager.LocalizeAll(Force: true);
```

### 微信小游戏适配建议

#### 1. 语言数据优化
```csharp
// 只加载需要的语言，减少包体大小
public class WeChatLanguageLoader : MonoBehaviour
{
    void Start()
    {
        // 根据玩家选择只加载单一语言
        string targetLanguage = PlayerPrefs.GetString("Language", "Chinese (Simplified)");
        
        foreach (var source in LocalizationManager.Sources)
        {
            int langIndex = source.GetLanguageIndex(targetLanguage);
            source.LoadLanguage(langIndex, unloadOtherLanguages: true);
        }
    }
}
```

#### 2. 减小语言资源大小
- 移除不需要的语言
- 使用缩写和简化的翻译
- 考虑使用服务器端翻译文件

#### 3. 字体优化
- 为中文创建优化的 TMP 字体资源
- 只包含常用汉字（3500 常用字）
- 使用多个字体资源按需加载

```csharp
// 动态字体加载示例
public class DynamicFontLoader : MonoBehaviour
{
    public TMP_FontAsset basicChineseFont; // 常用字
    public TMP_FontAsset extendedChineseFont; // 扩展字
    
    public void LoadFontForText(string text)
    {
        // 检测需要哪个字体
        bool needsExtended = ContainsRareCharacters(text);
        
        if (needsExtended)
        {
            // 加载扩展字体
        }
    }
}
```

### 常见问题

#### 1. 术语未找到
**问题**：显示术语键而不是翻译文本

**解决方案**：
- 检查术语键是否正确
- 确认 I2Languages 数据源已加载
- 验证当前语言存在该术语的翻译

#### 2. RTL 语言显示异常
**问题**：阿拉伯语或希伯来语文本显示错乱

**解决方案**：
```csharp
// 启用 RTL 修复
localize.IgnoreRTL = false;

// 或在代码中手动应用
string text = LocalizationManager.ApplyRTLfix(arabicText);
```

#### 3. 语言切换后 UI 未更新
**问题**：切换语言后部分 UI 没有更新

**解决方案**：
```csharp
// 强制刷新所有本地化组件
LocalizationManager.LocalizeAll(Force: true);

// 或订阅语言改变事件
LocalizationManager.OnLocalizeEvent += OnLanguageChanged;

void OnLanguageChanged()
{
    // 手动更新需要刷新的内容
}
```

#### 4. 动态创建的 UI 未本地化
**问题**：运行时创建的 UI 元素没有本地化

**解决方案**：
```csharp
// 创建 UI 后手动触发本地化
GameObject newUI = Instantiate(uiPrefab);
Localize localize = newUI.GetComponent<Localize>();
if (localize != null)
{
    localize.OnLocalize(Force: true);
}
```

### 最佳实践

1. **统一术语命名规范**
   - 使用层级结构：`Category/Subcategory/Term`
   - 示例：`Menu/Settings/Language`, `Battle/Victory/Title`

2. **避免硬编码文本**
   - 所有用户可见的文本都应该使用本地化系统
   - 使用常量或枚举管理术语键

3. **提供翻译上下文**
   - 在术语描述中说明使用场景
   - 注明字符长度限制

4. **测试所有语言**
   - 确保所有语言都能正确显示
   - 检查文本长度是否适应 UI 布局
   - 验证 RTL 语言的显示效果

5. **使用回调机制**
   - 复杂的本地化逻辑使用 LocalizeCallBack
   - 可以自定义本地化行为

```csharp
public class CustomLocalizeCallback : MonoBehaviour, ILocalizationParamsManager
{
    public string GetParameterValue(string ParamName)
    {
        switch (ParamName)
        {
            case "PlayerName":
                return GameManager.Instance.PlayerName;
            case "Gold":
                return GameManager.Instance.Gold.ToString();
            default:
                return null;
        }
    }
}
```

## 开发工具推荐

- 微信开发者工具：用于调试微信小游戏
- Unity Profiler：用于性能分析和优化
- TexturePacker：用于纹理优化
- Audacity：用于音频压缩和优化
- TextMesh Pro Font Asset Creator：用于创建 SDF 字体资源
- Google Spreadsheet：用于多语言协作翻译管理
