using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using UnityEditorInternal;
using Stom;

namespace Stom.NativePlugin
{
    public class MakeDataCrossAds : EditorWindow
    {

        private DataCrossAds dataEditor;
        private List<CrossAds> data = new List<CrossAds>();
        private ReorderableList reorderList;
        private Vector2 scrollViewVector;

        private const string Name_constData = "CrossAdsData";
        private const string Name_constFileData = "AdsData";

        void OnEnable()
        {
            if (EditorPrefs.HasKey(Name_constData))
                dataEditor = JsonUtility.FromJson<DataCrossAds>(EditorPrefs.GetString(Name_constData));
            else
            {
                dataEditor = new DataCrossAds();
                EditorPrefs.SetString(Name_constData, JsonUtility.ToJson(typeof(DataCrossAds)));
            }

            // Get data from local 
            data = dataEditor.elements;

            reorderList = new ReorderableList(data, typeof(CrossAds), true, true, true, true);

            reorderList.drawHeaderCallback = (Rect rect) => { EditorGUITool.DrawHeader("Game list", 110.0f, Color.cyan, rect); };
            reorderList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                reorderList.elementHeight = 130.0f;
                rect.y += 10.0f;
                string nameGame = EditorGUI.TextField(new Rect(rect.x, rect.y, rect.width - 50.0f, EditorGUIUtility.singleLineHeight), "Name", data[index].nameGame);
                string landscape = EditorGUI.TextField(new Rect(rect.x, rect.y + 20.0f, rect.width - 50.0f, EditorGUIUtility.singleLineHeight), "Landscape image", data[index].imageHorizontal);
                string portrait = EditorGUI.TextField(new Rect(rect.x, rect.y + 40.0f, rect.width - 50.0f, EditorGUIUtility.singleLineHeight), "Portrait image", data[index].imageVertical);
                string linkAndroid = EditorGUI.TextField(new Rect(rect.x, rect.y + 60.0f, rect.width - 50.0f, EditorGUIUtility.singleLineHeight), "Link Android", data[index].linkAndroid);
                string linkiOs = EditorGUI.TextField(new Rect(rect.x, rect.y + 80.0f, rect.width - 50.0f, EditorGUIUtility.singleLineHeight), "Link iOs", data[index].linkiOs);
                CrossAds _indexData = new CrossAds(nameGame, landscape, portrait, linkAndroid, linkiOs);
                data[index] = _indexData;
            };
        }

        void OnGUI()
        {
            scrollViewVector = GUI.BeginScrollView(new Rect(0, 45, position.width, position.height), scrollViewVector, new Rect(0, 0, 500, 1600));
            EditorGUITool.Box(20, () =>
             {
                 reorderList.DoLayoutList();
             }, true);

            if (EditorGUITool.MakeButton("Folder path", 100.0f, Color.cyan, true, position.width - 20.0f))
                SetttupPathFolder();

            GUI.EndScrollView();
        }

        private void SetttupPathFolder()
        {
            string _path = Application.dataPath;
            string _folderPath = _path.Substring(0, _path.Length - 7);
            _folderPath = EditorUtility.OpenFolderPanel("Folder", _folderPath, "");

            if (_folderPath != "")
            {
                dataEditor.elements = data;
                string _dataStr = JsonUtility.ToJson(dataEditor);
                // Save data editor
                EditorPrefs.SetString(Name_constData, _dataStr);
                // Write text
                _folderPath = _folderPath + "/" + Name_constFileData + ".txt";
                Utility.WriteData(_folderPath, _dataStr);

            }
        }
    }
}
