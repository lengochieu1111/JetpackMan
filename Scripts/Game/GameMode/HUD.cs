using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : RyoMonoBehaviour
{
    [Header("Widget")]
    [SerializeField] private PlayerStateWidget _playerWidget;
    [SerializeField] private ShowResultWidget _showResultWidget;
    [SerializeField] private ReviveWidget _reviveWidget;
    [SerializeField] private MatchPausedWidget _matchPausedWidget;
    public PlayerStateWidget PlayerWidget => _playerWidget;
    public ShowResultWidget ShowResultWidget => _showResultWidget;
    public ReviveWidget ReviveWidget => _reviveWidget;
    public MatchPausedWidget MatchPausedWidget  => _matchPausedWidget;

    #region Load Components
    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadPlayerStateWidget();
        this.LoadReviveWidget();
        this.LoadDeathWidget();
        this.LoadMatchPausedWidget();
    }

    private void LoadPlayerStateWidget()
    {
        if (this._playerWidget != null) return;
        this._playerWidget = GetComponentInChildren<PlayerStateWidget>(true);
    }
    
    private void LoadReviveWidget()
    {
        if (this._reviveWidget != null) return;
        this._reviveWidget = GetComponentInChildren<ReviveWidget>(true);
    }
    
    private void LoadDeathWidget()
    {
        if (this._showResultWidget != null) return;
        this._showResultWidget = GetComponentInChildren<ShowResultWidget>(true);
    }
    
    private void LoadMatchPausedWidget()
    {
        if (this._matchPausedWidget != null) return;
        this._matchPausedWidget = GetComponentInChildren<MatchPausedWidget>(true);
    }
    #endregion

    public void PrepareToStartMatch()
    {
        this.SetActive_PlayerStateWidget(true);
        this.SetActive_ReviveWidgetWidget(false);
        this.SetActive_ShowResultWidget(false);
        this.SetActive_MatchPausedWidget(false);
    }

    public void PauseMatch()
    {
        this.SetActive_PlayerStateWidget(true);
        this.SetActive_ReviveWidgetWidget(false);
        this.SetActive_ShowResultWidget(false);
        this.SetActive_MatchPausedWidget(true);
    }

    public void ResumeMatch()
    {
        this.SetActive_PlayerStateWidget(true);
        this.SetActive_ReviveWidgetWidget(false);
        this.SetActive_ShowResultWidget(false);
        this.SetActive_MatchPausedWidget(false);
    }

    public void PrepareForRevival()
    {
        this.SetActive_PlayerStateWidget(true);
        this.SetActive_ReviveWidgetWidget(true);
        this.SetActive_ShowResultWidget(false);
        this.SetActive_MatchPausedWidget(false);
    }

    public void RevivePlayer()
    {
        this.SetActive_PlayerStateWidget(true);
        this.SetActive_ReviveWidgetWidget(false);
        this.SetActive_ShowResultWidget(false);
        this.SetActive_MatchPausedWidget(false);
    }

    public void ShowResult(bool isWin)
    {
        this.SetActive_PlayerStateWidget(false);
        this.SetActive_ReviveWidgetWidget(false);
        this.SetActive_MatchPausedWidget(false);
        this.SetActive_ShowResultWidget(true);

        this.ShowResultWidget?.SetActive_WinPanel(isWin);
        this.ShowResultWidget?.SetActive_LosePanel(!isWin);

    }

    /*
     * Set Active Widget 
     */

    public void SetActive_PlayerStateWidget(bool isActive)
    {
        this.PlayerWidget?.gameObject.SetActive(isActive);
    }

    public void SetActive_ReviveWidgetWidget(bool isActive)
    {
        this.ReviveWidget?.gameObject.SetActive(isActive);
    }

    public void SetActive_ShowResultWidget(bool isActive)
    {
        this.ShowResultWidget?.gameObject.SetActive(isActive);
    }
    
    public void SetActive_MatchPausedWidget(bool isActive)
    {
        this.MatchPausedWidget?.gameObject.SetActive(isActive);
    }

    /*
     * Player State Widget 
     */
    #region Player State Widget
    public void UpdateCoin_Text(string newText)
    {
        this.PlayerWidget?.UpdateCoin_Text(newText);
    }

    public void UpdateCrystal_Text(string newText)
    {
        this.PlayerWidget?.UpdateCrystal_Text(newText);
    }
    
    public void UpdateProgress_Level(float percen)
    {
        this.PlayerWidget?.UpdateProgress_Level(percen);
    }
    
    public void UpdateProgress_Energy(float percen)
    {
        this.PlayerWidget?.UpdateProgress_Energy(percen);
    }
    #endregion

    /*
     * Revive Widget
     */
    #region Revive Widget
    public void UpdateTimeCounter_Text(string text)
    {
        this.ReviveWidget?.UpdateTimeCounter_Text(text);
    }
    #endregion

    /*
     * Show Result Widget
     */
    #region Show Result Widget
    public void UpdateMatchCoind_Text(string text)
    {
        this.ShowResultWidget?.UpdateMatchCoind_Text(text);
    }

    public void UpdateAllCoind_Text(string text)
    {
        this.ShowResultWidget?.UpdateAllCoind_Text(text);
    }
    #endregion

}
