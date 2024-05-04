using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string nameTag;
        public GameObject gamePrefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>>poolDict;
    #region Singleton

    public static ObjectPooling instance;

    private void Awake()
    {
        instance = this; 
    }
    
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        poolDict = new Dictionary<string, Queue<GameObject>>();
        
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            
            for(int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.gamePrefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDict.Add(pool.nameTag, objectPool);
        }
    }

    public GameObject spawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDict.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with this tag " + tag + " doesnt exist dude.");
            return null;
        }
        GameObject object2Spawn = poolDict[tag].Dequeue();

        object2Spawn.SetActive(true);
        object2Spawn.transform.position = position;
        object2Spawn.transform.rotation = rotation;

        poolDict[tag].Enqueue(object2Spawn);
        return object2Spawn;
    }

    // Update is called once per frame
}
