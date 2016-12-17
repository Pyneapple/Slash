using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using UnityEditorInternal;

using Stom;


//==============================================================================================
//======== Editor for native plugin and other service in game
//==============================================================================================
namespace Stom.NativePlugin
{
    public enum FunctionalPlugin
    {
        FACEBOOK,
        IN_APPS,
        ADS,
        NATIVE,
        SERVICE
    }

    //==============================================================================================
    //======== Plugin editor
    //======== This class will control plugin for service project
    //==============================================================================================
    public class PluginEditor : GameEditor<FunctionalPlugin>
    {
        public override GameObject TargetEditor
        {
            get
            {
                if (!targetPluginObj)
                {
                    targetPluginObj = GameObject.Find(Default_Name);
                    if (!targetPluginObj)
                    {
                        targetPluginObj = new GameObject(Default_Name);
                        targetPluginObj.AddComponent<PluginPersistent>();
                    }

                    achivementSerzializedObject = null;
                    leaderboardSerializedObject = null;
                    adsObjSerializedObject = null;
                    adsCrossSerializedObject = null;
                }
                return targetPluginObj;
            }
        }

        private GameObject ContainerEditor
        {
            get
            {
                if (container == null)
                {
                    container = GameObject.Find(const_ContainerEditor);
                    if (!container)
                        container = new GameObject(const_ContainerEditor);
                }
                return container;
            }
        }
        private GameObject container;
        private const string const_ContainerEditor = "_Container[Editor]";

        #region Facebook parameters
        private FacebookIntegrate faceIngre;
        private ReorderableList faceReorderList;
        private SerializedObject faceButtonSerializedObject;
        private FacebookSetting facebookSetting;

        private FacebookButtonContainer FacebookContainer
        {
            get
            {
                if (!facebookContainer)
                {
                    facebookContainer = ContainerEditor.GetComponent<FacebookButtonContainer>();
                    if (!facebookContainer)
                        facebookContainer = ContainerEditor.AddComponent<FacebookButtonContainer>();

                    faceButtonSerializedObject = null;
                }
                return facebookContainer;
            }
        }
        private FacebookButtonContainer facebookContainer;
        #endregion

        #region Service parameters
        private GameService serviceIngre;
        private ReorderableList serviceButtonReorderList;
        private SerializedObject serviveButtonSerializedObject;

        private SerializedObject achivementSerzializedObject;
        private ReorderableList achivemnetReorderList;

        private SerializedObject leaderboardSerializedObject;
        private ReorderableList leaderboardReorderList;

        private ServiceButtonContainer ServiceContainer
        {
            get
            {
                if(serviceContainer == null)
                {
                    serviceContainer = ContainerEditor.GetComponent<ServiceButtonContainer>();
                    if (serviceContainer == null)
                        serviceContainer = ContainerEditor.AddComponent<ServiceButtonContainer>();

                    serviveButtonSerializedObject = null;
                }
                return serviceContainer;
            }
        }
        private ServiceButtonContainer serviceContainer;
        #endregion

        #region InApp Purchuse parameters
        private IAPurchase iapIngre;
        private ReorderableList iapConsumeObjReorderList;
        private SerializedObject iapConsumeObjSerializedObject;
        private ReorderableList iapNoneConsumeObjReorderList;
        private SerializedObject iapNoneConsumeSeroalizedObject;

        private ReorderableList iapButtonReorderList;
        private SerializedObject iapButtonSerializedObject;

        private IAPButtonContainer IAPContainer { get
            {
                if(iapContainer == null)
                {
                    iapContainer = ContainerEditor.GetComponent<IAPButtonContainer>();
                    if (!iapContainer)
                        iapContainer = ContainerEditor.AddComponent<IAPButtonContainer>();

                    iapConsumeObjSerializedObject = null;
                    iapNoneConsumeSeroalizedObject = null;
                    iapButtonSerializedObject = null;
                }
                return iapContainer;
            } }
        private IAPButtonContainer iapContainer;
        #endregion

        #region Ads Purchase paramers
        private AdsController adsIngre;

        private ReorderableList adsObjReorderList;
        private SerializedObject adsObjSerializedObject;

        private ReorderableList adsButtonReorderList;
        private SerializedObject adsButtonSerializedObject;

        private GoogleAds admobIngre;
        private AdsStom adsCrossIngre;
        private SerializedObject adsCrossSerializedObject;

        private UnityAds unityAdsIngre;
        private ChartBoostAds chartboostIngre;

        private AdsButtonContainer AdsContainer
        {
            get
            {
                if (adsContainer == null)
                {
                    adsContainer = ContainerEditor.GetComponent<AdsButtonContainer>();
                    if (!adsContainer)
                        adsContainer = ContainerEditor.AddComponent<AdsButtonContainer>();

                    adsButtonSerializedObject = null;
                    adsButtonReorderList = null;
                }
                return adsContainer;
            }
        }
        private AdsButtonContainer adsContainer;
        #endregion

        #region Native 
        private NativeIntegrate nativeIngre;
        private ReorderableList nativeButtonReorderList;
        private SerializedObject nativeButtonSerializedObject;

