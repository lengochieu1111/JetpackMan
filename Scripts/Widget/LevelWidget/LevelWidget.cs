using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelWidget : RyoMonoBehaviour, IDataPersistence
{
    [SerializeField] private List<LevelIndexButton_LevelWidget> _levelIndexButtons;
    [SerializeField] private int _currentLevel;
    [SerializeField] private List<int> _levelsAreOpen;
    public int CurrentLevel
    {
        get { return this._currentLevel; }
        private set { this._currentLevel = value; }
    }
    public List<int> LevelsAreOpen
    {
        get { return this._levelsAreOpen; }
        private set { this._levelsAreOpen = value; }
    }
    public List<LevelIndexButton_LevelWidget> LevelIndexButtons => _levelIndexButtons;

    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadLevelIndexButtons();
    }

    private void LoadLevelIndexButtons()
    {
        if (this._levelIndexButtons.Count > 0) return;

        this._levelIndexButtons = GetComponentsInChildren<LevelIndexButton_LevelWidget>().ToList();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        DataPersistenceManager.Instance.SendData(this);
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        DataPersistenceManager.Instance.ReceiveData(this);
    }

    public void LevelChange(LevelIndexButton_LevelWidget levelIndexButton)
    {
        for (int i = 0; i < this.LevelIndexButtons.Count; i++)
        {
            if (levelIndexButton.Equals(this.LevelIndexButtons[i]))
            {
                //
                // this.LevelsAreOpen.Add(i);
                // 

                if (this.LevelsAreOpen.Contains(i))
                {
                    this.CurrentLevel = i;
                    GUIManager.Instance.LevelChange(i);
                }
            }
        }

    }

    /*
     * Button
     */

    public void PressLevelIndexButton(LevelIndexButton_LevelWidget levelIndexButton)
    {
        this.LevelChange(levelIndexButton);
    }

    public void PressExitButton()
    {
        SoundManager.Instance.PlayAudio_Compress();
        this.gameObject.SetActive(false);
    }

    public void PressStartButton()
    {
        SoundManager.Instance.PlayAudio_Compress();
        DataPersistenceManager.Instance.ReceiveData(this);
        //
        this.StartGame();
    }

    public void StartGame()
    {
        this.LoadSceneMatch();
    }

    private void LoadSceneMatch()
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextScene % sceneCount, LoadSceneMode.Single);
    }

    /*
     * DataPersistence
     */

    public void LoadGame(GameData data)
    {
        this.CurrentLevel = data.CurrentLevel;

        foreach (int level in data.LevelsAreOpen)
        {
            this.LevelsAreOpen.Add(level);
        }

        this.CheckLevelsAreOpen();
    }

    private void CheckLevelsAreOpen()
    {
        for (int i = 0; i < this.LevelIndexButtons.Count; i++)
        {
            if (this.LevelsAreOpen.Contains(i))
            {
                this.LevelIndexButtons[i].LevelOpen();
            }
            else
            {
                this.LevelIndexButtons[i].LevelClosed();
            }
        }
    }

    public void SaveGame(ref GameData data)
    {
        data.CurrentLevel = this.CurrentLevel;

        foreach (int level in this.LevelsAreOpen)
        {
            if (data.LevelsAreOpen.Contains(level)) continue;

            data.LevelsAreOpen.Add(level);
        }
    }
}
