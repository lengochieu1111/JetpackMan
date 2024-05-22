using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Patterns.Singleton;
using System;

public class GUIManager : Singleton<GUIManager>, IDataPersistence
{
    [SerializeField] private MainGameWidget _mainGameWidget;
    [SerializeField] private LevelWidget _levelWidget;
    [SerializeField] private ShopWidget _shopWidget;
    [SerializeField] private SettingWidget _settingWidget;
    public MainGameWidget MainGameWidget => _mainGameWidget;
    public LevelWidget LevelWidget => _levelWidget;
    public ShopWidget ShopWidget => _shopWidget;
    public SettingWidget SettingWidget => _settingWidget;

    #region LoadComponents
    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadMainGameWidget();
        this.LoadLevelWidget();
        this.LoadShopWidget();
        this.LoadSettingWidget();
    }

    private void LoadMainGameWidget()
    {
        if (this._mainGameWidget != null) return;
        this._mainGameWidget = GetComponentInChildren<MainGameWidget>();
    }
    
    private void LoadLevelWidget()
    {
        if (this._levelWidget != null) return;
        this._levelWidget = GetComponentInChildren<LevelWidget>(true);

        this._levelWidget.gameObject.SetActive(false);
    }
    
    private void LoadShopWidget()
    {
        if (this._shopWidget != null) return;
        this._shopWidget = GetComponentInChildren<ShopWidget>(true);

        this._shopWidget.gameObject.SetActive(false);
    }
    
    private void LoadSettingWidget()
    {
        if (this._settingWidget != null) return;
        this._settingWidget = GetComponentInChildren<SettingWidget>(true);

        this._settingWidget.gameObject.SetActive(false);
    }

    #endregion

    protected override void OnEnable()
    {
        base.OnEnable();

    }

    public void LevelChange(int index)
    {
        this.MainGameWidget?.LevelChange(index);
    }

    /*
     * 
     */

    public void SetActive_LevelWidget(bool isActive)
    {
        this.LevelWidget?.gameObject.SetActive(isActive);
    }
    
    public void SetActive_ShopWidget(bool isActive)
    {
        this.ShopWidget?.gameObject.SetActive(isActive);
    }
    
    public void SetActive_SettingWidget(bool isActive)
    {
        this.SettingWidget?.gameObject.SetActive(isActive);
    }

    /*
     * 
     */

    public void LoadGame(GameData data)
    {
        this.LevelChange(data.CurrentLevel);
    }

    public void SaveGame(ref GameData data) { }
}