        private NativeButtonContainer NativeContainer
        {
            get
            {
                if(!nativeContainer)
                {
                    nativeContainer = ContainerEditor.GetComponent<NativeButtonContainer>();
                    if (!nativeContainer)
                        nativeContainer = ContainerEditor.AddComponent<NativeButtonContainer>();

                    nativeButtonSerializedObject = null;
                }
                return nativeContainer;
            }
        }
        private NativeButtonContainer nativeContainer;
        #endregion

        #region Link Settings
        private readonly string linkFacebookSdk             = "https://developers.facebook.com/docs/unity/";
        private readonly string linkFacebookApp             = "https://developers.facebook.com/";
        private readonly string linkFacebookDocument        = "https://docs.google.com/document/d/16tSP2ViNJmN2wC9-vT390JUb-gEnw5SrCryTInYovxk/edit?usp=sharing";

        private readonly string linkInAppPurchaseDocument   = "https://docs.google.com/document/d/1hga3VdaspDFXtKtkrp3WBup8Z4TUtMhlIEVlQfWm9Ig/edit?usp=sharing";

        private readonly string linkAdmobPlugin             = "https://github.com/googleads/googleads-mobile-unity/releases";
        private readonly string linkAdmobDocument           = "https://docs.google.com/document/d/1uAuz76i4YcumEciGBsWswLkp_P8AWh3ha_TpizR2xYA/edit?usp=sharing";
        private readonly string linkAdmobApp                = "https://apps.admob.com";

        private readonly string linkUnityAds                = "https://unity3d.com/services/ads";
        private readonly string linkUnityAdsDocument        = "https://docs.google.com/document/d/13vm6Alorf_g6PSCYOrKPCmr_EkglvCe-l0_mshLBkrU/edit?usp=sharing";

        private readonly string linkGameServiceDownload     = "https://github.com/playgameservices/play-games-plugin-for-unity/tree/master/current-build";
        private readonly string linkGameServiceDocument     = "https://docs.google.com/document/d/1m8gTjX2TJjk5rJieK9rfhNOnlne4KRbJYlGzHuLOADw/edit?usp=sharing";

        private readonly string linkChartboostDownload      = "https://answers.chartboost.com/hc/en-us/articles/200780379";
        private readonly string linkChartboostApp           = "https://www.chartboost.com/";
        private readonly string linkChartboostDocument      = "";

        #endregion

        #region Define constants
        private const string const_DefineFacebook       = "FACEBOOK";
        private const string const_NameFaceAsset        = "FaceBookLinkSetting";
        private const string const_pathFacebook         = "Assets/FacebookSDK";

        private const string const_DefineIAP            = "IAP";
        private const string const_NameIapConsume       = "InAppPuchaseConsume";
        private const string const_NameIapNoneConsume   = "InAppPuchaseNoneConsume";
        private const string const_pathIap              = "Assets/Plugins/UnityPurchasing";

        private const string const_DefineAdmob          = "GOOGLE_ADS";
        private const string const_pathAdmob            = "Assets/GoogleMobileAds";

        private const string const_DefineAdsUnity       = "UNITYADS";

        private const string const_DefineGameService    = "GAME_SERVICE";
        private const string const_pathGameService      = "Assets/GooglePlayGames";

        private const string const_DefineChartBoost     = "CHARTBOOST";
        private const string const_pathChartBoost       = "Assets/Chartboost";
        #endregion

        #region Overrite Method
        public override void Initial(ProjectEditor mainEditor)
        {
            base.Initial(mainEditor);
            facebookSetting = ScriptableObjectEditor.GetAssetScritableObject<FacebookSetting>(const_NameFaceAsset);
        }
        public override void OnGUIGameEditor(ref Vector2 scrollViewVector, Rect position)
        {
            base.OnGUIGameEditor(ref scrollViewVector, position);

            //// After load new sence, targetSerObj will return null
            //// So this time, all serizable obj need reset
            //if (targetPluginObj == null)
            //{
            //    serviveButtonSerializedObject = null;
            //    faceButtonSerializedObject = null;
            //    achivementSerzializedObject = null;
            //    iapConsumeObjSerializedObject = null;
            //    iapNoneConsumeSeroalizedObject = null;
            //    leaderboardSerializedObject = null;
            //    iapButtonSerializedObject = null;
            //    adsObjSerializedObject = null;
            //    adsCrossSerializedObject = null;
            //    nativeButtonSerializedObject = null;
            //}
        }
        protected override void OnGUIGameValidate(int selected)
        {
            base.OnGUIGameValidate(selected);
            FunctionalPlugin _func = (FunctionalPlugin)System.Enum.Parse(typeof(FunctionalPlugin), Utility.GetStringOfType<FunctionalPlugin>()[selected]);
            switch (_func)
            {
                case FunctionalPlugin.FACEBOOK:
                    OnGUIFaceBook();
                    break;
                case FunctionalPlugin.ADS:
                    OnGUIAds();
                    break;
                case FunctionalPlugin.NATIVE:
                    OnGUINative();
                    break;
                case FunctionalPlugin.IN_APPS:
                    OnGUIInAppPurchase();
                    break;
                case FunctionalPlugin.SERVICE:
                    OnGUIService();
                    break;
            }
        }
        #endregion

