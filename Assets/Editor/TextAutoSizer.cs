using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class TextAutoSizer : EditorWindow
{
    private bool _bestFit = true;
    private int _minSize = 10;
    private int _maxSize = 40;
    private bool _horizontalOverflow = false;
    private bool _verticalOverflow = false;
    private bool _resizeTextForBestFit = true;

    // 自动换行设置
    private bool _modifyWordWrap = false;
    private bool _wordWrap = true;

    // 对齐方式设置
    private bool _modifyAlignment = false;
    private TextAnchor _alignment = TextAnchor.MiddleCenter;
    private Vector2 _scrollPosition;

    [MenuItem("Tools/Text Auto Sizer")]
    public static void ShowWindow()
    {
        GetWindow(typeof(TextAutoSizer), false, "Text Auto Sizer");
    }

    private void OnGUI()
    {
        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

        EditorGUILayout.LabelField("Text Auto Size Settings", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        EditorGUILayout.HelpBox("This tool will modify Text components in the scene and prefabs.", MessageType.Info);

        // 自适应大小设置
        using (new EditorGUILayout.VerticalScope("box"))
        {
            EditorGUILayout.LabelField("Size Settings", EditorStyles.boldLabel);
            _bestFit = EditorGUILayout.Toggle("Enable Best Fit", _bestFit);

            using (new EditorGUI.DisabledScope(!_bestFit))
            {
                _minSize = EditorGUILayout.IntField("Min Size", _minSize);
                _maxSize = EditorGUILayout.IntField("Max Size", _maxSize);
                _resizeTextForBestFit = EditorGUILayout.Toggle("Resize Text For Best Fit", _resizeTextForBestFit);
            }

            _horizontalOverflow = EditorGUILayout.Toggle("Horizontal Overflow", _horizontalOverflow);
            _verticalOverflow = EditorGUILayout.Toggle("Vertical Overflow", _verticalOverflow);
        }

        EditorGUILayout.Space();

        // 自动换行设置
        using (new EditorGUILayout.VerticalScope("box"))
        {
            EditorGUILayout.LabelField("Word Wrap Settings", EditorStyles.boldLabel);
            _modifyWordWrap = EditorGUILayout.Toggle("Modify Word Wrap", _modifyWordWrap);

            using (new EditorGUI.DisabledScope(!_modifyWordWrap))
            {
                _wordWrap = EditorGUILayout.Toggle("Enable Word Wrap", _wordWrap);
            }
        }

        EditorGUILayout.Space();

        // 对齐方式设置
        using (new EditorGUILayout.VerticalScope("box"))
        {
            EditorGUILayout.LabelField("Alignment Settings", EditorStyles.boldLabel);
            _modifyAlignment = EditorGUILayout.Toggle("Modify Alignment", _modifyAlignment);

            using (new EditorGUI.DisabledScope(!_modifyAlignment))
            {
                _alignment = (TextAnchor)EditorGUILayout.EnumPopup("Text Alignment", _alignment);
            }
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Apply to Selected Objects"))
        {
            ApplyToSelected();
        }

        if (GUILayout.Button("Apply to All Scene Objects"))
        {
            ApplyToAllInScene();
        }

        if (GUILayout.Button("Apply to All Prefabs"))
        {
            ApplyToAllPrefabs();
        }

        EditorGUILayout.EndScrollView();
    }

    private void ApplyToSelected()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            ApplyToGameObject(obj);
        }
    }

    private void ApplyToAllInScene()
    {
        if (!EditorUtility.DisplayDialog("Confirm",
            "This will modify all Text components in the current scene. Are you sure?",
            "Yes", "Cancel"))
        {
            return;
        }

        Text[] allTexts = FindObjectsOfType(typeof(Text)) as Text[];
        foreach (Text text in allTexts)
        {
            ConfigureText(text);
        }
    }

    private void ApplyToAllPrefabs()
    {
        if (!EditorUtility.DisplayDialog("Confirm",
            "This will modify all Text components in all prefabs. This operation cannot be undone. Are you sure?",
            "Yes", "Cancel"))
        {
            return;
        }

        string[] allPrefabPaths = GetAllPrefabPaths();
        int modifiedCount = 0;

        foreach (string prefabPath in allPrefabPaths)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if (prefab == null) continue;

            bool modified = false;
            Text[] texts = prefab.GetComponentsInChildren<Text>(true);
            
            if (texts.Length > 0)
            {
                foreach (Text text in texts)
                {
                    ConfigureText(text);
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
            Debug.Log($"Modified {modifiedCount} prefabs");
        }
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

    private void ApplyToGameObject(GameObject obj)
    {
        // 处理当前对象上的 Text 组件
        Text text = obj.GetComponent(typeof(Text)) as Text;
        if (text != null)
        {
            ConfigureText(text);
        }

        // 递归处理子对象
        foreach (Transform child in obj.transform)
        {
            ApplyToGameObject(child.gameObject);
        }
    }

    private void ConfigureText(Text text)
    {
        Undo.RecordObject(text, "Configure Text Auto Size");

        // 无论是否启用自适应，都设置相关属性
        text.resizeTextForBestFit = _bestFit && _resizeTextForBestFit;
        text.resizeTextMinSize = _minSize;
        text.resizeTextMaxSize = _maxSize;

        // 设置换行和溢出方式
        if (_modifyWordWrap)
        {
            text.horizontalOverflow = !_wordWrap ? HorizontalWrapMode.Overflow : HorizontalWrapMode.Wrap;
        }
        else
        {
            text.horizontalOverflow = _horizontalOverflow ? HorizontalWrapMode.Overflow : HorizontalWrapMode.Wrap;
        }
        text.verticalOverflow = _verticalOverflow ? VerticalWrapMode.Overflow : VerticalWrapMode.Truncate;

        // 设置对齐方式
        if (_modifyAlignment)
        {
            text.alignment = _alignment;
        }

        EditorUtility.SetDirty(text);
    }
}
