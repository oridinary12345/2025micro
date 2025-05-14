using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 资源显示组件，用于替代富文本sprite标签，提高性能
/// </summary>
public class ResourceDisplay : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI valueText; // 使用TextMeshPro但不使用富文本功能
    
    [Tooltip("是否在数值前添加空格")]
    [SerializeField] private bool addSpaceBeforeValue = true;
    
    /// <summary>
    /// 设置资源显示
    /// </summary>
    /// <param name="icon">资源图标</param>
    /// <param name="value">资源数值</param>
    public void SetValue(Sprite icon, int value)
    {
        if (iconImage != null)
        {
            iconImage.sprite = icon;
            iconImage.enabled = icon != null;
        }
        
        if (valueText != null)
        {
            valueText.text = addSpaceBeforeValue ? " " + value.ToString() : value.ToString();
        }
    }
    
    /// <summary>
    /// 设置资源显示
    /// </summary>
    /// <param name="icon">资源图标</param>
    /// <param name="valueString">资源数值文本</param>
    public void SetValue(Sprite icon, string valueString)
    {
        if (iconImage != null)
        {
            iconImage.sprite = icon;
            iconImage.enabled = icon != null;
        }
        
        if (valueText != null)
        {
            valueText.text = addSpaceBeforeValue ? " " + valueString : valueString;
        }
    }
    
    /// <summary>
    /// 仅设置数值
    /// </summary>
    /// <param name="value">资源数值</param>
    public void SetValueOnly(int value)
    {
        if (valueText != null)
        {
            valueText.text = addSpaceBeforeValue ? " " + value.ToString() : value.ToString();
        }
    }
    
    /// <summary>
    /// 仅设置数值文本
    /// </summary>
    /// <param name="valueString">资源数值文本</param>
    public void SetValueOnly(string valueString)
    {
        if (valueText != null)
        {
            valueText.text = addSpaceBeforeValue ? " " + valueString : valueString;
        }
    }
    
    /// <summary>
    /// 仅设置图标
    /// </summary>
    /// <param name="icon">资源图标</param>
    public void SetIconOnly(Sprite icon)
    {
        if (iconImage != null)
        {
            iconImage.sprite = icon;
            iconImage.enabled = icon != null;
        }
    }
    
    /// <summary>
    /// 设置图标颜色
    /// </summary>
    /// <param name="color">颜色</param>
    public void SetIconColor(Color color)
    {
        if (iconImage != null)
        {
            iconImage.color = color;
        }
    }
    
    /// <summary>
    /// 设置文本颜色
    /// </summary>
    /// <param name="color">颜色</param>
    public void SetTextColor(Color color)
    {
        if (valueText != null)
        {
            valueText.color = color;
        }
    }
    
    /// <summary>
    /// 设置文本字体大小
    /// </summary>
    /// <param name="fontSize">字体大小</param>
    public void SetTextFontSize(float fontSize)
    {
        if (valueText != null)
        {
            valueText.fontSize = fontSize;
        }
    }
}
