using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using UnityEditorInternal;

using Stom;
using Stom.NativePlugin;

//==============================================================================================
//======== Define data editor window
//==============================================================================================
public class DataEditor
{
	public bool enableFacebook;
	public bool enableSelfAds;
	public bool enableAdmob;
	public bool enableUnityAds;
	public bool enableService;
	public bool enableIap;
    public bool enableChartboost;

	public bool enableMoblieControl;

	public bool enableDebug;
	public bool removeDebugBuild;

	public string nameAsset;

	public string idAndroidBanner;
	public string idiOsBanner;
	public string idAndroidFullBanner;
	public string idiOsFullBanner;

	public int selectedType;
	public int selectedMethod;
}

//==============================================================================================
//======== Main class draw GUI editor window
//==============================================================================================
public class ProjectEditor : EditorWindow {

	enum FunctionalEditor
	{
		GAME,
		PLUGIN,
		UTILITY,
		ABOUT
	}

	public DataEditor DataEditor
	{
		get
		{
			if(dataEditor == null)
				dataEditor = GetDataEditor();
			return dataEditor;
		}
        set { dataEditor = value; }
	}

	private DataEditor dataEditor;
	private Vector2 scrollViewVector;

	private const string prefixPathProject = "Assets/StomLibrary/";
	private Texture icon;

    private GameplayEditor gameplayEditor;
    private PluginEditor pluginEditor;
    private UtilityEditor utilityEditor;

    #region Define constants
    private const string const_DefineFacebook       = "FACEBOOK";
    private const string const_DefineIAP            = "IAP";
    private const string const_DefineAdmob          = "GOOGLE_ADS";
    private const string const_DefineAdsUnity       = "UNITYADS";
    private const string const_DefineGameService    = "GAME_SERVICE";
    private const string const_DefineChartBoost     = "CHARTBOOST";

    private const string const_EnableMoblieControl  = "MOBILE_INPUT";
    #endregion

    #region Inital show window
    [MenuItem("StomStudio/ProjectManager")]
	static void Init()
	{
		EditorWindow.GetWindow<ProjectEditor>().minSize = new Vector2(500.0f, 250.0f);
	}
	#endregion

	void OnEnable()
	{
        gameplayEditor = ScriptableObject.CreateInstance<GameplayEditor>();
        gameplayEditor.Initial(this);

        pluginEditor = ScriptableObject.CreateInstance<PluginEditor>();
        pluginEditor.Initial(this);

        utilityEditor = ScriptableObject.CreateInstance<UtilityEditor>();
        utilityEditor.Initial(this);

        icon = AssetDatabase.LoadAssetAtPath<Texture>(prefixPathProject + "Textures/Logo_512x512.png");

		// Sysnchronous define
		SynchronousDefineSymbols();
		SetScriptingDefineSymbols();

	}
	void OnDisable()
	{
        DestroyImmediate(gameplayEditor);
        DestroyImmediate(pluginEditor);
        DestroyImmediate(utilityEditor);
	}

    #region OnGUI Functions
    /// <summary>
    /// Method run frequently to draw GUI elements in editor window
    /// </summary>
    void OnGUI()
	{
		int oldSelected = DataEditor.selectedType;

		GUILayout.Space(20);    
		GUILayout.BeginHorizontal();
		GUILayout.Space(20);
		DataEditor.selectedType = EditorGUITool.Toolbar<FunctionalEditor>(DataEditor.selectedType, position.width - 50, Color.cyan);
		GUILayout.EndHorizontal();

		if (oldSelected != DataEditor.selectedType)
		{
			scrollViewVector = Vector2.zero;
			DataEditor.selectedMethod = 0;
			SaveDataEditor(DataEditor);
		}

		OnGUIValidate(DataEditor.selectedType);
	}
    /// <summary>
    /// Method draw editor plugin
    /// </summary>
	private void OnGUIPlugin()
	{
        pluginEditor.OnGUIGameEditor(ref scrollViewVector, position);
    }
    /// <summary>
    /// Method draw editor utility
    /// </summary>
	private void OnGUIUtility()
	{
        utilityEditor.OnGUIGameEditor(ref scrollViewVector,position);
    }
    /// <summary>
    /// Method draw editor gameplay
    /// </summary>
    private void OnGUIGameplayEditor()
    {
        gameplayEditor.OnGUIGameEditor(ref scrollViewVector, position);
    }
    /// <summary>
    /// Method draw editor about
    /// </summary>
    private void OnGUIAbout()
	{
		GUILayout.Space(30.0f);
		EditorGUILayout.BeginHorizontal();
		GUILayout.Space(20.0f);
		EditorGUITool.Box(10, () =>
		{
			EditorGUILayout.BeginHorizontal();
			Rect _rect = EditorGUILayout.BeginHorizontal();
			GUI.DrawTexture(new Rect(_rect.x , _rect.y , 133, 133), icon, ScaleMode.StretchToFill, true, 10.0F);
			EditorGUILayout.EndHorizontal();
			GUILayout.Space(145.0f);
			EditorGUILayout.BeginVertical();
			GUILayout.Space(2.0f);
			EditorGUITool.Label("StomStudio", 81.0f, 500.0f - 243.0f, true);
			EditorGUITool.Box(5, () =>
			 {
				 GUILayout.Label("Stom-Studio is an unit of the software ");
				 GUILayout.Label("company VietBrain. We tend to provide  ");
				 GUILayout.Label("game on Mobile World and Apple TV.");
				 GUILayout.Label("Our projects are offered on over the world,");
				 GUILayout.Label("with all Countries and Regions.");
			 }, 500.0f - 210.0f,85.0f,Color.white,false);
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();

			GUILayout.Space(15.0f);
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(30.0f);
			if (EditorGUITool.MakeButton("Fanpage", 100.0f))
				Application.OpenURL("https://www.facebook.com/StomStudio/");
			GUILayout.Space(100.0f);
			if (EditorGUITool.MakeButton("Website", 100.0f))
				Application.OpenURL("http://stomstudio.com/");
			EditorGUILayout.EndHorizontal();

		},500.0f - 40.0f,200.0f,Color.white,false);
		EditorGUILayout.EndHorizontal();
	}

