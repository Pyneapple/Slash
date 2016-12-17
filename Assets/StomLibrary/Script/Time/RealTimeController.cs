using UnityEngine;
using System.Collections;
using System;

namespace Stom
{
    public class RealTimeController : MonoBehaviour
    {
        #region Property
        public static RealTimeController Instance
        {
            get
            {
                if (instance == null)
                    instance = SingletonManager.AddComponent<RealTimeController>();
                return instance;
            }
        }
        private static RealTimeController instance;
        #endregion

        // This event will raise when only time change
        // Use it for optimization
        public System.Action OnTimeChange;
        public System.Action OnTimeFinish;

        private float timeStartCount;
        private float lastMinutes;
        private float lastSeconds;

        // Use for optimize string
        private StringFast strTime = new StringFast(32);
        private bool counting;

        // Const key will save to to local data
        private const string const_keyTime = "Time";
        private const string const_keyRemainTime = "RemainTime";

        void Awake()
        {
            // Start game will get time last exit game
            // If don't have it, time will time start game now
            DateTime lastTime = Convert.ToDateTime(GetTimeData());
            int timeInverval = (int)(DateTime.Now - lastTime).TotalSeconds;

            CustomDebug.LogInfo("Last time save", lastTime);
            CustomDebug.LogInfo("Interval time", timeInverval.ToString() + " seconds");
        }

        void Update()
        {
            if (counting)
            {
                timeStartCount -= Time.deltaTime;
                int minutes = (int)Mathf.Floor(timeStartCount / 60);
                int seconds = (int)(timeStartCount % 60);

                if (minutes != lastMinutes || seconds != lastSeconds)
                {
                    strTime.Clear();
                    strTime.Append(minutes.ToString("00")).Append(":").Append(seconds.ToString("00"));
                    OnTimeChange();
                }

                lastMinutes = minutes;
                lastSeconds = seconds;

                // Stop count when time reach to zero
                if (timeStartCount < 0)
                {
                    counting = false;
                    OnTimeFinish();
                }
            }
        }

        #region Static method
        /// <summary>
        /// Method active time count.
        /// </summary>
        /// <param name="seconds">Seconds need count</param>
        public static void  ActiveTime(float seconds,bool isInitial = false)
        {
            // Only active one time
            if (!Instance.counting)
            {
                if(!isInitial)
                    SaveTimeData();
                // This value caculate by seconds
                Instance.timeStartCount = seconds;
                Instance.counting = true;
            }
        }

        /// <summary>
        /// Method return string time in currrent couting
        /// </summary>
        /// <returns></returns>
        public static string GetStringTime() { return Instance.strTime.ToString(); }

        /// <summary>
        /// Get time interval between current time and time start couting
        /// </summary>
        /// <returns></returns>
        public static int GetTimeStartCouting()
        {
            DateTime lastTime = Convert.ToDateTime(GetTimeData());
            return (int)(DateTime.Now - lastTime).TotalSeconds;
        }

        /// <summary>
        /// Method stop counting 
        /// </summary>
        public static void StopCounting() { Instance.counting = false; }
        #endregion

        #region Private static methods
        /// <summary>
        /// Method get time after game start run
        /// </summary>
        private static string GetTimeData()
        {
            if (!PlayerPrefs.HasKey(const_keyTime))
                return DateTime.Now.ToString();
            else
                return PlayerPrefs.GetString(const_keyTime);
        }

        /// <summary>
        /// Method save time 
        /// </summary>
        private static void SaveTimeData() {PlayerPrefs.SetString(const_keyTime, DateTime.Now.ToString()); }
        #endregion
    }
}
