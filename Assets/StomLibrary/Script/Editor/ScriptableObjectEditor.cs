using System.IO;
using UnityEditor;
using UnityEngine;

namespace Stom
{
    public class ScriptableObjectEditor
    {
        private const string SettingsPath = "StomLibrary/Settings/";
        private const string SettingsAssetExtension = ".asset";

        public static T CreateScritableObject<T>(string name) where T : ScriptableObject
        {
            var newAsset = ScriptableObject.CreateInstance<T>();
            string properPath = Path.Combine(Application.dataPath, SettingsPath);
            if (!Directory.Exists(properPath))
            {
                Directory.CreateDirectory(properPath);
            }

            string fullPath = Path.Combine(
                Path.Combine("Assets", SettingsPath),
                name + SettingsAssetExtension);
            AssetDatabase.CreateAsset(newAsset, fullPath);
            AssetDatabase.SaveAssets();

            return newAsset;
        }

        public static T GetAssetScritableObject<T>(string name) where T : ScriptableObject
        {
            string fullPath = Path.Combine(
               Path.Combine("Assets", SettingsPath),
               name + SettingsAssetExtension);
            return (T) AssetDatabase.LoadAssetAtPath<T>(fullPath);
        }
    }
}
