using UnityEngine;
using System.Collections;
using System;
using Stom.NativePlugin;

namespace Stom
{
    public class ServiceButton : ButtonRegister
    {
        private GameService Service { get { return PluginPersistent.Instance.Get_GameService; } }

        public void ShowLeaderBoard()
        {
            Service.ShowLeaderboard();
        }

        public void ShowAchivement()
        {
            Service.ShowAchievements();
        }

        #region Overrite Method
        protected override void ButtonCall(string nameMethod)
        {
            ButtonCall<ServiceButton>(nameMethod);
        }
        #endregion
    }
}
