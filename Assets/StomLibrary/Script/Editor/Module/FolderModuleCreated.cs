using UnityEngine;
using System.Collections;
using UnityEditor;

/*
   _     _      _     _      _     _      _     _      _     _      _     _
  (c).-.(c)    (c).-.(c)    (c).-.(c)    (c).-.(c)    (c).-.(c)    (c).-.(c)
   / ._. \      / ._. \      / ._. \      / ._. \      / ._. \      / ._. \
 __\( Y )/__  __\( Y )/__  __\( Y )/__  __\( Y )/__  __\( Y )/__  __\( Y )/__
(_.-/'-'\-._)(_.-/'-'\-._)(_.-/'-'\-._)(_.-/'-'\-._)(_.-/'-'\-._)(_.-/'-'\-._)
   || M ||      || O ||      || N ||      || K ||      || E ||      || Y ||
 _.' `-' '._  _.' `-' '._  _.' `-' '._  _.' `-' '._  _.' `-' '._  _.' `-' '._
(.-./`-'\.-.)(.-./`-'\.-.)(.-./`-'\.-.)(.-./`-'\.-.)(.-./`-'\.-.)(.-./`-'\.-.)
 `-'     `-'  `-'     `-'  `-'     `-'  `-'     `-'  `-'     `-'  `-'     `-'
 
                 -Make Game More Fun ! =))
*/

/// <summary>
/// Editor script to improve workflow.
/// Auto make standard folders to own project
/// </summary>

    namespace Stom {
    public class FolderModuleCreated
    {

        private const string AnimationFolder = "Animations";
        private const string SceneFolder = "Scenes";
        private const string ScriptFolder = "Scripts";
        private const string PluginFolder = "Plugins";
        private const string PrefabFolder = "Prefabs";
        private const string SoundFolder = "Sounds";
        private const string GraphicFolder = "Graphics";
        private const string ResourceFolder = "Resources";
        private const string AssetDataFolder = "AssetData";
        private const string OtherFolder = "Others";

        [MenuItem("Project/Create standard folder")]
        public static void CreateStandardFolderToProject()
        {
            CreateSpecificFolder(AnimationFolder);

            CreateSpecificFolder(SceneFolder);
            CreateSpecificFolder(SceneFolder + "/Demo");
            CreateSpecificFolder(SceneFolder + "/Done");

            CreateSpecificFolder(ScriptFolder);
            CreateSpecificFolder(PluginFolder);
            CreateSpecificFolder(PrefabFolder);
            CreateSpecificFolder(SoundFolder);

            CreateSpecificFolder(ResourceFolder);

            CreateSpecificFolder(GraphicFolder);
            CreateSpecificFolder(AssetDataFolder);
            CreateSpecificFolder(OtherFolder);

        }

        private static void CreateSpecificFolder(string pathFolder)
        {
            string[] splitString = pathFolder.Split('/');

            string parentFolder = null;
            string newFolder = splitString[splitString.Length - 1];

            if (splitString.Length > 1)
            {
                for (int i = 0; i < splitString.Length - 1; i++)
                    parentFolder += ("/" + splitString[i]);
            }

            if (!AssetDatabase.IsValidFolder("Assets/" + pathFolder))
                AssetDatabase.CreateFolder("Assets" + parentFolder, newFolder);
        }
    }
}
