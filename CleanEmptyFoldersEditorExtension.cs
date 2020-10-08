using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class CleanEmptyFoldersEditorExtension : EditorWindow
{
    private static string _deletedFolders;

    [MenuItem("Tools/Clean Empty Folders")]
    private static void Cleanup()
    {
        _deletedFolders = string.Empty;

        var directoryInfo = new DirectoryInfo(Application.dataPath);
        foreach(var subDirectory in directoryInfo.GetDirectories("*.*", SearchOption.AllDirectories))
        {
            if (subDirectory.Exists)
            {
                ScanDirectory(subDirectory);
            }
        }

        Debug.Log("Deleted Folders:\n" + (_deletedFolders.Length > 0 ? _deletedFolders : "NONE"));
    }

    private static string ScanDirectory(DirectoryInfo subDirectory)
    {
        Debug.Log("Scanning Directory: " + subDirectory.FullName);

        var filesInSubDirectory = subDirectory.GetFiles("*.*", SearchOption.AllDirectories);

        if (filesInSubDirectory.Length == 0 ||
            filesInSubDirectory.All(t => t.FullName.EndsWith(".meta")))
        {
            _deletedFolders += subDirectory.FullName + "\n";
            subDirectory.Delete(true);
            
            var dirMetaFile = subDirectory.FullName + ".meta";
            if (File.Exists(dirMetaFile))
            {
                File.Delete(dirMetaFile);
            }
        }

        return _deletedFolders;
    }
}
