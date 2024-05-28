using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Normal,
    Premmium
}

public class LevelSpawnItems : RyoMonoBehaviour
{
    [SerializeField] private ItemType _itemType;
    [SerializeField] private bool _canSpawnItem;
    [SerializeField] private bool _isFinalStage;
    [SerializeField] private bool _isSpawningItem;
    [SerializeField] private float _itemSpawnDistance;
    [SerializeField] private float[] _nextItemSpawnDistance = { 80, 200 };
    [SerializeField] private int _itemType_Normal;
    [SerializeField] private int _itemType_Premium;
    private float _distanceCounter;
    public bool IsSpawningItem
    {
        get { return this._isSpawningItem; }
        private set
        {
            if (value)
                this.StartItemSpawning();
            else
                this.EndItemSpawning();

            this._isSpawningItem = value;
        }
    }
    public ItemType ItemType
    {
        get { return this._itemType; }
        private set { this._itemType = value; }
    }
    
    public bool CanSpawnItem
    {
        get { return this._canSpawnItem; }
        private set { this._canSpawnItem = value; }
    }
    
    public bool IsFinalStage
    {
        get { return this._isFinalStage; }
        private set { this._isFinalStage = value; }
    }

    public float ItemSpawnDistance
    {
        get { return this._itemSpawnDistance; }
        private set { this._itemSpawnDistance = value;}
    }
    public float[] NextItemSpawnDistance => this._nextItemSpawnDistance; 

    protected override void SetupValues()
    {
        base.SetupValues();

        this._itemType_Normal = 0;
        this._itemType_Premium = 0;
        this._canSpawnItem = true;
        this._isFinalStage = false;
        this.ItemSpawnDistance = Random.Range(this.NextItemSpawnDistance[0], this.NextItemSpawnDistance[1]);
    }

    private void Update()
    {
        if (this.CanSpawnItem == false) return;

        if (Level.Instance.DistancToStartingPoint > this._distanceCounter + this.ItemSpawnDistance && this.IsSpawningItem == false)
        {
            this.IsSpawningItem = true;
        }

        if (this.IsSpawningItem && ItemSpawner.Instance.SpawnedCount == 0)
        {
            this.IsSpawningItem = false;
        }

    }

    private void StartItemSpawning()
    {
        int itemType_Old;
        string itemSpawn_Name = ItemSpawner.CoinMeshs[0];

        if (this.ItemType == ItemType.Normal)
        {
            this.ItemType = ItemType.Premmium;

            itemType_Old = this._itemType_Normal;
            do
            {
                this._itemType_Normal = Random.Range(0, ItemSpawner.CoinMeshs.Length);
            }
            while (this._itemType_Normal == itemType_Old);

            itemSpawn_Name = ItemSpawner.CoinMeshs[this._itemType_Normal];


            int random = Random.Range(0, 2);
            if (random == 0)
            {
                Vector3 spawnPos = CameraManager.Instance.RightCornerOfCamera.position;
                spawnPos.x += 5;
                Transform magnet = ItemSpawner.Instance.Spawn(ItemSpawner.Magnet, spawnPos, Quaternion.identity);
                magnet.gameObject.SetActive(true);
            }

        }
        else
        {
            this.ItemType = ItemType.Normal;

            itemType_Old = this._itemType_Premium;
            do
            {
                this._itemType_Premium = Random.Range(0, ItemSpawner.AdvancedItems.Length);
            }
            while (this._itemType_Premium == itemType_Old);

            itemSpawn_Name = ItemSpawner.AdvancedItems[this._itemType_Premium];

        }

        Vector3 spawnPosition = CameraManager.Instance.RightCornerOfCamera.position;
        spawnPosition.x += 10;
        Transform item = ItemSpawner.Instance.Spawn(itemSpawn_Name, spawnPosition, Quaternion.identity);
        item.gameObject.SetActive(true);

        if (this.IsFinalStage)
        {
            this.SpawnGunItem();
        }

    }

    private void EndItemSpawning()
    {
        this._distanceCounter = Level.Instance.DistancToStartingPoint;
        this.ItemSpawnDistance = Random.Range(this.NextItemSpawnDistance[0], this.NextItemSpawnDistance[1]);
    }

    private void SpawnGunItem()
    {
        Vector3 spawnPosition = CameraManager.Instance.RightCornerOfCamera.position;
        spawnPosition.x += 35;
        Transform item = ItemSpawner.Instance.Spawn(ItemSpawner.GunItem, spawnPosition, Quaternion.identity);
        item.gameObject.SetActive(true);
    }

    /*
     * 
     */

    public void SetCanSpawnItem(bool canSpawnItem)
    {
        this.CanSpawnItem = canSpawnItem;
    }

    public void FinalStageOfTheMap()
    {
        this.IsFinalStage = true;
    }

    public void DestroyItem()
    {

    }





}
