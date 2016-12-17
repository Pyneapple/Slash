using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Stom.NativePlugin
{
    [System.Serializable]
    public struct CoinIap
    {
        public int coin;
        public float usd;

        public CoinIap(int coin, float usd)
        {
            this.coin = coin;
            this.usd = usd;
        }
    }

    public class CoinIAPs : ScriptableObject
    {
        public List<CoinIap> coinIaps;
    }
}




