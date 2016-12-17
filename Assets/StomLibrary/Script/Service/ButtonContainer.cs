using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Stom
{
    public class ButtonContainer : MonoBehaviour
    {
#if UNITY_EDITOR
        [System.Serializable]
        public struct ButtonService
        {
            public Button button;
            public string identify;
        }
        public List<ButtonService> buttons;
#endif
    }
}
