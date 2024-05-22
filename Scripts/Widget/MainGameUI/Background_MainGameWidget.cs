using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Background_MainGameWidget : RyoMonoBehaviour
{
    [SerializeField] private LevelPanels_MainGameWidget _levelPanels;
    public LevelPanels_MainGameWidget LevelPanels => _levelPanels;

    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadLevelPanels();
    }

    private void LoadLevelPanels()
    {
        if (this._levelPanels != null) return;
        this._levelPanels = GetComponentInChildren<LevelPanels_MainGameWidget>(true);
    }

    public void LevelChange(int index)
    {
        this.LevelPanels?.SetActive_LevelPanel(index);
    }

}
