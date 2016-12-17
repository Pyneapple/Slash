using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using UnityEditorInternal;
using System.Linq;
using System;
using System.Diagnostics;
using System.IO;

namespace Stom
{
    public enum FunctionalUtility
    {
        GRAPHIC_IMPORT,
        TOOL,
        SCREENSHOT,
        EXPORT_PLUGIN
    }

    //==============================================================================================
    //======== Utility Editor
    //======== This class have tool for project
    //==============================================================================================
    public class UtilityEditor : GameEditor<FunctionalUtility>
    {
        #region Screen Capture
        private List<Vector2> screenResolutions = new List<Vector2>();
        private ReorderableList roListScreen;

        //-----------Screen resolution default------------------------------//
        private readonly Vector2 iPhone3_4 = new Vector2(960, 640);
        private readonly Vector2 iPhone5 = new Vector2(1136, 640);
        private readonly Vector2 iPhone6 = new Vector2(1134, 750);
        private readonly Vector2 iPhone6Plus = new Vector2(2208, 1242);
        private readonly Vector2 iPad = new Vector2(2048, 1536);
        private readonly Vector2 iPadPro = new Vector2(2732, 2048);
        private readonly Vector2 android = new Vector2(1920, 1080);

        private bool isLandscape = true;
        #endregion

