using Patterns.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner<T> : Singleton<Spawner<T>>
{
    [Header("Spawner")]
    [SerializeField] protected Transform holder;

    [SerializeField] protected int spawnedCount = 0;

    [SerializeField] protected List<Transform> prefabs;
    [SerializeField] protected List<Transform> poolObjs;
    protected List<Transform> Prefabs
    {
        get { return this.prefabs; }
        private set { this.prefabs = value; }
    }

    public List<Transform> PoolObjs
    {
        get { return this.poolObjs; }
        private set { this.poolObjs = value; }
    }
    public Transform Holder
    {
        get { return this.holder; }
        private set { this.holder = value; }
    }
    public int SpawnedCount
    {
        get { return this.spawnedCount; }
        private set { this.spawnedCount = value; }
    }

    #region LoadComponents
    protected override void LoadComponents()
    {
        this.LoadPrefabs();
        this.LoadHolder();
    }

    protected virtual void LoadHolder()
    {
        if (this.Holder != null) return;
        this.Holder = transform.Find("Holder");
    }

    protected virtual void LoadPrefabs()
    {
        if (this.Prefabs.Count > 0) return;

        Transform prefabObj = transform.Find("Prefabs");
        foreach (Transform prefab in prefabObj)
        {
            this.Prefabs.Add(prefab);
        }

        this.HidePrefabs();
    }
    protected virtual void HidePrefabs()
    {
        foreach (Transform prefab in this.Prefabs)
        {
            prefab.gameObject.SetActive(false);
        }
    }
    #endregion

    public virtual Transform Spawn(string prefabName, Vector3 spawnPos, Quaternion rotation)
    {
        Transform prefab = this.GetPrefabByName(prefabName);
        if (prefab == null)
        {
            Debug.LogWarning("Prefab not found: " + prefabName);
            return null;
        }

        return this.Spawn(prefab, spawnPos, rotation);
    }

    public virtual Transform Spawn(Transform prefab, Vector3 spawnPos, Quaternion rotation)
    {
        Transform newPrefab = this.GetObjectFromPool(prefab);
        newPrefab.SetPositionAndRotation(spawnPos, rotation);

        newPrefab.SetParent(this.Holder);
        this.SpawnedCount++;

        return newPrefab;
    }

    protected virtual Transform GetObjectFromPool(Transform prefab)
    {
        foreach (Transform poolObj in this.PoolObjs)
        {
            if (poolObj == null) continue;

            if (poolObj.name == prefab.name)
            {
                this.PoolObjs.Remove(poolObj);
                return poolObj;
            }
        }

        Transform newPrefab = Instantiate(prefab);
        newPrefab.name = prefab.name;
        return newPrefab;
    }

    public virtual void Destroy(Transform obj)
    {
        if (this.PoolObjs.Contains(obj)) return;

        this.PoolObjs.Add(obj);
        obj.SetParent(this.Holder);
        obj.gameObject.SetActive(false);
        this.SpawnedCount--;
    }

    public virtual Transform GetPrefabByName(string prefabName)
    {
        foreach (Transform prefab in this.Prefabs)
        {
            if (prefab.name == prefabName) return prefab;
        }

        return null;
    }

    public virtual Transform RandomPrefab()
    {
        int rand = Random.Range(0, this.Prefabs.Count);
        return this.Prefabs[rand];
    }

    public virtual void Hold(Transform obj)
    {
        obj.parent = this.Holder;
    }

}
