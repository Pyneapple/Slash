using UnityEngine;
using System.Collections;

namespace Stom
{
    public class TimeScaleManager : MonoBehaviour
    {

        public static TimeScaleManager Instance
        {
            get
            {
                if (!instance)
                    instance = SingletonManager.AddComponent<TimeScaleManager>();
                return instance;
            }
        }

        private static TimeScaleManager instance;

        public static float TimeScale
        {
            get { return Time.timeScale; }
            set { if (Instance.isPlaying) Time.timeScale = value; }
        }

        private bool isPlaying;
        private float reserveTime;

#if UNITY_EDITOR
        public static void StopPlaying()
        {
            Instance.isPlaying = false;
            Instance.reserveTime = Time.timeScale;
            Time.timeScale = 0.0f;
        }
        public static void ResumePlaying()
        {
            Instance.isPlaying = true;
            Time.timeScale = Instance.reserveTime;
        }
#endif
    }
}