        private DataGraphicImporter dataGraphicImporter;
        private string namePrefix;
        private const string const_dataGraphic = "DataGraphicImporter";
        private static readonly List<string> ImageExtensions = new List<string> { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG" };
        private static readonly List<string> SpineExportExtensions = new List<string> { ".JSON", ".ATLAS" };

        private GameObject targetObjectCopy;
        private GameObject lastTargetObject;
        private List<Component> undoListCopy = new List<Component>();

        #region Tool Parameters
        private int selected;
        private string pathFolderAsset = "";
        private TypeExport typeExport;
        private Texture2D texture;
        private const string const_pathPackedAlast = "/StaticAssets/Graphics/PackedAtlas/";
        #endregion

        #region Overrite method
        public override void Initial(ProjectEditor mainEditor)
        {
            base.Initial(mainEditor);

            dataGraphicImporter = ScriptableObjectEditor.GetAssetScritableObject<DataGraphicImporter>(const_dataGraphic);
            if (!dataGraphicImporter)
                dataGraphicImporter = ScriptableObjectEditor.CreateScritableObject<DataGraphicImporter>(const_dataGraphic);

            #region Screen Capture
            screenResolutions = new List<Vector2>();
            roListScreen = new ReorderableList(screenResolutions, typeof(int), true, true, true, true);
            roListScreen.drawHeaderCallback = (Rect rect) =>
            {
                GUI.color = Color.cyan;
                GUI.Label(new Rect(rect.x + parentPosition.width / 3, rect.y, parentPosition.width, EditorGUIUtility.singleLineHeight), "Screenshot resolution", EditorStyles.boldLabel);
                GUI.color = Color.white;
            };
            roListScreen.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                if (ValidateResolution(screenResolutions[index]) == iPhone3_4)
                {
                    screenResolutions[index] = EditorGUI.Vector2Field(new Rect(rect.x, rect.y, 200, EditorGUIUtility.singleLineHeight), GUIContent.none, screenResolutions[index]);
                    EditorGUI.LabelField(new Rect(rect.x + 250, rect.y, 200, EditorGUIUtility.singleLineHeight), "Iphone 3+4 (3.5 Inch)", EditorStyles.boldLabel);
                }
                else if (ValidateResolution(screenResolutions[index]) == iPhone5)
                {
                    screenResolutions[index] = EditorGUI.Vector2Field(new Rect(rect.x, rect.y, 200, EditorGUIUtility.singleLineHeight), GUIContent.none, screenResolutions[index]);
                    EditorGUI.LabelField(new Rect(rect.x + 250, rect.y, 200, EditorGUIUtility.singleLineHeight), "Iphone 5 (4 Inch)", EditorStyles.boldLabel);
                }
                else if (ValidateResolution(screenResolutions[index]) == iPhone6)
                {
                    screenResolutions[index] = EditorGUI.Vector2Field(new Rect(rect.x, rect.y, 200, EditorGUIUtility.singleLineHeight), GUIContent.none, screenResolutions[index]);
                    EditorGUI.LabelField(new Rect(rect.x + 250, rect.y, 200, EditorGUIUtility.singleLineHeight), "Iphone 6 (4.7 Inch)", EditorStyles.boldLabel);
                }
                else if (ValidateResolution(screenResolutions[index]) == iPhone6Plus)
                {
                    screenResolutions[index] = EditorGUI.Vector2Field(new Rect(rect.x, rect.y, 200, EditorGUIUtility.singleLineHeight), GUIContent.none, screenResolutions[index]);
                    EditorGUI.LabelField(new Rect(rect.x + 250, rect.y, 200, EditorGUIUtility.singleLineHeight), "Iphone 6 Plus (5.5 Inch)", EditorStyles.boldLabel);
                }
                else if (ValidateResolution(screenResolutions[index]) == iPad)
                {
                    screenResolutions[index] = EditorGUI.Vector2Field(new Rect(rect.x, rect.y, 200, EditorGUIUtility.singleLineHeight), GUIContent.none, screenResolutions[index]);
                    EditorGUI.LabelField(new Rect(rect.x + 250, rect.y, 200, EditorGUIUtility.singleLineHeight), "IPad (Air and Mini Retina)", EditorStyles.boldLabel);
                }
                else if (ValidateResolution(screenResolutions[index]) == iPadPro)
                {
                    screenResolutions[index] = EditorGUI.Vector2Field(new Rect(rect.x, rect.y, 200, EditorGUIUtility.singleLineHeight), GUIContent.none, screenResolutions[index]);
                    EditorGUI.LabelField(new Rect(rect.x + 250, rect.y, 200, EditorGUIUtility.singleLineHeight), "IPad Pro", EditorStyles.boldLabel);
                }
                else if (ValidateResolution(screenResolutions[index]) == android)
                {
                    screenResolutions[index] = EditorGUI.Vector2Field(new Rect(rect.x, rect.y, 200, EditorGUIUtility.singleLineHeight), GUIContent.none, screenResolutions[index]);
                    EditorGUI.LabelField(new Rect(rect.x + 250, rect.y, 200, EditorGUIUtility.singleLineHeight), "Full HD android", EditorStyles.boldLabel);
                }
                else
                    screenResolutions[index] = EditorGUI.Vector2Field(new Rect(rect.x, rect.y, 200, EditorGUIUtility.singleLineHeight), GUIContent.none, screenResolutions[index]);
            };

            // Add default to list
            screenResolutions.Add(iPhone3_4);
            screenResolutions.Add(iPhone5);
            screenResolutions.Add(iPhone6);
            screenResolutions.Add(iPhone6Plus);
            screenResolutions.Add(iPad);
            screenResolutions.Add(iPadPro);
            screenResolutions.Add(android);
            #endregion
        }
        protected override void OnGUIGameValidate(int selected)
        {
            base.OnGUIGameValidate(selected);

            FunctionalUtility _func = (FunctionalUtility)System.Enum.Parse(typeof(FunctionalUtility), Utility.GetStringOfType<FunctionalUtility>()[selected]);
            switch (_func)
            {
                case FunctionalUtility.TOOL:
                    OnGUITool();
                    break;
                case FunctionalUtility.SCREENSHOT:
                    OnGUICapture();
                    break;
                case FunctionalUtility.EXPORT_PLUGIN:
                    OnGUIExportPlugin();
                    break;
                case FunctionalUtility.GRAPHIC_IMPORT:
                    OnGUIGraphicImporter();
                    break;
            }
        }
        #endregion

