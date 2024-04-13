using MVCS.Architecture.BaseCharacter;
using Patterns.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : Singleton<Level>
{
    [SerializeField] private LevelPlaneFollowPlayer _levelPlane;
    [SerializeField] private Transform _playerStartingPoint;
    [SerializeField] private int _distancToStartingPoint;
    [SerializeField] private bool _isCountingDistance;
    public bool IsCountingDistance
    {
        get { return this._isCountingDistance; }
        private set { this._isCountingDistance = value; }
    }
    public LevelPlaneFollowPlayer LevelPlane => this._levelPlane; 

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

    #region Load Components
    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadLevelPlane();
        this.LoadPlayerStartingPoint();
    }

    private void LoadPlayerStartingPoint()
    {
        if (this._playerStartingPoint != null) return;
        this._playerStartingPoint = this.transform.Find("PlayerStartingPoint");
    }

    private void LoadLevelPlane()
    {
        if (this._levelPlane != null) return;
        this._levelPlane = GetComponentInChildren<LevelPlaneFollowPlayer>();
    }
    #endregion

    protected override void SetupValues()
    {
        base.SetupValues();

        this.IsCountingDistance = false;
        this.DistancToStartingPoint = 0;
    }

    private void Update()
    {
        if (this.IsCountingDistance && this.PlayerStartingPoint && this.LevelPlane)
        {
            this.DistancToStartingPoint = (int)(this.LevelPlane.transform.position.x - this.PlayerStartingPoint.position.x);
        }
    }

    public void PrepareToStartMatch()
    {
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
        this.LevelPlane?.StartFollowingDeadPlayer();
    }

    public void PrepareForRevival() 
    {
        this.IsCountingDistance = false;
    }

    public void RevivePlayer()
    {
        this.IsCountingDistance = true;
        this.LevelPlane?.StartFollowingRespawnPlayer();
    }

    private void DeterminePlayerStartingPoint()
    {
        float screenHeight = Screen.height;
        Camera camera = Camera.main;

        Vector3 spawnPosition = camera.ScreenToWorldPoint(new Vector3(0f, screenHeight * 0.45f, 0f));
        spawnPosition.z = this.transform.position.z;
        this.PlayerStartingPoint.position = spawnPosition;
    }

}
