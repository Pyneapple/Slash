using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.Linq;


namespace Stom
{
    public static class Utility
    {
        /// <summary>
        /// Method get list string method that contain public method, property method
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <returns></returns>
        public static List<string>  GetStringMethodsOfClass<T>()
        {
            List<string> strMethods = new List<string>();
            Type _type = typeof(T);
            MethodInfo[] method = _type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            for (int i = 0; i < method.Length; i++)
                strMethods.Add(method[i].Name);
            return strMethods;
        }

        /// <summary>
        /// Method write text file to local device
        /// </summary>
        /// <param name="path"></param>
        /// <param name="text"></param>
        public static void WriteData(string path,string text)
        {
            System.IO.File.WriteAllText(path,text);
        }

        /// <summary>
        /// Return array strings from a specified type
        /// Note: This method use Linq library and it etremely slow. (only use in editor)
        /// </summary>
        /// <typeparam name="T">Enum type</typeparam>
        /// <returns></returns>
        public static string[] GetStringOfType<T>()
        {
            return System.Enum.GetValues(typeof(T)).Cast<Enum>().Select(v => v.ToString()).ToArray();
        }

        /// <summary>
        /// Method return random arrays from specify array
        /// </summary>
        /// <param name="arrays">Array need to random</param>
        /// <returns></returns>
        public static int[] RandomInts(int[] arrays)
        {  
            return  arrays.OrderBy(n => Guid.NewGuid()).ToArray();
        }

        /// <summary>
        /// Method to load data (Scriptable Object) from local device
        /// </summary>
        /// <typeparam name="T">Class Scriptable object</typeparam>
        /// <param name="const_key">Ket to load data</param>
        /// <returns></returns>
        public static T LoadDataScriptableObj <T>(string const_key,string pathAsset = null) where T : ScriptableObject,new ()
        {
            string _str = PlayerPrefs.GetString(const_key, null);
            if (_str != "")
            {
                T _info = new T();
                JsonUtility.FromJsonOverwrite(_str, _info);
                return _info;
            }
            else
            {
                T _infoPlayer = GameObject.Instantiate(Resources.Load<T>(pathAsset)) as T;
                SaveData<T>(_infoPlayer, const_key);
                return _infoPlayer;
            }
        }



        /// <summary>
        /// Method to load data (Scriptable Object) from local device
        /// </summary>
        /// <typeparam name="T">Class Scriptable object</typeparam>
        /// <param name="const_key">Ket to load data</param>
        /// <returns></returns>
        public static T LoadData<T>(string const_key) where T : new()
        {
            string _str = PlayerPrefs.GetString(const_key, null);


            if (_str != "")
            {
                return JsonUtility.FromJson<T>(_str);
            }
            else
            {
                SaveData<T>(new T(),const_key);
                return LoadData<T>(const_key);
            }
        }

        /// <summary>
        /// Method to save data (Scriptable Object) from local device
        /// </summary>
        /// <typeparam name="T">Type save</typeparam>
        /// <param name="const_key">Ket to load data</param>
        /// <returns></returns>
        public static void SaveData<T>(T data, string const_key)
        {
            string _convertStr = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(const_key, _convertStr);
        }

        /// <summary>
        /// Method get component from object and add to other
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="go"></param>
        /// <param name="toAdd"></param>
        /// <returns></returns>
        public static T AddComponent<T>(this GameObject go, T toAdd) where T : Component
        {
            return go.AddComponent<T>().GetCopyOf(toAdd) as T;
        }

        /// <summary>
        /// Method copy component add to  other
        /// </summary>
        public static T GetCopyOf<T>(this Component comp, T other) where T : Component
        {
            Type type = comp.GetType();
            if (type != other.GetType()) return null; // type mis-match
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
            PropertyInfo[] pinfos = type.GetProperties(flags);
            foreach (var pinfo in pinfos)
            {
                if (pinfo.CanWrite)
                {
                    try
                    {
                        pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
                    }
                    catch { }                 }
            }
            FieldInfo[] finfos = type.GetFields(flags);
            foreach (var finfo in finfos)
            {
                finfo.SetValue(comp, finfo.GetValue(other));
            }
            return comp as T;
        }

        public static float GetWidthOrthographicCamera(Camera _camera)
        {
            return _camera.orthographicSize *( (float) Screen.width / (float) Screen.height);
        }
    }
}