        #region GUIPlugin
        private void OnGUIFaceBook()
        {
            EditorGUITool.Label("Facebook Setting", 120.0f, parentPosition.width - 20.0f, true);

            EditorGUITool.Box(4, () =>
            {
                //Add component FacebookIntegrate
                faceIngre = TargetEditor.GetComponent<FacebookIntegrate>();
                if (faceIngre == null)
                    faceIngre = TargetEditor.AddComponent<FacebookIntegrate>();
                // Inital list buttons
                if (faceButtonSerializedObject == null)
                    ReorderListTool.InitialReorderList<FacebookButton>(FacebookContainer, ref faceButtonSerializedObject, ref faceReorderList);

                EditorGUILayout.BeginHorizontal();
                GUI.color = Color.cyan;
                EditorGUILayout.LabelField("Enable Facebook: ", GUILayout.Width(100.0f));
                GUI.color = Color.white;
                var _enableFacebook = EditorGUILayout.Toggle(DataEditor.enableFacebook, GUILayout.Width(10));
                EditorGUILayout.EndHorizontal();
                GUILayout.Space(10.0f);

                if (_enableFacebook != DataEditor.enableFacebook)
                {
                    if (AssetDatabase.IsValidFolder(const_pathFacebook))
                    {
                        if (!facebookSetting)
                        {
                            facebookSetting = ScriptableObjectEditor.GetAssetScritableObject<FacebookSetting>(const_NameFaceAsset);
                            if (!facebookSetting)
                                facebookSetting = ScriptableObjectEditor.CreateScritableObject<FacebookSetting>(const_NameFaceAsset);
                        }

                        DataEditor.enableFacebook = _enableFacebook;
                                // Implement targer build platform
                                parentEditor.SetScriptingDefineSymbols();
                                // Save data editor
                                ProjectEditor.SaveDataEditor(DataEditor);
                    }
                }

                        #region Link Settings

                        EditorGUITool.Border(20, parentPosition.width - 50.0f, () =>
                {
                    EditorGUITool.Box(10, () =>
                    {
                        EditorGUILayout.BeginHorizontal();
                        if (EditorGUITool.MakeButton("Download", 100.0f, new Color(.7f, 1f, 1f), false, 0.0f))
                            Application.OpenURL(linkFacebookSdk);

                        if (EditorGUITool.MakeButton("Create App", 100.0f, new Color(.7f, 1f, 1f), false, 0.0f))
                            Application.OpenURL(linkFacebookApp);

                        if (EditorGUITool.MakeButton("Document", 100.0f, new Color(.7f, 1f, 1f), false, 0.0f))
                            Application.OpenURL(linkFacebookDocument);
                        EditorGUILayout.EndHorizontal();
                    });
                });
                        #endregion

                        // Enable facebook process
                        if (DataEditor.enableFacebook)
                {
                    if (!facebookSetting)
                        facebookSetting = ScriptableObjectEditor.CreateScritableObject<FacebookSetting>(const_NameFaceAsset);


                    GUILayout.Space(10.0f);
                    EditorGUITool.Border(20.0f, parentPosition.width - 50.0f, () =>
                    {
                        EditorGUITool.Box(40, () =>
                        {
                            facebookSetting.linkAndroid = EditorGUITool.TextField("Link Android ", 130.0f, facebookSetting.linkAndroid, parentPosition.width - 300.0f);
                            facebookSetting.linkIOS = EditorGUITool.TextField("Link iOs ", 130.0f, facebookSetting.linkIOS, parentPosition.width - 300.0f);
                            facebookSetting.linkAppFb = EditorGUITool.TextField("Link app facebook ", 130.0f, facebookSetting.linkAppFb, parentPosition.width - 300.0f);
                            facebookSetting.linkPicture = EditorGUITool.TextField("Link picture ", 130.0f, facebookSetting.linkPicture, parentPosition.width - 300.0f);
                        });
                    });

                    // Apply value
                    EditorUtility.SetDirty(facebookSetting);
                    faceIngre.linkAndroid = facebookSetting.linkAndroid;
                    faceIngre.linkIOS = facebookSetting.linkIOS;
                    faceIngre.linkAppFb = facebookSetting.linkAppFb;
                    faceIngre.linkPicture = facebookSetting.linkPicture;
                }

                GUILayout.Space(10.0f);

                        #region Draw ReorderList
                        EditorGUITool.Border(20.0f, parentPosition.width - 50.0f, () =>
                {
                    EditorGUILayout.HelpBox("In this field, you can add button to use basic function from facebook integration" +
                        "\n_Login: To login facebook" +
                        "\n_ShareLink: Share game to facebook can custom emotion symbol" +
                        "\n_FeedShare: Share game to facebook can't custom emotion symbol" +
                        "\n_AppInvite: Invite friend play game in facebook" +
                        "\n_Logout: To logout facebook", MessageType.Info);
                    if (faceButtonSerializedObject != null && FacebookContainer != null)
                    {
                        faceButtonSerializedObject.Update();
                        faceReorderList.DoLayoutList();
                        faceButtonSerializedObject.ApplyModifiedProperties();
                    }
                });
                        #endregion

                        // Apply button
                        if (EditorGUITool.MakeButton("Apply", 100.0f, Color.cyan, true, parentPosition.width - 20.0f))
                    ReorderListTool.ApplyModifyButtonSeriable<FacebookButton>(faceReorderList);

                GUILayout.Space(10.0f);

            }, 0, 0, Color.white, true);
        }
        private void OnGUIAds()
        {
            EditorGUITool.Label("Ads Setting", 80.0f, parentPosition.width - 10.0f, true);

            #region Initial Ads
            adsIngre = TargetEditor.GetComponent<AdsController>();
            if (!adsIngre)
                adsIngre = TargetEditor.AddComponent<AdsController>();
            // Initial for serialize ads component
            if (adsObjSerializedObject == null)
                ReorderListTool.InitialReorderListAds<AdsController>(TargetEditor, ref adsObjSerializedObject, ref adsObjReorderList);
            // Initial for serizlize ads button
            if (adsButtonSerializedObject == null)
                ReorderListTool.InitialReorderList<AdsButton>(AdsContainer, ref adsButtonSerializedObject, ref adsButtonReorderList);
            #endregion

            #region Admod
            bool _enableAdmob = EditorGUITool.Toggle(DataEditor.enableAdmob, "Enable admob", 120.0f, Color.cyan);
            admobIngre = TargetEditor.GetComponent<GoogleAds>();

            if (_enableAdmob != DataEditor.enableAdmob)
            {
                if (AssetDatabase.IsValidFolder(const_pathAdmob))
                {
                    if (_enableAdmob)
                    {
                        if (!admobIngre)
                        {
                            // Add component google admob
                            admobIngre = TargetEditor.AddComponent<GoogleAds>();
                        }
                    }
                    else
                    {
                        // Remove component google admob
                        DestroyImmediate(TargetEditor.GetComponent<GoogleAds>());
                        admobIngre = null;
                    }

                    DataEditor.enableAdmob = _enableAdmob;
                    parentEditor.SetScriptingDefineSymbols();
                    ProjectEditor.SaveDataEditor(DataEditor);
                }
            }

            #region Link Setting
            GUILayout.Space(10.0f);
            EditorGUITool.Border(20.0f, parentPosition.width - 50.0f, () =>
            {
                EditorGUITool.Box(10, () =>
                {
                    EditorGUILayout.BeginHorizontal();
                    if (EditorGUITool.MakeButton("Download ", 100.0f, new Color(.7f, 1f, 1f), false, 0.0f))
                        Application.OpenURL(linkAdmobPlugin);
                    if (EditorGUITool.MakeButton("Create App", 100.0f, new Color(.7f, 1f, 1f), false, 0.0f))
                        Application.OpenURL(linkAdmobApp);
                    if (EditorGUITool.MakeButton("Document", 100.0f, new Color(.7f, 1f, 1f), false, 0.0f))
                        Application.OpenURL(linkAdmobDocument);
                    EditorGUILayout.EndHorizontal();
                });
            });
            #endregion

            #region Enable admob process
            if (_enableAdmob)
            {
                GUILayout.Space(10.0f);
                EditorGUITool.Border(20.0f, parentPosition.width - 50.0f, () =>
                {
                    EditorGUITool.Box(20, () =>
                    {
                        // Normal banner
                        EditorGUILayout.LabelField("Admob banner", EditorStyles.boldLabel, GUILayout.Width(150.0f));
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(20.0f);

                        DataEditor.idAndroidBanner = EditorGUITool.TextField("Id Android: ", 80.0f, DataEditor.idAndroidBanner, 100.0f);
                        DataEditor.idiOsBanner = EditorGUITool.TextField("Id iOS: ", 50.0f, DataEditor.idiOsBanner, 100.0f);
                        GUILayout.EndHorizontal();

                        // Full banner
                        EditorGUILayout.LabelField("Admob full banner", EditorStyles.boldLabel, GUILayout.Width(150.0f));
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(20.0f);

                        DataEditor.idAndroidFullBanner = EditorGUITool.TextField("Id Android: ", 80.0f, DataEditor.idAndroidFullBanner, 100.0f);
                        DataEditor.idiOsFullBanner = EditorGUITool.TextField("Id iOS: ", 50.0f, DataEditor.idiOsFullBanner, 100.0f);
                        GUILayout.EndHorizontal();

                        if (admobIngre != null)
                        {
                            if (admobIngre.banner != null)
                            {
                                admobIngre.banner.idBannerAndroid = DataEditor.idAndroidBanner;
                                admobIngre.banner.idBannerIos = DataEditor.idiOsBanner;
                            }
                            if (admobIngre.fullBanner != null)
                            {
                                admobIngre.fullBanner.idBannerAndroid = DataEditor.idAndroidFullBanner;
                                admobIngre.fullBanner.idBannerIos = DataEditor.idiOsFullBanner;
                            }

                            // Save data
                            ProjectEditor.SaveDataEditor(DataEditor);
                        }
                    });
                });
                GUILayout.Space(10.0f);
                #endregion

            }

            #endregion

            GUILayout.Space(10.0f);

            #region UnityAds
            bool _enableUnityAds = EditorGUITool.Toggle(DataEditor.enableUnityAds, "Enable Unity Ads", 120.0f, Color.cyan);

#if UNITY_ADS
            if (_enableUnityAds != DataEditor.enableUnityAds)
            {
                unityAdsIngre = TargetEditor.GetComponent<UnityAds>();
                if (_enableUnityAds)
                {
                    if (!unityAdsIngre)
                        // Add component unity ads
                        unityAdsIngre = TargetEditor.AddComponent<UnityAds>();
                }
                else
                {
                    if (unityAdsIngre)
                        // Remove component unity ads
                        DestroyImmediate(TargetEditor.GetComponent<UnityAds>());
                    unityAdsIngre = null;
                }

                DataEditor.enableUnityAds = _enableUnityAds;
                parentEditor.SetScriptingDefineSymbols();
                ProjectEditor.SaveDataEditor(DataEditor);
            }
#else
            //-- Remove component unity ads if unity ads don't active
            if (DataEditor.enableUnityAds)
            {
                if (unityAdsIngre)
                    // Remove component google admob
                    DestroyImmediate(TargetEditor.GetComponent<UnityAds>());
                unityAdsIngre = null;
                DataEditor.enableUnityAds = false;
                parentEditor.SetScriptingDefineSymbols();
                ProjectEditor.SaveDataEditor(DataEditor);
            }
#endif

            if (DataEditor.enableUnityAds)
            {
                if (PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android).Contains(const_DefineAdsUnity)
           && PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS).Contains(const_DefineAdsUnity))
                    EditorGUILayout.HelpBox("Unity Ads enabled!", MessageType.Info);
                else
                    EditorGUILayout.HelpBox("Unity ads don't enable", MessageType.Error);
            }

