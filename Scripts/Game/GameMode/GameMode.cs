using MVCS.Architecture.BaseCharacter;
using Patterns.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : Singleton<GameMode>
{
    [SerializeField] private PlayerState _playerState;
    [SerializeField] private BaseCharacter _player;
    [SerializeField] private HUD _hud;
    [SerializeField] private int _matchDistance;
    [SerializeField] private float _timeRevive = 0.8f;
    private Coroutine _coroutineRevive;
    private int _matchDistanceLast;

    public PlayerState PlayerState
    {
        get { return this._playerState; }
        private set { this._playerState = value; }
    }
    public BaseCharacter Player
    {
        get { return this._player; }
        private set { this._player = value; }
    }
    public HUD HUD
    {
        get { return this._hud; }
        private set { this._hud = value; }
    }
    public int MatchDistance
    {
        get { return this._matchDistance; }
        private set { this._matchDistance = value; }
    }

    #region LoadComponents
    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadPlayerState();
        this.LoadPlayer();
        this.LoadHUD();
    }

    private void LoadHUD()
    {
        if (this.HUD != null) return;
        this.HUD = GetComponentInChildren<HUD>();
    }

    private void LoadPlayer()
    {
        if (this.Player != null) return;
        this.Player = GetComponentInChildren<BaseCharacter>();
    }

    private void LoadPlayerState()
    {
        if (this.PlayerState != null) return;
        this.PlayerState = GetComponentInChildren<PlayerState>();
    }
    #endregion

    private void Update()
    {
        if (GameManager.Instance.MatchState == MatchState.InProgress)
        {
            this.UpdateMatchDistance();
            if (this._matchDistanceLast == this.MatchDistance) return;
            this.UpdatePlayerStateWidget_DistanceCounter();
            this._matchDistanceLast = this.MatchDistance;
        }
        else if (GameManager.Instance.MatchState == MatchState.WaitingToRevived)
        {
            string str_timeCounter = ((int)GameManager.Instance.TimeCounter).ToString();
            this.UpdateReviveWidget_TimeCounter(str_timeCounter);
        }
    }
    public void PrepareToStartMatch()
    {
        this.PlayerState?.PrepareToStartMatch();
        this.HUD?.PrepareToStartMatch();
        this.Player?.PrepareToStartMatch();

        this.SetStartingPointForPlayer();
        this.Player?.gameObject.SetActive(false);

        this.UpdatePlayerStateWidget_CoinGold();
        this.UpdatePlayerStateWidget_CoinSilver();
        this.UpdatePlayerStateWidget_DistanceCounter();
    }

    public void StartMatch()
    {
        this.Player.gameObject.SetActive(true);
        this.Player?.StartMatch();
    }

    public void PrepareForRevival()
    {
        this.HUD?.PrepareForRevival();
    }

    public void PauseMatch()
    {
        this.HUD?.PauseMatch();
    }

    public void ResumeMatch()
    {
        this.HUD?.ResumeMatch();
    }

    public void RevivePlayer()
    {
        this.HUD?.RevivePlayer();
        this._coroutineRevive = StartCoroutine(this.DelayRevive());
    }

    public void ShowResult()
    {
        this.HUD?.ShowResult();

        this.UpdateShowResultWidget_MatchCoin();
        this.UpdateShowResultWidget_AllCoin();
        this.UpdateShowResultWidget_MatchDistance();
    }

    private void SetStartingPointForPlayer()
    {
        Vector3 spawnPosition = Level.Instance.PlayerStartingPoint.position;
        spawnPosition.z = this.Player.transform.position.z;
        this.Player.transform.position = spawnPosition;
    }

    private IEnumerator DelayRevive()
    {
        float screenHeight = Screen.height;
        Camera camera = Camera.main;

        Vector3 repawnPosition = camera.ScreenToWorldPoint(new Vector3(0f, screenHeight * 0.5f, 0f));
        Vector3 currentPosition = this.Player.transform.position;

        float timeCounter = 0;
        while(timeCounter < this._timeRevive)
        {
            timeCounter += Time.deltaTime;
            this.Player.transform.position = new Vector3(currentPosition.x, 
                Mathf.Lerp(currentPosition.y, repawnPosition.y, timeCounter / this._timeRevive), currentPosition.z);

            yield return null;
        }

        Level.Instance.RevivePlayer();
        this.Player?.RevivePlayer();
    }

    private void UpdateMatchDistance()
    {
        this.MatchDistance = (int)(this.Player.transform.position.x - Level.Instance.PlayerStartingPoint.position.x);
    }

    public void HandlePlayerPickUp_Coin(ECoinType coinType)
    {
        if (coinType == ECoinType.Silver)
        {
            this.PlayerState?.AddOne_SilverCoin();
            this.UpdatePlayerStateWidget_CoinSilver();
        }
        else
        {
            this.PlayerState?.AddOne_GoldCoin();
            this.UpdatePlayerStateWidget_CoinGold();
        }

    }

    #region Player State Widget

    private void UpdatePlayerStateWidget_CoinGold()
    {
        this.HUD?.UpdateCoinGold_Text(this.PlayerState.GoldCoin.ToString());
    }
    
    private void UpdatePlayerStateWidget_CoinSilver()
    {
        this.HUD?.UpdateCoinSilver_Text(this.PlayerState.SilverCoin.ToString());
    }
    
    private void UpdatePlayerStateWidget_DistanceCounter()
    {
        this.HUD?.UpdateDistanceCounter_Text(this.MatchDistance.ToString());
    }
    #endregion

    #region Revive Widget
    private void UpdateReviveWidget_TimeCounter(string text)
    {
        this.HUD?.UpdateTimeCounter_Text(text);
    }
    #endregion

    #region Show Result Widget
    private void UpdateShowResultWidget_MatchCoin()
    {
        this.HUD?.UpdateMatchCoind_Text(this.PlayerState.GoldCoin.ToString());
    }

    private void UpdateShowResultWidget_AllCoin()
    {
        this.HUD?.UpdateAllCoind_Text((this.PlayerState.AllGoldCoins + this.PlayerState.GoldCoin).ToString());
    }

    private void UpdateShowResultWidget_MatchDistance()
    {
        this.HUD?.UpdateMatchDistance_Text(this.MatchDistance.ToString());
    }
    #endregion

}
