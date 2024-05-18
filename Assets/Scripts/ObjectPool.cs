using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private List<Pool> pools = new List<Pool>();

    public static ObjectPool instance;

    private void Awake()
    {
        instance = this;

        for (int i = 0; i < pools.Count; i++)
        {
            for (int x = 0; x < pools[i].poolSize; x++)
            {
                GameObject poolObject = Instantiate(pools[i].poolObject);
                pools[i].objectList.Add(poolObject);
                poolObject.SetActive(false);
                poolObject.name = poolObject.name.Replace("(Clone)", "");
                pools[i].stringID = poolObject.name;

                if (pools[i].parentTransform)
                    poolObject.transform.SetParent(pools[i].parentTransform);
            }
        }
    }

    public GameObject GetObjectFromPool(string objectString)
    {
        GameObject pooledObject = null;

        for (int i = 0; i < pools.Count; i++)
        {
            if (pools[i].stringID == objectString)
            {
                for (int x = 0; x < pools[i].objectList.Count; x++)
                {
                    if (!pools[i].objectList[x].activeInHierarchy)
                    {
                        pooledObject = pools[i].objectList[x];
                        break;
                    }
                }
            }
        }

        return pooledObject;
    }

    public List<GameObject> GetEntirePool(string objectString)
    {
        List<GameObject> gameObjects = new List<GameObject>();

        for (int i = 0; i < pools.Count; i++)
            if (pools[i].stringID == objectString)
                gameObjects.AddRange(pools[i].objectList);

        return gameObjects;
    }

    [System.Serializable]
    public class Pool
    {
        public string stringID;
        public int poolSize;
        public GameObject poolObject;
        public Transform parentTransform;
        public List<GameObject> objectList = new List<GameObject>();
    }
}
