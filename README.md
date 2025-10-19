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

## 开发工具推荐

- 微信开发者工具：用于调试微信小游戏
- Unity Profiler：用于性能分析和优化
- TexturePacker：用于纹理优化
- Audacity：用于音频压缩和优化