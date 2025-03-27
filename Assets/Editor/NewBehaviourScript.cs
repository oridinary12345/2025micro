using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class MyEditorUtils
{
    [MenuItem("GameTools/遍历Hierarchy")]
    static void GetAllSceneObjectsWithInactive()
    {
        var allGos = Resources.FindObjectsOfTypeAll(typeof(GameObject));
        var previousSelection = Selection.objects;
        Selection.objects = allGos;
        var selectedTransforms = Selection.GetTransforms(SelectionMode.Editable | SelectionMode.ExcludePrefab);
        Selection.objects = previousSelection;
        foreach (var trans in selectedTransforms)
        {
            if (trans.gameObject.layer == LayerMask.NameToLayer("MiniMap"))
            {
                Debug.Log(trans.name);
            }
        }
    }

    [MenuItem("GameTools/打开文件")]
    static void OpenMyFile()
    {
        RunCmd("\"C:/Program Files/Notepad++/notepad++.exe\" " + "\"" + Application.dataPath + AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]).Replace("Assets/", "/") + "\"");
    }
    [MenuItem("GameTools/打开文件META")]
    static void OpenMyFileMETA()
    {
        RunCmd("\"C:/Program Files/Notepad++/notepad++.exe\" " + "\"" + Application.dataPath + AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]).Replace("Assets/", "/") + ".meta\"");
    }
    public static void RunCmd(string cmd, string args = "", string workdir = null)
    {
        string[] res = new string[2];
        var p = CreateCmdProcess(cmd, args, workdir);
        res[0] = p.StandardOutput.ReadToEnd();
        res[1] = p.StandardError.ReadToEnd();
        p.Close();
    }

    public static System.Diagnostics.Process CreateCmdProcess(string cmd, string args, string workdir = null)
    {
        var pStartInfo = new System.Diagnostics.ProcessStartInfo(cmd);
        pStartInfo.Arguments = args;
        pStartInfo.CreateNoWindow = false;
        pStartInfo.UseShellExecute = false;
        pStartInfo.RedirectStandardError = true;
        pStartInfo.RedirectStandardInput = true;
        pStartInfo.RedirectStandardOutput = true;
        pStartInfo.StandardErrorEncoding = System.Text.UTF8Encoding.UTF8;
        pStartInfo.StandardOutputEncoding = System.Text.UTF8Encoding.UTF8;
        if (!string.IsNullOrEmpty(workdir))
            pStartInfo.WorkingDirectory = workdir;
        return System.Diagnostics.Process.Start(pStartInfo);
    }

    [MenuItem("Assets/移动", false, 10)]
    static void MoveMyFile()
    {
        string pathStr = "";
        string[] pathStrA;
        foreach (var guid in Selection.assetGUIDs)
        {
            pathStr = Application.dataPath + AssetDatabase.GUIDToAssetPath(guid).Replace("Assets/", "/");
            pathStrA = pathStr.Replace("/Assets", "#").Split('#');
            if (pathStrA[0].Contains("_uTR"))
            {
                if (File.Exists(pathStr.Replace("_uTR", "")))
                {
                    File.Delete(pathStr.Replace("_uTR", ""));
                    File.Copy(pathStr, pathStr.Replace("_uTR", ""));
                }
                else
                {
                    Debug.LogError("不存在文件" + pathStr.Replace("_uTR", ""));
                }
            }
            else
            {
                if (File.Exists(pathStr.Replace("/Assets", "_uTR/Assets")))
                {
                    File.Delete(pathStr.Replace("/Assets", "_uTR/Assets"));
                    File.Copy(pathStr, pathStr.Replace("/Assets", "_uTR/Assets"));
                }
                else
                {
                    Debug.LogError("不存在文件" + pathStr.Replace("/Assets", "_uTR/Assets"));
                }
            }

        }
    }
}
