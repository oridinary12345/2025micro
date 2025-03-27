using UnityEngine;
using UnityEditor;
using System.Collections;

namespace SgLib
{
    public class EditorTools
    {
        [MenuItem("Tools/Reset PlayerPrefs", false)]
        public static void ResetPlayerPref()
        {
            PlayerPrefs.DeleteAll();
            Debug.Log("*** PlayerPrefs was reset! ***");
        }



        [MenuItem("Tools/OpenPersistentDataPath", false)]
        public static void OpenPersistentDataPath()
        {
            OpenDirectory(Application.persistentDataPath);
        }

        public static void OpenDirectory(string path)
        {
            if (string.IsNullOrEmpty(path)) return;

            path = path.Replace("/", "\\");
            if (!System.IO.Directory.Exists(path))
            {
                Debug.LogError("No Directory: " + path);
                return;
            }

            System.Diagnostics.Process.Start("explorer.exe", path);
        }
    }


}
