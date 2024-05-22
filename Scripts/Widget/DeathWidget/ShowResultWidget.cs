using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShowResultWidget : BaseWidget
{
    [SerializeField] private Transform _losePanel;
    [SerializeField] private Transform _winPanel;

    [SerializeField] private TextMeshProUGUI _matchCoin_Text;
    [SerializeField] private TextMeshProUGUI _allCoin_Text;
    public Transform LosePanel => this._losePanel;
    public Transform WinPanel => this._winPanel;
    public TextMeshProUGUI MatchCoin_Text => this._matchCoin_Text;
    public TextMeshProUGUI AllCoin_Text => this._allCoin_Text;

    #region LoadComponents
    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadLosePanel();
        this.LoadWinPanel();

        this.LoadMatchCoin_Text();
        this.LoadAllCoin_Text();
    }

    private void LoadLosePanel()
    {
        if (this._losePanel != null) return;

        this._losePanel = this.FindChildByName(transform, "LosePanel");
        this._losePanel?.gameObject.SetActive(false);
    }
    
    private void LoadWinPanel()
    {
        if (this._winPanel != null) return;

        this._winPanel = this.FindChildByName(transform, "WinPanel");
        this._winPanel?.gameObject.SetActive(false);
    }

    private void LoadMatchCoin_Text()
    {
        if (this._matchCoin_Text != null) return;

        Transform coind_Match = this.FindChildByName(transform, "MatchCoin_Text");
        this._matchCoin_Text = coind_Match?.GetComponent<TextMeshProUGUI>();
    }
    
    private void LoadAllCoin_Text()
    {
        if (this._allCoin_Text != null) return;

        Transform coind_All = this.FindChildByName(transform, "AllCoin_Text");
        this._allCoin_Text = coind_All?.GetComponent<TextMeshProUGUI>();
    }


    private Transform FindChildByName(Transform parrent, string childName)
    {
        Transform childObject = parrent.Find(childName);

        if (childObject != null)
        {
            return childObject;
        }
        else
        {
            foreach (Transform child in parrent)
            {
                childObject = this.FindChildByName(child, childName);

                if (childObject != null)
                    return childObject;
            }

            return null;
        }
    }

    #endregion


    /*
     * 
     */

    public void SetActive_LosePanel(bool isActive)
    {
        this.LosePanel?.gameObject.SetActive(isActive);
    }

    public void SetActive_WinPanel(bool isActive)
    {
        this.WinPanel?.gameObject.SetActive(isActive);
    }

    /*
     * 
     */

    public void UpdateMatchCoind_Text(string text)
    {
        if (this.MatchCoin_Text == null) return;
        this.MatchCoin_Text.text = text;
    }
    
    public void UpdateAllCoind_Text(string text)
    {
        if (this.AllCoin_Text == null) return;
        this.AllCoin_Text.text = text;
    }

    /*
     * 
     */

    public void PressNextButton()
    {
        GameManager.Instance.NextLevel();
    }

    public void PressRetryButton()
    {
        this.PlayAudioClicked();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public void PressQuitButton()
    {
        this.PlayAudioClicked();

        int sceneTotal = SceneManager.sceneCountInBuildSettings;
        int sceneNext = SceneManager.GetActiveScene().buildIndex + 1;

        SceneManager.LoadScene(sceneNext % sceneTotal, LoadSceneMode.Single);
    }

    /*
     * 
     */
    private void PlayAudioClicked()
    {
        SoundManager.Instance.PlayAudio_Compress();
    }

}
