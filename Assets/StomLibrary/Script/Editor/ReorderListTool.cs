using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using Stom.NativePlugin;

namespace Stom
{
    public class ReorderListTool 
    {
        private const string const_properButton     = "button";
        private const string const_properFunction   = "identify";
        public static void InitialReorderList<T>(ButtonContainer container, ref SerializedObject serObj, ref ReorderableList list) where T : ButtonRegister
        {
            List<string> strMethod = new List<string>();
            strMethod = Utility.GetStringMethodsOfClass<T>();

            serObj = new SerializedObject(container);
            list = new ReorderableList(serObj, serObj.FindProperty("buttons"), true, true, true, true);
            ReorderableList _roList = list;

            // Draw list
            list.drawHeaderCallback     = (Rect rect) => { EditorGUITool.DrawHeader("Buttons", 50.0f, Color.cyan, rect); };
            list.drawElementCallback    = (Rect rect, int index, bool isActive, bool isFocused) => { ReorderListTool.DrawElementButtonSeriable(_roList, strMethod, index, rect); };
            list.onAddCallback          = (ReorderableList l) => { ReorderListTool.AddElementButtonSeriable(_roList); };
            list.onSelectCallback       = (ReorderableList l) => { ReorderListTool.SelectElementButtonSeriable(l); };
            list.onRemoveCallback       = (ReorderableList l) => { ReorderListTool.RemoveElementButtonSeriable<T>(l); };
        }

        public static void ApplyModifyButtonSeriable<T>(ReorderableList list) where T : ButtonRegister
        {
            for (int i = 0; i < list.serializedProperty.arraySize; i++)
            {
                SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(i);
                Button _button = (Button)element.FindPropertyRelative(const_properButton).objectReferenceValue;
                // If button is null, do nothing
                if (_button == null)
                    return;
                // Check component exist, add component
                if (!_button.GetComponent<T>())
                {
                    if (_button.GetComponent<ButtonRegister>())
                    {
                        EditorGUITool.ShowDialogError("Button order: " + i.ToString("00") + " contain other service component. Can't apply new service component!");
                        return;
                    }
                    _button.gameObject.AddComponent<T>();
                }
                // Get string method     
                string _strFunc = element.FindPropertyRelative(const_properFunction).stringValue;
                // Apply
                if (_button != null)
                    _button.GetComponent<T>().RegistListner(_button, _strFunc);
            }
        }

        public static void DrawElementButtonSeriable(ReorderableList list, List<string> methods, int index,Rect rect)
        {
            var element = list.serializedProperty.GetArrayElementAtIndex(index);
            rect.x += 10.0f;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width / 4, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("button"), GUIContent.none);
            rect.x += (rect.width / 4);

            // Make Popup
            string _strType = element.FindPropertyRelative(const_properFunction).stringValue;
            int[] values = new int[methods.Count];

            for (int i = 0; i < values.Length; i++)
                values[i] = i;

            int _index = EditorGUI.IntPopup(new Rect(rect.x, rect.y, rect.width/2, EditorGUIUtility.singleLineHeight), methods.FindIndex(i => i == _strType), methods.ToArray(), values);
            element.FindPropertyRelative(const_properFunction).stringValue = (_index >= 0) ? methods[_index] : "None";
        }

        public static void AddElementButtonSeriable(ReorderableList list)
        {
            int index = list.serializedProperty.arraySize;
            list.serializedProperty.arraySize++;
            list.index = index;
            SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
            element.FindPropertyRelative(const_properButton).objectReferenceValue = null;
            element.FindPropertyRelative(const_properFunction).stringValue = "None";
        }

        public static void SelectElementButtonSeriable(ReorderableList list)
        {
            Button _button = (Button)list.serializedProperty.GetArrayElementAtIndex(list.index).FindPropertyRelative(const_properButton).objectReferenceValue;
            if (_button != null)
            {
                EditorGUIUtility.PingObject(_button.gameObject);
                Selection.activeObject = _button.gameObject;
            }
        }

        public static void RemoveElementButtonSeriable <T>(ReorderableList list) where T : ButtonRegister
        {
            Button _button = (Button)list.serializedProperty.GetArrayElementAtIndex(list.index).FindPropertyRelative("button").objectReferenceValue;
            if (_button != null)
            {
                EditorGUIUtility.PingObject(_button.gameObject);
                Selection.activeObject = _button.gameObject;
            }

            if (_button)
            {
                T _component = _button.GetComponent<T>();
                if (_component)
                {
                    _component.UnRegistLisner(_button);
                    GameObject.DestroyImmediate(_component);
                }
            }
            ReorderableList.defaultBehaviours.DoRemoveButton(list);
        }

