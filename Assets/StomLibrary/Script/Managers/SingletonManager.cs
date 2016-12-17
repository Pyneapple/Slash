using UnityEngine;
using System.Collections;
using System;

public class SingletonManager : MonoBehaviour {

    /// <summary>
    /// Method creature singleton manager
    /// </summary>
    public static void CreatureInstance()
    {
        if(instance == null)
        {
           // instance = new SingletonManager();
            GameObject findObj = GameObject.Find("SingletonManager");
            if(findObj == null)
            {
                findObj = new GameObject("SingletonManager");
                instance = findObj.AddComponent<SingletonManager>();

                // Make persistent data
                DontDestroyOnLoad(instance.gameObject);
            }          
        }
    }

    public static SingletonManager Instance
    {
        get
        {
            if(instance == null)
                CreatureInstance();
            return instance;
        }
    }
    private static SingletonManager instance;

    /// <summary>
    /// Method call from other singleton to add component
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T AddComponent<T> () where T : UnityEngine.Component
    {
        T t = Instance.gameObject.GetComponentInChildren<T>();

        if (t != null)
            return t;

        Type typeParameterType = typeof(T);
        t = new GameObject(typeParameterType.ToString()).AddComponent<T>();

        t.transform.SetParent(Instance.transform);

        return t;
    }
}