            #region Link Settings
            EditorGUITool.Border(20.0f, parentPosition.width - 50.0f, () =>
            {
                EditorGUITool.Box(10, () =>
                {
                    EditorGUILayout.BeginHorizontal();
                    if (EditorGUITool.MakeButton("Create App", 100.0f))
                        Application.OpenURL(linkUnityAds);

                    if (EditorGUITool.MakeButton("Document", 100.0f))
                        Application.OpenURL(linkUnityAdsDocument);
                    EditorGUILayout.EndHorizontal();
                });
            });
            #endregion

            #endregion

            GUILayout.Space(10.0f);

            #region Chartboost
            bool _enableChartboost = EditorGUITool.Toggle(DataEditor.enableChartboost, "Enable Chartboost", 120.0f, Color.cyan);

            if (_enableChartboost != DataEditor.enableChartboost)
            {
                // Check path valid for active chartboost
                if (AssetDatabase.IsValidFolder(const_pathChartBoost))
                {
                    chartboostIngre = TargetEditor.GetComponent<ChartBoostAds>();
                    if (_enableChartboost)
                    {
                        if (!chartboostIngre)
                            // Add component chartboost
                            chartboostIngre = TargetEditor.AddComponent<ChartBoostAds>();
                        // Add chartboost object prefab
                        var _chartboostObj = GameObject.Find("Chartboost");
                        if (_chartboostObj == null)
                        {
                            GameObject _obj = GameObject.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Chartboost/Chartboost.prefab"));
                            _obj.name = "_chartboost";
                        }
                    }
                    else
                    {
                        if (chartboostIngre)
                            // Remove component chartboost
                            DestroyImmediate(TargetEditor.GetComponent<ChartBoostAds>());
                        chartboostIngre = null;
                        // Remove object prefab
                        var _chartboostObj = GameObject.Find("_chartboost");
                        if (_chartboostObj != null)
                            DestroyImmediate(_chartboostObj);
                    }

                    DataEditor.enableChartboost = _enableChartboost;
                    parentEditor.SetScriptingDefineSymbols();
                    ProjectEditor.SaveDataEditor(DataEditor);
                }
            }

