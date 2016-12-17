using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace Stom.NativePlugin
{
    /// <summary>
    /// Define event use for service
    /// </summary>
    public enum ServiceEvent
    {
        GAMEOVER,
        WINGAME,
        HOME,
        PLAYER_DEAD
    }

    /// <summary>
    /// Define event callback
    /// </summary>
    public enum ServiceCallBackEvent
    {
        REWARD_SUCCESS
    }

    [System.Serializable]
    public class IntEvent : UnityEvent<int> { }

    public class EventManager : MonoBehaviour
    {
        private Dictionary<string, UnityEvent> eventDictionary = new Dictionary<string, UnityEvent>();
        private IntEvent achievementEvent = new IntEvent();
        private IntEvent iapConsumeEvent = new IntEvent();
        private IntEvent iapNonecConsumeEvent = new IntEvent();

        #region Singleton Parameters
        private static EventManager instance;
        public static EventManager Instancce
        {
            get
            {
                if (instance == null)
                    instance = SingletonManager.AddComponent<EventManager>();
                return instance;
            }
        }
        #endregion

        #region Public static method
        public static void TriggerEventService(ServiceEvent eventName) { TriggerEvent(eventName.ToString()); }

        public static void StartListenerRewardComplete(UnityAction listener) { StartListening(ServiceCallBackEvent.REWARD_SUCCESS.ToString(), listener); }

        public static void StopListnerRewardComplete(UnityAction listener) { StopListening(ServiceCallBackEvent.REWARD_SUCCESS.ToString(), listener); }
        #endregion

        #region Public generic function event
        public static void StartListening<T>(T eventName, UnityAction listener) { StartListening(eventName.ToString(), listener); }
        public static void StopListening<T>(T eventName, UnityAction listener) { StopListening(eventName.ToString(), listener); }
        public static void TriggerEvent<T>(T eventName) { TriggerEvent(eventName.ToString()); }
        #endregion

        #region Private string method
        private static void StartListening(string eventName, UnityAction listener)
        {
            UnityEvent thisEvent = null;
            if (Instancce.eventDictionary.TryGetValue(eventName, out thisEvent))
                thisEvent.AddListener(listener);
            else
            {
                thisEvent = new UnityEvent();
                thisEvent.AddListener(listener);
                Instancce.eventDictionary.Add(eventName, thisEvent);
            }
        }

        private static void StopListening(string eventName, UnityAction listener)
        {
            if (Instancce == null) return;
            UnityEvent thisEvent = null;
            if (Instancce.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.RemoveListener(listener);
            }
        }

        private static void TriggerEvent(string eventName)
        {
            UnityEvent thisEvent = null;
            if (Instancce.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.Invoke();
            }
        }
        #endregion
            
        #region Event for Achivement
        public static void AchivementStartListening(UnityAction<int> listener) { Instancce.achievementEvent.AddListener(listener); }
        public static void AchivementStopListening(UnityAction<int> listnener) { Instancce.achievementEvent.RemoveListener(listnener); }
        public static void AchivementCallback(int i) { instance.achievementEvent.Invoke(i); }
        #endregion

        #region Event for In-APP Purchase
        //----In-App Purchase consume
        public static void IapConsumeStartListening(UnityAction<int> listener) { Instancce.iapConsumeEvent.AddListener(listener); }
        public static void IapConsumeStopListening(UnityAction<int> listnener) { Instancce.iapConsumeEvent.RemoveListener(listnener); }
        public static void IapConsumeCallback(int i) { instance.iapConsumeEvent.Invoke(i); }

        //----In-App Purchase none consume
        public static void IapNoneConsumeStartListening(UnityAction<int> listener) { Instancce.iapNonecConsumeEvent.AddListener(listener); }
        public static void IapNoneConsumeStopListening(UnityAction<int> listnener) { Instancce.iapNonecConsumeEvent.RemoveListener(listnener); }
        public static void IapNoneConsumeCallback(int i) { instance.iapNonecConsumeEvent.Invoke(i); }
        #endregion
    }
}
