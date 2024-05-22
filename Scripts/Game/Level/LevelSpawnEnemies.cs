using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawnEnemies : RyoMonoBehaviour
{
    [SerializeField] private bool _canSpawnEnemy;
    [SerializeField] private bool _isSpawningEnemy;
    [SerializeField] private float _enemySpawnDistance;
    [SerializeField] private int _maximumSpawnTrap = 1;
    [SerializeField] private float[] _nextEnemySpawnDistance = { 50, 100 };
    [SerializeField] private int _enemyType;
    private float _randomReproductionDistance;
    private float _distanceCounter;
    private int _spawnCounter;

    public bool IsSpawningEnemy
    {
        get { return this._isSpawningEnemy; }
        private set
        {
            if (value)
                this.StartItemSpawning();
            else
                this.EndItemSpawning();

            this._isSpawningEnemy = value;
        }
    }
    public bool CanSpawnEnemy
    {
        get { return this._canSpawnEnemy; }
        private set { this._canSpawnEnemy = value; }
    }
    public float ItemSpawnDistance
    {
        get { return this._enemySpawnDistance; }
        private set { this._enemySpawnDistance = value; }
    }
    public int EnemyType
    {
        get { return this._enemyType; }
        private set { this._enemyType = value; }
    }
    public int MaximumSpawn
    {
        get { return this._maximumSpawnTrap; }
        private set { this._maximumSpawnTrap = value; }
    }
    public float[] NextItemSpawnDistance => this._nextEnemySpawnDistance;

    #region Property Dr_Bones
    [Header("Dr Bones")]
    [SerializeField] private bool _isSpawningDr_Bones;
    [SerializeField] private float[] _dr_BonesReproductionDistance = { 5, 25 };
    [SerializeField] private int[] _dr_BonesMaximumReproduction = { 1, 3 };
    public bool IsSpawningDr_Bones
    {
        get { return this._isSpawningDr_Bones; }
        private set
        {
            if (value && this.IsSpawningDr_Bones == false)
            {
                this._randomReproductionDistance = Random.Range(this.Dr_BonesReproductionDistance[0], this.Dr_BonesReproductionDistance[1]);
                this.MaximumSpawn = Random.Range(this.Dr_BonesMaximumReproduction[0], this.Dr_BonesMaximumReproduction[1] + 1);
            }

            this._isSpawningDr_Bones = value;
        }
    }
    public float[] Dr_BonesReproductionDistance => _dr_BonesReproductionDistance;
    public int[] Dr_BonesMaximumReproduction => _dr_BonesMaximumReproduction;
    #endregion

    protected override void SetupValues()
    {
        base.SetupValues();

        this._canSpawnEnemy = true;
        this._isSpawningEnemy = false;
        this.ItemSpawnDistance = Random.Range(this.NextItemSpawnDistance[0], this.NextItemSpawnDistance[1]);
    }

    private void Update()
    {
        if(this.CanSpawnEnemy == false) return;

        if (Level.Instance.DistancToStartingPoint > this.ItemSpawnDistance && this.IsSpawningEnemy == false)
        {
            this.IsSpawningEnemy = true;
        }

        if (this.IsSpawningDr_Bones)
        {
            this.Dr_BonesSpawnProcess();
        }

    }



    private void StartItemSpawning()
    {
        //int itemType_Old = this.EnemyType;

        //do
        //{
        //    this.EnemyType = Random.Range(0, ItemSpawner.CoinMeshs.Length);
        //}
        //while (this.EnemyType == itemType_Old);

        this._distanceCounter = Level.Instance.DistancToStartingPoint;
        this._spawnCounter = 0;

        this.IsSpawningDr_Bones = true;
    }

    private void EndItemSpawning()
    {
        this.IsSpawningDr_Bones = false;

        this._distanceCounter = Level.Instance.DistancToStartingPoint;
        this.ItemSpawnDistance = Random.Range(this.NextItemSpawnDistance[0], this.NextItemSpawnDistance[1]);
        this.ItemSpawnDistance += this._distanceCounter;
    }

    private void Dr_BonesSpawnProcess()
    {
        if (this._spawnCounter >= this.MaximumSpawn)
        {
            this.IsSpawningEnemy = false;
        }
        else
        {
            if (Level.Instance.DistancToStartingPoint > this._distanceCounter + this._randomReproductionDistance)
            {
                this._distanceCounter = Level.Instance.DistancToStartingPoint;
                this._randomReproductionDistance = Random.Range(this.Dr_BonesReproductionDistance[0], this.Dr_BonesReproductionDistance[1]);
                this.Dr_BonesSpawnRandom();
                this._spawnCounter++;
            }
        }

    }

    private void Dr_BonesSpawnRandom()
    {
        Vector3 spawnPosition = CameraManager.Instance.RightCornerOfCamera.position;
        spawnPosition.x += 10;
        spawnPosition.y = -4f;
        Transform item = EnemySpawner.Instance.Spawn(EnemySpawner.Dr_Bones, spawnPosition, Quaternion.identity);
        item.gameObject.SetActive(true);
    }

    public void SetCanSpawnEnemy(bool canSpawnEnemy)
    {
        this.CanSpawnEnemy = canSpawnEnemy;
    }

    public void FinalStageOfTheMap()
    {
        this.SpawnBoss();
    }

    private void SpawnBoss()
    {
        Vector3 spawnPosition = CameraManager.Instance.RightCornerOfCamera.position;
        spawnPosition.x = -5;
        spawnPosition.y = 2;
        Transform enemy = EnemySpawner.Instance.Spawn(EnemySpawner.UFO_One, spawnPosition, Quaternion.identity);
        enemy.SetParent(CameraManager.Instance.RightCornerOfCamera);
        enemy.localPosition = spawnPosition;
        enemy.gameObject.SetActive(true);
    }

    public void SetEnemyAttack(bool isAtacking)
    {
        foreach (Transform child in CameraManager.Instance.RightCornerOfCamera)
        {
            UFO ufo = child.GetComponent<UFO>();
            if (ufo != null)
            {
                ufo.SetIsAttacking(isAtacking);
            }
        }
        
    }

    public void DestroyEnemy()
    {

    }

}
