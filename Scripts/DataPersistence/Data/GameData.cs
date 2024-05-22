using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public bool IsActive_Sound;
    public bool IsActive_Music;
    public int AllCrystals;
    public int AllCoins;
    public List<int> LevelsAreOpen;
    public int CurrentLevel;
    public List<bool> OpenedItems;
    public int CurrentBullet;
    public int CurrentCharacter;

    public GameData()
    {
        this.IsActive_Sound = false;
        this.IsActive_Music = false;
        this.AllCoins = 0;
        this.AllCrystals = 0;
        this.LevelsAreOpen = new List<int> { 0 };
        this.OpenedItems = new List<bool> { false, false, false };
        this.CurrentLevel = 0;
    }
}
