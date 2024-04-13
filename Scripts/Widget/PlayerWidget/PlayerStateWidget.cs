using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateWidget : RyoMonoBehaviour
{
    [SerializeField] private Text _goldCoin_Text;
    [SerializeField] private Text _silverCoin_Text;
    [SerializeField] private Text _distanceCounter_Text;
    public Text GoldCoin_Text
    {
        get { return this._goldCoin_Text; }
        private set { this._goldCoin_Text = value;}
    }
    public Text SilverCoin_Text
    {
        get { return this._silverCoin_Text; }
        private set { this._silverCoin_Text = value; }
    }
    public Text DistanceCounter_Text
    {
        get { return this._distanceCounter_Text; }
        private set { this._distanceCounter_Text = value; }
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();

        Transform coinGold = this.transform.Find("GoldCoin_Text");
        this.GoldCoin_Text = coinGold?.GetComponent<Text>();
        
        Transform coinSilver = this.transform.Find("SilverCoin_Text");
        this.SilverCoin_Text = coinSilver?.GetComponent<Text>();
        
        Transform distance = this.transform.Find("Distance_Text");
        this.DistanceCounter_Text = distance?.GetComponent<Text>();
    }

    public void UpdateGoldCoin_Text(string text)
    {
        if (this.GoldCoin_Text == null) return;
        this.GoldCoin_Text.text = text;
    }
    
    public void UpdateSilverCoin_Text(string text)
    {
        if (this.SilverCoin_Text == null) return;
        this.SilverCoin_Text.text = text;
    }
    
    public void UpdateDistanceCounter(string text)
    {
        if (this.DistanceCounter_Text == null) return;
        this.DistanceCounter_Text.text = text + " m";
    }

}
