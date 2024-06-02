using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
      public static ObjectPooler SharedInstance;
      public List<GameObject> pooledObjects;
      public GameObject objectToPool;
      public int amountToPool;
      public List<GameObject> pooledRocketObjects;
      public GameObject objectRocketToPool;
      public List<GameObject> pooledBulletObjects;
      public GameObject objectBulletToPool;
      public List<GameObject> pooledBulletFreezeObjects;
      public GameObject objectBulletFreezeToPool;
      
    
        void Awake()
        {
            SharedInstance = this;
        }
    
        void Start()
        {
            pooledObjects = new List<GameObject>();
            GameObject tmp;
            for (int i = 0; i < amountToPool; i++)
            {
                tmp = Instantiate(objectToPool);
                tmp.SetActive(false);
                pooledObjects.Add(tmp);
            }
        }
        public GameObject GetPooledObject()
        {
            foreach (var objToPull in pooledObjects)
            {
                if (!objToPull.activeInHierarchy)
                {
                    return objToPull;
                }
            }
            GameObject newObject = Instantiate(objectToPool);
            pooledObjects.Add(newObject);
            return newObject;
        }
        
        public GameObject GetRocketPooledObject()
        {
            foreach (var objToPull in pooledRocketObjects)
            {
                if (!objToPull.activeInHierarchy)
                {
                    return objToPull;
                }
            }
            GameObject newObject = Instantiate(objectRocketToPool);
            pooledRocketObjects.Add(newObject);
            return newObject;
        }
        
       
        public GameObject GetBulletPooledObject()
        {
            foreach (var objToPull in pooledBulletObjects)
            {
                if (!objToPull.activeInHierarchy)
                {
                    return objToPull;
                }
            }
            GameObject newObject = Instantiate(objectBulletToPool);
            pooledBulletObjects.Add(newObject);
            return newObject;
        }
        
        public GameObject GetBulletFreezePooledObject()
        {
            foreach (var objToPull in pooledBulletFreezeObjects)
            {
                if (!objToPull.activeInHierarchy)
                {
                    return objToPull;
                }
            }
            GameObject newObject = Instantiate(objectBulletFreezeToPool);
            pooledBulletFreezeObjects.Add(newObject);
            return newObject;
        }
        
        
}
