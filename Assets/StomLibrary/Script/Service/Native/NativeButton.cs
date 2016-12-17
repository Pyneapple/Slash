using UnityEngine;
using System.Collections;
using System;
using Stom.NativePlugin;

namespace Stom { 
    public class NativeButton : ButtonRegister
    {
        private NativeIntegrate NativeIngr { get { return PluginPersistent.Instance.Get_NativeIngr; } }

        public void Rate()
        {
            NativeIngr.Rate();
        }

        public void MoreGame()
        {
            NativeIngr.MoreGame();
        }

        #region Overrite Method
        protected override void ButtonCall(string nameMethod)
        {
            ButtonCall<NativeButton>(nameMethod);
        }
        #endregion
    }
}
