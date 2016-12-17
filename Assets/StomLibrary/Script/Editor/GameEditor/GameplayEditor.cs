using UnityEngine;
using System.Collections;
using UnityEditor;
using Stom;

public enum FunctionalGameEditor
{
    MAP,
    SCENE,
    DATA,
    SHOP,
    PLATFORM
}

//==============================================================================================
//======== Gameplay editor
//======== This class will add with new project to suitable features
//==============================================================================================
public partial class GameplayEditor : GameEditor<FunctionalGameEditor> { }

