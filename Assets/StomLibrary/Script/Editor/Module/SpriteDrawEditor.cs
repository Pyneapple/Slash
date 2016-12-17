using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using Stom;
using System.Linq;

//==============================================================================================
//======== Class for draw object attach a sprite to it
//======== Funtional: Contain delete, rotate, scale and delete a element draw in map
//==============================================================================================

/// <summary>
/// Follow input: 
/// === Left mouse  :   Draw element
/// === Right mouse :   Delete map
/// === X keycode   :   Unselect element
/// === C keycode   :   Clear grabage
/// === E keycode   :   Rotate
/// === Ctrl + E    :   Resume draw after rotate
/// === Ctrl + scroll view  :   Scale object
/// === A,D,W,S     :   Move select element in editor ( left,right,above,bottom)
/// === Esc         :   Exit draw map
/// </summary>
/// 
public class SpriteDrawEditor:Editor
{
    public class DataMapEditor
    {
        public string pathAsset;
        public string sortingLayer;
        public int sortOrder;
        public TypeSorting  sortingType;
    }

    public enum TypeSorting
    {
        Default,
        BaseOnXAxis,
        BaseOnYAxis
    }

    private Transform ObjectContainer
    {
        get
        {
            if (objContainer == null)
            {
                GameObject _obj = GameObject.Find("SagaContainer");
                if(_obj == null)
                    _obj = new GameObject("SagaContainer");
                objContainer = _obj.transform;
                _obj.transform.position = Vector3.zero;
            }
            return objContainer;
        }
    }
    private Transform objContainer;

    private string NameInstance;

    private DataMapEditor DataEditor
    {
        get
        {
            // Load data editor if it don't loaded
            if (dataEditor == null)
                dataEditor = EditorUtilityLib.LoadData<DataMapEditor>(NameInstance);
            return dataEditor;
        }
    }
    private DataMapEditor dataEditor;

    private List<Texture2D> textureButtons;
    private Object[] spriteObjs;
    private bool activeDraw = false;

    private Sprite selectedSpr;
    private GameObject drawObj;
    private int numHorizontal;
    private int numVertical;
    // Paramater for keep track current sprite used
    private int currentSelect;

    private Vector2 mousePos;
    
    // Settting color mode
    private Color colorMode = Color.white;

    private string[] sortingLayers;
    private int selected;
    private readonly int offsetYAxis = 10000;                    // Limit in distance 2^16/offset 
    private readonly int offsetXAxis = 100;                      // Limit in distance 2^16/offset 
    private const float size = 50.0f;

    private bool isRotate;
    private bool isScale;

    /// <summary>
    /// Method regist this editor is a element belong main editor
    /// </summary>
    /// <param name="nameEditor">Name will regist</param>
    public void InitialSpriteDraw(string nameEditor)
    {
        this.NameInstance = nameEditor;
        selected = 0;
        currentSelect = -1;
    }

    /// <summary>
    /// Method load texture from asset
    /// </summary>
    public void LoadTetxure()
    {
        // Make texture enable read/write
        TextureImporter A = (TextureImporter)AssetImporter.GetAtPath(DataEditor.pathAsset);
        if(A == null)
        {
            Debug.LogError("Path asset not found!");
            return;
        }

        if (!A.isReadable)
        {
            A.isReadable = true;
            AssetDatabase.ImportAsset(DataEditor.pathAsset, ImportAssetOptions.ForceUpdate);
        }

        textureButtons = new List<Texture2D>();
        spriteObjs = AssetDatabase.LoadAllAssetsAtPath(DataEditor.pathAsset);

        for (int i = 1; i < spriteObjs.Length; i++)
            textureButtons.Add(textureFromSprite(spriteObjs[i] as Sprite));
    }
    
