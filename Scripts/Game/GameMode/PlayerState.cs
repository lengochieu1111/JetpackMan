using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : RyoMonoBehaviour, IDataPersistence
{
    [SerializeField] private int _allCoins;
    [SerializeField] private int _allCrystals;
    [SerializeField] private int _matchCoin;
    [SerializeField] private int _matchCrystal;
    [SerializeField] private int _matchEnergy;
    [SerializeField] private int _matchEnergyMax = 100;

    public int AllCoins
    {
        get { return this._allCoins; }
        private set { this._allCoins = value; }
    }
    public int AllCrystals
    {
        get { return this._allCrystals; }
        private set { this._allCrystals = value; }
    }
    public int MatchCoin
    {
        get { return this._matchCoin; }
        private set { this._matchCoin = value; }
    }

    public int MatchCrystal
    {
        get { return this._matchCrystal; }
        private set { this._matchCrystal = value; }
    }
    public int MatchEnergy
    {
        get { return this._matchEnergy; }
        private set { this._matchEnergy = value; }
    }
    
    public int MatchEnergyMax
    {
        get { return this._matchEnergyMax; }
        private set { this._matchEnergyMax = value; }
    }

    protected override void SetupValues()
    {
        base.SetupValues();

        this.MatchCoin = 0;
        this.MatchCrystal = 0;
        this.MatchEnergy = 0;
        this.MatchEnergyMax = 100;
    }

    public void PrepareToStartMatch()
    {
        this.MatchCoin = 0;
        this.MatchCrystal = 0;
        this.MatchEnergy = 0;
        this.MatchEnergyMax = 100;
    }

    public void AddOne_MatchCoin()
    {
        this.MatchCoin++;
    }
    
    public void AddOne_MatchCrytal()
    {
        this.MatchCrystal++;
    }
    
    public void DoubleCoin()
    {
        this.MatchCoin *= 2;
    }

    public void Add_MatchEnergy(int energy)
    {
        this.MatchEnergy = Mathf.Clamp(this.MatchEnergy + energy, 0, this.MatchEnergyMax);
    }

    public void Reduce_MatchEnergy(int energy)
    {
        this.MatchEnergy = Mathf.Clamp(this.MatchEnergy - energy, 0, this.MatchEnergyMax);
    }


    #region Data Persistence
    public void LoadGame(GameData data)
    {
        this.AllCoins = data.AllCoins;
        this.AllCrystals = data.AllCrystals;
    }

    public void SaveGame(ref GameData data)
    {
        data.AllCoins = this.AllCoins + this.MatchCoin;
        data.AllCrystals = this.AllCrystals + this.MatchCrystal;
    }
    #endregion

    public bool Reduce_Coin(int reducce)
    {
        if (reducce > this.AllCoins) return false;

        this.AllCoins -= reducce;
        return true;
    }

    public bool Reduce_Crystal(int reducce)
    {
        if (reducce > this.MatchCrystal) return false;

        this.MatchCrystal -= reducce;
        return true;
    }

}
