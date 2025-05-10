using UnityEngine;
using UnityEditor;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

/// <summary>
/// TextMeshPro 升级工具，用于批量更新 UI 预制体
/// </summary>
public class TextMeshProUpgrader : EditorWindow
{
    private TMP_FontAsset defaultFont;
    private TMP_FontAsset chineseFont;
    private Material defaultMaterial;
    private Material chineseMaterial;
    private bool addFontSwitcher = true;
    private bool updateTextSettings = true;
    private Vector2 scrollPosition;
    private List<GameObject> prefabsToUpdate = new List<GameObject>();
    
    [MenuItem("Tools/TextMeshPro Upgrader")]
    public static void ShowWindow()
    {
        GetWindow<TextMeshProUpgrader>("TextMeshPro 升级工具");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("TextMeshPro 批量升级工具", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        // 字体资源设置
        EditorGUILayout.LabelField("字体资源设置", EditorStyles.boldLabel);
        defaultFont = (TMP_FontAsset)EditorGUILayout.ObjectField("默认字体", defaultFont, typeof(TMP_FontAsset), false);
        defaultMaterial = (Material)EditorGUILayout.ObjectField("默认材质", defaultMaterial, typeof(Material), false);
        chineseFont = (TMP_FontAsset)EditorGUILayout.ObjectField("中文字体", chineseFont, typeof(TMP_FontAsset), false);
        chineseMaterial = (Material)EditorGUILayout.ObjectField("中文材质", chineseMaterial, typeof(Material), false);

        EditorGUILayout.Space();

        // 更新选项
        EditorGUILayout.LabelField("更新选项", EditorStyles.boldLabel);
        addFontSwitcher = EditorGUILayout.Toggle("添加字体切换器", addFontSwitcher);
        updateTextSettings = EditorGUILayout.Toggle("更新文本设置", updateTextSettings);

        EditorGUILayout.Space();

        // 查找预制体按钮
        if (GUILayout.Button("查找所有 UI 预制体"))
        {
            FindUIPrefabs();
        }

        EditorGUILayout.Space();

        // 显示找到的预制体
        EditorGUILayout.LabelField("待更新预制体", EditorStyles.boldLabel);
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        for (int i = prefabsToUpdate.Count - 1; i >= 0; i--)
        {
            EditorGUILayout.BeginHorizontal();
            prefabsToUpdate[i] = (GameObject)EditorGUILayout.ObjectField(prefabsToUpdate[i], typeof(GameObject), false);
            if (GUILayout.Button("移除", GUILayout.Width(60)))
            {
                prefabsToUpdate.RemoveAt(i);
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space();

        // 更新按钮
        GUI.enabled = prefabsToUpdate.Count > 0 && defaultFont != null;
        if (GUILayout.Button("开始更新"))
        {
            UpdatePrefabs();
        }
        GUI.enabled = true;
    }

    private void FindUIPrefabs()
    {
        prefabsToUpdate.Clear();
        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/Resources/UI" });
        
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            
            // 检查预制体是否包含 TextMeshProUGUI 组件
            if (prefab.GetComponentInChildren<TextMeshProUGUI>(true) != null)
            {
                prefabsToUpdate.Add(prefab);
            }
        }
    }

    private void UpdatePrefabs()
    {
        foreach (var prefab in prefabsToUpdate)
        {
            if (prefab == null) continue;

            string prefabPath = AssetDatabase.GetAssetPath(prefab);
            GameObject instance = PrefabUtility.LoadPrefabContents(prefabPath);

            try
            {
                // 更新所有 TextMeshProUGUI 组件
                var textComponents = instance.GetComponentsInChildren<TextMeshProUGUI>(true);
                foreach (var text in textComponents)
                {
                    UpdateTextComponent(text);
                }

                // 添加字体切换器
                if (addFontSwitcher && chineseFont != null)
                {
                    var switcher = instance.GetComponent<LocalizedFontSwitcher>();
                    if (switcher == null)
                    {
                        switcher = instance.AddComponent<LocalizedFontSwitcher>();
                    }
                    
                    // 配置字体切换器
                    var serializedObject = new SerializedObject(switcher);
                    serializedObject.FindProperty("defaultFont").objectReferenceValue = defaultFont;
                    var localizedFonts = serializedObject.FindProperty("localizedFonts");
                    localizedFonts.ClearArray();
                    localizedFonts.arraySize = 1;
                    var element = localizedFonts.GetArrayElementAtIndex(0);
                    element.FindPropertyRelative("localeCode").stringValue = "zh-CN";
                    element.FindPropertyRelative("font").objectReferenceValue = chineseFont;
                    if (chineseMaterial != null)
                    {
                        element.FindPropertyRelative("material").objectReferenceValue = chineseMaterial;
                    }
                    serializedObject.ApplyModifiedProperties();

                    // 自动查找文本组件
                    switcher.findAllTextComponentsInChildren = true;
                }

                // 保存更改
                PrefabUtility.SaveAsPrefabAsset(instance, prefabPath);
                Debug.Log($"已更新预制体: {prefab.name}");
            }
            finally
            {
                // 清理
                PrefabUtility.UnloadPrefabContents(instance);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("所有预制体更新完成！");
    }

    private void UpdateTextComponent(TextMeshProUGUI text)
    {
        if (!updateTextSettings) return;

        // 设置默认字体和材质
        text.font = defaultFont;
        if (defaultMaterial != null)
        {
            text.fontMaterial = defaultMaterial;
        }

        // 更新文本设置
        text.enableWordWrapping = true;  // 启用自动换行
        text.overflowMode = TextOverflowModes.Overflow;  // 设置溢出模式
        text.enableKerning = true;  // 启用字距调整
        text.extraPadding = true;  // 添加额外内边距以防止裁剪
    }
}
