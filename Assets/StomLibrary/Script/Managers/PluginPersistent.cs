using UnityEngine;
using System.Collections;

namespace Stom.NativePlugin
{
    public class PluginPersistent : MonoBehaviour
    {
        #region Singleton Instance
        public static PluginPersistent Instance { get { return instance; } }
        private static PluginPersistent instance;
        #endregion

        #region Public property
        public AdsController            Get_AdsController   { get { return adsController; } }
        public GoogleAds                Get_GoogleAds       { get { return admob; } }
        public UnityAds                 Get_UnityAds        { get { return unityAds; } }
        public ChartBoostAds            Get_ChartBoost      { get { return chartBoost; } }
        public IAPurchase               Get_InAppPurchase   { get { return inAppPurchase; } }
        public FacebookIntegrate        Get_FacebookIngr    { get { return facebookIng; } }
        public GameService              Get_GameService     { get { return gameService; } }
        public NativeIntegrate          Get_NativeIngr      { get { return nativeIntegrate; } }
        public IAPNoneConsumeProcess    Get_IAPNCPProcess { get { return iapNCPProcess; } }
        #endregion

        #region Private Parameters
        private AdsController           adsController;
        private GoogleAds               admob;
        private UnityAds                unityAds;
        private ChartBoostAds           chartBoost;
        private IAPurchase              inAppPurchase;
        private FacebookIntegrate       facebookIng;
        private GameService             gameService;
        private NativeIntegrate         nativeIntegrate;
        private IAPNoneConsumeProcess   iapNCPProcess;
        #endregion

        void Awake()
        {
            if (instance == null)
            {
                instance = this;

                adsController   = GetComponent<AdsController>();
                admob           = GetComponent<GoogleAds>();
                unityAds        = GetComponent<UnityAds>();
                chartBoost      = GetComponent<ChartBoostAds>();
                inAppPurchase   = GetComponent<IAPurchase>();
                facebookIng     = GetComponent<FacebookIntegrate>();
                gameService     = GetComponent<GameService>();
                nativeIntegrate = GetComponent<NativeIntegrate>();
                iapNCPProcess   = GetComponent<IAPNoneConsumeProcess>();

                DontDestroyOnLoad(this.gameObject);
            }
            else
                Destroy(this.gameObject);
        }
    }
}
