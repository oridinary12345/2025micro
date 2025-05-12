using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Events;
using UnityEngine.Localization.Tables;
using UnityEngine.Localization.Settings;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using UnityEditor.Localization;

public class LocalizationComponentAdder : EditorWindow
{
    private bool _processInactive = true;
    private bool _includeChildren = true;
    private bool _processTextMeshPro = true;
    private bool _autoGenerateKeys = true;
    private string _keyPrefix = "";
    private string _tableReference = "GameStrings";
    private string _exportPath = "Assets/Localization/ExportedTexts.csv";
    private Vector2 _scrollPosition;
    private bool _showAdvancedOptions = false;
    private bool _showPreview = false;
    private List<LocalizationPreviewItem> _previewItems = new List<LocalizationPreviewItem>();
    private Vector2 _previewScrollPosition;
    private string _searchText = "";
    private bool _useObjectPathForKey = true;

    private class LocalizationPreviewItem
    {
        public GameObject GameObject;
        public string OriginalText;
        public string KeyName;
        public bool Selected = true;
    }

    [MenuItem("Tools/Add Localization Components")]
    public static void ShowWindow()
    {
        GetWindow(typeof(LocalizationComponentAdder), false, "本地化组件添加器");
    }

    private void OnGUI()
    {
        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

        EditorGUILayout.HelpBox("此工具将为文本组件添加LocalizeStringEvent组件，使用Unity的本地化系统。", MessageType.Info);
        
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
            
            // 表名选择
            List<string> tableNames = GetAvailableStringTables();
            int selectedIndex = tableNames.IndexOf(_tableReference);
            if (selectedIndex < 0 && tableNames.Count > 0) selectedIndex = 0;
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("字符串表", GUILayout.Width(120));
            if (tableNames.Count > 0)
            {
                selectedIndex = EditorGUILayout.Popup(selectedIndex, tableNames.ToArray());
                if (selectedIndex >= 0 && selectedIndex < tableNames.Count)
                {
                    _tableReference = tableNames[selectedIndex];
                }
            }
            else
            {
                EditorGUILayout.LabelField("未找到字符串表", EditorStyles.boldLabel);
            }
            EditorGUILayout.EndHorizontal();
            
            // 键名生成选项
            _autoGenerateKeys = EditorGUILayout.Toggle("自动生成键名", _autoGenerateKeys);
            if (_autoGenerateKeys)
            {
                _keyPrefix = EditorGUILayout.TextField("键名前缀", _keyPrefix);
                _useObjectPathForKey = EditorGUILayout.Toggle("使用对象路径生成键名", _useObjectPathForKey);
            }
            
            _exportPath = EditorGUILayout.TextField("导出路径", _exportPath);
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("操作", EditorStyles.boldLabel);

        if (GUILayout.Button("预览选中对象的本地化"))
        {
            PreviewSelectedObjects();
        }

        if (GUILayout.Button("处理选中对象"))
        {
            ProcessSelectedObjects();
        }

        if (GUILayout.Button("处理场景中所有对象"))
        {
            if (EditorUtility.DisplayDialog("确认",
                "这将处理场景中的所有文本组件。是否继续？",
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
        
        // 显示预览区域
        if (_showPreview && _previewItems.Count > 0)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("预览", EditorStyles.boldLabel);
            
            // 搜索框
            _searchText = EditorGUILayout.TextField("搜索", _searchText);
            
            // 全选/取消全选按钮
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("全选", GUILayout.Width(100)))
            {
                foreach (var item in _previewItems)
                {
                    item.Selected = true;
                }
            }
            if (GUILayout.Button("取消全选", GUILayout.Width(100)))
            {
                foreach (var item in _previewItems)
                {
                    item.Selected = false;
                }
            }
            EditorGUILayout.EndHorizontal();
            
            // 预览列表
            _previewScrollPosition = EditorGUILayout.BeginScrollView(_previewScrollPosition, GUILayout.Height(300));
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("选择", GUILayout.Width(50));
            EditorGUILayout.LabelField("对象", GUILayout.Width(200));
            EditorGUILayout.LabelField("原始文本", GUILayout.Width(200));
            EditorGUILayout.LabelField("键名", GUILayout.Width(200));
            EditorGUILayout.EndHorizontal();
            
            foreach (var item in _previewItems)
            {
                // 如果有搜索文本，则过滤
                if (!string.IsNullOrEmpty(_searchText))
                {
                    if (!item.GameObject.name.ToLower().Contains(_searchText.ToLower()) && 
                        !item.OriginalText.ToLower().Contains(_searchText.ToLower()) &&
                        !item.KeyName.ToLower().Contains(_searchText.ToLower()))
                    {
                        continue;
                    }
                }
                
                EditorGUILayout.BeginHorizontal();
                item.Selected = EditorGUILayout.Toggle(item.Selected, GUILayout.Width(50));
                EditorGUILayout.ObjectField(item.GameObject, typeof(GameObject), true, GUILayout.Width(200));
                EditorGUILayout.TextField(item.OriginalText, GUILayout.Width(200));
                EditorGUILayout.TextField(item.KeyName, GUILayout.Width(200));
                EditorGUILayout.EndHorizontal();
            }
            
            EditorGUILayout.EndScrollView();
            
            // 应用选中项按钮
            if (GUILayout.Button("应用选中项"))
            {
                ApplySelectedPreviewItems();
            }
        }
        
        EditorGUILayout.EndScrollView();
    }

    private List<string> GetAvailableStringTables()
    {
        List<string> tableNames = new List<string>();
        
        // 获取所有可用的字符串表
        if (LocalizationSettings.StringDatabase != null)
        {
            // 使用AssetDatabase查找所有StringTableCollection资源
            string[] guids = AssetDatabase.FindAssets("t:StringTableCollection");
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var tableCollection = AssetDatabase.LoadAssetAtPath<StringTableCollection>(path);
                if (tableCollection != null && !string.IsNullOrEmpty(tableCollection.TableCollectionName))
                {
                    tableNames.Add(tableCollection.TableCollectionName);
                }
            }
            
            // 如果没有找到表，添加默认的GameStrings
            if (tableNames.Count == 0)
            {
                tableNames.Add("GameStrings");
            }
        }
        
        return tableNames;
    }
    