        #region GUI Utility
        private void OnGUITool()
        {
            #region Modify sorting layer
            EditorGUITool.Label("Modify sorting layer", 140.0f, parentPosition.width - 30.0f, true);
            EditorGUITool.BorderBox(30.0f,10, () => { SortingLayerTool(); });
            #endregion
            GUILayout.Space(20.0f);
            #region Replace alast sprite
            EditorGUITool.Label("Replace alast sprite", 140.0f, parentPosition.width - 30.0f, true);
            EditorGUITool.BorderBox(30.0f,10, () => { ModifyAtlasSpriteTool(); });
            #endregion
            GUILayout.Space(20.0f);
            #region Copy components
            EditorGUITool.Label("Copy components", 125.0f, parentPosition.width - 30.0f, true);
            EditorGUITool.BorderBox(30.0f, 10.0f, () =>
            {
                EditorGUILayout.BeginHorizontal();
                targetObjectCopy = EditorGUILayout.ObjectField("Target Object", targetObjectCopy, typeof(GameObject), true,GUILayout.Width(280.0f)) as GameObject;

                GUILayout.Space(20.0f);

                if (EditorGUITool.MakeButton("Paste", 60.0f))
                {
                    GameObject _select = Selection.activeGameObject;
                    if(lastTargetObject !=_select)
                    {
                        if(Selection.activeGameObject != null) 
                            lastTargetObject = _select;
                        undoListCopy.Clear();
                    }

                    if (lastTargetObject == null)
                    {
                        EditorGUITool.ShowDialogError("Please select active gameobject!");
                        return;
                    }

                    // Copy method
                    CopyComponent(targetObjectCopy, lastTargetObject);
                }
                GUILayout.Space(10.0f);

                if (EditorGUITool.MakeButton("Undo", 60.0f))
                {
                    if(lastTargetObject != null)
                        UndoCopyComponent(lastTargetObject);
                }

                EditorGUILayout.EndHorizontal();
            });
            #endregion

        }
        private void OnGUIExportPlugin()
        {
            EditorGUITool.Label("Export Package Setting", 160.0f, parentPosition.width, true);

            GUILayout.BeginHorizontal();
            GUILayout.Space(20.0f);
            EditorGUITool.Box(20, () =>
            {
                EditorGUILayout.BeginHorizontal();
                DataEditor.nameAsset = EditorGUITool.TextField("Name Asset ", 80.0f, DataEditor.nameAsset, parentEditor.position.width / 3);
                typeExport = (TypeExport)EditorGUILayout.EnumPopup(GUIContent.none, typeExport, new GUILayoutOption[] { GUILayout.Width(100.0f) });             
                EditorGUILayout.EndHorizontal();
                GUILayout.Space(20.0f);          
                if (EditorGUITool.MakeButton("Export", 100.0f, Color.cyan, true, parentEditor.position.width - 60.0f))
                {
                    ProjectEditor.SaveDataEditor(DataEditor);
                    string _path = EditorGUITool.OpenFolderPanel();
                    if (_path != "")
                    {
                        ExportPackage.Export(typeExport,_path, DataEditor.nameAsset);
                        ProjectEditor.SaveDataEditor(DataEditor);
                    }
                }

            }, (int)(parentPosition.width - 50.0f), 60, Color.white, false);
            GUILayout.EndHorizontal();
        }
        private void OnGUICapture()
        {
            EditorGUITool.Label("Capture ScreenShot Setting", 188.0f, parentPosition.width, true);

            if (FindObjectOfType<CaptureScreenshot>() == null)
            {
                EditorGUILayout.HelpBox("To capture screenshot in editor, make sure gameview is free aspect", MessageType.Warning);
                if (EditorGUITool.MakeButton("Settup Capture", parentPosition.width / 3, Color.green, true, parentPosition.width))
                    SettupCaptureScreenshot();
            }
            else
            {
                EditorGUILayout.HelpBox("Hit S to capture screenshot in gameplay", MessageType.Info);
                if (EditorGUITool.MakeButton("Remove capture", parentPosition.width / 3, Color.green, true, parentPosition.width - 50.0f))
                    DestroyImmediate(FindObjectOfType<CaptureScreenshot>());
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10.0f);
            if (EditorGUITool.MakeButton("Switch", 100.0f, Color.cyan, false, parentPosition.width - 50.0f))
            {
                for (int i = 0; i < screenResolutions.Count; i++)
                    screenResolutions[i] = new Vector2(screenResolutions[i].y, screenResolutions[i].x);
                isLandscape = (isLandscape) ? false : true;

            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal(GUILayout.Width(parentPosition.width - 20.0f));
            GUILayout.Space(10.0f);
            EditorGUILayout.BeginVertical();
            roListScreen.DoLayoutList();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
        private void OnGUIGraphicImporter()
        {

            #region Settup path
            EditorGUITool.Label("Settup path", 80.0f, parentPosition.width, true);
            GUILayout.BeginHorizontal();
            GUILayout.Space(20.0f);
            EditorGUITool.Box(20, () =>
            {
                // Graphic path
                EditorGUILayout.BeginHorizontal();
                EditorGUITool.TextField("Graphic path ", 80.0f, dataGraphicImporter.graphicPath, parentEditor.position.width / 4);
                GUILayout.Space(10.0f);
                if (EditorGUITool.MakeButton("Browser", 80.0f, Color.cyan, false, parentEditor.position.width - 60.0f))
                {
                    string _path = EditorGUITool.OpenFolderPanel(dataGraphicImporter.graphicPath);
                    if (_path != "")
                    {
                        EditorUtility.SetDirty(dataGraphicImporter);
                        dataGraphicImporter.graphicPath = _path;
                    }

                }
                GUILayout.Space(10.0f);
                if (EditorGUITool.MakeButton("Reveal", 80.0f, Color.cyan, false, parentEditor.position.width - 60.0f))
                {
                    if (dataGraphicImporter.graphicPath != "")
                        Process.Start(dataGraphicImporter.graphicPath);

                }
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(10.0f);

                // Asset path
                EditorGUILayout.BeginHorizontal();
                EditorGUITool.TextField("Asset path ", 80.0f, dataGraphicImporter.assetPath, parentEditor.position.width / 4);
                GUILayout.Space(10.0f);
                if (EditorGUITool.MakeButton("Browser", 80.0f, Color.cyan, false, parentEditor.position.width - 60.0f))
                {
                    string _path = EditorGUITool.OpenFolderPanel(dataGraphicImporter.assetPath);
                    if (_path != "")
                    {
                        EditorUtility.SetDirty(dataGraphicImporter);
                        dataGraphicImporter.assetPath = _path;
                    }

                }
                GUILayout.Space(10.0f);
                if (EditorGUITool.MakeButton("Reveal", 80.0f, Color.cyan, false, parentEditor.position.width - 60.0f))
                {
                    if (dataGraphicImporter.assetPath != "")
                        Process.Start(dataGraphicImporter.assetPath);
                }
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(10.0f);

                // Texture packer path 
                EditorGUILayout.BeginHorizontal();
                EditorGUITool.TextField("TexturePacker path ", 80.0f, dataGraphicImporter.texturePackerPath, parentEditor.position.width / 4);
                GUILayout.Space(10.0f);
                if (EditorGUITool.MakeButton("Browser", 80.0f, Color.cyan, false, parentEditor.position.width - 60.0f))
                {
                    string _path = null;
                    if (dataGraphicImporter.texturePackerPath != "")
                        _path = EditorGUITool.OpenFolderPanel(dataGraphicImporter.texturePackerPath);
                    else
                        _path = EditorGUITool.OpenFolderPanel(dataGraphicImporter.graphicPath);

                    if (_path != "")
                    {
                        EditorUtility.SetDirty(dataGraphicImporter);
                        dataGraphicImporter.texturePackerPath = _path + "/TexturePacker";
                        Directory.CreateDirectory(dataGraphicImporter.texturePackerPath);
                    }

                }
                GUILayout.Space(10.0f);
                if (EditorGUITool.MakeButton("Reveal", 80.0f, Color.cyan, false, parentEditor.position.width - 60.0f))
                {
                    if (dataGraphicImporter.assetPath != "")
                        Process.Start(dataGraphicImporter.texturePackerPath);
                }
                EditorGUILayout.EndHorizontal();
            }, (int)(parentPosition.width - 50.0f), 60, Color.white, false);
            GUILayout.EndHorizontal();
            #endregion

            GUILayout.Space(20.0f);

            #region Copy folder
            EditorGUITool.Label("Copy folder", 80.0f, parentPosition.width, true);
            GUILayout.BeginHorizontal();
            GUILayout.Space(20.0f);
            EditorGUITool.Box(20, () =>
            {
                GUILayout.BeginHorizontal();
                var _nameFolder = EditorGUITool.TextField("Name ", 80.0f, dataGraphicImporter.nameFolder, parentEditor.position.width / 4);
                if (_nameFolder != dataGraphicImporter.nameFolder)
                {
                    EditorUtility.SetDirty(dataGraphicImporter);
                    dataGraphicImporter.nameFolder = _nameFolder;
                }

                GUILayout.Space(10.0f);
                dataGraphicImporter.typeCopy = (TypeCopy)EditorGUILayout.EnumPopup(GUIContent.none, dataGraphicImporter.typeCopy, GUILayout.Width(100.0f));
                GUILayout.EndHorizontal();

                GUILayout.Space(10.0f);
                //-- Source path
                EditorGUILayout.BeginHorizontal();
                EditorGUITool.TextField("Source path ", 80.0f, dataGraphicImporter.sourcePath, parentEditor.position.width / 4);
                GUILayout.Space(10.0f);
                if (EditorGUITool.MakeButton("Browser", 80.0f, Color.cyan, false, parentEditor.position.width - 60.0f))
                {
                    string _path = null;
                    if (dataGraphicImporter.sourcePath == "")
                        _path = EditorGUITool.OpenFolderPanel(dataGraphicImporter.graphicPath);
                    else
                        _path = EditorGUITool.OpenFolderPanel(dataGraphicImporter.sourcePath);

                    if (_path != "")
                    {
                        EditorUtility.SetDirty(dataGraphicImporter);
                        dataGraphicImporter.sourcePath = _path;
                    }
                }
                GUILayout.Space(10.0f);
                if (EditorGUITool.MakeButton("Reveal", 80.0f, Color.cyan, false, parentEditor.position.width - 60.0f))
                {
                    if (dataGraphicImporter.sourcePath != "")
                        Process.Start(dataGraphicImporter.sourcePath);
                }
                EditorGUILayout.EndHorizontal();

                GUILayout.Space(10.0f);

                //-- Destination path
                EditorGUILayout.BeginHorizontal();
                EditorGUITool.TextField("Dest path ", 80.0f, dataGraphicImporter.destinationPath, parentEditor.position.width / 4);
                GUILayout.Space(10.0f);
                if (EditorGUITool.MakeButton("Browser", 80.0f, Color.cyan, false, parentEditor.position.width - 60.0f))
                {
                    string _path = null;

                    // Open folder panel with default path
                    if (dataGraphicImporter.sourcePath == "")
                        _path = EditorGUITool.OpenFolderPanel(dataGraphicImporter.assetPath);
                    else
                        _path = EditorGUITool.OpenFolderPanel(dataGraphicImporter.destinationPath);

                    // If path return not empty, start copy data
                    if (_path != "")
                    {
                        EditorUtility.SetDirty(dataGraphicImporter);
                        dataGraphicImporter.destinationPath = _path;

                        _nameFolder = "";
                        if (dataGraphicImporter.nameFolder != "")
                            _nameFolder = dataGraphicImporter.nameFolder;
                        else
                        {
                            var _listString = dataGraphicImporter.sourcePath.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                            _nameFolder += _listString[_listString.Length - 1];
                        }

                        dataGraphicImporter.destinationPath += "/" + _nameFolder;
                        DirectoryCopy(dataGraphicImporter.sourcePath, dataGraphicImporter.destinationPath, true, dataGraphicImporter.typeCopy, _nameFolder);

                        AssetDatabase.Refresh();
                    }
                }
                GUILayout.Space(10.0f);
                if (EditorGUITool.MakeButton("Reveal", 80.0f, Color.cyan, false, parentEditor.position.width - 60.0f))
                {
                    if (dataGraphicImporter.destinationPath != "")
                        Process.Start(dataGraphicImporter.destinationPath);
                }
                EditorGUILayout.EndHorizontal();

            }, (int)(parentPosition.width - 50.0f), 60, Color.white, false);
            GUILayout.EndHorizontal();
            #endregion

            GUILayout.Space(20.0f);

            #region Rename Folder
            EditorGUITool.Label("Rename file in folder", 80.0f, parentPosition.width, true);
            GUILayout.BeginHorizontal();
            GUILayout.Space(20.0f);
            EditorGUITool.Box(20, () =>
            {
                EditorGUILayout.BeginHorizontal();
                // Name fodler rename.
                namePrefix = EditorGUITool.TextField("PrefixName ", 80.0f, namePrefix, parentEditor.position.width / 4);

                // Button browser folder
                GUILayout.Space(10.0f);
                if (EditorGUITool.MakeButton("Add", 80.0f, Color.cyan, false, parentEditor.position.width - 60.0f))
                {
                    var _path = EditorGUITool.OpenFolderPanel(dataGraphicImporter.graphicPath);
                    if (_path != "" && namePrefix != "")
                    {
                        RenameFileInFolder(_path, namePrefix);
                        Process.Start(_path);
                    }
                }

                // Button remove prefix folder
                GUILayout.Space(10.0f);
                if (EditorGUITool.MakeButton("Remove", 80.0f, Color.cyan, false, parentEditor.position.width - 60.0f))
                {
                    var _path = EditorGUITool.OpenFolderPanel(dataGraphicImporter.graphicPath);
                    if (_path != "" && namePrefix != "")
                    {
                        RemovePrefixNameInFolder(_path, namePrefix);
                        Process.Start(_path);
                    }
                }
                EditorGUILayout.EndHorizontal();
            }, (int)(parentPosition.width - 50.0f), 60, Color.white, false);
            #endregion
        }
        #endregion

        #region Tool
        /// <summary>
        /// Method modify sorting layer in child for selected object
        /// </summary>
        private void SortingLayerTool()
        {
            var sortingLayers = EditorUtilityLib.GetSortingLayerNames();
            int[] sizes = new int[sortingLayers.Length];
            for (int i = 0; i < sizes.Length; i++)
                sizes[i] = i;
            EditorGUITool.Parallel(() =>
            {
                selected = EditorGUITool.InitPop("Sorting Layer", 90.0f, selected, sortingLayers, sizes, 100.0f, Color.white);
            }, 250.0f, () =>
              {
                  if (EditorGUITool.MakeButton("ValidateSprite", 100.0f))
                  {
                      if (Selection.activeGameObject == null)
                      {
                          EditorUtility.DisplayDialog("Error!", "Please select spine object to validate!", "Ok");
                          return;
                      }
                      else if (Selection.activeGameObject.GetInstanceID() != Selection.activeGameObject.transform.root.gameObject.GetInstanceID())
                      {
                          if (!EditorUtility.DisplayDialog("Warning!", "You don't select root object.Continue ?", "Ok", "Cancel"))
                              return;
                      }

                      var sprites = Selection.activeGameObject.GetComponentsInChildren<SpriteRenderer>(true);
                      for (int i = 0; i < sprites.Length; i++)
                          sprites[i].sortingLayerName = sortingLayers[selected];
                      EditorUtility.DisplayDialog("Dialong", "Succeed!", "Ok");
                  }
              }, 220.0f);
        }

        private void ModifyAtlasSpriteTool()
        {
            EditorGUITool.Parallel(() =>
            {
                EditorGUILayout.ObjectField(texture, typeof(Texture2D), true);
            }, 250.0f, () =>
            {
                if (EditorGUITool.MakeButton("Browser", 100.0f))
                {
                    pathFolderAsset = EditorUtility.OpenFilePanelWithFilters("Load asset", Application.dataPath + const_pathPackedAlast, new string[] { "Image files", "png" });
                    if (pathFolderAsset != "")
                    {
                        pathFolderAsset = pathFolderAsset.Split(new string[] { "/Assets/" }, StringSplitOptions.None)[1];
                        pathFolderAsset = "Assets/" + pathFolderAsset;
                        texture = AssetDatabase.LoadAssetAtPath(pathFolderAsset, typeof(Texture2D)) as Texture2D;
                    }
                }
            }, 100.0f);

            GUILayout.Space(10.0f);

            if (EditorGUITool.MakeButton("Apply", 100.0f, Color.cyan, true, parentPosition.width - 30.0f))
            {
                if (Selection.activeGameObject == null)
                {
                    EditorUtility.DisplayDialog("Error!", "Please select gameobject to modify atlas!", "Ok");
                    return;
                }
                else if (Selection.activeGameObject.GetInstanceID() != Selection.activeGameObject.transform.root.gameObject.GetInstanceID())
                {
                    if (!EditorUtility.DisplayDialog("Warning!", "You don't select root object", "Ok","Cancel"))
                        return;
                }
                else
                    ModifyAtlasForSprite(Selection.activeGameObject);

            }
        }

        private void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs,TypeCopy typeCopy,string prefixName = null)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
                Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                // Only copy image file
                if (!ImageExtensions.Contains(file.Extension.ToUpperInvariant())&& !SpineExportExtensions.Contains(file.Extension.ToUpperInvariant()))
                    continue;

                string temppath = null;
                // Add prefix name
                if (typeCopy == TypeCopy.PrefixName)
                    temppath = Path.Combine(destDirName, prefixName + "_" + file.Name);
                // Add .txt to atlas file
                else if (file.Extension.ToUpperInvariant() == ".ATLAS")
                    temppath = Path.Combine(destDirName, file.Name + ".txt");
                // Keep same name
                else
                    temppath = Path.Combine(destDirName, file.Name);


                file.CopyTo(temppath, true);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs,typeCopy);
                }
            }
        }

        private void RenameFileInFolder(string sourceDirName, string prefixName)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // Get the files in the directory and rename them.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                if (file.Name.IndexOf(prefixName + "_") == 0)
                {
                    continue;
                }
                var temppath = Path.Combine(sourceDirName, prefixName + "_" + file.Name);
                file.MoveTo(temppath);
            }
        }

        private void RemovePrefixNameInFolder(string sourceDirName, string prefixRemove)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // Get the files in the directory and rename them.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {               
                if (file.Name.IndexOf(prefixRemove + "_") == 0)
                {
                    var _oldName = file.Name.Remove(0, prefixRemove.Length + 1);
                    var temppath = Path.Combine(sourceDirName, _oldName);
                    file.MoveTo(temppath);
                }
            }
        }

        /// <summary>
        /// Method copy all component from object to another object
        /// </summary>
        private void CopyComponent(GameObject copyObject,GameObject targetObject)
        {
            foreach(Component _component in copyObject.GetComponents<Component>())
            {
                UnityEditorInternal.ComponentUtility.CopyComponent(_component);
                UnityEditorInternal.ComponentUtility.PasteComponentAsNew(targetObject);
                undoListCopy.Add(_component);
            }
        }

        /// <summary>
        /// Method destroy component when click undo button
        /// </summary>
        private void UndoCopyComponent(GameObject targetObject)
        {
            foreach (Component _component in targetObject.GetComponents<Component>())
            {
                if (_component.GetType().Equals(typeof(Transform)))
                    continue;

                if (targetObject.GetComponent(_component.GetType()))
                    DestroyImmediate(_component);
            }

            undoListCopy.Clear();
        }
        #endregion

        #region Utility method
        /// <summary>
        /// Method settup capture screenshot
        /// </summary>
        private void SettupCaptureScreenshot()
        {
            string _path = Application.dataPath;
            string _folderPath = _path.Substring(0, _path.Length - 7);
            _folderPath = EditorUtility.OpenFolderPanel("Folder", _folderPath, "");

            if (_folderPath != "")
            {
                //  GameObject _captureObj = new GameObject("CaptureObject");
                CaptureScreenshot _capture = TargetEditor.AddComponent<CaptureScreenshot>();
                _capture.screenResolutions = new Vector2[screenResolutions.Count];
                // Apply resolutions need capture
                for (int i = 0; i < screenResolutions.Count; i++)
                    _capture.screenResolutions[i] = screenResolutions[i];
                _capture.folderPath = _folderPath + "/";
            }
        }

        /// <summary>
        /// Method will modify atlas in sprite by name
        /// Note: Make sure new atlas have same name  elements
        /// </summary>
        private void ModifyAtlasForSprite(GameObject targetModify)
        {
            EditorUtility.SetDirty(targetModify);

            SpriteRenderer[] childObjectSprites = targetModify.GetComponentsInChildren<SpriteRenderer>(true);
            var _sprites = AssetDatabase.LoadAllAssetsAtPath(pathFolderAsset);

            if (_sprites.Length == 0)
            {
                EditorUtility.DisplayDialog("Error!", "Can't load asset", "Ok");
                return;
            }

            List<Sprite> spriteCanApply = new List<Sprite>();
            for (int i = 1; i < _sprites.Length; i++)
                spriteCanApply.Add(_sprites[i] as Sprite);

            foreach (SpriteRenderer spr in childObjectSprites)
            {
                if (spr.sprite == null)
                    continue;
                Sprite _spr = GetSpriteInListWithName(spr.sprite.name, spriteCanApply);
                if (_spr != null)
                    spr.sprite = _spr;

            }

            EditorUtility.DisplayDialog("Dialong", "Succeed!", "Ok");
        }

        /// <summary>
        /// Method get sprite from list sprite with name
        /// </summary>
        private Sprite GetSpriteInListWithName(string nameSprite, List<Sprite> sprites)
        {
            foreach (Sprite spr in sprites)
            {
                if (spr == null)
                    continue;
                if (spr.name == nameSprite)
                    return spr;
            }
            return null;
        }

        /// <summary>
        /// Method validate resolution screen
        /// </summary>
        private Vector2 ValidateResolution(Vector2 resolution)
        {
            return (isLandscape) ? resolution : new Vector2(resolution.y, resolution.x);
        }
        #endregion
    }
}
