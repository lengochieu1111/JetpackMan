using MVCS.Architecture.BaseCharacter;
using Patterns.Singleton;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMode : Singleton<GameMode>
{
    [SerializeField] private PlayerState _playerState;
    [SerializeField] private BaseCharacter _player;
    [SerializeField] private HUD _hud;
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
            float percen = Level.Instance.DistancToStartingPoint * 1.0f / Level.Instance.CurrentLevelDistance;
            this.UpdatePlayerStateWidget_ProgressLevel(percen);
        }
        else if (GameManager.Instance.MatchState == MatchState.WaitingToRevived)
        {
            string str_timeCounter = ((int)GameManager.Instance.TimeCounter).ToString();
            this.UpdateReviveWidget_TimeCounter(str_timeCounter);
        }
    }

    /*
     * 
     */

    public void PrepareToStartMatch()
    {
        this.PlayerState?.PrepareToStartMatch();
        this.HUD?.PrepareToStartMatch();

        this.SetStartingPointForPlayer();
        this.Player?.gameObject.SetActive(false);

        this.UpdatePlayerStateWidget_MatchCoin();
        this.UpdatePlayerStateWidget_MatchCrystal();
        this.UpdatePlayerStateWidget_ProgressEnergy();
        this.UpdatePlayerStateWidget_ProgressLevel(0);
    }

    public void StartMatch()
    {
        this.Player?.gameObject.SetActive(true);
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

    public void ShowResult(bool isWin)
    {
        this.HUD?.ShowResult(isWin);
        this.Player?.gameObject?.SetActive(false);
        this.UpdateShowResultWidget_MatchCoin();
        this.UpdateShowResultWidget_AllCoin();
    }

    /*
     * 
     */

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
    /*
     * 
     */

    public void NextLevel()
    {
        this.SetStartingPointForPlayer();
        this.Player?.gameObject.SetActive(false);
    }

    /*
     * 
     */

    public void HandlePlayerPickUp_Coin()
    {
        this.PlayerState?.AddOne_MatchCoin();
        this.UpdatePlayerStateWidget_MatchCoin();
    }
    
    public void HandlePlayerPickUp_Crystal()
    {
        this.PlayerState?.AddOne_MatchCrytal();
        this.UpdatePlayerStateWidget_MatchCrystal();
    }
    
    public void HandlePlayerPickUp_Energy(int energy)
    {
        if (this.PlayerState == null) return;

        this.PlayerState?.Add_MatchEnergy(energy);
        this.UpdatePlayerStateWidget_ProgressEnergy();

        if (this.PlayerState.MatchEnergy >= this.PlayerState.MatchEnergyMax)
        {
            this.HUD?.PlayerWidget?.SetActive_SkillButton(true);
        }
    }

    public void HandlePlayerUseSkill()
    {
        this.PlayerState?.Reduce_MatchEnergy(this.PlayerState.MatchEnergyMax);
        this.UpdatePlayerStateWidget_ProgressEnergy();
        this.HUD?.PlayerWidget?.SetActive_SkillButton(false);
    }

    public void HandlePlayerPickUp_DoubleCoin()
    {
        this.PlayerState?.DoubleCoin();
        this.UpdatePlayerStateWidget_MatchCoin();
    }

    

    #region Player State Widget

    private void UpdatePlayerStateWidget_MatchCoin()
    {
        this.HUD?.UpdateCoin_Text(this.PlayerState.MatchCoin.ToString());
    }
    
    private void UpdatePlayerStateWidget_MatchCrystal()
    {
        this.HUD?.UpdateCrystal_Text(this.PlayerState.MatchCrystal.ToString());
    }
    
    private void UpdatePlayerStateWidget_ProgressEnergy()
    {
        this.HUD?.UpdateProgress_Energy(this.PlayerState.MatchEnergy * 1.0f / this.PlayerState.MatchEnergyMax);
    }
    
    private void UpdatePlayerStateWidget_ProgressLevel(float percen)
    {
        this.HUD?.UpdateProgress_Level(percen);
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
        this.HUD?.UpdateMatchCoind_Text(this.PlayerState.MatchCoin.ToString());
    }

    private void UpdateShowResultWidget_AllCoin()
    {
        this.HUD?.UpdateAllCoind_Text((this.PlayerState.AllCoins + this.PlayerState.MatchCoin).ToString());
    }
    #endregion

}