            #region Link Setting
            GUILayout.Space(10.0f);
            EditorGUITool.Border(20.0f, parentPosition.width - 50.0f, () =>
            {
                EditorGUITool.Box(10, () =>
                {
                    EditorGUILayout.BeginHorizontal();
                    if (EditorGUITool.MakeButton("Download ", 100.0f, new Color(.7f, 1f, 1f), false, 0.0f))
                        Application.OpenURL(linkChartboostDownload);
                    if (EditorGUITool.MakeButton("Create App", 100.0f, new Color(.7f, 1f, 1f), false, 0.0f))
                        Application.OpenURL(linkChartboostApp);
                    if (EditorGUITool.MakeButton("Document", 100.0f, new Color(.7f, 1f, 1f), false, 0.0f))
                        Application.OpenURL(linkChartboostDocument);
                    EditorGUILayout.EndHorizontal();
                });
            });
            #endregion
            #endregion

            GUILayout.Space(10.0f);

            #region Ads Stom
            var _enableSelfAds = EditorGUITool.Toggle(DataEditor.enableSelfAds, "Enable cross ads ", 120.0f, Color.cyan);
            if (_enableSelfAds != DataEditor.enableSelfAds)
            {
                DataEditor.enableSelfAds = _enableSelfAds;

                ProjectEditor.SaveDataEditor(DataEditor);

                if (!_enableSelfAds)
                {
                    // Remove after uncheck
                    if (TargetEditor.GetComponent<AdsStom>())
                    {
                        adsCrossSerializedObject = null;
                        DestroyImmediate(TargetEditor.GetComponent<AdsStom>());
                    }
                }
            }
            // Enable ads
            if (DataEditor.enableSelfAds)
            {
                // Add component
                adsCrossIngre = TargetEditor.GetComponent<AdsStom>();
                // Intial Object to serialized  
                if (adsCrossSerializedObject == null)
                {
                    if (adsCrossIngre == null)
                        adsCrossIngre = TargetEditor.AddComponent<AdsStom>();
                    adsCrossSerializedObject = new SerializedObject(adsCrossIngre);
                }
                EditorGUITool.Box(20, () =>
                {
                    EditorGUITool.PropertyField("Url :", adsCrossSerializedObject, "url", 50.0f, 340.0f);
                    EditorGUILayout.BeginHorizontal();
                    EditorGUITool.PropertyField("Image :", adsCrossSerializedObject, "images", 50.0f, 100.0f);
                    EditorGUITool.PropertyField("TypeShow :", adsCrossSerializedObject, "typeShow", 50.0f, 100.0f);
                    EditorGUILayout.EndHorizontal();

                    if (EditorGUITool.MakeButton("Initial Data", 100.0f, Color.red, true, parentPosition.width - 50.0f))
                        EditorWindow.GetWindow<MakeDataCrossAds>();

                    adsCrossSerializedObject.ApplyModifiedProperties();
                }, true);
            }
            #endregion

