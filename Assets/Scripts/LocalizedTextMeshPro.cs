using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

/// <summary>
/// TextMeshPro本地化组件，用于替代I2.Loc.Localize组件
/// </summary>
[RequireComponent(typeof(TextMeshProUGUI))]
public class LocalizedTextMeshPro : MonoBehaviour
{
    [SerializeField]
    private string termKey;
    
    [SerializeField]
    private LocalizeStringEvent localizeEvent;
    
    [SerializeField]
    private TextMeshProUGUI textComponent;
    
    private void Awake()
    {
        if (textComponent == null)
            textComponent = GetComponent<TextMeshProUGUI>();
            
        if (localizeEvent == null)
            localizeEvent = GetComponent<LocalizeStringEvent>();
    }
    
    private void OnEnable()
    {
        if (localizeEvent != null && !string.IsNullOrEmpty(termKey))
        {
            // 确保LocalizeStringEvent已正确设置
            localizeEvent.StringReference.TableReference = "GameStrings";
            localizeEvent.StringReference.TableEntryReference = termKey;
            
            // 确保事件已连接到TextMeshPro组件
            if (localizeEvent.OnUpdateString.GetPersistentEventCount() == 0)
            {
                UnityEngine.Events.UnityAction<string> setTextAction = new UnityEngine.Events.UnityAction<string>((text) => textComponent.text = text);
                localizeEvent.OnUpdateString.AddListener(setTextAction);
            }
        }
    }
    
    /// <summary>
    /// 设置本地化术语
    /// </summary>
    /// <param name="key">本地化术语键</param>
    public void SetTerm(string key)
    {
        termKey = key;
        
        if (localizeEvent != null)
        {
            localizeEvent.StringReference.TableEntryReference = key;
            localizeEvent.RefreshString();
        }
    }
    
    /// <summary>
    /// 刷新本地化文本
    /// </summary>
    public void RefreshText()
    {
        if (localizeEvent != null)
        {
            localizeEvent.RefreshString();
        }
    }
}
