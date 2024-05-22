using Patterns.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum MatchState
{
    WaitingToStart,
    InProgress,
    IsPaused,
    WaitingToRevived,
    ShowResult,
    LeavingMatch
}

public class GameManager : Singleton<GameManager>, IDataPersistence
{
    [SerializeField] private MatchState _matchState;
    [SerializeField] private int _currentLevel = 1;
    [SerializeField] private List<int> _levelsAreOpen;
    [SerializeField] private bool _hasSpawnedLevel;
    [SerializeField] private bool _isWin;
    [SerializeField] private int _totalTime_Rivive = 6;

    private Coroutine _coroutineDelayedStart;
    private Coroutine _nextLevelCoroutine;
    private float _timeCounter;
    public int CurrentLevel
    {
        get { return this._currentLevel; }
        private set 
        {
            if (Level.Instance.LevelPlane == null && !this.HasSpawnedLevel)
            {
                LevelPlaneSpawner.SpawnLevel(value);
                this.HasSpawnedLevel = true;
            }

            this._currentLevel = value; 
        }
    }
    public List<int> LevelsAreOpen
    {
        get { return this._levelsAreOpen; }
        private set { this._levelsAreOpen = value; }
    }
    public bool HasSpawnedLevel
    {
        get { return this._hasSpawnedLevel; }
        private set { this._hasSpawnedLevel = value; }
    }
    public bool IsWin
    {
        get { return this._isWin; }
        private set 
        { 
            if (value)
            {
                if (this.CurrentLevel < LevelPlaneSpawner.LevelPlanes.Length - 1)
                {
                    if (this.LevelsAreOpen.Contains(this.CurrentLevel + 1) == false)
                    {
                        this.LevelsAreOpen.Add(this.CurrentLevel + 1);
                    }
                }
                else
                {
                    CameraManager.Instance.StartEndGame();
                }

            }

            this._isWin = value; 
        }
    }
    public int TotalTime_Rivive
    {
        get { return this._totalTime_Rivive; }
        private set { this._totalTime_Rivive = value; }
    }
    public float TimeCounter => _timeCounter;
    public MatchState MatchState
    {
        get { return this._matchState; }
        private set 
        {
            if (this.IsWin && value != MatchState.ShowResult) return;  // TEST

            if (value == MatchState.WaitingToStart)
            {
                this.PrepareToStartMatch();
            }
            else if (value == MatchState.InProgress)
            {
                if (this.MatchState == MatchState.WaitingToRevived)
                    this.RevivePlayer();
                else if (this.MatchState == MatchState.IsPaused)
                    this.ResumeMatch();
                else
                    this.StartMatch();
            }
            else if (value == MatchState.IsPaused)
            {
                this.PauseMatch();
            }
            else if (value == MatchState.WaitingToRevived)
            {
                this.PrepareForRevival();
                this._timeCounter = this.TotalTime_Rivive;
            }
            else if (value == MatchState.ShowResult)
            {
                this.ShowResult();
            }
            else if (value == MatchState.LeavingMatch)
            {
                this.LeavingMatch();
            }

            this._matchState = value; 
        }
    }

    protected override void SetupValues()
    {
        base.SetupValues();

        this._matchState = MatchState.WaitingToStart;
        this._hasSpawnedLevel = false;
        this._isWin = false;
    }

    //protected override void OnEnable()
    //{
    //    base.OnEnable();

    //    if (DataPersistenceManager.Instance != null)
    //        DataPersistenceManager.Instance.SendData(this);
    //}

    //protected override void OnDisable()
    //{
    //    base.OnDisable();

    //    if (DataPersistenceManager.Instance != null)
    //        DataPersistenceManager.Instance.ReceiveData(this);
    //}

    protected override void Start()
    {
        base.Start();

        this.MatchState = MatchState.WaitingToStart;

    }

    private void Update()
    {
        if (this.MatchState == MatchState.WaitingToRevived)
        {
            this._timeCounter -= Time.deltaTime;
            if (this._timeCounter < 0)
            {
                this.MatchState = MatchState.ShowResult;
            }
        }
    }

    private IEnumerator DelayedStart()
    {
        yield return new WaitForSecondsRealtime(2f);

        this.MatchState = MatchState.InProgress;

    }

    private void PrepareToStartMatch()
    {
        Level.Instance.PrepareToStartMatch();
        CameraManager.Instance.PrepareToStartMatch();
        GameMode.Instance.PrepareToStartMatch();

        this._coroutineDelayedStart = StartCoroutine(this.DelayedStart());
    }

    private void StartMatch()
    {
        Level.Instance.StartMatch();
        GameMode.Instance.StartMatch();
    }

    private void PauseMatch()
    {
        GameMode.Instance.PauseMatch();
        Time.timeScale = 0f;
    }

    private void ResumeMatch()
    {
        GameMode.Instance.ResumeMatch();
        Time.timeScale = 1f;
    }

    private void PrepareForRevival()
    {
        GameMode.Instance.PrepareForRevival();
        Level.Instance.PrepareForRevival();
    }

    private void RevivePlayer()
    {
        GameMode.Instance.RevivePlayer();
    }

    private void ShowResult()
    {
        Level.Instance.ShowResult();
        GameMode.Instance.ShowResult(this.IsWin);
    }
    
    private void LeavingMatch()
    {
        int sceneTotal = SceneManager.sceneCountInBuildSettings;
        int sceneNext = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(sceneNext % sceneTotal, LoadSceneMode.Single);
    }

    public void SetMatchState(MatchState state)
    {
        this.MatchState = state;
    }

    public void HandlePlayerDeath()
    {
        Level.Instance.HandlePlayerDeath();
    }

    /*
     * 
     */

    public void NextLevel()
    {
        this._nextLevelCoroutine = StartCoroutine(this.NextLevel_Coroutine());
    }

    private IEnumerator NextLevel_Coroutine()
    {
        this.StartTheNextLevel();
        yield return new WaitForSecondsRealtime(1);
        this.EndOfNextLevel();
    }

    public void StartTheNextLevel()
    {
        CameraManager.Instance.NextLevel();
        Level.Instance.NextLevel();
        GameMode.Instance.NextLevel();
    }

    public void EndOfNextLevel()
    {
        this.CurrentLevel++;
        DataPersistenceManager.Instance.ReceiveData(this);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    /*
     * 
     */

    public void LoadGame(GameData data)
    {
        this.CurrentLevel = data.CurrentLevel;
        this.LevelsAreOpen = data.LevelsAreOpen;
    }

    public void SaveGame(ref GameData data)
    {
        data.CurrentLevel = this.CurrentLevel;
        data.LevelsAreOpen = this.LevelsAreOpen;
    }

    /*
     * 
     */

    public void SetIsWin(bool isWin)
    {
        this.IsWin = isWin;
    }
}