    /// <summary>
    /// Draw GUI Element
    /// </summary>
    public void OnGUIWindow(Rect postion)
    {
        if (textureButtons == null)
            LoadTetxure();

        EditorGUITool.Box(20, () =>
        {
            #region Button Controller

            EditorGUILayout.BeginHorizontal();
            string _pathAsset = EditorGUITool.TextField("Asset path:", 100.0f, DataEditor.pathAsset, 250.0f);
            if(_pathAsset != DataEditor.pathAsset)
            {
                DataEditor.pathAsset = _pathAsset;
                // Save Data
                EditorUtilityLib.SaveData<DataMapEditor>(DataEditor, NameInstance);
            }

            // If texture modify in asset, it can't load again
            // With button, you can load it again with modify texture
            if (EditorGUITool.MakeButton("LoadTexture", 100.0f, Color.cyan, false, 250.0f))
                LoadTetxure();

            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10.0f);

            if (!activeDraw)
            {
                // Draw button
                if(EditorGUITool.MakeButton("Draw Saga",130.0f,Color.red,true,postion.width-50.0f))
                    ActiveDraw();
            }
            else
            {
                // Disable draw button
                if (EditorGUITool.MakeButton("Disable draw", 130.0f, Color.yellow, true, postion.width - 50.0f))
                    DeactivateDraw();
            }
          
            GUILayout.Space(20.0f);

            #endregion

            #region Settings
            // Settings for sorting layer

            EditorGUITool.Parallel(() =>
            {
                sortingLayers = EditorUtilityLib.GetSortingLayerNames();
                int[] sizes = new int[sortingLayers.Length];
                for (int i = 0; i < sizes.Length; i++)
                    sizes[i] = i;
                selected = sortingLayers.ToList().FindIndex(_ele => _ele == DataEditor.sortingLayer);
                selected = Mathf.Clamp(selected, 0, int.MaxValue);
                int _newSelected = EditorGUITool.InitPop("Sorting Layer", 146.0f, selected, sortingLayers, sizes, 100.0f, Color.white);
                if (_newSelected != selected)
                {
                    selected = _newSelected;
                    DataEditor.sortingLayer = sortingLayers[selected];
                    EditorUtilityLib.SaveData<DataMapEditor>(DataEditor, NameInstance);
                    // Renew obj
                    if (activeDraw)
                        MakeObj(selectedSpr);
                }
            }, 200.0f, () =>
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(10.0f);
                if (EditorGUITool.MakeButton("Apply All",80.0f))
                    ChangeSortingLayerAll();

                GUILayout.Space(10.0f);

                if (EditorGUITool.MakeButton("Apply Target", 80.0f))
                    ChangeSortingLayerSprite();

                EditorGUILayout.EndHorizontal();
            }, 200.0f);

            GUILayout.Space(10.0f);

            EditorGUITool.Parallel(() =>
            {
                // Setting type sorting
                EditorGUILayout.BeginHorizontal();
                TypeSorting _selectedType = (TypeSorting) EditorGUILayout.EnumPopup("Order in Layer", DataEditor.sortingType, GUILayout.Width(250.0f));
                if(_selectedType != DataEditor.sortingType)
                {
                    DataEditor.sortingType = _selectedType;
                    EditorUtilityLib.SaveData<DataMapEditor>(DataEditor, NameInstance);
                    if (activeDraw)
                        MakeObj(selectedSpr);
                }

            }, 200.0f, () =>
            {
                if (DataEditor.sortingType == TypeSorting.Default)
                {
                    GUILayout.Space(10.0f);
                    // Setting for sort order
                    int _sortOder = EditorGUILayout.IntField(GUIContent.none, DataEditor.sortOrder, GUILayout.Width(80.0f));
                    if (_sortOder != DataEditor.sortOrder)
                    {
                        DataEditor.sortOrder = _sortOder;
                        EditorUtilityLib.SaveData<DataMapEditor>(DataEditor, NameInstance);
                        // Renew obj
                        if (activeDraw)
                            MakeObj(selectedSpr);
                    }               
                }
                EditorGUILayout.EndHorizontal();
            }, 50.0f);

            GUILayout.Space(10.0f);

            #endregion

            EditorGUITool.Parallel(() =>
            {
                if (EditorGUITool.MakeButton("Build", 100.0f, Color.green, false, 0.0f))
                    BuildObject();
            }, 120.0f, () =>
              {
                  if (EditorGUITool.MakeButton("Undo", 100.0f, Color.red, false, 0.0f))
                      UndoBuild();
              }, 120.0f);


            GUILayout.Space(5.0f);

            #region Draw tile buttons

            if (textureButtons == null)
                return;

            EditorGUITool.Box(20, () =>
            {
                 numHorizontal = (int)(postion.width / (size + 10.0f)) - 1;
                 numVertical = (int)((textureButtons.Count - 1) / numHorizontal);
 
                for (int j = 0; j < numVertical; j++)
                {
                    EditorGUILayout.BeginHorizontal();
                    for (int i = 0; i < numHorizontal; i++)
                    {
                        CreateButtonTile(textureButtons[i + j * numHorizontal], i + j * numHorizontal);
                        GUILayout.Space(5.0f);
                    }
                    EditorGUILayout.EndHorizontal();
                    GUILayout.Space(5.0f);
                }

                // Draw odd element tile buttons
                int _odd = (textureButtons.Count - 1) % (numHorizontal);
                EditorGUILayout.BeginHorizontal();
                for (int i = textureButtons.Count - _odd; i < textureButtons.Count; i++)
                {
                    CreateButtonTile(textureButtons[i], i);
                    GUILayout.Space(10.0f);
                }
                EditorGUILayout.EndHorizontal();
            },postion.width - 50.0f,0,Color.white,false);        
            #endregion

        }, postion.width,0,colorMode,false);    
    }

    /// <summary>
    /// Event scene editor
    /// </summary>
    public void SceneGUI()
    {
        // Process draw obj
        if (activeDraw)
        {
            #region Setup Mouse Coordinates
            int controlId = GUIUtility.GetControlID(FocusType.Passive);
            Event e = Event.current;
            Ray ray = Camera.current.ScreenPointToRay(new Vector3(e.mousePosition.x, -e.mousePosition.y + Camera.current.pixelHeight));
            Vector3 mousePos = ray.origin;
            #endregion

            #region Button control
            if (e.type == EventType.KeyDown)
            {
                switch (e.keyCode)
                {
                    case KeyCode.A:
                        EditorWindow.GetWindow<ProjectEditor>().Repaint();
                        ActiveSelectSprite(currentSelect - 1);
                        ForceSceneFocus();
                        break;
                    case KeyCode.D:
                        EditorWindow.GetWindow<ProjectEditor>().Repaint();
                        ActiveSelectSprite(currentSelect + 1);
                        ForceSceneFocus();
                        break;
                    case KeyCode.S:
                        EditorWindow.GetWindow<ProjectEditor>().Repaint();
                        ActiveSelectSprite(currentSelect + numHorizontal);
                        ForceSceneFocus();
                        break;
                    case KeyCode.W:
                        EditorWindow.GetWindow<ProjectEditor>().Repaint();
                        ActiveSelectSprite(currentSelect - numHorizontal);
                        ForceSceneFocus();
                        break;
                }
            }

            #endregion

            #region Event mouse process
            if (drawObj == null && selectedSpr != null)
                MakeObj(selectedSpr);

            if (drawObj != null)
            {
                if (!isRotate)
                {
                    drawObj.transform.position = new Vector3(mousePos.x, mousePos.y, 0.0f);
                    // Update order in sorting layer
                    switch (DataEditor.sortingType)
                    {
                        case TypeSorting.Default:
                            break;
                        case TypeSorting.BaseOnXAxis:
                            // Update sorting layer base on X axis
                            UpdateSortingLayerBaseXAxist(drawObj);
                            break;
                        case TypeSorting.BaseOnYAxis:
                            // Update sorting layer base on Y axis
                            UpdateSortingLayerBaseYAxist(drawObj);
                            break;
                    }               
                }
            }

            // After click to scene, obj will deploy
            if (e.isMouse && e.type == EventType.MouseDown && e.button == 0)
            {
                GUIUtility.hotControl = controlId;
                e.Use();
                if (!isRotate && drawObj != null)
                    DeployObjToScene();

                if (drawObj == null)
                {
                    var hits = Physics.RaycastAll(ray);
                    // After ray hit object, remove obj in scene
                    if (hits.Length > 0)
                    {
                        var _obj = hits.OrderBy(_ele => _ele.transform.GetComponent<SpriteRenderer>().sortingOrder).ToList();
                        // Take element with order sorting layer
                        drawObj = _obj[_obj.Count - 1].transform.GetChild(0).gameObject;
                        // Null parent tranform
                        drawObj.transform.SetParent(null);
                        // Make hide 
                        drawObj.hideFlags = HideFlags.HideInHierarchy;
                        // Set default sprite select
                        selectedSpr = drawObj.GetComponent<SpriteRenderer>().sprite;
                        // Destroy orginal obj
                        DestroyImmediate(_obj[_obj.Count - 1].transform.gameObject);
                    }
                }
            }

            // Delete object with left mouse
            if (e.isMouse & (e.type == EventType.mouseDown || e.type == EventType.mouseDrag) && e.button == 1)
            {
                GUIUtility.hotControl = controlId;
                e.Use();

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                    DestroyImmediate(hit.transform.gameObject);

            }

            // Release hot control when mouse up
            if (e.isMouse && e.type == EventType.MouseUp && (e.button == 0 || e.button == 1))
            {
                GUIUtility.hotControl = 0;
                e.Use();
            }   

            #endregion

            #region Quick Shotcurt
            // Ctrl + 1 : Clear all
            if (e.control && e.keyCode == KeyCode.Alpha1)
                ClearAll();
            // X Keyboard : Null obj
            if (e.keyCode == KeyCode.X)
                NullObject();
            // C Keyboard : Clear grabage
            if (e.keyCode == KeyCode.C)
                DeleteGrabage();
            // E Keyboard : Rotate obj
            if (e.keyCode == KeyCode.E)
                ActiveRotate();
            // Esc: Deactivate draw
            if (e.keyCode == KeyCode.Escape)
            {
                GUIUtility.hotControl = controlId;
                e.Use();
                DeactivateDraw();
                EditorWindow.GetWindow<ProjectEditor>().Repaint();
            }
            // Scale obj
            if (e.type == EventType.ScrollWheel && e.control)
            {
                GUIUtility.hotControl = controlId;
                e.Use();
                isScale = true;

                if(drawObj )
                {
                    drawObj.transform.localScale = new Vector3(drawObj.transform.localScale.x + e.delta.y / 30.0f, drawObj.transform.localScale.y + e.delta.y / 30.0f, 1.0f);
                }
            }

            if (!e.control & isScale)
            {
                GUIUtility.hotControl = 0;
                e.Use();
                isScale = false;
            }
            #endregion        
        }

        // After rotate active, we need check key shortcut to return draw mod
        if (isRotate)
        {
            int controlId = GUIUtility.GetControlID(FocusType.Passive);
            Event e = Event.current;
            if (e.keyCode == KeyCode.E && e.control)
            {           
                activeDraw = true;
                isRotate = false;
                drawObj.hideFlags = HideFlags.HideInHierarchy;
                Selection.activeObject = null;
            }
        }
    }

    /// <summary>
    /// Method draw tiles button in editor
    /// </summary>
    private void CreateButtonTile(Texture _spr,int index)
    {
        Rect _rect = EditorGUILayout.BeginHorizontal(GUILayout.Width(size));
        if (currentSelect == index)
        {
            GUI.color = Color.red;
            GUI.Box(new Rect(_rect.x - 4.0f, _rect.y, size + 8.0f, size + 8.0f), GUIContent.none,EditorStyles.objectFieldThumb);
            GUI.color = Color.white;
        }

        if (GUILayout.Button(_spr,EditorStyles.objectFieldThumb, new GUILayoutOption[] { GUILayout.Width(size), GUILayout.Height(size)}))
        {
            ActiveSelectSprite(index);

            if (isRotate)
            {
                isRotate = false;
                activeDraw = true;
            }

            if(drawObj != null)
                DestroyImmediate(drawObj);
        }
        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// Method change sorting layer object base on Y axist
    /// </summary>
    private void UpdateSortingLayerBaseYAxist(GameObject obj)
    {
        SpriteRenderer[] _sprs = obj.GetComponentsInChildren<SpriteRenderer>();
        for(int i=0;i<_sprs.Length;i++)
            _sprs[i].sortingOrder = -(int)(obj.transform.position.y * offsetYAxis);
    }

    /// <summary>
    /// Method change sorting layer object base on X axist
    /// </summary>
    private void UpdateSortingLayerBaseXAxist(GameObject obj)
    {
        SpriteRenderer[] _sprs = obj.GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < _sprs.Length; i++)
            _sprs[i].sortingOrder = -(int)(obj.transform.position.x * offsetXAxis);
    }

    /// <summary>
    /// Method make object
    /// </summary>
    private void MakeObj(Sprite _spr)
    {
        if (drawObj != null)
            DestroyImmediate(drawObj);

        drawObj = new GameObject("DrawObjBuffer");
        drawObj.AddComponent<SpriteRenderer>().sprite = selectedSpr;
        drawObj.GetComponent<SpriteRenderer>().sortingLayerName = sortingLayers[selected];
        drawObj.GetComponent<SpriteRenderer>().sortingOrder = DataEditor.sortOrder;
        drawObj.hideFlags = HideFlags.HideInHierarchy;

        ForceSceneFocus();
    }

    /// <summary>
    /// Method draw object to scene
    /// </summary>
    private void DeployObjToScene()
    {
        // Make container hold box collider and rigid for detect obj
        GameObject _buildObj = new GameObject("Obj_Element");
        _buildObj.transform.position = drawObj.transform.position;
        _buildObj.transform.rotation = drawObj.transform.rotation;
        _buildObj.transform.localScale = drawObj.transform.localScale;
        _buildObj.AddComponent<SpriteRenderer>().sprite = drawObj.GetComponent<SpriteRenderer>().sprite;
        _buildObj.GetComponent<SpriteRenderer>().sortingLayerName = sortingLayers[selected];
        _buildObj.GetComponent<SpriteRenderer>().sortingOrder = drawObj.GetComponent<SpriteRenderer>().sortingOrder;
        // _buildObj.AddComponent(drawObj.GetComponent<SpriteRenderer>());
        _buildObj.AddComponent<Rigidbody>();
        _buildObj.AddComponent<BoxCollider>();
        _buildObj.transform.SetParent(ObjectContainer);

        drawObj.transform.SetParent(_buildObj.transform);
        drawObj.hideFlags = HideFlags.None;
        drawObj = null;
        // Make other obj
        MakeObj(selectedSpr);
    }

    /// <summary>
    /// Method active rotate in editor
    /// </summary>
    private void ActiveRotate()
    {
        isRotate = true;
        drawObj.hideFlags = HideFlags.None;
        Selection.activeObject = drawObj;
        EditorGUIUtility.PingObject(drawObj);
        activeDraw = false;
    }

    /// <summary>
    /// Method active element by control keycode editor
    /// </summary>
    /// <param name="index">Index element will active</param>
    private void ActiveSelectSprite(int index)
    {
        // Set button now focusing
        currentSelect = index;
        currentSelect = Mathf.Clamp(currentSelect, 0, textureButtons.Count - 1);
        selectedSpr = spriteObjs[currentSelect + 1] as Sprite;
        // Active draw
        if (!activeDraw)
            ActiveDraw();
        MakeObj(selectedSpr);
    }

    #region Button Function
    private void ClearAll()
    {
        selectedSpr = null;
        DestroyImmediate(drawObj);
        DestroyImmediate(ObjectContainer.gameObject);
    }

    private void NullObject()
    {
        selectedSpr = null;
        currentSelect = -1;
        EditorWindow.GetWindow<ProjectEditor>().Repaint();
        ForceSceneFocus();
        DestroyImmediate(drawObj);
    }

    private void ActiveDraw()
    {
        activeDraw = true;
        colorMode = Color.black;
        ForceSceneFocus();
    }

    private void DeactivateDraw()
    {
        isRotate = false;
        activeDraw = false;
        selectedSpr = null;
        currentSelect = -1;
        colorMode = Color.white;
        if (drawObj != null)
            DestroyImmediate(drawObj);
    }

    private void BuildObject()
    {
        var clearObj = ObjectContainer.GetComponentsInChildren<Rigidbody>();

        for(int i = 0; i < clearObj.Length; i++)
        {
            clearObj[i].transform.GetChild(0).name = "Element" + i.ToString("000");
            clearObj[i].transform.GetChild(0).SetParent(ObjectContainer.transform);
            DestroyImmediate(clearObj[i].gameObject);
        }

        objContainer.name = "MapDrawed";
        objContainer = null;

        System.GC.Collect();
    }

    private void UndoBuild()
    {
        GameObject _objs = GameObject.Find("MapDrawed");
        _objs.name = "SagaContainer";

        if (_objs == null)
            return;

        for(int i = 0; i < _objs.transform.childCount; i++)
        {
            Transform _objRaycast = _objs.transform.GetChild(i);
            GameObject _objBuffer = new GameObject("Obj_Buffer");
            _objBuffer.AddComponent<SpriteRenderer>().sprite = _objRaycast.GetComponent<SpriteRenderer>().sprite;
            _objBuffer.GetComponent<SpriteRenderer>().sortingLayerName = _objRaycast.GetComponent<SpriteRenderer>().sortingLayerName;
            _objBuffer.GetComponent<SpriteRenderer>().sortingOrder = _objRaycast.GetComponent<SpriteRenderer>().sortingOrder;
            _objBuffer.AddComponent<BoxCollider2D>();
            _objBuffer.transform.SetParent(_objRaycast);
            _objBuffer.transform.localPosition = Vector3.zero;
            _objBuffer.transform.localEulerAngles = Vector3.zero;
            _objBuffer.transform.localScale = new Vector3(1, 1, 1);

            DestroyImmediate(_objRaycast.GetComponent<BoxCollider2D>());
            _objRaycast.gameObject.AddComponent<BoxCollider>();
            _objRaycast.gameObject.AddComponent<Rigidbody>();
            _objRaycast.gameObject.name = "Obj_Element";
        }
    }

    #endregion

    #region Utility Function
    public static Texture2D textureFromSprite(Sprite sprite)
    {
        if (sprite.rect.width != sprite.texture.width)
        {
            Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                         (int)sprite.textureRect.y,
                                                         (int)sprite.textureRect.width,
                                                         (int)sprite.textureRect.height);
            newText.SetPixels(newColors);
            newText.Apply();
            return newText;
        }
        else
            return sprite.texture;
    }

    private void DeleteGrabage()
    {
        var _objs = GameObject.Find("DrawObjBuffer");
        if (_objs == null)
            return;
        if(_objs.hideFlags == HideFlags.HideInHierarchy)
            DestroyImmediate(_objs.transform.root.gameObject);
    }

    private void ChangeSortingLayerSprite()
    {
        if (ObjectContainer == null)
            return;
        for(int i = 0; i < ObjectContainer.childCount; i++)
        {
            SpriteRenderer _sprRender = ObjectContainer.GetChild(i).GetComponent<SpriteRenderer>();
            // Update with only sprite selected
            if (_sprRender.sprite == selectedSpr)
            {
                _sprRender.sortingLayerName = DataEditor.sortingLayer;
                ObjectContainer.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>().sortingLayerName = DataEditor.sortingLayer;
            }
        }
    }

    private void ChangeSortingLayerAll()
    {
        if (ObjectContainer == null)
            return;
        for (int i = 0; i < ObjectContainer.childCount; i++)
        {
            ObjectContainer.GetChild(i).GetComponent<SpriteRenderer>().sortingLayerName = DataEditor.sortingLayer;
            ObjectContainer.GetChild(i).GetChild(0).GetComponent<SpriteRenderer>().sortingLayerName = DataEditor.sortingLayer;
        }
    }

    private void ForceSceneFocus()
    {
        if (SceneView.sceneViews.Count > 0)
        {
            SceneView sceneView = (SceneView)SceneView.sceneViews[0];
            sceneView.Focus();
        }
    }
    #endregion
}
