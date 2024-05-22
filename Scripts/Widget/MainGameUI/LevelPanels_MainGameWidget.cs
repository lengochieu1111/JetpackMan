using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelPanels_MainGameWidget : RyoMonoBehaviour
{
    [SerializeField] private List<GameObject> _levelPanelList;
    public List<GameObject> LevelPanelList => _levelPanelList;

    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadLevelPanelList();
    }

    private void LoadLevelPanelList()
    {
        if (this._levelPanelList.Count > 0) return;

        for (int i = 0; i < this.transform.childCount; i++)
        {
            this._levelPanelList.Add(this.transform.GetChild(i).gameObject);
        }
    }

    public void SetActive_LevelPanel(int index)
    {
        if (index >= this.LevelPanelList.Count) return;

        foreach (GameObject levelPanel in this.LevelPanelList)
        {
            levelPanel.SetActive(false);
        }

        this.LevelPanelList[index].SetActive(true);
    }

}
