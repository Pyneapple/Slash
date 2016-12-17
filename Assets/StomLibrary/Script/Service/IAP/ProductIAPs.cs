using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Stom.NativePlugin
{
    [System.Serializable]
    public struct ProductIAP
    {
        public string nameProduct;
        public float usd;

        public ProductIAP(string nameProduct, float usd)
        {
            this.nameProduct = nameProduct;
            this.usd = usd;
        }
    }

    public class ProductIAPs : ScriptableObject
    {
        public List<ProductIAP> productIaps;
    }
}
