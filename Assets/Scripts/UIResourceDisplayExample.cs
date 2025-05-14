using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// ResourceDisplay组件使用示例
/// </summary>
public class UIResourceDisplayExample : MonoBehaviour
{
    [Header("资源显示")]
    [SerializeField] private ResourceDisplay coinDisplay;
    [SerializeField] private ResourceDisplay gemDisplay;
    [SerializeField] private ResourceDisplay energyDisplay;
    
    [Header("资源图标")]
    [SerializeField] private Sprite coinIcon;
    [SerializeField] private Sprite gemIcon;
    [SerializeField] private Sprite energyIcon;
    
    [Header("测试按钮")]
    [SerializeField] private Button addCoinButton;
    [SerializeField] private Button addGemButton;
    [SerializeField] private Button addEnergyButton;
    
    // 资源数量
    private int coinAmount = 0;
    private int gemAmount = 0;
    private int energyAmount = 0;
    
    private void Start()
    {
        // 初始化资源显示
        UpdateResourceDisplays();
        
        // 添加按钮事件
        if (addCoinButton != null)
            addCoinButton.onClick.AddListener(AddCoin);
            
        if (addGemButton != null)
            addGemButton.onClick.AddListener(AddGem);
            
        if (addEnergyButton != null)
            addEnergyButton.onClick.AddListener(AddEnergy);
    }
    
    /// <summary>
    /// 更新所有资源显示
    /// </summary>
    private void UpdateResourceDisplays()
    {
        if (coinDisplay != null)
            coinDisplay.SetValue(coinIcon, coinAmount);
            
        if (gemDisplay != null)
            gemDisplay.SetValue(gemIcon, gemAmount);
            
        if (energyDisplay != null)
            energyDisplay.SetValue(energyIcon, energyAmount);
    }
    
    /// <summary>
    /// 添加金币
    /// </summary>
    public void AddCoin()
    {
        coinAmount += 10;
        
        if (coinDisplay != null)
            coinDisplay.SetValueOnly(coinAmount);
    }
    
    /// <summary>
    /// 添加宝石
    /// </summary>
    public void AddGem()
    {
        gemAmount += 5;
        
        if (gemDisplay != null)
            gemDisplay.SetValueOnly(gemAmount);
    }
    
    /// <summary>
    /// 添加能量
    /// </summary>
    public void AddEnergy()
    {
        energyAmount += 1;
        
        if (energyDisplay != null)
            energyDisplay.SetValueOnly(energyAmount);
    }
}
