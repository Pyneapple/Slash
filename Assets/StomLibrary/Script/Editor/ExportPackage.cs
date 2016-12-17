using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;
using System.Collections.Generic;

namespace Stom
{
    public enum TypeExport
    {
        Independent,
        IncludeProjectSettings,
        Library
    }

    public class ExportPackage : Editor
    {
        private const string pathProject = "Assets";

        public static void Export(TypeExport typeExport, string path, string name)
        {
            string combine_path = path + "/" + name + ".unitypackage";
            switch (typeExport)
            {
                case TypeExport.Independent:
                    AssetDatabase.ExportPackage(pathProject, combine_path, ExportPackageOptions.Interactive | ExportPackageOptions.Default | ExportPackageOptions.Recurse);
                    break;
                case TypeExport.IncludeProjectSettings:
                    AssetDatabase.ExportPackage(pathProject, combine_path, ExportPackageOptions.Interactive | ExportPackageOptions.IncludeLibraryAssets | ExportPackageOptions.Recurse);
                    break;
                case TypeExport.Library:
                    var _paths = Directory.GetDirectories(Application.dataPath + "/StomLibrary");
                    for (int i = 0; i < _paths.Length; i++)
                    {
                        _paths[i] = _paths[i].Substring((Application.dataPath + "/StomLibrary").Length + 1);
                        _paths[i] = "Assets/StomLibrary/" + _paths[i]; 
                    }

                    List<string> _newPaths = new List<string>(_paths);
                    _newPaths.Remove(_newPaths.Find(_ele => _ele.Contains("Settings")));

                    AssetDatabase.ExportPackage(_newPaths.ToArray(), combine_path, ExportPackageOptions.Interactive | ExportPackageOptions.Default | ExportPackageOptions.Recurse);
                    break;
            }
        }
    }
}
