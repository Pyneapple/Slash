using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Reflection;

#if FACEBOOK
using Facebook.Unity;
using System.Collections.Generic;
#endif

namespace Stom.NativePlugin
{
    public class FacebookIntegrate : MonoBehaviour
    {
        public string linkAndroid = "https://play.google.com/store/apps/details?id=com.vietbrain.SuperKongSaga";
        public string linkIOS = "https://itunes.apple.com/lt/app/super-kong-saga-magic-monkey/id1065736101";
        public string linkPicture = "https://scontent-hkg3-1.xx.fbcdn.net/hphotos-xtp1/v/t1.0-9/11667440_494793770676666_4943243402128542675_n.jpg?oh=dabd1ec68a80665a348c2d161f98bdbd&oe=578AC7AE";
        public string linkAppFb = "https://fb.me/843568162438930";

        /// <summary>
        /// Property check login
        /// </summary>
        /// <returns></returns>
        public static bool IsLoggedIn
        {
            get
            {
                bool rs = false;
#if FACEBOOK
        rs = FB.IsLoggedIn; 
#endif
                return rs;
            }
        }

        string linkShare
        {
            get
            {
#if UNITY_EDITOR
                return "";
#elif UNITY_ANDROID
			return linkAndroid;
#elif UNITY_IPHONE
			return linkIOS;
#endif
            }
        }

        #region START
#if FACEBOOK
    void Awake()
    {
        Init();
    }

    /// <summary>
    /// Run it first for use other functions
    /// </summary>
    public void Init()
    {
		if (!FB.IsInitialized) {
			FB.Init (InitCallback, OnHideUnity);
		}
		else
		{
			FB.ActivateApp ();
		}

    }

	private void InitCallback ()
	{
		if (FB.IsInitialized) {
			// Signal an app activation App Event
			FB.ActivateApp ();
			// Continue with Facebook SDK
			// ...
		} else {
			Debug.Log ("Failed to Initialize the Facebook SDK");
		}
	}

	private void OnHideUnity (bool isGameShown)
	{
		if (!isGameShown) {
			// Pause the game - we will need to hide
		} else {
			// Resume the game - we're getting focus again
		}
	}
#endif
        #endregion

        #region LOGIN
        /// <summary>
        /// Logs the user in with the requested read permissions.
        /// </summary>
        public void Login()
        {
#if FACEBOOK
        FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email", "user_friends" }, LoginCallback);
#endif
        }

        /// <summary>
        /// Logs the user in with the requested publish permissions.
        /// </summary>
        /*
        public void LogInWithPublishPermissions()
        {
#if FACEBOOK
                FB.LogInWithPublishPermissions(new List<string>() { "publish_actions" }, LoginCallback);
#endif
        }
        */

#if FACEBOOK
    private void LoginCallback(ILoginResult result)
    {

    }
#endif
        #endregion

        #region LOGOUT
        /// <summary>
        /// Logout Facebook
        /// </summary>
        public void LogOut()
        {
#if FACEBOOK
        FB.LogOut();
#endif
        }
        #endregion

        #region SHARE
        /// <summary>
        /// Share Link(Have Picture)
        /// </summary>
        public void ShareLink()
        {
#if FACEBOOK
        FB.ShareLink(
				new Uri(linkShare),
            "",
            "",
				new Uri(linkPicture),
            ShareLinkCallBack
            );
#endif
        }

#if FACEBOOK
    private void ShareLinkCallBack(IShareResult result)
    {

    } 
#endif

        public void FeedShare()
        {
#if FACEBOOK
		FB.FeedShare (
			string.Empty, //toId
				new System.Uri (linkShare), //link
			string.Empty, //linkName
			string.Empty, //linkCaption
			string.Empty, //linkDescription
				new System.Uri (linkPicture), //picture
			string.Empty, //mediaSource
			FeedShareCallBack //callback
		);
#endif
        }



#if FACEBOOK
    private void FeedShareCallBack(IShareResult result)
    {

    } 
#endif
        #endregion

        #region INVITE
        /// <summary>
        /// Invite friends play game with Facebook app link
        /// </summary>
        public void AppInvite()
        {
#if FACEBOOK
        FB.Mobile.AppInvite(
				new Uri(linkAppFb),
				new Uri(linkPicture),
            AppInviteCallBack
            ); 
#endif
        }

#if FACEBOOK
    private void AppInviteCallBack(IAppInviteResult result)
    {

    } 
#endif
        #endregion

    }
}


