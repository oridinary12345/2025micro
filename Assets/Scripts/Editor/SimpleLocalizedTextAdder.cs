using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class SimpleLocalizedTextAdder : EditorWindow
{
    [MenuItem("Tools/Add Simple Localization")]
    public static void ShowWindow()
    {
        GetWindow(typeof(SimpleLocalizedTextAdder), false, "简单本地化添加器");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("为选中对象添加本地化"))
        {
            ProcessSelectedObjects();
        }

        if (GUILayout.Button("为场景所有Text添加本地化"))
        {
            if (EditorUtility.DisplayDialog("确认", 
                "这将为场景中所有Text组件添加本地化组件。是否继续？", 
                "是", "取消"))
            {
                ProcessAllInScene();
            }
        }
    }

    private void ProcessSelectedObjects()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            AddLocalizationToObject(obj);
        }
    }

    private void ProcessAllInScene()
    {
        Text[] allTexts = FindObjectsOfType<Text>();
        foreach (Text text in allTexts)
        {
            AddLocalizationComponent(text);
        }
    }

    private void AddLocalizationToObject(GameObject obj)
    {
        // 处理对象及其所有子对象
        Text[] texts = obj.GetComponentsInChildren<Text>(true);
        foreach (Text text in texts)
        {
            AddLocalizationComponent(text);
        }
    }

    private void AddLocalizationComponent(Text text)
    {
        if (text.GetComponent<SimpleLocalizedText>() == null)
        {
            Undo.AddComponent<SimpleLocalizedText>(text.gameObject);
            Debug.Log($"已为 {text.gameObject.name} 添加简单本地化组件");
        }
    }
}
