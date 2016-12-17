using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Reflection;
using System;

namespace Stom {

    public abstract class ButtonRegister : MonoBehaviour
    {

#if UNITY_EDITOR
        /// <summary>
        /// This method only work in editor
        /// </summary>
        /// <param name="_button"></param>
        public void RegistListner(Button _button, string nameMethod)
        {
            UnityEditor.EditorUtility.SetDirty(_button.gameObject);
            while (_button.onClick.GetPersistentEventCount() != 0)
                UnityEditor.Events.UnityEventTools.RemovePersistentListener(_button.onClick, 0);
            UnityEditor.Events.UnityEventTools.AddStringPersistentListener(_button.onClick, ButtonCall, nameMethod);
        }

        public void UnRegistLisner(Button _button)
        {
            for (int i = 0; i < _button.onClick.GetPersistentEventCount(); i++)
            {
                if (_button.onClick.GetPersistentMethodName(i) == "ButtonCall")
                    UnityEditor.Events.UnityEventTools.RemovePersistentListener(_button.onClick, 0);
            }
        }
#endif

        protected abstract void ButtonCall(string nameMethod);

        protected void ButtonCall<T>(string nameMethod)
        {
            Type _type = typeof(T);
            MethodInfo _funcMethod = _type.GetMethod(nameMethod);
            if (_funcMethod != null)
                _funcMethod.Invoke(this, null);
        }
    }
}
