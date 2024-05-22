using MVCS.Architecture.BaseCharacter;
using Patterns.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : Singleton<Level>
{
    [SerializeField] private LevelPlaneFollowPlayer _levelPlane;
    [SerializeField] private LevelSpawnTraps _levelSpawnTraps;
    [SerializeField] private LevelSpawnItems _levelSpawnItems;
    [SerializeField] private LevelSpawnEnemies _levelSpawnEnemies;
    [SerializeField] private Transform _playerStartingPoint;

    [SerializeField] private bool _isFinalStage;
    [SerializeField] private int[] _distanceOfAllLevels = { 1000, 1200, 1800, 2200, 2400, 2600 };
    [SerializeField] private int _distancToStartingPoint;
    [SerializeField] private int _currentLevelDistance = 1000;
    [SerializeField] private bool _isCountingDistance;
    [SerializeField] private int _delaySpawnOnLevelTime = 5;
    private Coroutine _delaySpawnOnLevelCoroutine;
    public bool IsCountingDistance
    {
        get { return this._isCountingDistance; }
        private set { this._isCountingDistance = value; }
    }
    public LevelPlaneFollowPlayer LevelPlane
    {
        get { return this._levelPlane; }
        private set { this._levelPlane = value; }
    }
    public Transform PlayerStartingPoint
    {
        get { return this._playerStartingPoint; }
        private set { this._playerStartingPoint = value; }
    }
    public int DistancToStartingPoint
    {
        get { return this._distancToStartingPoint; }
        private set { this._distancToStartingPoint = value; }
    }
    public int CurrentLevelDistance
    {
        get { return this._currentLevelDistance; }
        private set { this._currentLevelDistance = value; }
    }
    public bool IsFinalStage
    {
        get { return this._isFinalStage; }
        private set { this._isFinalStage = value; }
    }
    public int[] DistanceOfAllLevels => _distanceOfAllLevels;
    public float DelaySpawnOnLevelTime => _delaySpawnOnLevelTime;
    public LevelSpawnTraps LevelSpawnTraps => this._levelSpawnTraps;
    public LevelSpawnEnemies LevelSpawnEnemies  => this._levelSpawnEnemies;
    public LevelSpawnItems LevelSpawnItems  => this._levelSpawnItems;

    #region Load Components
    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadPlayerStartingPoint();
        this.LoadLevelSpawnTraps();
        this.LoadLevelSpawnItems();
        this.LoadLevelSpawnEnemies();
    }

    private void LoadPlayerStartingPoint()
    {
        if (this._playerStartingPoint != null) return;
        this._playerStartingPoint = this.transform.Find("PlayerStartingPoint");
    }

    private void LoadLevelSpawnTraps()
    {
        if (this._levelSpawnTraps != null) return;
        this._levelSpawnTraps = this.GetComponent<LevelSpawnTraps>();
    }
    private void LoadLevelSpawnItems()
    {
        if (this._levelSpawnItems != null) return;
        this._levelSpawnItems = this.GetComponent<LevelSpawnItems>();
    }
    private void LoadLevelSpawnEnemies()
    {
        if (this._levelSpawnEnemies != null) return;
        this._levelSpawnEnemies = this.GetComponent<LevelSpawnEnemies>();
    }

    private void LoadLevelPlane()
    {
        this._levelPlane = GetComponentInChildren<LevelPlaneFollowPlayer>();
    }
    #endregion

    protected override void SetupValues()
    {
        base.SetupValues();

        this.IsCountingDistance = false;
        this.DistancToStartingPoint = 0;
        this._isFinalStage = false;
    }

    private void Update()
    {
        if (this.IsCountingDistance && this.PlayerStartingPoint && this.LevelPlane)
        {
            this.DistancToStartingPoint = (int)(this.LevelPlane.transform.position.x - this.PlayerStartingPoint.position.x);

            if (this.DistancToStartingPoint > this.CurrentLevelDistance && this.IsFinalStage == false)
            {
                this.IsFinalStage = true;
                this.FinalStageOfTheMap();
            }

            //if (this.DistancToStartingPoint > 200 && this.IsFinalStage == false)
            //{
            //    this.IsFinalStage = true;
            //    this.FinalStageOfTheMap();
            //}

        }

    }

    public void PrepareToStartMatch()
    {
        this.LoadLevelPlane();

        this.CurrentLevelDistance = DistanceOfAllLevels[GameManager.Instance.CurrentLevel];

        this.LevelPlane?.PrepareToStartMatch();
        this.IsCountingDistance = false;
        this.DistancToStartingPoint = 0;
        this.DeterminePlayerStartingPoint();
    }

    public void StartMatch()
    {
        this.IsCountingDistance = true;
        this.LevelPlane?.StartFollowingPlayerAlive();
    }

    public void HandlePlayerDeath()
    {
        this.SetCanSpawnOnLevel(false);
        this.LevelPlane?.StartFollowingDeadPlayer();
    }

    public void PrepareForRevival() 
    {
        this.IsCountingDistance = false;
        this.LevelPlane?.StopAllLevelScroller();
        this.SetCanSpawnOnLevel(false);
        this.LevelSpawnEnemies?.SetEnemyAttack(false);
    }

    public void RevivePlayer()
    {
        this.LevelSpawnEnemies?.DestroyEnemy();
        this.LevelSpawnItems?.DestroyItem();
        this.LevelSpawnTraps?.DestroyTrap();
        //
        this.IsCountingDistance = true;
        this.LevelPlane?.StartFollowingRespawnPlayer();
        this._delaySpawnOnLevelCoroutine = StartCoroutine(DelaySpawnOnLevel());
        this.LevelSpawnEnemies?.SetEnemyAttack(true);
    }

    private IEnumerator DelaySpawnOnLevel()
    {
        yield return new WaitForSeconds(this.DelaySpawnOnLevelTime);
        this.SetCanSpawnOnLevel(true);
    }

    public void ShowResult()
    {
        this.SetCanSpawnOnLevel(false);
        this.LevelPlane?.StopAllLevelScroller();
    }
    /*
     * 
     */

    private void FinalStageOfTheMap()
    {
        this.LevelSpawnEnemies?.FinalStageOfTheMap();
        this.LevelSpawnItems?.FinalStageOfTheMap();
        this.LevelSpawnTraps?.FinalStageOfTheMap();
    }

    public void NextLevel()
    {
        this.IsCountingDistance = false;
        // this.DeterminePlayerStartingPoint();
    }

    /*
     * 
     */

    private void DeterminePlayerStartingPoint()
    {
        float screenHeight = Screen.height;
        Camera camera = Camera.main;

        Vector3 spawnPosition = camera.ScreenToWorldPoint(new Vector3(0f, screenHeight * 0.45f, 0f));
        spawnPosition.z = this.transform.position.z;
        this.PlayerStartingPoint.position = spawnPosition;
    }

    private void SetCanSpawnOnLevel(bool isActive)
    {
        this.LevelSpawnEnemies.SetCanSpawnEnemy(isActive);
        this.LevelSpawnItems?.SetCanSpawnItem(isActive);
        this.LevelSpawnTraps?.SetCanSpawnTrap(isActive);
    }

}
