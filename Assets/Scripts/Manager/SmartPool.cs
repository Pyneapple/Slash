using UnityEngine;
using System.Collections.Generic;

public class Pool
{
    int nextId;                                         // To make name

    Stack<GameObject> inactive;
    GameObject prefabContrainer;
    GameObject prefab;

    public Pool(GameObject prefabs, int initQuantify)
    {
        this.prefab = prefabs;
        this.prefabContrainer = new GameObject(prefabs.name + "_pool");
		MonoBehaviour.DontDestroyOnLoad (this.prefabContrainer);
        //Intial stack
        inactive = new Stack<GameObject>(initQuantify);
    }

    // Method call sapwn
    public GameObject Spawn(Vector3 position, Quaternion rotation)
    {
        GameObject obj ;

        if (inactive.Count == 0)
        {
            // Instatite if stack empty
            obj = (GameObject)GameObject.Instantiate(prefab, position, rotation);

            if (nextId >= 10)
                obj.name = prefab.name + "_" + (nextId++);
            else
                obj.name = prefab.name + "_0" + (nextId++);

            obj.AddComponent<PoolIdentify>().pool = this;
        }
        else
        {
            obj = inactive.Pop();

            if (obj == null)
                return Spawn(position, rotation);
        }

        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);


        // Set to contrainer
        obj.transform.SetParent(prefabContrainer.transform);
        return obj;
    }

    public void Despawn(GameObject obj)
    {
        obj.SetActive(false);
        inactive.Push(obj);
    }

    /// <summary>
    /// Method to destroy pool
    /// </summary>
    public void DestroyAll()
    {
        // Return stack
        prefab = null;         

        // Clear stack
        inactive.Clear();

        // Destroy child
        for (int i = 0; i < prefabContrainer.transform.childCount; i++)
            MonoBehaviour.DestroyObject(prefabContrainer.transform.GetChild(i).gameObject);

        // Destroy parent
        Object.Destroy(prefabContrainer);

        Resources.UnloadUnusedAssets();
    }

    /// <summary>
    /// Method retunr pool for single pool prefabs
    /// </summary>
    public void ReturnPool()
    {
        Transform containerTrans = prefabContrainer.transform;
        for (int i = 0; i < containerTrans.childCount; i++)
        {
            if (containerTrans.GetChild(i).gameObject.activeSelf)
                SmartPool.Despawn(containerTrans.GetChild(i).gameObject);
        }       
    }
}

public class PoolIdentify : MonoBehaviour
{
    public Pool pool;
}

public class SmartPool : MonoBehaviour {

    const int DEFAULT_POOL_SIZE = 3;

    static Dictionary<GameObject, Pool> pools;                          // How infor pool

    // --INTIAL DICTIONARY FOR POOL--//
    static void Init(GameObject prefabs = null,int quantify = DEFAULT_POOL_SIZE)
    {
        if (pools == null)
            pools = new Dictionary<GameObject, Pool>();

        if (prefabs != null && pools.ContainsKey(prefabs) == false)
            pools[prefabs] = new Pool(prefabs, quantify);
    }

    //--METHOD PRELOAD SOME OBJECT TO RESERVE--//
    static public void Preload(GameObject prefab,int quantify)
    {
        Init(prefab, quantify);

        GameObject[] obs = new GameObject[quantify];
        for (int i = 0; i < quantify; i++)
            obs[i] = Spawn(prefab, Vector3.zero, Quaternion.identity);

        for (int i = 0; i < quantify; i++)
            Despawn(obs[i]);
    }

    //--METHOD ACTIVE POOL OBJECT--//
	static public GameObject Spawn(GameObject prefabs,Vector3 position,Quaternion rotarion)
    {
        Init(prefabs);
        return pools[prefabs].Spawn(position, rotarion);
    }

    //--METHOD DEACTIVE OBJECT POOL--
    static public void Despawn(GameObject prefabs)
    {
        PoolIdentify poolIndent = prefabs.GetComponent<PoolIdentify>();

        if (poolIndent == null)
            prefabs.SetActive(false);
        else
            poolIndent.pool.Despawn(prefabs);
    }

    //--METHOD DESTROY POOL FOR SINGLE PREFAB--//
    static public void DestroyPool(GameObject prefabs)
    {
        if(pools.ContainsKey(prefabs))
        {
            pools[prefabs].DestroyAll();
            pools.Remove(prefabs);
        }
    }

    //--METHOD RETURN POOL FOR SINGLE PREFAB--//
    static public void ReturnPool(GameObject prefabs)
    {
        if (pools == null)
            return;

        if (pools.ContainsKey(prefabs))
            pools[prefabs].ReturnPool();
    }

    //--METHOD RETURN POOL ALL--//
    static public void ReturnPoolAll()
    {
        var pools = FindObjectsOfType<PoolIdentify>();
        for (int i = 0; i < pools.Length; i++)
            SmartPool.Despawn(pools[i].gameObject);
    }
}
