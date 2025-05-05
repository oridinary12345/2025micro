using UnityEngine;

/// <summary>
/// Boot场景的本地化初始化器
/// 确保在游戏启动时正确初始化本地化系统
/// </summary>
public class BootLocalizationInitializer : MonoBehaviour
{
    [SerializeField]
    private bool initializeOnAwake = true;

    private void Awake()
    {
        if (initializeOnAwake)
        {
            Initialize();
        }
    }

    public void Initialize()
    {
        // 初始化本地化辅助类
        LocalizationHelper.Initialize();
    }
}
