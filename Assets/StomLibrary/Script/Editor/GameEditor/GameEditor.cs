using UnityEngine;
using System.Collections;
using UnityEditor;
using Stom;

//==============================================================================================
//======== Defien parent class for draw editor
//==============================================================================================
public class GameEditor<T> : EditorWindow {
    public DataEditor DataEditor
    {
        get { return parentEditor.DataEditor; }
        set { parentEditor.DataEditor = value; }
    }
    public virtual GameObject TargetEditor
    {
        get
        {
            if (!targetPluginObj)
            {
                targetPluginObj = GameObject.Find(Default_Name);
                if (!targetPluginObj)
                    targetPluginObj = new GameObject(Default_Name);
            }
            return targetPluginObj;
        }
    }

    protected GameObject targetPluginObj = null;
    protected string Default_Name = "_MainService(OnlyOne)";

    protected ProjectEditor parentEditor;
    protected Rect parentPosition { get { return parentEditor.position; } }

    /// <summary>
    /// Method intial draw editor can overrite
    /// </summary>
    /// <param name="mainEditor">Parent editor window</param>
    public virtual void Initial(ProjectEditor mainEditor)
    {
        this.parentEditor = mainEditor;

        SceneView.onSceneGUIDelegate += SceneGUI;
    }

    /// <summary>
    /// Method call to draw tool bar on GUI
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="scrollViewVector"></param>
    /// <param name="position"></param>
    public virtual void OnGUIGameEditor(ref Vector2 scrollViewVector, Rect position)
    {
        ProjectEditor.OnGUIDrawToolBar<T>(ref DataEditor.selectedMethod, ref scrollViewVector, position, DataEditor);
        OnGUIGameValidate(DataEditor.selectedMethod);

        GUI.EndScrollView();
    }

    /// <summary>
    /// Method for event scene in edit mode
    /// </summary>
    /// <param name="sceneView"></param>
    protected virtual void SceneGUI(SceneView sceneView) { }

    /// <summary>
    /// Method validate, need implement
    /// </summary>
    protected virtual void OnGUIGameValidate(int selected)
    {

    }

}
