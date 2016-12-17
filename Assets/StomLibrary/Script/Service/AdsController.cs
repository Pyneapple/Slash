using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Stom.NativePlugin
{
    [System.Serializable]
    public class EventAds
    {
        public ServiceEvent eventAds;
        public TypeAds typeAds;
        [Range(0.0f, 100.0f)]
        public float probability;

        public void Initial()
        {
            EventManager.StartListening(eventAds, ShowAds);
        }

        /// <summary>
        /// Show ads with probability
        /// </summary>
        private void ShowAds()
        {
            if (Random.Range(0, 100.0f) < probability)
                AdsController.ShowAds(typeAds);
        }
    }

    /// <summary>
    /// Define type ads
    /// </summary>
    public enum TypeAds
    {
        AdmobFullBanner,
        AdmobNormalBanner,
        AdmobHideBanner,
        UnityVideo,
        RewardVideoUnity,
        ChartBoostFullBanner,
        ChartBosstRewardVideo
    }

    /// <summary>
    /// Define type reward ads
    /// </summary>
    public enum TypeRewardAds
    {
        Unity,
        Chartboost 
    }

    public class AdsController : MonoBehaviour
    {
        #region Parameters
        public List<EventAds> eventAds;
        public const string const_RemoveAds = "Disable Ads";
        public bool DisableAds
        {
            set
            {
                disableAds = true;
                PlayerPrefs.SetInt(const_RemoveAds, 1);
            }
        }
        public TypeRewardAds rewardAds;
        private bool disableAds;
        #endregion

        #region Intial Function
        void Awake()
        {
            // Initial event
            for (int i = 0; i < eventAds.Count; i++)
                eventAds[i].Initial();

            //--Initial google ads
            GoogleAds admob = GetComponent<GoogleAds>();
            if (admob)
            {
                if (CheckEventAds(TypeAds.AdmobFullBanner)) admob.InitialFullBanner();
                if (CheckEventAds(TypeAds.AdmobNormalBanner)) admob.InitialBanner();
            }

            //--Initial chartboost
            ChartBoostAds chartBosst = GetComponent<ChartBoostAds>();
            if (chartBosst)
            {
                if (CheckEventAds(TypeAds.ChartBoostFullBanner)) chartBosst.LoadInterstitial();
                if (CheckEventAds(TypeAds.ChartBosstRewardVideo) || rewardAds == TypeRewardAds.Chartboost)
                    chartBosst.LoadRewardVideo();
            }

            // Check remove ads
            if (PlayerPrefs.HasKey(const_RemoveAds))
                disableAds = (PlayerPrefs.GetInt(const_RemoveAds) == 1) ? true : false;
            else
            {
                disableAds = false;
                PlayerPrefs.SetInt(const_RemoveAds, 0);
            }
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Method call show video ads
        /// Call from button
        /// </summary>
        public void ShowRewardVideo()
        {
             ShowRewardAds(rewardAds);
        }

        /// <summary>
        /// Method check reward video really load
        /// Call outside class
        /// </summary>
        public static bool CheckRewardVideoReally()
        {
            return CheckTypeRewardAdsLoaded(PluginPersistent.Instance.Get_AdsController.rewardAds);
        }

        /// <summary>
        /// Display ads process
        /// </summary>
        /// <param name="typeAds"></param>
        public static void ShowAds(TypeAds typeAds)
        {
            // If player really purchase buy remove, ads will no longer display
            if (PluginPersistent.Instance.Get_AdsController.disableAds)
                return;
            // Start check type and display
            GoogleAds admob = PluginPersistent.Instance.Get_GoogleAds;
            UnityAds unityAds = PluginPersistent.Instance.Get_UnityAds;
            ChartBoostAds chartBoost = PluginPersistent.Instance.Get_ChartBoost;
            switch (typeAds)
            {
                case TypeAds.AdmobFullBanner:
                    if (admob) admob.fullBanner.ShowFullBanner();
                    break;
                case TypeAds.AdmobNormalBanner:
                    if (admob) admob.banner.ShowBanner();
                    break;
                case TypeAds.AdmobHideBanner:
                    if (admob) admob.banner.HideBanner();
                    break;
                case TypeAds.UnityVideo:
                    if (unityAds) unityAds.ShowAd();
                    break;
                case TypeAds.RewardVideoUnity:
                    if (unityAds) unityAds.ShowRewardVideo();
                    break;
                case TypeAds.ChartBoostFullBanner:
                    if (chartBoost) chartBoost.ShowInterstitial();
                    break;
                case TypeAds.ChartBosstRewardVideo:
                    if (chartBoost) chartBoost.ShowRewardVideo();
                    break;
            }
        }
        #endregion

        #region Unitlity Function
        /// <summary>
        /// Method check reward video reward video really loaded
        /// </summary>
        /// <param name="typeRewardAds"></param>
        /// <returns></returns>
        private static bool CheckTypeRewardAdsLoaded(TypeRewardAds typeRewardAds)
        {
            switch (typeRewardAds)
            {
                case TypeRewardAds.Unity:
                    if (PluginPersistent.Instance.Get_UnityAds)
                        return PluginPersistent.Instance.Get_UnityAds.IsReady();
                    else return false;
                case TypeRewardAds.Chartboost:
                    if (PluginPersistent.Instance.Get_ChartBoost)
                        return PluginPersistent.Instance.Get_ChartBoost.IsReady();
                    else return false;
                default:
                    return false;
            }
        }

        private static void ShowRewardAds(TypeRewardAds typeRewardAds)
        {
            switch (typeRewardAds)
            {
                case TypeRewardAds.Unity:
                    var _unityAds = PluginPersistent.Instance.Get_UnityAds;
                    if (_unityAds) _unityAds.ShowRewardVideo();
                    break;
                case TypeRewardAds.Chartboost:
                    var _chartboost = PluginPersistent.Instance.Get_ChartBoost;
                    if (_chartboost) _chartboost.ShowRewardVideo();
                    break;
            }
        }

        private bool CheckEventAds(TypeAds typeAds)
        {
            for (int i = 0; i < eventAds.Count; i++)
            {
                if (eventAds[i].typeAds == typeAds)
                    return true;
            }
            return false;
        }

        #endregion
    }
}
