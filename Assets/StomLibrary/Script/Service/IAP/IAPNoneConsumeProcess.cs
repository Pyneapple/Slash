using UnityEngine;
using System.Collections;

namespace Stom.NativePlugin
{
    public class IAPNoneConsumeProcess : MonoBehaviour
    {
        public ProductIAPs iaps;

        public void Awake()
        {
            EventManager.IapNoneConsumeStartListening(ListnerNoneConsume);
        }

        private void ListnerNoneConsume(int index)
        {
            PlayerPrefs.SetString(iaps.productIaps[index].nameProduct, "buy");
        }

//       public static bool CheckNoneConsume(int index)
//    {
  //          return PlayerPrefs.HasKey(PluginPersistent.Instance.Get_IAPNCPProcess.iaps.productIaps[index].nameProduct);
    //  }

    }
}
