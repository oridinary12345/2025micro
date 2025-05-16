using UnityEngine;

/// <summary>
/// 资源管理器初始化器，确保ResourceManager在游戏启动时被创建
/// </summary>
public class ResourceManagerInitializer : MonoBehaviour
{
    private void Awake()
    {
        // 如果场景中没有ResourceManager实例，则从预制体创建一个
        if (ResourceManager.Instance == null)
        {
            GameObject resourceManagerPrefab = Resources.Load<GameObject>("sprite assets/ResourceManage");
            if (resourceManagerPrefab != null)
            {
                GameObject resourceManagerObj = Instantiate(resourceManagerPrefab);
                resourceManagerObj.name = "ResourceManager";
                DontDestroyOnLoad(resourceManagerObj);
                Debug.Log("已成功创建ResourceManager实例");
            }
            else
            {
                Debug.LogError("无法加载ResourceManage预制体，请确保它位于Resources/sprite assets/目录下");
            }
        }
    }
}