            GUILayout.Space(10.0f);

            #region Table Field
            // -- Ads buttons
            EditorGUITool.Box(20, () =>
            {
                EditorGUILayout.HelpBox("In this field, you can add button show reward video", MessageType.Info);
                EditorGUILayout.HelpBox("Make sure you use only one type reward ads", MessageType.Warning);
                if (AdsContainer != null && adsButtonSerializedObject != null)
                {
                    adsButtonSerializedObject.Update();
                    adsButtonReorderList.DoLayoutList();
                    adsButtonSerializedObject.ApplyModifiedProperties();
                }

                // -- Apply button
                if (EditorGUITool.MakeButton("Apply", 100.0f, Color.cyan, true, parentPosition.width - 50.0f))
                {
                    SerializedProperty element = adsButtonReorderList.serializedProperty.GetArrayElementAtIndex(0);
                    // Get string method     
                    string _strFunc = element.FindPropertyRelative("identify").stringValue;
                    // Check type reward ads use in game
                    switch (_strFunc)
                    {
                        case "ShowRewardUnityAds":
                            FindObjectOfType<AdsController>().rewardAds = TypeRewardAds.Unity;
                            break;
                        case "ShowRewardChartboost":
                            FindObjectOfType<AdsController>().rewardAds = TypeRewardAds.Chartboost;
                            break;
                    }
                    ReorderListTool.ApplyModifyButtonSeriable<AdsButton>(adsButtonReorderList);
                }
            });

            GUILayout.Space(10.0f);

