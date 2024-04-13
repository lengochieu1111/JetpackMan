using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : RyoMonoBehaviour, IDataPersistence
{
    [SerializeField] private int _allGoldCoins;
    [SerializeField] private int _goldCoin;
    [SerializeField] private int _silverCoin;
    [SerializeField] private int _distance;

    public int AllGoldCoins
    {
        get { return this._allGoldCoins; }
        private set { this._allGoldCoins = value; }
    }
    public int GoldCoin
    {
        get { return this._goldCoin; }
        private set { this._goldCoin = value; }
    }

    public int SilverCoin
    {
        get { return this._silverCoin; }
        private set { this._silverCoin = value; }
    }
    
    public int Distance
    {
        get { return this._distance; }
        private set { this._distance = value; }
    }

    protected override void SetupValues()
    {
        base.SetupValues();

        this.GoldCoin = 0;
        this.SilverCoin = 0;
        this.Distance = 0;
    }

    public void PrepareToStartMatch()
    {
        this.GoldCoin = 0;
        this.SilverCoin = 0;
        this.Distance = 0;
    }

    public void AddOne_GoldCoin()
    {
        this.GoldCoin++;
    }

    public void AddOne_SilverCoin()
    {
        this.SilverCoin++;
    }
    
    public void AddOne_Distance()
    {
        this.Distance++;
    }

    #region Data Persistence
    public void LoadGame(GameData data)
    {
        this.AllGoldCoins = data.AllGoldCoins;
    }

    public void SaveGame(ref GameData data)
    {
        data.AllGoldCoins = this.AllGoldCoins + this.GoldCoin;
    }
    #endregion

}
