using UnityEngine;
using UnityEditor;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.Localization.Settings;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor.Localization;
#endif

public class LocalizationTableEditor : EditorWindow
{
    private Vector2 scrollPosition;
    private StringTable enTable;
    private StringTable zhTable;
    private StringTableCollection collection;
    private bool showMissingOnly = false;
    private string searchString = "";
    
    // 新条目的临时存储
    private string _newKey = "";
    private string _newEnValue = "";
    private string _newZhValue = "";

    [MenuItem("Tools/Localization Table Editor")]
    public static void ShowWindow()
    {
        GetWindow(typeof(LocalizationTableEditor), false, "本地化表编辑器");
    }

    private void OnEnable()
    {
        LoadTables();
    }

    private void LoadTables()
    {
        // 加载字符串表集合
#if UNITY_EDITOR
        collection = LocalizationEditorSettings.GetStringTableCollection("GameStrings");
#endif

        if (collection != null)
        {
            // 加载英文和中文表
            enTable = collection.GetTable("en") as StringTable;
            zhTable = collection.GetTable("zh-CN") as StringTable;
        }
    }

    private void OnGUI()
    {
        if (collection == null || enTable == null || zhTable == null)
        {
            EditorGUILayout.HelpBox("未能找到本地化表。请确保 GameStrings 表存在且包含英文和中文版本。", MessageType.Error);
            if (GUILayout.Button("重新加载表"))
            {
                LoadTables();
            }
            return;
        }

        EditorGUILayout.BeginHorizontal();
        showMissingOnly = EditorGUILayout.Toggle("只显示缺失翻译", showMissingOnly);
        searchString = EditorGUILayout.TextField("搜索", searchString);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        
        // 显示表头
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Key", GUILayout.Width(200));
        EditorGUILayout.LabelField("英文", GUILayout.Width(300));
        EditorGUILayout.LabelField("中文", GUILayout.Width(300));
        EditorGUILayout.EndHorizontal();

        // 获取所有键
        var keys = collection.SharedData.Entries.Select(e => e.Key).ToList();
        
        foreach (var key in keys)
        {
            var enEntry = enTable.GetEntry(key);
            var zhEntry = zhTable.GetEntry(key);

            string enValue = enEntry?.Value ?? "";
            string zhValue = zhEntry?.Value ?? "";

            // 如果设置了只显示缺失翻译且两种语言都有翻译，则跳过
            if (showMissingOnly && !string.IsNullOrEmpty(enValue) && !string.IsNullOrEmpty(zhValue))
                continue;

            // 如果设置了搜索字符串且key不包含搜索字符串，则跳过
            if (!string.IsNullOrEmpty(searchString) && 
                !key.ToLower().Contains(searchString.ToLower()) && 
                !enValue.ToLower().Contains(searchString.ToLower()) && 
                !zhValue.ToLower().Contains(searchString.ToLower()))
                continue;

            EditorGUILayout.BeginHorizontal();
            
            // Key
            EditorGUILayout.LabelField(key, GUILayout.Width(200));
            
            // 英文值
            string newEnValue = EditorGUILayout.TextField(enValue, GUILayout.Width(300));
            if (newEnValue != enValue)
            {
                if (enEntry == null)
                    enEntry = enTable.AddEntry(key, newEnValue);
                else
                    enEntry.Value = newEnValue;
                EditorUtility.SetDirty(enTable);
            }
            
            // 中文值
            string newZhValue = EditorGUILayout.TextField(zhValue, GUILayout.Width(300));
            if (newZhValue != zhValue)
            {
                if (zhEntry == null)
                    zhEntry = zhTable.AddEntry(key, newZhValue);
                else
                    zhEntry.Value = newZhValue;
                EditorUtility.SetDirty(zhTable);
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();

        // 添加新条目的界面
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("添加新条目", EditorStyles.boldLabel);

        // 使用类字段来存储新条目的值
        _newKey = EditorGUILayout.TextField("Key", _newKey);
        _newEnValue = EditorGUILayout.TextField("英文", _newEnValue);
        _newZhValue = EditorGUILayout.TextField("中文", _newZhValue);

        if (GUILayout.Button("添加条目"))
        {
            if (!string.IsNullOrEmpty(_newKey))
            {
                if (!string.IsNullOrEmpty(_newEnValue))
                    enTable.AddEntry(_newKey, _newEnValue);
                if (!string.IsNullOrEmpty(_newZhValue))
                    zhTable.AddEntry(_newKey, _newZhValue);
                
                EditorUtility.SetDirty(enTable);
                EditorUtility.SetDirty(zhTable);
                
                _newKey = "";
                _newEnValue = "";
                _newZhValue = "";
            }
        }

        // 保存按钮
        EditorGUILayout.Space();
        if (GUILayout.Button("保存更改"))
        {
            AssetDatabase.SaveAssets();
            Debug.Log("本地化表已保存");
        }
    }
}