            // Ads component
            EditorGUITool.Box(20, () =>
            {
                EditorGUILayout.HelpBox("In this field, you can setting Ads" +
                    "\n_Event: Time to show ads" +
                    "\n_Type Ads: Chose type show ads " +
                    "\n_Probality: If you want ads don't make noise player, keep probality is low", MessageType.Info);
                if (adsObjSerializedObject != null)
                {
                    adsObjSerializedObject.Update();
                    adsObjReorderList.DoLayoutList();
                    adsObjSerializedObject.ApplyModifiedProperties();
                }
            });
            #endregion
        }
        private void OnGUIInAppPurchase()
        {
            EditorGUITool.Label("In-App Purchase Setting", 169.0f, parentPosition.width - 10.0f, true);

            // Add component In-App Purchase component
            iapIngre = TargetEditor.GetComponent<IAPurchase>();
            if (!iapIngre)
                iapIngre = TargetEditor.AddComponent<IAPurchase>();
            //// Initial serialized button
            if (iapButtonSerializedObject == null)
                ReorderListTool.InitialReorderList<IAPButton>(IAPContainer, ref iapButtonSerializedObject, ref iapButtonReorderList);
            // Initial serialized iap consume
            if (iapConsumeObjSerializedObject == null)
                ReorderListTool.InitialReorderListInAppPurchase<IAPButtonContainer>(IAPContainer.gameObject, iapIngre, ref iapConsumeObjSerializedObject, ref iapConsumeObjReorderList, true);
            // Initial serialized iap none consume
            if (iapNoneConsumeSeroalizedObject == null)
                ReorderListTool.InitialReorderListInAppPurchase<IAPButtonContainer>(IAPContainer.gameObject, iapIngre, ref iapNoneConsumeSeroalizedObject, ref iapNoneConsumeObjReorderList, false);

           
            EditorGUILayout.BeginHorizontal();
            // Enable native plugin
            var _enableIapp = EditorGUITool.Toggle(DataEditor.enableIap, "Enable In-App purchase: ", 150.0f, Color.cyan);
            if (EditorGUITool.MakeButton("Document", 100.0f))
                Application.OpenURL(linkInAppPurchaseDocument);
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(10.0f);

#if UNITY_PURCHASING
            if (_enableIapp != DataEditor.enableIap)
            {
                if (AssetDatabase.IsValidFolder(const_pathIap))
                {
                    DataEditor.enableIap = _enableIapp;
                    parentEditor.SetScriptingDefineSymbols();
                    ProjectEditor.SaveDataEditor(DataEditor);
                }
            }
#else
            if(DataEditor.enableIap)
            {
                DataEditor.enableIap = false;
                parentEditor.SetScriptingDefineSymbols();
                ProjectEditor.SaveDataEditor(DataEditor);
            }
#endif

            // Draw button
            EditorGUITool.Box(20, () =>
            {
                EditorGUILayout.HelpBox("In this field, you can add button to retore purchase from In-App Purchase function", MessageType.Info);
                if (IAPContainer != null & iapButtonSerializedObject != null)
                {
                    iapButtonSerializedObject.Update();
                    iapButtonReorderList.DoLayoutList();
                    iapButtonSerializedObject.ApplyModifiedProperties();
                }

                if (EditorGUITool.MakeButton("Apply", 100.0f, Color.cyan, true, parentPosition.width - 50.0f))
                    ReorderListTool.ApplyModifyButtonSeriable<IAPButton>(iapButtonReorderList);
            }, true);

            GUILayout.Space(10.0f);

#region In-App Purchase Consume
            EditorGUITool.Box(20, () =>
            {
                EditorGUILayout.HelpBox("In this field, you can setting In-App Purchase Consume type" +
        "\nWith type cosume, player can't restore purchase so it will use with coin,...", MessageType.Info);
                if (IAPContainer != null & iapConsumeObjSerializedObject != null)
                {
                    iapConsumeObjSerializedObject.Update();
                    iapConsumeObjReorderList.DoLayoutList();
                    iapConsumeObjSerializedObject.ApplyModifiedProperties();
                }

                if (EditorGUITool.MakeButton("Apply", 100.0f, Color.cyan, true, parentPosition.width - 50.0f))
                {
                    CoinIAPs iapConsume = ScriptableObjectEditor.GetAssetScritableObject<CoinIAPs>(const_NameIapConsume);
                    if (!iapConsume)
                        iapConsume = ScriptableObjectEditor.CreateScritableObject<CoinIAPs>(const_NameIapConsume);
                    EditorUtility.SetDirty(iapConsume);

                    // Aplly data iap consume
                    iapConsume.coinIaps = new List<CoinIap>();
                    for (int i = 0; i < IAPContainer.ProductListConsume.Count; i++)
                        iapConsume.coinIaps.Add(new CoinIap(IAPContainer.ProductListConsume[i].gameCoin, IAPContainer.ProductListConsume[i].realCoin));

                    Selection.activeObject = iapConsume;
                    EditorGUIUtility.PingObject(iapConsume);

                    ReorderListTool.ApplyButtonInAppPurchase(iapConsumeObjReorderList, true);
                }
            }, true);
#endregion

#region In-App Purchase None Consume
            EditorGUITool.Box(20, () =>
            {
                EditorGUILayout.HelpBox("In this field, you can setting In-App Purchase none Consume type" +
        "\nWith type cosume, player can restore purchase so it will use with coin,...", MessageType.Info);

                if (IAPContainer != null & iapNoneConsumeSeroalizedObject != null)
                {
                    iapNoneConsumeSeroalizedObject.Update();
                    iapNoneConsumeObjReorderList.DoLayoutList();
                    iapNoneConsumeSeroalizedObject.ApplyModifiedProperties();
                }

                if (EditorGUITool.MakeButton("Apply", 100.0f, Color.cyan, true, parentPosition.width - 50.0f))
                {
                    ProductIAPs iapNoneConsume = ScriptableObjectEditor.GetAssetScritableObject<ProductIAPs>(const_NameIapNoneConsume);
                    if (!iapNoneConsume)
                        iapNoneConsume = ScriptableObjectEditor.CreateScritableObject<ProductIAPs>(const_NameIapNoneConsume);
                    EditorUtility.SetDirty(iapNoneConsume);

                    // Aplly data iap none consume
                    iapNoneConsume.productIaps = new List<ProductIAP>();
                    for (int i = 0; i < IAPContainer.ProductListNoneConsume.Count; i++)
                        iapNoneConsume.productIaps.Add(new ProductIAP(IAPContainer.ProductListNoneConsume[i].nameProduct, IAPContainer.ProductListNoneConsume[i].realCoin));

                    Selection.activeObject = iapNoneConsume;
                    EditorGUIUtility.PingObject(iapNoneConsume);

                    // Call apply 
                    ReorderListTool.ApplyButtonInAppPurchase(iapNoneConsumeObjReorderList, false);
                }
            }, true);
#endregion
        }
        private void OnGUINative()
        {
            nativeIngre = TargetEditor.GetComponent<NativeIntegrate>();
            if (nativeIngre == null)
                nativeIngre = TargetEditor.AddComponent<NativeIntegrate>();

            if (nativeButtonSerializedObject == null)
                ReorderListTool.InitialReorderList<NativeButton>(NativeContainer, ref nativeButtonSerializedObject, ref nativeButtonReorderList);

            EditorGUITool.Label("Native Setting ", 100.0f, parentPosition.width - 50.0f, true);

            EditorGUITool.Box(20, () =>
            {
                nativeIngre.linkRateAndroid = EditorGUITool.TextField("Link Rate Anndroid", 150.0f, nativeIngre.linkRateAndroid, 200.0f);
                nativeIngre.linkRateIos = EditorGUITool.TextField("Link Rate IOS", 150.0f, nativeIngre.linkRateIos, 200.0f);
                nativeIngre.linkMoreGameAndroid = EditorGUITool.TextField("Link More Game Android", 150.0f, nativeIngre.linkMoreGameAndroid, 200.0f);
                nativeIngre.linkMoreGameIos = EditorGUITool.TextField("Link More Game IOS", 150.0f, nativeIngre.linkMoreGameIos, 200.0f);

                GUILayout.Space(20.0f);

                EditorGUILayout.HelpBox("In this field, you can function for button contain : Rate or More Game", MessageType.Info);

                if (nativeButtonSerializedObject != null && NativeContainer != null)
                {
                    nativeButtonSerializedObject.Update();
                    nativeButtonReorderList.DoLayoutList();
                    nativeButtonSerializedObject.ApplyModifiedProperties();
                }

                GUILayout.Space(10.0f);
                if (EditorGUITool.MakeButton("Apply", 100.0f, Color.cyan, true, parentPosition.width - 50.0f))
                    ReorderListTool.ApplyModifyButtonSeriable<NativeButton>(nativeButtonReorderList);
            });
        }
        private void OnGUIService()
        {
            EditorGUITool.Label("Game service Setting", 150.0f, parentPosition.width - 10.0f, true);

            //Add component GameServiceIntegrate
            serviceIngre = TargetEditor.GetComponent<GameService>();
            if (serviceIngre == null)
                serviceIngre = TargetEditor.AddComponent<GameService>();
            // Inital reorder list buttons
            if (serviveButtonSerializedObject == null)
                ReorderListTool.InitialReorderList<ServiceButton>(ServiceContainer, ref serviveButtonSerializedObject, ref serviceButtonReorderList);
            // Initial reorder list achivement
            if (achivementSerzializedObject == null)
                ReorderListTool.InitialReorderListAchivement<GameService>(TargetEditor, ref achivementSerzializedObject, ref achivemnetReorderList);
            // Initial reortder list leaderboard
            if (leaderboardSerializedObject == null)
                ReorderListTool.InitialReorderListLeaderboard<GameService>(TargetEditor, ref leaderboardSerializedObject, ref leaderboardReorderList);

            var _enableService = EditorGUITool.Toggle(DataEditor.enableService, "Enable Service: ", 100.0f, Color.cyan);
            if (_enableService != DataEditor.enableService)
            {
                if (AssetDatabase.IsValidFolder(const_pathGameService))
                {
                    DataEditor.enableService = _enableService;
                    parentEditor.SetScriptingDefineSymbols();
                    ProjectEditor.SaveDataEditor(DataEditor);
                }
            }

            #region Link Settings
            GUILayout.BeginHorizontal();
            GUILayout.Space(15.0f);
            EditorGUITool.Box(10, () =>
            {
                EditorGUILayout.BeginHorizontal();
                if (EditorGUITool.MakeButton("Download", 100.0f, new Color(.7f, 1f, 1f), false, 0.0f))
                    Application.OpenURL(linkGameServiceDownload);
                if (EditorGUITool.MakeButton("Document", 100.0f, new Color(.7f, 1f, 1f), false, 0.0f))
                    Application.OpenURL(linkGameServiceDocument);
                EditorGUILayout.EndHorizontal();
            }, (int)((parentPosition.width - 50.0f) * 2 / 3), 20, Color.white, false);
            GUILayout.EndHorizontal();
            GUILayout.Space(10.0f);
            #endregion

            // Button field
            EditorGUITool.Box(20, () =>
            {
                EditorGUILayout.HelpBox("In this field, you can add button that contain function to show leaderboard or achivement", MessageType.Info);
                serviveButtonSerializedObject.Update();
                serviceButtonReorderList.DoLayoutList();
                serviveButtonSerializedObject.ApplyModifiedProperties();

                if (EditorGUITool.MakeButton("Apply", 100.0f, Color.cyan, true, parentPosition.width - 35.0f))
                    ReorderListTool.ApplyModifyButtonSeriable<ServiceButton>(serviceButtonReorderList);
            }, 0, 0, Color.white, true);


            GUILayout.Space(10.0f);

            EditorGUITool.Box(20, () =>
            {
                EditorGUILayout.HelpBox("In this field, you can setting achivements" +
                    "\n_Id Android & Id iOs : Copy Id that you create to here " +
                    "\n_Event report: Time to post achivement to serve" +
                    "\n_Process: Process that post to unlock achivment ( Only Android)" +
                    "|n_Index CallbacK: This field you can't modify ( Ignore it)", MessageType.Info);
                achivementSerzializedObject.Update();
                achivemnetReorderList.DoLayoutList();
                achivementSerzializedObject.ApplyModifiedProperties();
            }, true);

            GUILayout.Space(10.0f);

            EditorGUITool.Box(20, () =>
            {
                EditorGUILayout.HelpBox("In this field, you can setting leaderboard" +
                    "\n_Id Android & Id iOs : Copy Id that you create to here" +
                    "\n_Event report: Time to post achivement to serve" +
                    "\n_Type Leaderboard: Chose type leaderboard to post", MessageType.Info);
                leaderboardSerializedObject.Update();
                leaderboardReorderList.DoLayoutList();
                leaderboardSerializedObject.ApplyModifiedProperties();
            }, true);
        }
        #endregion

    }
}