    /// <summary>
    /// Method validate function editor when select in toolbar
    /// </summary>
    private void OnGUIValidate(int selected)
	{
		FunctionalEditor _func = (FunctionalEditor)System.Enum.Parse(typeof(FunctionalEditor), Utility.GetStringOfType<FunctionalEditor>()[selected]);
		switch (_func)
		{
			case FunctionalEditor.GAME:
				OnGUIGameplayEditor();
				break;
			case FunctionalEditor.PLUGIN:
				OnGUIPlugin();
				break;
			case FunctionalEditor.UTILITY:
				OnGUIUtility();
				break;
			case FunctionalEditor.ABOUT:
				OnGUIAbout();
				break;
		}
	}
    #endregion

    #region Utility Function

    [MenuItem("StomStudio/ResetServiceEditor")]
    public static void ResetEditor()
    {
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, "");
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, "NO_GPGS");
    }

    public void SetScriptingDefineSymbols()
	{
		string defines = "";
		if (DataEditor.enableFacebook)
			defines += ";" + const_DefineFacebook;
		if (DataEditor.enableService)
			defines += ";" + const_DefineGameService;
		if (DataEditor.enableIap)
			defines += ";" + const_DefineIAP;
		if (DataEditor.enableAdmob)
			defines += ";" + const_DefineAdmob;
		if (DataEditor.enableUnityAds)
			defines += ";" + const_DefineAdsUnity;
		if (DataEditor.enableMoblieControl)
			defines += ";" + const_EnableMoblieControl;
        if (DataEditor.enableChartboost)
            defines += ";" + const_DefineChartBoost;
		if (DataEditor.enableDebug)
			defines += ";" + CustomDebug.define_nameDebugMode;
		if (DataEditor.removeDebugBuild)
			defines += ";" + CustomDebug.define_nameRemoveDebug;

		// Set define to targer build platform
		PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, defines);
		// Fix iOs
		defines += ";NO_GPGS";
		PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, defines);
	}
    public static void OnGUIDrawToolBar<T>(ref int currentSelect, ref Vector2 scrollViewVector, Rect position, DataEditor _dataEditor)
    {
        int oldSelect = currentSelect;
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Space(42);

        EditorGUILayout.BeginHorizontal(GUILayout.Width(position.width - 100));
        EditorGUILayout.BeginVertical(EditorStyles.objectFieldThumb);
        currentSelect = EditorGUITool.Toolbar<T>(currentSelect, position.width - 100, new Color(.3f, .8f, 1f));
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
        GUILayout.EndHorizontal();
        scrollViewVector = GUI.BeginScrollView(new Rect(0, 75, position.width, position.height), scrollViewVector, new Rect(0, 0, 500, 1600));
        if (oldSelect != currentSelect)
        {
            scrollViewVector = Vector2.zero;
            SaveDataEditor(_dataEditor);
        }
        GUILayout.Space(-60);
    }
    private void SynchronousDefineSymbols()
	{
		string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android);
		DataEditor.enableFacebook = defines.Contains(const_DefineFacebook);		
		DataEditor.enableService = defines.Contains(const_DefineGameService);
        DataEditor.enableUnityAds = defines.Contains(const_DefineAdsUnity);
        DataEditor.enableIap = defines.Contains(const_DefineIAP);
        DataEditor.enableAdmob = defines.Contains(const_DefineAdmob);

		SaveDataEditor(DataEditor);
	}
#endregion

#region Data Editor process
	public static void SaveDataEditor(DataEditor _dataEditor)
	{
		EditorPrefs.SetString("DataEditor", JsonUtility.ToJson(_dataEditor));
	}

	public static DataEditor GetDataEditor()
	{
		if (EditorPrefs.HasKey("DataEditor")) 
			return JsonUtility.FromJson<DataEditor>(EditorPrefs.GetString("DataEditor"));
		else
		{
			SaveDataEditor(new DataEditor());
			return new DataEditor();
		}      
	}
#endregion
}

