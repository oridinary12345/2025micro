using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

/// <summary>
/// 本地化字符串，用于替代I2.Loc.LocalizedString
/// </summary>
[Serializable]
public struct UnityLocalizedString
{
    [SerializeField]
    private string mTerm;

    [SerializeField]
    private LocalizedString unityLocalizedString;

    public UnityLocalizedString(string term)
    {
        mTerm = term;
        unityLocalizedString = new LocalizedString("GameStrings", term);
    }

    public UnityLocalizedString(UnityLocalizedString str)
    {
        mTerm = str.mTerm;
        unityLocalizedString = str.unityLocalizedString;
    }

    public static implicit operator string(UnityLocalizedString s)
    {
        return s.ToString();
    }

    public static implicit operator UnityLocalizedString(string term)
    {
        return new UnityLocalizedString(term);
    }

    public override string ToString()
    {
        // 如果本地化系统尚未初始化，使用LocalizationHelper
        if (!LocalizationSettings.InitializationOperation.IsDone)
        {
            return LocalizationHelper.GetTranslation(mTerm);
        }

        // 否则使用Unity本地化系统
        string result = unityLocalizedString.GetLocalizedString();
        if (string.IsNullOrEmpty(result))
        {
            return LocalizationHelper.GetTranslation(mTerm);
        }

        return result;
    }
}
