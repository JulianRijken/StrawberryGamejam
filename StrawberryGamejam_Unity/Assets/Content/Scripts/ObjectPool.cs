using System.Collections.Generic;
using UnityEngine;


public class ObjectPool<T> where T : MonoBehaviour
{

    private List<T> m_PooledObjects;

    public ObjectPool(int defaltSpawnCount = 0)
    {
        m_PooledObjects = new List<T>();
    }

    private T GetInactiveObject()
    {
        for (int i = 0; i < m_PooledObjects.Count; i++)
        {
            if (!m_PooledObjects[i].gameObject.activeSelf)
            {
                return m_PooledObjects[i];
            }
        }

        return null;
    }


    public T GetObject(Vector3 position, Quaternion rotation, Transform parent = null)
    {
        return GetObject(position, rotation, out _, parent);
    }

    public T GetObject(Vector3 position, Quaternion rotation, out bool createdNew, Transform parent = null)
    {
        // Get object from pool
        T spawnedObject = GetInactiveObject();

        // If object does not exist create a new object 
        if (!spawnedObject)
        {
            GameObject spawned = new GameObject();
            spawnedObject = spawned.AddComponent<T>();
            m_PooledObjects.Add(spawnedObject);

            createdNew = true;
        }
        else
        {
            createdNew = false;
        }


        // Set active and settings
        spawnedObject.gameObject.SetActive(true);

        if(parent) spawnedObject.transform.parent = parent;
        spawnedObject.transform.position = position;
        spawnedObject.transform.rotation = rotation;

        

        return spawnedObject;
    }


    public List<T> GetActiveObjects()
    {
        List<T> activeObjects = new List<T>();
        for (int i = 0; i < m_PooledObjects.Count; i++)
        {
            activeObjects.Add(m_PooledObjects[i]);
        }

        return activeObjects;
    }

}