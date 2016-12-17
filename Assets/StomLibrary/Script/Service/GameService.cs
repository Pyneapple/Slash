using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;

#if UNITY_ANDROID && GAME_SERVICE
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif
#if GAME_SERVICE
using UnityEngine.SocialPlatforms;
#endif

namespace Stom.NativePlugin
{
    public enum LeaderboardType
    {
        HIGHT_SCORE,
        TOTAL_SCORE,
        HIGH_LEVEL,
        TOTAL_LEVEL_UNLOCK,
        TOTAL_ACHIVEMENT,
        HIGHT_COMBO,
        HIGHT_EXP,
		FRUIT_SLASHED
    }

    [System.Serializable]
    public class Leaderboards
    {
        public LeaderboardType name;
        public string idLeadboardIos;
        public string idLeadboardAndroid;
        public ServiceEvent eventReport;
        public int value;

        public void Init()
        {
#if GAME_SERVICE
		 EventManager.StartListening (eventReport, ReportScore);
#endif
        }

        public void ReportScore()
        {
#if GAME_SERVICE
			string leaderboardID = 
#if UNITY_ANDROID
				idLeadboardAndroid;
#endif
#if UNITY_IOS
				idLeadboardIos;
#endif
			Social.ReportScore (value, leaderboardID, success => {
				Debug.Log(success ? "Reported score successfully" : "Failed to report score");
			});
#endif
        }
    }

    [System.Serializable]
    public class Achievements
    {
        public string idAchievementAndroid;
        public string idAchievementIos;
        public double progress;
        // Event listner to report achievement
        public string eventReportListener;
        // Index achivement in list achivement will call after report succeed
        public int index;

        public void Init()
        {
#if GAME_SERVICE
		    EventManager.StartListening (eventReportListener, ReportProgress);
#endif
        }

        public void ReportProgress()
        {
#if GAME_SERVICE
		string achievementID = 
#if UNITY_ANDROID
				idAchievementAndroid;
#endif
#if UNITY_IOS
				idAchievementIos;
#endif
			Social.ReportProgress (achievementID, progress, result => {
			if (result)
			{
				Debug.Log ("Successfully reported achievement progress");
				 EventManager.AchivementCallback(index);
			}
			else
				Debug.Log ("Failed to report achievement");
		});
#endif
        }
    }

    public class GameService : MonoBehaviour
    {
        public List<Leaderboards> Leaderboard;
        public List<Achievements> Achievement;
#if GAME_SERVICE
	void Awake()
	{
		
#if UNITY_ANDROID
		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
			.RequireGooglePlus()
			.Build();
		
		PlayGamesPlatform.InitializeInstance(config);
		PlayGamesPlatform.DebugLogEnabled = true;
		PlayGamesPlatform.Activate();
#endif

		foreach(Leaderboards lb in Leaderboard)
			lb.Init ();

		foreach(Achievements ac in Achievement)
			ac.Init ();

	}
		
	// Use this for initialization
	void Start () {
		Social.localUser.Authenticate (ProcessAuthentication);
	}

	void ProcessAuthentication (bool success) {
		if (success) {
			Debug.Log ("Authenticated, checking achievements");

			// Request loaded achievements, and register a callback for processing them
			Social.LoadAchievements (ProcessLoadedAchievements);
		}
		else
			Debug.Log ("Failed to authenticate");
	}

	void ProcessLoadedAchievements (IAchievement[] achievements) {
		if (achievements.Length == 0)
			Debug.Log ("Error: no achievements found");
		else
			Debug.Log ("Got " + achievements.Length + " achievements");
	}
#endif

        #region Manual report
        /*
        public void ReportLeaderboard(int index) { Leaderboard[index].ReportScore(); }
        public void ReportAchievements(int index) { Achievement[index].ReportProgress(); }
        */
        #endregion

        #region Public Function
        /// <summary>
        /// Method call outside to set leaderboard value
        /// </summary>
        /// <param name="lbType"></param>
        /// <param name="value"></param>
        public static void SetLeaderboardValue(LeaderboardType lbType,int value)
        {
            var _leaderboard = PluginPersistent.Instance.Get_GameService.Leaderboard;
            for(int i=0;i< _leaderboard.Count; i++)
            {
                if (_leaderboard[i].name == lbType)
                    _leaderboard[i].value = value;
            }
        }
        #endregion

        #region Button Call
        public void ShowLeaderboard()                                           // Show Leaderboard
        {
#if GAME_SERVICE
		Social.ShowLeaderboardUI ();
#endif
        }

        public void ShowAchievements()                                            // Show Achievements
        {
#if GAME_SERVICE
		Social.ShowAchievementsUI ();
#endif
        }
        #endregion

        private void OnValidate()
        {
            for (int i = 0; i < Achievement.Count; i++)
                Achievement[i].index = i;
        }
    }
}