    private void PreviewSelectedObjects()
    {
        _previewItems.Clear();
        
        foreach (GameObject obj in Selection.gameObjects)
        {
            CollectPreviewItems(obj);
        }
        
        _showPreview = true;
    }
    
    private void CollectPreviewItems(GameObject obj)
    {
        if (_includeChildren)
        {
            // 收集Text组件
            Text[] texts = obj.GetComponentsInChildren<Text>(_processInactive);
            foreach (Text text in texts)
            {
                if (!string.IsNullOrEmpty(text.text))
                {
                    string key = _autoGenerateKeys ? GenerateKeyFromText(text) : text.text;
                    _previewItems.Add(new LocalizationPreviewItem
                    {
                        GameObject = text.gameObject,
                        OriginalText = text.text,
                        KeyName = key
                    });
                }
            }
            
            // 收集TextMeshPro组件
            if (_processTextMeshPro)
            {
                TMP_Text[] tmpTexts = obj.GetComponentsInChildren<TMP_Text>(_processInactive);
                foreach (TMP_Text tmpText in tmpTexts)
                {
                    if (!string.IsNullOrEmpty(tmpText.text))
                    {
                        string key = _autoGenerateKeys ? GenerateKeyFromTMP(tmpText) : tmpText.text;
                        _previewItems.Add(new LocalizationPreviewItem
                        {
                            GameObject = tmpText.gameObject,
                            OriginalText = tmpText.text,
                            KeyName = key
                        });
                    }
                }
            }
        }
        else
        {
            // 只处理当前对象
            Text text = obj.GetComponent<Text>();
            if (text != null && !string.IsNullOrEmpty(text.text))
            {
                string key = _autoGenerateKeys ? GenerateKeyFromText(text) : text.text;
                _previewItems.Add(new LocalizationPreviewItem
                {
                    GameObject = text.gameObject,
                    OriginalText = text.text,
                    KeyName = key
                });
            }
            
            if (_processTextMeshPro)
            {
                TMP_Text tmpText = obj.GetComponent<TMP_Text>();
                if (tmpText != null && !string.IsNullOrEmpty(tmpText.text))
                {
                    string key = _autoGenerateKeys ? GenerateKeyFromTMP(tmpText) : tmpText.text;
                    _previewItems.Add(new LocalizationPreviewItem
                    {
                        GameObject = tmpText.gameObject,
                        OriginalText = tmpText.text,
                        KeyName = key
                    });
                }
            }
        }
    }
    
    private void ApplySelectedPreviewItems()
    {
        int processedCount = 0;
        
        foreach (var item in _previewItems)
        {
            if (item.Selected)
            {
                // 处理Text组件
                Text text = item.GameObject.GetComponent<Text>();
                if (text != null)
                {
                    ProcessText(text, item.KeyName);
                    processedCount++;
                }
                
                // 处理TextMeshPro组件
                TMP_Text tmpText = item.GameObject.GetComponent<TMP_Text>();
                if (tmpText != null)
                {
                    ProcessTextMeshPro(tmpText, item.KeyName);
                    processedCount++;
                }
            }
        }
        
        Debug.Log($"已处理 {processedCount} 个选中的文本组件");
        _showPreview = false;
        _previewItems.Clear();
    }
    
    private void ProcessSelectedObjects()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            ProcessGameObject(obj);
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
            
            ProcessText(text);
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
                
