using Patterns.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum MatchState
{
    WaitingToStart,
    InProgress,
    IsPaused,
    WaitingToRevived,
    ShowResult,
    LeavingMatch
}

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private MatchState _matchState;
    [SerializeField] private int _totalTime_Rivive = 6;
    private Coroutine _coroutineDelayedStart;
    private float _timeCounter;
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
                this.ShowResult();
            }

            this._matchState = value; 
        }
    }

    protected override void SetupValues()
    {
        base.SetupValues();

        this.MatchState = MatchState.WaitingToStart;
    }

    private void Update()
    {
        if (GameManager.Instance.MatchState == MatchState.WaitingToRevived)
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
        CameraManager.Instance.PrepareToStartMatch();
        Level.Instance.PrepareToStartMatch();
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
        GameMode.Instance.ShowResult();
    }

    public void SetMatchState(MatchState state)
    {
        this.MatchState = state;
    }

    public void HandlePlayerDeath()
    {
        Level.Instance.HandlePlayerDeath();
    }

}
