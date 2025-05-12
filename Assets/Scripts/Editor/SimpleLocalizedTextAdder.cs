using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;

public class SimpleLocalizedTextAdder : EditorWindow
{
    private bool _processInactive = true;
    private bool _includeChildren = true;
    private bool _processTextMeshPro = true;
    private bool _autoGenerateKeys = true;
    private string _keyPrefix = "UI_";
    private string _exportPath = "Assets/Localization/ExportedTexts.csv";
    private Vector2 _scrollPosition;
    private bool _showAdvancedOptions = false;

    [MenuItem("Tools/Add Simple Localization")]
    public static void ShowWindow()
    {
        GetWindow(typeof(SimpleLocalizedTextAdder), false, "简单本地化添加器");
    }

    private void OnGUI()
    {
        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

        EditorGUILayout.HelpBox("此工具将为文本组件添加SimpleLocalizedText组件，实现简单的本地化功能。", MessageType.Info);
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("基本设置", EditorStyles.boldLabel);
        _processInactive = EditorGUILayout.Toggle("处理未激活的对象", _processInactive);
        _includeChildren = EditorGUILayout.Toggle("包含子对象", _includeChildren);
        _processTextMeshPro = EditorGUILayout.Toggle("处理TextMeshPro组件", _processTextMeshPro);

        EditorGUILayout.Space();
        _showAdvancedOptions = EditorGUILayout.Foldout(_showAdvancedOptions, "高级选项");
        if (_showAdvancedOptions)
        {
            EditorGUI.indentLevel++;
            _autoGenerateKeys = EditorGUILayout.Toggle("自动生成键名", _autoGenerateKeys);
            if (_autoGenerateKeys)
            {
                _keyPrefix = EditorGUILayout.TextField("键名前缀", _keyPrefix);
            }
            _exportPath = EditorGUILayout.TextField("导出路径", _exportPath);
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("操作", EditorStyles.boldLabel);

        if (GUILayout.Button("为选中对象添加本地化"))
        {
            ProcessSelectedObjects();
        }

        if (GUILayout.Button("为场景所有文本添加本地化"))
        {
            if (EditorUtility.DisplayDialog("确认", 
                "这将为场景中所有文本组件添加本地化组件。是否继续？", 
                "是", "取消"))
            {
                ProcessAllInScene();
            }
        }

        if (GUILayout.Button("处理所有预制体"))
        {
            if (EditorUtility.DisplayDialog("确认",
                "这将处理所有预制体中的文本组件。此操作可能需要较长时间。是否继续？",
                "是", "取消"))
            {
                ProcessAllPrefabs();
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("导出/导入", EditorStyles.boldLabel);

        if (GUILayout.Button("导出场景文本为CSV"))
        {
            ExportSceneTextsToCSV();
        }

        EditorGUILayout.EndScrollView();
    }

    private void ProcessSelectedObjects()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            AddLocalizationToObject(obj);
        }
        Debug.Log("已完成选中对象的本地化组件添加");
    }

    private void ProcessAllInScene()
    {
        // 处理普通Text组件
        Text[] allTexts = FindObjectsOfType<Text>(_processInactive);
        int total = allTexts.Length;
        int current = 0;

        foreach (Text text in allTexts)
        {
            EditorUtility.DisplayProgressBar("添加本地化组件", 
                $"处理 {text.gameObject.name} ({current}/{total})", 
                (float)current / total);
            
            AddLocalizationComponent(text);
            current++;
        }

        // 处理TextMeshPro组件
        if (_processTextMeshPro)
        {
            TMP_Text[] allTMPTexts = FindObjectsOfType<TMP_Text>(_processInactive);
            total = allTMPTexts.Length;
            current = 0;

            foreach (TMP_Text tmpText in allTMPTexts)
            {
                EditorUtility.DisplayProgressBar("添加本地化组件", 
                    $"处理 {tmpText.gameObject.name} ({current}/{total})", 
                    (float)current / total);
                
                AddLocalizationComponentToTMP(tmpText);
                current++;
            }
        }

        EditorUtility.ClearProgressBar();
        Debug.Log("已完成场景中所有文本组件的本地化组件添加");
    }

    private void ProcessAllPrefabs()
    {
        string[] allPrefabPaths = GetAllPrefabPaths();
        int total = allPrefabPaths.Length;
        int current = 0;
        int modifiedCount = 0;

        foreach (string prefabPath in allPrefabPaths)
        {
            EditorUtility.DisplayProgressBar("处理预制体", 
                $"处理 {Path.GetFileName(prefabPath)} ({current}/{total})", 
                (float)current / total);
            
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if (prefab == null) continue;

            bool modified = false;
            
            // 处理Text组件
            Text[] texts = prefab.GetComponentsInChildren<Text>(_processInactive);
            foreach (Text text in texts)
            {
                if (AddLocalizationComponentToPrefab(text))
                    modified = true;
            }

            // 处理TextMeshPro组件
            if (_processTextMeshPro)
            {
                TMP_Text[] tmpTexts = prefab.GetComponentsInChildren<TMP_Text>(_processInactive);
                foreach (TMP_Text tmpText in tmpTexts)
                {
                    if (AddLocalizationComponentToPrefabTMP(tmpText))
                        modified = true;
                }
            }

            if (modified)
            {
                EditorUtility.SetDirty(prefab);
                modifiedCount++;
            }

            current++;
        }

        if (modifiedCount > 0)
        {
            AssetDatabase.SaveAssets();
            Debug.Log($"已修改 {modifiedCount} 个预制体");
        }

        EditorUtility.ClearProgressBar();
    }

    private void AddLocalizationToObject(GameObject obj)
    {
        // 处理对象及其所有子对象
        if (_includeChildren)
        {
            // 处理Text组件
            Text[] texts = obj.GetComponentsInChildren<Text>(_processInactive);
            foreach (Text text in texts)
            {
                AddLocalizationComponent(text);
            }

            // 处理TextMeshPro组件
            if (_processTextMeshPro)
            {
                TMP_Text[] tmpTexts = obj.GetComponentsInChildren<TMP_Text>(_processInactive);
                foreach (TMP_Text tmpText in tmpTexts)
                {
                    AddLocalizationComponentToTMP(tmpText);
                }
            }
        }
        else
        {
            // 只处理当前对象
            Text text = obj.GetComponent<Text>();
            if (text != null)
            {
                AddLocalizationComponent(text);
            }

            if (_processTextMeshPro)
            {
                TMP_Text tmpText = obj.GetComponent<TMP_Text>();
                if (tmpText != null)
                {
                    AddLocalizationComponentToTMP(tmpText);
                }
            }
        }
    }

    private void AddLocalizationComponent(Text text)
    {
        if (text.GetComponent<SimpleLocalizedText>() == null)
        {
            Undo.AddComponent<SimpleLocalizedText>(text.gameObject);
            
            // 如果启用了自动生成键名，则生成键名并设置到Text中
            if (_autoGenerateKeys && !string.IsNullOrEmpty(text.text))
            {
                string key = GenerateKeyFromText(text);
                text.text = key;
            }
            
            Debug.Log($"已为 {text.gameObject.name} 添加简单本地化组件");
        }
    }

    private void AddLocalizationComponentToTMP(TMP_Text tmpText)
    {
        // 检查是否已经有SimpleLocalizedText组件
        if (tmpText.GetComponent<SimpleLocalizedText>() == null)
        {
            Undo.AddComponent<SimpleLocalizedText>(tmpText.gameObject);
            
            // 如果启用了自动生成键名，则生成键名并设置到Text中
            if (_autoGenerateKeys && !string.IsNullOrEmpty(tmpText.text))
            {
                string key = GenerateKeyFromTMP(tmpText);
                tmpText.text = key;
            }
            
            Debug.Log($"已为 {tmpText.gameObject.name} 添加简单本地化组件");
        }
    }

    private bool AddLocalizationComponentToPrefab(Text text)
    {
        if (text.GetComponent<SimpleLocalizedText>() == null)
        {
            text.gameObject.AddComponent<SimpleLocalizedText>();
            
            // 如果启用了自动生成键名，则生成键名并设置到Text中
            if (_autoGenerateKeys && !string.IsNullOrEmpty(text.text))
            {
                string key = GenerateKeyFromText(text);
                text.text = key;
            }
            
            return true;
        }
        return false;
    }

    private bool AddLocalizationComponentToPrefabTMP(TMP_Text tmpText)
    {
        if (tmpText.GetComponent<SimpleLocalizedText>() == null)
        {
            tmpText.gameObject.AddComponent<SimpleLocalizedText>();
            
            // 如果启用了自动生成键名，则生成键名并设置到Text中
            if (_autoGenerateKeys && !string.IsNullOrEmpty(tmpText.text))
            {
                string key = GenerateKeyFromTMP(tmpText);
                tmpText.text = key;
            }
            
            return true;
        }
        return false;
    }

    private string GenerateKeyFromText(Text text)
    {
        // 从对象路径和文本内容生成键名
        string objectPath = GetGameObjectPath(text.gameObject);
        string sanitizedText = SanitizeTextForKey(text.text);
        string key = $"{_keyPrefix}{objectPath}_{sanitizedText}";
        
        // 确保键名不超过一定长度
        if (key.Length > 100)
        {
            key = key.Substring(0, 100);
        }
        
        return key;
    }

    private string GenerateKeyFromTMP(TMP_Text tmpText)
    {
        // 从对象路径和文本内容生成键名
        string objectPath = GetGameObjectPath(tmpText.gameObject);
        string sanitizedText = SanitizeTextForKey(tmpText.text);
        string key = $"{_keyPrefix}{objectPath}_{sanitizedText}";
        
        // 确保键名不超过一定长度
        if (key.Length > 100)
        {
            key = key.Substring(0, 100);
        }
        
        return key;
    }

    private string GetGameObjectPath(GameObject obj)
    {
        // 获取对象的层级路径，用于生成唯一的键名
        string path = obj.name;
        Transform parent = obj.transform.parent;
        
        // 只取最近的两级父对象名称
        int depth = 0;
        while (parent != null && depth < 2)
        {
            path = parent.name + "_" + path;
            parent = parent.parent;
            depth++;
        }
        
        // 移除特殊字符
        path = SanitizeTextForKey(path);
        return path;
    }

    private string SanitizeTextForKey(string text)
    {
        // 移除特殊字符，只保留字母、数字和下划线
        StringBuilder sb = new StringBuilder();
        bool lastWasUnderscore = false;
        
        foreach (char c in text)
        {
            if (char.IsLetterOrDigit(c))
            {
                sb.Append(c);
                lastWasUnderscore = false;
            }
            else if (!lastWasUnderscore && (c == ' ' || c == '_' || c == '-'))
            {
                sb.Append('_');
                lastWasUnderscore = true;
            }
        }
        
        // 限制长度
        string result = sb.ToString();
        if (result.Length > 30)
        {
            result = result.Substring(0, 30);
        }
        
        return result;
    }

    private string[] GetAllPrefabPaths()
    {
        List<string> prefabPaths = new List<string>();
        string[] allAssetPaths = AssetDatabase.GetAllAssetPaths();

        foreach (string path in allAssetPaths)
        {
            if (path.EndsWith(".prefab"))
            {
                prefabPaths.Add(path);
            }
        }

        return prefabPaths.ToArray();
    }

    private void ExportSceneTextsToCSV()
    {
        Dictionary<string, string> textEntries = new Dictionary<string, string>();
        
        // 收集所有Text组件的文本
        Text[] allTexts = FindObjectsOfType<Text>(_processInactive);
        foreach (Text text in allTexts)
        {
            if (!string.IsNullOrEmpty(text.text))
            {
                string key = _autoGenerateKeys ? GenerateKeyFromText(text) : text.text;
                if (!textEntries.ContainsKey(key))
                {
                    textEntries.Add(key, text.text);
                }
            }
        }
        
        // 收集所有TextMeshPro组件的文本
        if (_processTextMeshPro)
        {
            TMP_Text[] allTMPTexts = FindObjectsOfType<TMP_Text>(_processInactive);
            foreach (TMP_Text tmpText in allTMPTexts)
            {
                if (!string.IsNullOrEmpty(tmpText.text))
                {
                    string key = _autoGenerateKeys ? GenerateKeyFromTMP(tmpText) : tmpText.text;
                    if (!textEntries.ContainsKey(key))
                    {
                        textEntries.Add(key, tmpText.text);
                    }
                }
            }
        }
        
        // 导出为CSV
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Key,Id,Shared Comments,English(en),Chinese (Simplified)(zh-CN),French(fr)");
        
        foreach (var entry in textEntries)
        {
            sb.AppendLine($"{entry.Key},,Text from {entry.Key},{entry.Value},,");
        }
        
        // 确保目录存在
        string directory = Path.GetDirectoryName(_exportPath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        
        // 写入文件
        File.WriteAllText(_exportPath, sb.ToString());
        Debug.Log($"已导出文本到 {_exportPath}");
        
        // 在编辑器中显示文件
        AssetDatabase.Refresh();
        Selection.activeObject = AssetDatabase.LoadAssetAtPath<TextAsset>(_exportPath);
    }
}