                ProcessTextMeshPro(tmpText);
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
                if (ProcessText(text))
                    modified = true;
            }

            // 处理TextMeshPro组件
            if (_processTextMeshPro)
            {
                TMP_Text[] tmpTexts = prefab.GetComponentsInChildren<TMP_Text>(_processInactive);
                foreach (TMP_Text tmpText in tmpTexts)
                {
                    if (ProcessTextMeshPro(tmpText))
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

    private void ProcessGameObject(GameObject obj)
    {
        if (_includeChildren)
        {
            // 处理所有子对象
            Text[] texts = obj.GetComponentsInChildren<Text>(_processInactive);
            foreach (Text text in texts)
            {
                ProcessText(text);
            }
            
            // 处理TextMeshPro组件
            if (_processTextMeshPro)
            {
                TMP_Text[] tmpTexts = obj.GetComponentsInChildren<TMP_Text>(_processInactive);
                foreach (TMP_Text tmpText in tmpTexts)
                {
                    ProcessTextMeshPro(tmpText);
                }
            }
        }
        else
        {
            // 只处理当前对象
            Text text = obj.GetComponent<Text>();
            if (text != null)
            {
                ProcessText(text);
            }
            
            if (_processTextMeshPro)
            {
                TMP_Text tmpText = obj.GetComponent<TMP_Text>();
                if (tmpText != null)
                {
                    ProcessTextMeshPro(tmpText);
                }
            }
        }
    }

    private bool ProcessText(Text text)
    {
        return ProcessText(text, null);
    }
    
    private bool ProcessText(Text text, string keyOverride)
    {
        bool modified = false;

        // 检查是否已经有LocalizeStringEvent组件
        LocalizeStringEvent localizeEvent = text.GetComponent<LocalizeStringEvent>();
        if (localizeEvent == null)
        {
            localizeEvent = Undo.AddComponent<LocalizeStringEvent>(text.gameObject);
            modified = true;

            // 设置LocalizeStringEvent的目标组件
            string key = keyOverride;
            if (string.IsNullOrEmpty(key))
            {
                key = _autoGenerateKeys ? GenerateKeyFromText(text) : text.text;
            }
            
            localizeEvent.StringReference.TableReference = _tableReference;
            localizeEvent.StringReference.TableEntryReference = key;
            
            var textReference = new UnityEventString();
            textReference.AddListener((string s) => text.text = s);
            localizeEvent.OnUpdateString = textReference;

            Debug.Log($"已为 {text.gameObject.name} 添加本地化组件，键名: {key}");
        }

        return modified;
    }
    
    private bool ProcessTextMeshPro(TMP_Text tmpText)
    {
        return ProcessTextMeshPro(tmpText, null);
    }
    
    private bool ProcessTextMeshPro(TMP_Text tmpText, string keyOverride)
    {
        bool modified = false;

        // 检查是否已经有LocalizeStringEvent组件
        LocalizeStringEvent localizeEvent = tmpText.GetComponent<LocalizeStringEvent>();
        if (localizeEvent == null)
        {
            localizeEvent = Undo.AddComponent<LocalizeStringEvent>(tmpText.gameObject);
            modified = true;

            // 设置LocalizeStringEvent的目标组件
            string key = keyOverride;
            if (string.IsNullOrEmpty(key))
            {
                key = _autoGenerateKeys ? GenerateKeyFromTMP(tmpText) : tmpText.text;
            }
            
            localizeEvent.StringReference.TableReference = _tableReference;
            localizeEvent.StringReference.TableEntryReference = key;
            
            var textReference = new UnityEventString();
            textReference.AddListener((string s) => tmpText.text = s);
            localizeEvent.OnUpdateString = textReference;

            Debug.Log($"已为 {tmpText.gameObject.name} 添加本地化组件，键名: {key}");
        }

        return modified;
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
    
    private string GenerateKeyFromText(Text text)
    {
        // 从对象路径和文本内容生成键名
        string key;
        
        if (_useObjectPathForKey)
        {
            string objectPath = GetGameObjectPath(text.gameObject);
            string sanitizedText = SanitizeTextForKey(text.text);
            key = $"{_keyPrefix}{objectPath}_{sanitizedText}";
        }
        else
        {
            // 只使用文本内容生成键名
            string sanitizedText = SanitizeTextForKey(text.text);
            key = $"{_keyPrefix}{sanitizedText}";
        }
        
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
        string key;
        
        if (_useObjectPathForKey)
        {
            string objectPath = GetGameObjectPath(tmpText.gameObject);
            string sanitizedText = SanitizeTextForKey(tmpText.text);
            key = $"{_keyPrefix}{objectPath}_{sanitizedText}";
        }
        else
        {
            // 只使用文本内容生成键名
            string sanitizedText = SanitizeTextForKey(tmpText.text);
            key = $"{_keyPrefix}{sanitizedText}";
        }
        
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
