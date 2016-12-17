using UnityEngine;
using System.Collections;

namespace Stom.NativePlugin
{
    public class NativeIntegrate : ButtonRegister
    {
        public string linkRateAndroid;
        public string linkRateIos;
        public string linkMoreGameAndroid = "https://play.google.com/store/apps/dev?id=6748886794767341922";
        public string linkMoreGameIos = "https://itunes.apple.com/us/developer/luan-nguyen-dinh/id1050673156";

        protected override void ButtonCall(string nameMethod)
        {
            ButtonCall<NativeIntegrate>(nameMethod);
        }

        public void Rate()
        {
#if !UNITY_EDITOR
#if UNITY_ANDROID
		Application.OpenURL(linkRateAndroid);
#elif UNITY_IOS
		Application.OpenURL(linkRateIos);
#endif
#endif
        }

        public void MoreGame()
        {

#if !UNITY_EDITOR
#if UNITY_ANDROID
		Application.OpenURL(linkMoreGameAndroid);
#elif UNITY_IOS
		Application.OpenURL(linkMoreGameIos);
#endif
#endif
        }
    }
}
