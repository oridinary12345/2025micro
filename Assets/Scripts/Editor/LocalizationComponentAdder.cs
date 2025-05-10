using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Events;
using System.Collections.Generic;

public class LocalizationComponentAdder : EditorWindow
{
    private bool _processInactive = true;
    private bool _includeChildren = true;

    [MenuItem("Tools/Add Localization Components")]
    public static void ShowWindow()
    {
        GetWindow(typeof(LocalizationComponentAdder), false, "本地化组件添加器");
    }

    private void OnGUI()
    {
        EditorGUILayout.HelpBox("此工具将为Text组件添加LocalizeStringEvent组件。", MessageType.Info);
        
        _processInactive = EditorGUILayout.Toggle("处理未激活的对象", _processInactive);
        _includeChildren = EditorGUILayout.Toggle("包含子对象", _includeChildren);

        EditorGUILayout.Space();

        if (GUILayout.Button("处理选中对象"))
        {
            ProcessSelectedObjects();
        }

        if (GUILayout.Button("处理场景中所有对象"))
        {
            if (EditorUtility.DisplayDialog("确认",
                "这将处理场景中的所有Text组件。是否继续？",
                "是", "取消"))
            {
                ProcessAllInScene();
            }
        }

        if (GUILayout.Button("处理所有预制体"))
        {
            if (EditorUtility.DisplayDialog("确认",
                "这将处理所有预制体中的Text组件。此操作不能撤销。是否继续？",
                "是", "取消"))
            {
                ProcessAllPrefabs();
            }
        }
    }

    private void ProcessSelectedObjects()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            ProcessGameObject(obj);
        }
    }

    private void ProcessAllInScene()
    {
        Text[] allTexts = FindObjectsOfType<Text>(_processInactive);
        foreach (Text text in allTexts)
        {
            ProcessText(text);
        }
    }

    private void ProcessAllPrefabs()
    {
        string[] allPrefabPaths = GetAllPrefabPaths();
        int modifiedCount = 0;

        foreach (string prefabPath in allPrefabPaths)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if (prefab == null) continue;

            bool modified = false;
            Text[] texts = prefab.GetComponentsInChildren<Text>(_processInactive);
            
            if (texts.Length > 0)
            {
                foreach (Text text in texts)
                {
                    if (ProcessText(text))
                        modified = true;
                }

                if (modified)
                {
                    EditorUtility.SetDirty(prefab);
                    modifiedCount++;
                }
            }
        }

        if (modifiedCount > 0)
        {
            AssetDatabase.SaveAssets();
            Debug.Log($"已修改 {modifiedCount} 个预制体");
        }
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
        }
        else
        {
            // 只处理当前对象
            Text text = obj.GetComponent<Text>();
            if (text != null)
            {
                ProcessText(text);
            }
        }
    }

    private bool ProcessText(Text text)
    {
        bool modified = false;

        // 检查是否已经有LocalizeStringEvent组件
        LocalizeStringEvent localizeEvent = text.GetComponent<LocalizeStringEvent>();
        if (localizeEvent == null)
        {
            localizeEvent = Undo.AddComponent<LocalizeStringEvent>(text.gameObject);
            modified = true;

            // 设置LocalizeStringEvent的目标组件
            localizeEvent.StringReference.TableEntryReference = text.text;
            var textReference = new UnityEventString();
            textReference.AddListener((string s) => text.text = s);
            localizeEvent.OnUpdateString = textReference;

            Debug.Log($"已为 {text.gameObject.name} 添加本地化组件");
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
}