        public static void InitialReorderListAchivement<T>(GameObject objSeri, ref SerializedObject serObj, ref ReorderableList list) where T : MonoBehaviour
        {
            serObj = new SerializedObject(objSeri.GetComponent<T>());
            list = new ReorderableList(serObj, serObj.FindProperty("Achievement"), true, true, true, true);
            ReorderableList _roList = list;

            list.drawHeaderCallback = (Rect rect) => { EditorGUITool.DrawHeader("Achievement", 80.0f, Color.cyan, rect); };
            list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                _roList.elementHeight = 75.0f;
                var element = _roList.serializedProperty.GetArrayElementAtIndex(index);
                rect.x += 10.0f;
                rect.y += 10.0f;

                LabelPropertyField(new Rect(rect.x, rect.y, rect.width, rect.height)        , "Id Android"      , "idAchievementAndroid", 70.0f, rect.width / 4 , element);
                LabelPropertyField(new Rect(rect.x, rect.y + 20.0f, rect.width, rect.height), "Id IOS"          , "idAchievementIos"    , 70.0f, rect.width / 4 , element);
                LabelPropertyField(new Rect(rect.x, rect.y + 40.0f, rect.width, rect.height), "Process"         , "progress"            , 70.0f, rect.width / 4 , element);
                rect.x += (rect.width / 4 + 100.0f);
                LabelPropertyField(new Rect(rect.x, rect.y, rect.width, rect.height)        , "Event report"    , "eventReportListener" , 90.0f, rect.width / 4 , element);
                LabelPropertyField(new Rect(rect.x, rect.y + 20.0f, rect.width, rect.height), "Index callback"  , "index"               , 90.0f, rect.width / 4, element);
            };
        }

        public static void InitialReorderListLeaderboard<T>(GameObject objSeri, ref SerializedObject serObj, ref ReorderableList list) where T : MonoBehaviour
        {
            serObj = new SerializedObject(objSeri.GetComponent<T>());
            list = new ReorderableList(serObj, serObj.FindProperty("Leaderboard"), true, true, true, true);
            ReorderableList _roList = list;

            list.drawHeaderCallback = (Rect rect) => { EditorGUITool.DrawHeader("Leaderboard", 80.0f, Color.cyan, rect); };
            list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                _roList.elementHeight = 55.0f;
                var element = _roList.serializedProperty.GetArrayElementAtIndex(index);
                rect.x += 10.0f;
                rect.y += 10.0f;

                LabelPropertyField(new Rect(rect.x, rect.y, rect.width, rect.height)        , "Id Android"      , "idLeadboardAndroid"  , 65.0f, rect.width / 5, element);
                LabelPropertyField(new Rect(rect.x, rect.y + 20.0f, rect.width, rect.height), "Id IOS"          , "idLeadboardIos"      , 65.0f, rect.width / 5, element);            
                rect.x += (rect.width / 4 + 100.0f);
                LabelPropertyField(new Rect(rect.x, rect.y, rect.width, rect.height)        , "Type Leaderboard", "name"                , 115.0f, rect.width / 5, element);
                LabelPropertyField(new Rect(rect.x, rect.y + 20.0f, rect.width, rect.height), "Event report"    , "eventReport"         , 115.0f, rect.width / 5, element);
            };
        }

        /// <summary>
        /// This method intial reorderlist to serialized list 
        /// Data from in-app purchase container will copy to data in-app purchase run time
        /// Obj container will hold all referent other object and copy to in-app purchase
        /// If you remove in-app pruchase , this method still run but you will lost referent copy
        /// from container to in-app purchase run time
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objContainer"></param>
        /// <param name="iapPurchase"></param>
        /// <param name="serObj"></param>
        /// <param name="listPurchase"></param>
        /// <param name="isConsume"></param>
        public static void InitialReorderListInAppPurchase<T>(GameObject objContainer,IAPurchase iapPurchase, ref SerializedObject serObj, ref ReorderableList listPurchase,bool isConsume) where T : MonoBehaviour
        {
            serObj = new SerializedObject(objContainer.GetComponent<T>());
            List<ProductPurchase> _iapRuntimeListPs = null;
            // Intial list with consume or noconsume
            if (isConsume)
            {             
                listPurchase = new ReorderableList(serObj, serObj.FindProperty("ProductListConsume"), true, true, true, true);
                iapPurchase.ProductListConsume = new List<ProductPurchase>();
                _iapRuntimeListPs = iapPurchase.ProductListConsume;
            }
            else
            {
                iapPurchase.ProductListNoneConsume = new List<ProductPurchase>();
                listPurchase = new ReorderableList(serObj, serObj.FindProperty("ProductListNoneConsume"), true, true, true, true);
                _iapRuntimeListPs = iapPurchase.ProductListNoneConsume;
            }
            // When intial reorderlist, copy data from container to in-app pruchase
            // Make sure list data in-app purchase dont' out range list data
            for (int i = 0; i < listPurchase.count; i++)
                _iapRuntimeListPs.Add(new ProductPurchase("", ""));

            ReorderableList _roList = listPurchase;

            listPurchase.drawHeaderCallback = (Rect rect) =>
            {
                if(isConsume)
                    EditorGUITool.DrawHeader("In-App Purchase Consume", 150.0f, Color.cyan, rect);
                else
                    EditorGUITool.DrawHeader("In-App Purchase None Consume", 180.0f, Color.cyan, rect);
            };
            // Element change
            listPurchase.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                _roList.elementHeight = 95.0f;
                var element = _roList.serializedProperty.GetArrayElementAtIndex(index);
                rect.x += 10.0f;
                rect.y += 10.0f;
                LabelPropertyField(new Rect(rect.x, rect.y, rect.width, rect.height)        , "Id Android"  , "productNameApple", 70.0f, rect.width / 4, element);
                LabelPropertyField(new Rect(rect.x, rect.y + 20.0f, rect.width, rect.height), "Id IOS"      , "productNameGooglePlay", 70.0f, rect.width / 4, element);
                rect.x += (rect.width / 4 + 100.0f);
                if(isConsume)
                    LabelPropertyField(new Rect(rect.x, rect.y, rect.width, rect.height)    , "Coin"        , "gameCoin", 90.0f, rect.width / 4, element);
                else
                    LabelPropertyField(new Rect(rect.x, rect.y, rect.width, rect.height)    , "Product"     , "nameProduct", 90.0f, rect.width / 4, element);
                LabelPropertyField(new Rect(rect.x, rect.y + 20.0f, rect.width, rect.height), "USD"         , "realCoin", 90.0f, rect.width / 4, element);
                LabelPropertyField(new Rect(rect.x, rect.y + 40.0f, rect.width, rect.height), "Button"      , "button", 90.0f, rect.width / 4, element);

                string _idiOs = element.FindPropertyRelative("productNameApple").stringValue;
                string _idAndroid = element.FindPropertyRelative("productNameGooglePlay").stringValue;

                // Apply data from in-app purchase container to in-app purchase run time
                if (index > _iapRuntimeListPs.Count)
                {
                    _iapRuntimeListPs.Add(new ProductPurchase("", ""));
                    _iapRuntimeListPs[index] = new ProductPurchase(_idiOs, _idAndroid);
                }else
                    _iapRuntimeListPs[index] = new ProductPurchase(_idiOs, _idAndroid);
            };
            // Select element
            listPurchase.onSelectCallback = (ReorderableList l) => { ReorderListTool.SelectElementButtonSeriable(l); };
            // Remove element
            listPurchase.onRemoveCallback = (ReorderableList l) => 
            {
                // Remove element in in-app purchase runtime
                _iapRuntimeListPs.RemoveAt(l.index);
                ReorderListTool.RemoveElementInAppPurchaseSeriable(l, isConsume,iapPurchase);
            };
            // Add element
            listPurchase.onAddCallback = (ReorderableList l) =>
            {
                // Do default behavious
                ReorderableList.defaultBehaviours.DoAddButton(l);
                // Add new element to In-App Purchase code run-time
                _iapRuntimeListPs.Add(new ProductPurchase("", ""));
                // Add event listener for none consume
                if (!isConsume)
                {
                    IAPNoneConsumeProcess _noneConsumeProcess = iapPurchase.gameObject.GetComponent<IAPNoneConsumeProcess>();
                    if (_noneConsumeProcess == null)
                    {
                        _noneConsumeProcess = iapPurchase.gameObject.AddComponent<IAPNoneConsumeProcess>();
                        var _productIAPs = ScriptableObjectEditor.GetAssetScritableObject<ProductIAPs>("InAppPuchaseNoneConsume");
                        // Make new asset
                        if (_productIAPs == null)
                            _productIAPs = ScriptableObjectEditor.CreateScritableObject<ProductIAPs>("InAppPuchaseNoneConsume");
                        _noneConsumeProcess.iaps = _productIAPs;
                    }
                }
            };
        }

        /// <summary>
        /// This method will add component IAPButton in Button
        /// And set function for button base on editor interface
        /// </summary>
        /// <param name="list"></param>
        /// <param name="isConsume"></param>
        public static void ApplyButtonInAppPurchase(ReorderableList list,bool isConsume)
        {
            for (int i=0;i<list.serializedProperty.arraySize;i++)
            {
                var _button = (Button) list.serializedProperty.GetArrayElementAtIndex(i).FindPropertyRelative("button").objectReferenceValue;
                if (_button == null) return;
                IAPButton _iapButton = _button.GetComponent<IAPButton>();
                if (_iapButton == null)
                {
                    if (_button.GetComponent<ButtonRegister>())
                    {
                        EditorGUITool.ShowDialogError("Button order: " + i.ToString("00") + "contain other service component. Can't apply new service component!");
                        return;
                    }
                    _button.gameObject.AddComponent<IAPButton>().RegistPurchaseButton(_button, i, isConsume);
                }
                else
                    _iapButton.RegistPurchaseButton(_button, i, isConsume);
            }
        }

        /// <summary>
        /// This method will remove component InAppPurchase button from button
        /// </summary>
        /// <param name="list"></param>
        public static void RemoveElementInAppPurchaseSeriable(ReorderableList list,bool isConsume, IAPurchase iapPurchase)
        {
            Button _button = (Button)list.serializedProperty.GetArrayElementAtIndex(list.index).FindPropertyRelative("button").objectReferenceValue;

            _button = (Button)list.serializedProperty.GetArrayElementAtIndex(list.index).FindPropertyRelative("button").objectReferenceValue;
            // Focus button removed
            if (_button != null)
            {
                EditorGUIUtility.PingObject(_button.gameObject);
                Selection.activeObject = _button.gameObject;
            }
            // Start remove component InAppPurchase
            if (_button)
            {
                IAPButton _iapComponent = _button.GetComponent<IAPButton>();
                if (_iapComponent)
                {
                    _button.GetComponent<IAPButton>().UnRegistPurchaseButton(_button, isConsume);
                    GameObject.DestroyImmediate(_button.GetComponent<IAPButton>());
                }
            }
            // Do default behaviours
            ReorderableList.defaultBehaviours.DoRemoveButton(list);
            // Remove iap listener if don't use non comsume iap
            if (list.count == 0)
            {
                if (!isConsume)
                {
                    IAPNoneConsumeProcess _noneConsumeProcess = iapPurchase.gameObject.GetComponent<IAPNoneConsumeProcess>();
                    if (_noneConsumeProcess != null)
                        GameObject.DestroyImmediate(_noneConsumeProcess);
                }
            }
        }

        public static void InitialReorderListAds<T>(GameObject objSeri, ref SerializedObject serObj, ref ReorderableList list) where T : MonoBehaviour
        {
            serObj = new SerializedObject(objSeri.GetComponent<T>());
            list = new ReorderableList(serObj, serObj.FindProperty("eventAds"), true, true, true, true);
            ReorderableList _roList = list;

            list.drawHeaderCallback = (Rect rect) => { EditorGUITool.DrawHeader("Ads Controller", 80.0f, Color.cyan, rect); };
            list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                rect.x += 10;
                rect.y += 10;
                _roList.elementHeight = 55.0f;
                var element = _roList.serializedProperty.GetArrayElementAtIndex(index);
                LabelPropertyField(new Rect(rect.x, rect.y, rect.width, rect.height), "Event ", "eventAds", 70.0f, rect.width / 4, element);
                LabelPropertyField(new Rect(rect.x, rect.y + 20.0f, rect.width, rect.height), "Probability", "probability", 70.0f, rect.width / 2, element);
                rect.x += (rect.width / 4 + 120.0f);
                LabelPropertyField(new Rect(rect.x, rect.y, rect.width, rect.height), "Type Ads ", "typeAds", 70.0f, rect.width / 4, element);              
            };
        }

        public static void LabelPropertyField(Rect rect,string label, string properName,float widthLabel,float widthContent,SerializedProperty element)
        {
            GUI.Label(new Rect(rect.x, rect.y, widthLabel, EditorGUIUtility.singleLineHeight), label);
            rect.x += 5.0f;
            EditorGUI.PropertyField(new Rect(rect.x + widthLabel, rect.y, widthContent, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative(properName), GUIContent.none);
        }
    }
}
