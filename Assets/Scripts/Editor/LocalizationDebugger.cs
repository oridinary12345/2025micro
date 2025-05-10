using UnityEngine;
using UnityEditor;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;
#if UNITY_EDITOR
using UnityEditor.Localization;
#endif

public class LocalizationDebugger : EditorWindow
{
    [MenuItem("Tools/Localization Debugger")]
    public static void ShowWindow()
    {
        GetWindow(typeof(LocalizationDebugger), false, "本地化调试器");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("当前本地化状态", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        // 显示初始化状态
        bool isInitialized = LocalizationSettings.Instance != null &&
                           LocalizationSettings.InitializationOperation.IsDone;
        EditorGUILayout.LabelField($"本地化系统初始化: {(isInitialized ? "是" : "否")}");

        // 显示当前选择的语言
        if (LocalizationSettings.SelectedLocale != null)
        {
            EditorGUILayout.LabelField($"当前语言: {LocalizationSettings.SelectedLocale.Identifier.CultureInfo.DisplayName}");
            EditorGUILayout.LabelField($"语言代码: {LocalizationSettings.SelectedLocale.Identifier.Code}");
        }
        else
        {
            EditorGUILayout.LabelField("当前未选择语言");
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("可用语言", EditorStyles.boldLabel);

        // 显示所有可用的语言
        if (LocalizationSettings.AvailableLocales != null)
        {
            foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"- {locale.Identifier.CultureInfo.DisplayName} ({locale.Identifier.Code})");
                if (GUILayout.Button("设为当前", GUILayout.Width(100)))
                {
                    LocalizationSettings.SelectedLocale = locale;
                    LocalizationHelper.ClearCache();
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("测试翻译", EditorStyles.boldLabel);

        // 添加一个测试按钮
        if (GUILayout.Button("测试 'GameStrings' 表"))
        {
            TestGameStrings();
        }

        // 添加手动初始化按钮
        if (GUILayout.Button("手动初始化本地化系统"))
        {
            LocalizationHelper.Initialize();
        }
    }

    private void TestGameStrings()
    {
        var stringTable = LocalizationSettings.StringDatabase.GetTable("GameStrings");
        if (stringTable != null)
        {
            Debug.Log($"找到 GameStrings 表，包含 {stringTable.SharedData.Entries.Count} 个条目");
            foreach (var entry in stringTable.SharedData.Entries)
            {
                string translation = LocalizationHelper.GetTranslation(entry.Key);
                Debug.Log($"Key: {entry.Key}, Translation: {translation}");
            }
        }
        else
        {
            Debug.LogError("未找到 GameStrings 表");
        }
    }
}
