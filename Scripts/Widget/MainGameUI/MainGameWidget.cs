using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameWidget : RyoMonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Background_MainGameWidget _background;
    public Animator Animator => _animator;
    public Background_MainGameWidget Background => _background;

    #region LoadComponents
    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadAnimator();
        this.LoadBackground();
    }

    private void LoadAnimator()
    {
        if (this._animator != null) return;
        this._animator = GetComponent<Animator>();
    }

    private void LoadBackground()
    {
        if (this._background != null) return;
        this._background = GetComponentInChildren<Background_MainGameWidget>();
    }

    #endregion


    /*
     * Button
     */

    public void PressStartButton()
    {
        this.Animator?.SetTrigger(AnimationString.isStartMatch);

        // this.StartGame();
    }

    public void PressShopButton()
    {
        GUIManager.Instance.SetActive_ShopWidget(true);
    }
    
    public void PressSettingButton()
    {
        GUIManager.Instance.SetActive_SettingWidget(true);
    }

    public void PressLevelButton()
    {
        GUIManager.Instance.SetActive_LevelWidget(true);
    }

    public void PressExitButton()
    {
        Application.Quit();
    }

    /*
     * 
     */

    public void StartGame()
    {
        this.LoadSceneMatch();
    }

    private void LoadSceneMatch()
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextScene % sceneCount, LoadSceneMode.Single);
        // SceneManager.LoadSceneAsync(nextScene % sceneCount, LoadSceneMode.Single);
    }

    public void LevelChange(int index)
    {
        this.Background?.LevelChange(index);
    }

    // public void 

}
