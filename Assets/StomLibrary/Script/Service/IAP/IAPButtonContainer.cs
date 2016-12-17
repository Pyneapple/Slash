using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Stom.NativePlugin
{
    public class IAPButtonContainer : ButtonContainer
    {
#if UNITY_EDITOR
        [System.Serializable]
        public struct IAPInfoConsume
        {
            public string productNameApple;
            public string productNameGooglePlay;
            public int gameCoin;                               
            public float realCoin;
            public Button button;                                 // Button function                          
        }

        [System.Serializable]
        public struct IAPInfoNoneConsume
        {
            public string productNameApple;
            public string productNameGooglePlay;
            public string nameProduct;                               // Coin in gameplay
            public float realCoin;                                  // Coin in-app purchase
            public Button button;                                 // Button function
        }

        public List<IAPInfoConsume> ProductListConsume;
        public List<IAPInfoNoneConsume> ProductListNoneConsume;      
#endif
    }
}
